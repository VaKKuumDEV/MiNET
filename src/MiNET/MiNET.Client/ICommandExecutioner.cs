#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Client
{
	public interface ICommandExecutioner
	{
		public bool CanExecute(string text);
		public void Execute(BedrockTraceHandler caller, string text);
	}

	public interface IGenericPacketHandler
	{
		public void HandlePacket(BedrockTraceHandler caller, Packet packet);
	}

	public class PlaceAllBlocksExecutioner : ICommandExecutioner, IGenericPacketHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(PlaceAllBlocksExecutioner));

		public bool CanExecute(string text)
		{
			return text.Contains("blockstates");
		}

		private bool _runningBlockMetadataDiscovery;

		public void Execute(BedrockTraceHandler caller, string text)
		{
			if (text.Contains("blockstates"))
			{
				ExecuteBlockstates(caller);
			}
			else if (text.Contains("test"))
			{
				SendCommand(caller.Client, $"/gamerule showcoordinates true");
			}
			else
			{
				Log.Debug($"Found no matching method for '{text}'");
			}
		}

		private static void SendCommand(MiNetClient client, string command)
		{
			var request = McpeCommandRequest.CreateObject();
			request.command = command;
			request.commandType = 0;
			request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			request.isinternal = false;
			request.version = 36;
			client.SendPacket(request);
		}

		private void SetGameRules(BedrockTraceHandler caller)
		{
			var client = caller.Client;
			SendCommand(client, $"/gamerule randomtickspeed 0");
			SendCommand(client, $"/gamerule doentitydrops false");
			SendCommand(client, $"/gamerule domobloot false");
			SendCommand(client, $"/gamerule domobspawning false");
			SendCommand(client, $"/gamerule dotiledrops false");
			SendCommand(client, $"/gamerule mobgriefing false");
			SendCommand(client, $"/gamerule doweathercycle false");
			SendCommand(client, $"/gamerule showcoordinates true");
			
			SendCommand(client, $"/gamerule dotiledrops false");
		}
		async void ExecuteBlockstates(BedrockTraceHandler caller)
		{
			BlockstateGenerator.init();
			var client = caller.Client;
			Log.Warn("Starting blockstate generator wait....");
			SetGameRules(caller);
			SendCommand(client, $"/tp TheGrey 20 52 0");
			var x = 0;
			foreach (var block in BlockstateGenerator.Schemas)
			{
				await Task.Delay(100);
				SendCommand(client, $"/setblock {x} 50 0 {block.Command}");
				BlockstateGenerator.blockPosition.Add(x, block);
				x += 2;
			}
		}

		public void HandlePacket(BedrockTraceHandler caller, Packet packet)
		{
			switch (packet)
			{
				case McpeUpdateBlock mcpePacket:
					HandleMcpeUpdateBlock(caller, mcpePacket);
					break;
				default:
					return;
			}
		}

		private void HandleMcpeUpdateBlock(BedrockTraceHandler caller, McpeUpdateBlock message)
		{
			if (BlockstateGenerator.blockPosition.TryGetValue(message.coordinates.X, out var state))
			{
				Log.Warn($"Got runtimeID for {state.Name}");
				BlockstateGenerator.createContainer(state.Name, message.blockRuntimeId, state.Id, state.Data, state.States);
				if (state.Name == "minecraft:barrier")
				{
					BlockstateGenerator.write();
				}
			}
			else
			{
				Log.Error($"Got unknown block with runtimeID {message.blockRuntimeId}");
			}
		}
	}
}