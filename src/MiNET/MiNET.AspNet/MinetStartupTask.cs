using log4net.Config;
using log4net;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using MiNET.Net;
using MiNET.Utils;
using MiNET.AspNet.Utils;

namespace MiNET.AspNet
{
	public class MinetStartupTask(ILogger<MinetStartupTask> logger) : IStartupTask, IShutdownTask
	{
		private readonly ILogger<MinetStartupTask> _logger = logger;
		private static MiNetServer? Server = null;

		public Task ExecuteAsync(CancellationToken cancellationToken = default)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				log4net.Repository.ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
				XmlConfigurator.Configure(logRepository, new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.xml")));

				_logger.LogInformation(MiNetServer.MiNET);

				var currentProcess = Process.GetCurrentProcess();
				currentProcess.ProcessorAffinity = Config.GetProperty("ProcessorAffinity", (int) currentProcess.ProcessorAffinity);

				var service = new MiNetServer();
				_logger.LogInformation($"Starting MiNET for Minecraft Bedrock Edition {McpeProtocolInfo.GameVersion}...");

				service.StartServer();
				Server = service;
			}
			else
				_logger.LogError("Unsupported platform.");

			return Task.CompletedTask;
		}

		public Task ExecuteShutdownAsync(CancellationToken cancellationToken = default)
		{
			Server?.StopServer();
			Server = null;
			return Task.CompletedTask;
		}
	}
}
