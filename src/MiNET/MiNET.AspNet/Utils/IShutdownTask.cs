namespace MiNET.ApNet.Utils
{
	public interface IShutdownTask
	{
		Task ExecuteShutdownAsync(CancellationToken cancellationToken = default);
	}
}
