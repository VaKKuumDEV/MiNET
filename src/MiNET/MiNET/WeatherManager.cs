using MiNET.Net;
using MiNET.Worlds;

namespace MiNET
{
	public class WeatherManager
	{
		protected Level Level { get; set; }
		public enum weatherTypes
		{
			clear, rain, thunder
		}

		public WeatherManager(Level level)
		{
			Level = level;
		}

		public virtual void setWeather(weatherTypes types)
		{
			McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
			if (types == weatherTypes.clear) { levelEvent.eventId = 3003; }
			if (types == weatherTypes.rain) { levelEvent.eventId = 3001; }
			if (types == weatherTypes.thunder) { levelEvent.eventId = 3002; }
			levelEvent.data = 10000;
			Level.RelayBroadcast(levelEvent);
		}
	}
}
