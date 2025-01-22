namespace MiNET.AspNet.Utils
{
	public interface IShutdownTask
	{
		Task ExecuteShutdownAsync(CancellationToken cancellationToken = default);
	}
}
