namespace MiNET.AspNet.Utils
{
	public interface IStartupTask
	{
		Task ExecuteAsync(CancellationToken cancellationToken = default);
	}
}
