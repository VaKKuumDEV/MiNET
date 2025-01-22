namespace MiNET.ApNet.Utils
{
	public static class WebHostExtensions
	{
		public static IServiceCollection AddStartupTask<T>(this IServiceCollection services) where T : class, IStartupTask => services.AddTransient<IStartupTask, T>();
		public static IServiceCollection AddShutdownTask<T>(this IServiceCollection services) where T : class, IShutdownTask => services.AddTransient<IShutdownTask, T>();

		public static async Task RunWithTasksAsync(this IHost webHost, CancellationToken cancellationToken = default)
		{
			// Получить все асинхронные задачи из DI
			IEnumerable<IStartupTask> startupTasks = webHost.Services.GetServices<IStartupTask>();

			// Выполнить эти задачи
			foreach (IStartupTask startupTask in startupTasks)
			{
				await startupTask.ExecuteAsync(cancellationToken);
			}

			// Запустить сервис как обычно
			await webHost.RunAsync(cancellationToken);
		}

		public static async Task ShutdownWithTasksAsync(this IHost webHost, CancellationToken cancellationToken = default)
		{
			// Получить все асинхронные задачи из DI
			IEnumerable<IShutdownTask> startupTasks = webHost.Services.GetServices<IShutdownTask>();

			// Выполнить эти задачи
			foreach (IShutdownTask startupTask in startupTasks)
			{
				await startupTask.ExecuteShutdownAsync(cancellationToken);
			}
		}
	}
}
