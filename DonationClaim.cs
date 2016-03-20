using System.Text.RegularExpressions;
using Oxide.Ext.MySql;
using System.Text;
using Oxide.Core;
using Oxide.Game.Rust.Libraries;
using Oxide.Core.Plugins;
using Oxide.Core;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace Oxide.Plugins

{
    [Info("Donation Claim", "LeoCurtss", "0.4")]
    [Description("Player can claim rewards from PayPal donations.")]

    class DonationClaim : RustPlugin
    {
	
	private readonly Ext.MySql.Libraries.MySql _mySql = Interface.GetMod().GetLibrary<Ext.MySql.Libraries.MySql>();
	private Ext.MySql.Connection _mySqlConnection;

	string MySQLIP => Config.Get<string>("MySQL IP");
	int MySQLPort => Config.Get<int>("MySQL Port");
	string MySQLDatabase => Config.Get<string>("MySQL Database");
	string MySQLusername => Config.Get<string>("MySQL username");
	string MySQLpassword => Config.Get<string>("MySQL password");
	string Package1ItemName => Config.Get<string>("Package 1 Item Name");
	string Package1ConsoleCommand01 => Config.Get<string>("Package 1 Console Command 01");
	string Package1ConsoleCommand02 => Config.Get<string>("Package 1 Console Command 02");
	string Package1ConsoleCommand03 => Config.Get<string>("Package 1 Console Command 03");
	string Package1ConsoleCommand04 => Config.Get<string>("Package 1 Console Command 04");
	string Package1ConsoleCommand05 => Config.Get<string>("Package 1 Console Command 05");
	string Package1ConsoleCommand06 => Config.Get<string>("Package 1 Console Command 06");
	string Package1ConsoleCommand07 => Config.Get<string>("Package 1 Console Command 07");
	string Package1ConsoleCommand08 => Config.Get<string>("Package 1 Console Command 08");
	string Package1ConsoleCommand09 => Config.Get<string>("Package 1 Console Command 09");
	string Package1ConsoleCommand10 => Config.Get<string>("Package 1 Console Command 10");
	string Package2ItemName => Config.Get<string>("Package 2 Item Name");
	string Package2ConsoleCommand01 => Config.Get<string>("Package 2 Console Command 01");
	string Package2ConsoleCommand02 => Config.Get<string>("Package 2 Console Command 02");
	string Package2ConsoleCommand03 => Config.Get<string>("Package 2 Console Command 03");
	string Package2ConsoleCommand04 => Config.Get<string>("Package 2 Console Command 04");
	string Package2ConsoleCommand05 => Config.Get<string>("Package 2 Console Command 05");
	string Package2ConsoleCommand06 => Config.Get<string>("Package 2 Console Command 06");
	string Package2ConsoleCommand07 => Config.Get<string>("Package 2 Console Command 07");
	string Package2ConsoleCommand08 => Config.Get<string>("Package 2 Console Command 08");
	string Package2ConsoleCommand09 => Config.Get<string>("Package 2 Console Command 09");
	string Package2ConsoleCommand10 => Config.Get<string>("Package 2 Console Command 10");
	string Package3ItemName => Config.Get<string>("Package 3 Item Name");
	string Package3ConsoleCommand01 => Config.Get<string>("Package 3 Console Command 01");
	string Package3ConsoleCommand02 => Config.Get<string>("Package 3 Console Command 02");
	string Package3ConsoleCommand03 => Config.Get<string>("Package 3 Console Command 03");
	string Package3ConsoleCommand04 => Config.Get<string>("Package 3 Console Command 04");
	string Package3ConsoleCommand05 => Config.Get<string>("Package 3 Console Command 05");
	string Package3ConsoleCommand06 => Config.Get<string>("Package 3 Console Command 06");
	string Package3ConsoleCommand07 => Config.Get<string>("Package 3 Console Command 07");
	string Package3ConsoleCommand08 => Config.Get<string>("Package 3 Console Command 08");
	string Package3ConsoleCommand09 => Config.Get<string>("Package 3 Console Command 09");
	string Package3ConsoleCommand10 => Config.Get<string>("Package 3 Console Command 10");
	string Package4ItemName => Config.Get<string>("Package 4 Item Name");
	string Package4ConsoleCommand01 => Config.Get<string>("Package 4 Console Command 01");
	string Package4ConsoleCommand02 => Config.Get<string>("Package 4 Console Command 02");
	string Package4ConsoleCommand03 => Config.Get<string>("Package 4 Console Command 03");
	string Package4ConsoleCommand04 => Config.Get<string>("Package 4 Console Command 04");
	string Package4ConsoleCommand05 => Config.Get<string>("Package 4 Console Command 05");
	string Package4ConsoleCommand06 => Config.Get<string>("Package 4 Console Command 06");
	string Package4ConsoleCommand07 => Config.Get<string>("Package 4 Console Command 07");
	string Package4ConsoleCommand08 => Config.Get<string>("Package 4 Console Command 08");
	string Package4ConsoleCommand09 => Config.Get<string>("Package 4 Console Command 09");
	string Package4ConsoleCommand10 => Config.Get<string>("Package 4 Console Command 10");
	string Package5ItemName => Config.Get<string>("Package 5 Item Name");
	string Package5ConsoleCommand01 => Config.Get<string>("Package 5 Console Command 01");
	string Package5ConsoleCommand02 => Config.Get<string>("Package 5 Console Command 02");
	string Package5ConsoleCommand03 => Config.Get<string>("Package 5 Console Command 03");
	string Package5ConsoleCommand04 => Config.Get<string>("Package 5 Console Command 04");
	string Package5ConsoleCommand05 => Config.Get<string>("Package 5 Console Command 05");
	string Package5ConsoleCommand06 => Config.Get<string>("Package 5 Console Command 06");
	string Package5ConsoleCommand07 => Config.Get<string>("Package 5 Console Command 07");
	string Package5ConsoleCommand08 => Config.Get<string>("Package 5 Console Command 08");
	string Package5ConsoleCommand09 => Config.Get<string>("Package 5 Console Command 09");
	string Package5ConsoleCommand10 => Config.Get<string>("Package 5 Console Command 10");

	
	protected override void LoadDefaultConfig()
	{
		PrintWarning("Creating a new configuration file.");
		Config.Clear();
		Config["MySQL IP"] = "localhost";
		Config["MySQL Port"] = 3306;
		Config["MySQL Database"] = "rustserver";
		Config["MySQL username"] = "";
		Config["MySQL password"] = "";
		Config["Package 1 Item Name"] = "--";
		Config["Package 1 Console Command 01"] = "";
		Config["Package 1 Console Command 02"] = "";
		Config["Package 1 Console Command 03"] = "";
		Config["Package 1 Console Command 04"] = "";
		Config["Package 1 Console Command 05"] = "";
		Config["Package 1 Console Command 06"] = "";
		Config["Package 1 Console Command 07"] = "";
		Config["Package 1 Console Command 08"] = "";
		Config["Package 1 Console Command 09"] = "";
		Config["Package 1 Console Command 10"] = "";
		Config["Package 2 Item Name"] = "--";
		Config["Package 2 Console Command 01"] = "";
		Config["Package 2 Console Command 02"] = "";
		Config["Package 2 Console Command 03"] = "";
		Config["Package 2 Console Command 04"] = "";
		Config["Package 2 Console Command 05"] = "";
		Config["Package 2 Console Command 06"] = "";
		Config["Package 2 Console Command 07"] = "";
		Config["Package 2 Console Command 08"] = "";
		Config["Package 2 Console Command 09"] = "";
		Config["Package 2 Console Command 10"] = "";
		Config["Package 3 Item Name"] = "--";
		Config["Package 3 Console Command 01"] = "";
		Config["Package 3 Console Command 02"] = "";
		Config["Package 3 Console Command 03"] = "";
		Config["Package 3 Console Command 04"] = "";
		Config["Package 3 Console Command 05"] = "";
		Config["Package 3 Console Command 06"] = "";
		Config["Package 3 Console Command 07"] = "";
		Config["Package 3 Console Command 08"] = "";
		Config["Package 3 Console Command 09"] = "";
		Config["Package 3 Console Command 10"] = "";
		Config["Package 4 Item Name"] = "--";
		Config["Package 4 Console Command 01"] = "";
		Config["Package 4 Console Command 02"] = "";
		Config["Package 4 Console Command 03"] = "";
		Config["Package 4 Console Command 04"] = "";
		Config["Package 4 Console Command 05"] = "";
		Config["Package 4 Console Command 06"] = "";
		Config["Package 4 Console Command 07"] = "";
		Config["Package 4 Console Command 08"] = "";
		Config["Package 4 Console Command 09"] = "";
		Config["Package 4 Console Command 10"] = "";
		Config["Package 5 Item Name"] = "--";
		Config["Package 5 Console Command 01"] = "";
		Config["Package 5 Console Command 02"] = "";
		Config["Package 5 Console Command 03"] = "";
		Config["Package 5 Console Command 04"] = "";
		Config["Package 5 Console Command 05"] = "";
		Config["Package 5 Console Command 06"] = "";
		Config["Package 5 Console Command 07"] = "";
		Config["Package 5 Console Command 08"] = "";
		Config["Package 5 Console Command 09"] = "";
		Config["Package 5 Console Command 10"] = "";
		
		SaveConfig();
	}

	void Loaded()
	{
		LoadConfig();
		
		//Lang API dictionary
		lang.RegisterMessages(new Dictionary<string,string>{
			["DC_NoUnclaimed"] = "There are no unclaimed rewards available for that email address: {0}",
			["DC_Claimed"] = "You have claimed the {0} donation package.  Thank you for your donation!"
		}, this);
	}

    private string GetMessage(string name, string sid = null) {
		return lang.GetMessage(name, this, sid);
	}
	
	[ChatCommand("claimreward")]
        void ClaimRewardCommand(BasePlayer player, string command, string[] args)
        {
            string playerEmail = string.Join("", args);
			
			playerEmail = playerEmail.Replace("@","@@");
			
			_mySqlConnection = _mySql.OpenDb(MySQLIP, MySQLPort, MySQLDatabase, MySQLusername, MySQLpassword, this);
			
			string packageClaimed = "";
			
		var sql = Ext.MySql.Sql.Builder.Append("CALL rustserver.claim_donation('" + playerEmail + "');");
		_mySql.Query(sql, _mySqlConnection, list =>
		
		{
			
			string playerName = Regex.Replace(player.displayName,"(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))","");
			
			var sb = new StringBuilder();
			foreach (var entry in list)
			{
				sb.AppendFormat("{0}", entry["item_name"]);
				sb.AppendLine();
			}
			
			packageClaimed = sb.ToString();
			
			if (packageClaimed.Length < 3) 
			{
				//SendReply(player, "There are no unclaimed rewards available for that email address. (" + playerEmail.Replace("@@","@") + ")");
				SendReply(player,string.Format(GetMessage("DC_NoUnclaimed",player.UserIDString),playerEmail.Replace("@@","@")));
			}
			
			if (packageClaimed.Contains(Package1ItemName))
			{
				if (Package1ConsoleCommand01 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand01,playerName));}
				if (Package1ConsoleCommand02 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand02,playerName));}
				if (Package1ConsoleCommand03 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand03,playerName));}
				if (Package1ConsoleCommand04 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand04,playerName));}
				if (Package1ConsoleCommand05 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand05,playerName));}
				if (Package1ConsoleCommand06 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand06,playerName));}
				if (Package1ConsoleCommand07 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand07,playerName));}
				if (Package1ConsoleCommand08 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand08,playerName));}
				if (Package1ConsoleCommand09 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand09,playerName));}
				if (Package1ConsoleCommand10 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package1ConsoleCommand10,playerName));}

				
				//SendReply(player, "You have claimed the " + packageClaimed + "donation package.  Thank you for your donation!");
				SendReply(player,string.Format(GetMessage("DC_Claimed",player.UserIDString),packageClaimed));
				Puts(player + " has claimed donation package " + packageClaimed);
			}

			if (packageClaimed.Contains(Package2ItemName))
			{
				if (Package2ConsoleCommand01 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand01,playerName));}
				if (Package2ConsoleCommand02 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand02,playerName));}
				if (Package2ConsoleCommand03 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand03,playerName));}
				if (Package2ConsoleCommand04 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand04,playerName));}
				if (Package2ConsoleCommand05 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand05,playerName));}
				if (Package2ConsoleCommand06 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand06,playerName));}
				if (Package2ConsoleCommand07 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand07,playerName));}
				if (Package2ConsoleCommand08 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand08,playerName));}
				if (Package2ConsoleCommand09 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand09,playerName));}
				if (Package2ConsoleCommand10 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package2ConsoleCommand10,playerName));}

				
				//SendReply(player, "You have claimed the " + packageClaimed + "donation package.  Thank you for your donation!");
				SendReply(player,string.Format(GetMessage("DC_Claimed",player.UserIDString),packageClaimed));
				Puts(player + " has claimed donation package " + packageClaimed);
			}
			
			if (packageClaimed.Contains(Package3ItemName))
			{
				if (Package3ConsoleCommand01 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand01,playerName));}
				if (Package3ConsoleCommand02 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand02,playerName));}
				if (Package3ConsoleCommand03 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand03,playerName));}
				if (Package3ConsoleCommand04 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand04,playerName));}
				if (Package3ConsoleCommand05 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand05,playerName));}
				if (Package3ConsoleCommand06 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand06,playerName));}
				if (Package3ConsoleCommand07 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand07,playerName));}
				if (Package3ConsoleCommand08 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand08,playerName));}
				if (Package3ConsoleCommand09 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand09,playerName));}
				if (Package3ConsoleCommand10 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package3ConsoleCommand10,playerName));}

				
				//SendReply(player, "You have claimed the " + packageClaimed + "donation package.  Thank you for your donation!");
				SendReply(player,string.Format(GetMessage("DC_Claimed",player.UserIDString),packageClaimed));
				Puts(player + " has claimed donation package " + packageClaimed);
			}
			
			if (packageClaimed.Contains(Package4ItemName))
			{
				if (Package4ConsoleCommand01 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand01,playerName));}
				if (Package4ConsoleCommand02 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand02,playerName));}
				if (Package4ConsoleCommand03 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand03,playerName));}
				if (Package4ConsoleCommand04 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand04,playerName));}
				if (Package4ConsoleCommand05 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand05,playerName));}
				if (Package4ConsoleCommand06 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand06,playerName));}
				if (Package4ConsoleCommand07 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand07,playerName));}
				if (Package4ConsoleCommand08 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand08,playerName));}
				if (Package4ConsoleCommand09 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand09,playerName));}
				if (Package4ConsoleCommand10 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package4ConsoleCommand10,playerName));}

				
				//SendReply(player, "You have claimed the " + packageClaimed + "donation package.  Thank you for your donation!");
				SendReply(player,string.Format(GetMessage("DC_Claimed",player.UserIDString),packageClaimed));
				Puts(player + " has claimed donation package " + packageClaimed);
			}
			
			if (packageClaimed.Contains(Package5ItemName))
			{
				if (Package5ConsoleCommand01 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand01,playerName));}
				if (Package5ConsoleCommand02 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand02,playerName));}
				if (Package5ConsoleCommand03 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand03,playerName));}
				if (Package5ConsoleCommand04 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand04,playerName));}
				if (Package5ConsoleCommand05 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand05,playerName));}
				if (Package5ConsoleCommand06 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand06,playerName));}
				if (Package5ConsoleCommand07 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand07,playerName));}
				if (Package5ConsoleCommand08 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand08,playerName));}
				if (Package5ConsoleCommand09 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand09,playerName));}
				if (Package5ConsoleCommand10 != "") {ConsoleSystem.Run.Server.Normal(String.Format(Package5ConsoleCommand10,playerName));}

				
				//SendReply(player, "You have claimed the " + packageClaimed + "donation package.  Thank you for your donation!");
				SendReply(player,string.Format(GetMessage("DC_Claimed",player.UserIDString),packageClaimed));
				Puts(player + " has claimed donation package " + packageClaimed);
			}
			
		});	

    }	
	
  }

}