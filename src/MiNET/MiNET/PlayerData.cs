using MiNET.Items;
using System.Collections.Generic;

namespace MiNET
{
    public class PlayerData
    {
		public string Name { get; set; }
		public string UserID { get; set; }
		public bool Banned { get; set; }
		public string BanReason { get; set; }
		public List<string> Inventory { get; set; }
    }
}