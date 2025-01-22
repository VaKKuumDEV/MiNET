namespace MiNET.ApNet.Utils
{
	public interface IStartupTask
	{
		Task ExecuteAsync(CancellationToken cancellationToken = default);
	}
}
