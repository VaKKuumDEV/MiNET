using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Client
{
	public class BlockstateGenerator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BlockstateGenerator));
		private static BlockPalette BlockPalette = null;
		public static List<Schema> Schemas = new List<Schema>();
		public static Dictionary<int, Schema> blockPosition = new Dictionary<int, Schema>();

		public class Schema
		{
			public string Command { get; set; }
			public string Name { get; set; }
			public int Id { get; set; }
			public int Data { get; set; }
			public List<schemaStates> States { get; set; } = new List<schemaStates>();
		}

		public class schemaStates
		{
			public string Type { get; set; }
			public string Name { get; set; }
			public string Value { get; set; }
		}


		public static void preInit()
		{
			var assembly = Assembly.GetAssembly(typeof(Schema));
			using (var stream = assembly.GetManifestResourceStream("MiNET.Client.schema.json"))
			using (StreamReader reader = new StreamReader(stream))
			{
				Schemas = JsonConvert.DeserializeObject<List<Schema>>(reader.ReadToEnd());
			}
		}

		public static void init()
		{
			BlockPalette = new BlockPalette();
		}

		public static void createContainer(string name, uint blockRuntime, int id, int data, List<schemaStates> states)
		{
			var stateList = new List<IBlockState>();
			foreach (var state in states)
			{
				stateList.AddRange(createState(state.Name, state.Value, state.Type));
			}
			var container = new BlockStateContainer();
			container.Id = id;
			container.Data = (short)data;
			container.Name = name;
			container.RuntimeId = (int) blockRuntime;
			container.States = stateList;
			BlockPalette.Add((int)blockRuntime, container);
		}

		public static void write()
		{
			File.WriteAllText("newResources/blockstates.json", JsonConvert.SerializeObject(BlockPalette.Values, Formatting.Indented));
			Log.Warn($"Completed successfully. All blockstates exported to 'newResources/blockstates.json'");
		}

		private static List<IBlockState> createState(string name, string value, string type)
		{
			var result = new List<IBlockState>();
			switch (type)
			{
				case "3":
					result.Add(new BlockStateInt()
					{
						Name = name,
						Value = Int32.Parse(value)
					});
					break;
				case "1":
					result.Add(new BlockStateByte()
					{
						Name = name,
						Value = (byte)Int32.Parse(value)
					});
					break;
				case "8":
					result.Add(new BlockStateString()
					{
						Name = name,
						Value = value
					});
					break;
			}
			return result;
		}
	}
}
