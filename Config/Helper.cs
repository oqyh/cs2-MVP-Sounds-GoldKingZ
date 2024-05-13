using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text.Json;
using MVP_Sounds_GoldKingZ.Config;
using System.Text.Encodings.Web;

namespace MVP_Sounds_GoldKingZ;

public class Helper
{
    public static void AdvancedPrintToChat(CCSPlayerController player, string message, params object[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string messages = part.Trim();
                player.PrintToChat(" " + messages);
            }
        }else
        {
            player.PrintToChat(message);
        }
    }
    public static void AdvancedPrintToServer(string message, params object[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string messages = part.Trim();
                Server.PrintToChatAll(" " + messages);
            }
        }else
        {
            Server.PrintToChatAll(message);
        }
    }
    
    public static bool IsPlayerInGroupPermission(CCSPlayerController player, string groups)
    {
        var excludedGroups = groups.Split(',');
        foreach (var group in excludedGroups)
        {
            if (group.StartsWith("#"))
            {
                if (AdminManager.PlayerInGroup(player, group))
                    return true;
            }
            else if (group.StartsWith("@"))
            {
                if (AdminManager.PlayerHasPermissions(player, group))
                    return true;
            }
        }
        return false;
    }
    public static List<CCSPlayerController> GetCounterTerroristController() 
    {
        var playerList = Utilities.FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller").Where(p => p != null && p.IsValid && !p.IsBot && !p.IsHLTV && p.Connected == PlayerConnectedState.PlayerConnected && p.Team == CsTeam.CounterTerrorist).ToList();
        return playerList;
    }
    public static List<CCSPlayerController> GetTerroristController() 
    {
        var playerList = Utilities.FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller").Where(p => p != null && p.IsValid && !p.IsBot && !p.IsHLTV && p.Connected == PlayerConnectedState.PlayerConnected && p.Team == CsTeam.Terrorist).ToList();
        return playerList;
    }
    public static List<CCSPlayerController> GetAllController() 
    {
        var playerList = Utilities.FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller").Where(p => p != null && p.IsValid && !p.IsBot && !p.IsHLTV && p.Connected == PlayerConnectedState.PlayerConnected).ToList();
        return playerList;
    }
    public static int GetCounterTerroristCount()
    {
        return Utilities.GetPlayers().Count(p => p != null && p.IsValid && !p.IsBot && !p.IsHLTV && p.Connected == PlayerConnectedState.PlayerConnected && p.TeamNum == (byte)CsTeam.CounterTerrorist);
    }
    public static int GetTerroristCount()
    {
        return Utilities.GetPlayers().Count(p => p != null && p.IsValid && !p.IsBot && !p.IsHLTV && p.Connected == PlayerConnectedState.PlayerConnected && p.TeamNum == (byte)CsTeam.Terrorist);
    }
    public static int GetAllCount()
    {
        return Utilities.GetPlayers().Count(p => p != null && p.IsValid && !p.IsBot && !p.IsHLTV && p.Connected == PlayerConnectedState.PlayerConnected);
    }
    public static void ClearVariables()
    {
        Globals.vip_Kit.Clear();
        Globals.vvip_Kit.Clear();
        Globals.admin_Kit.Clear();
        Globals.playedPaths.Clear();
        Globals.Choosed_MVP.Clear();
        Globals.allow_groups.Clear();
        Globals.client_mute.Clear();
        Globals.client_cantoggle.Clear();
        Globals.Show_Center = false;
        Globals.Show_Center_Bottom = false;
    }
    
    public static string ReplaceMessages(string Message, string date, string time, string PlayerName, string SteamId, string ipAddress, string reason)
    {
        var replacedMessage = Message
                                    .Replace("{TIME}", time)
                                    .Replace("{DATE}", date)
                                    .Replace("{PLAYERNAME}", PlayerName.ToString())
                                    .Replace("{STEAMID}", SteamId.ToString())
                                    .Replace("{IP}", ipAddress.ToString())
                                    .Replace("{REASON}", reason);
        return replacedMessage;
    }
    public static string RemoveLeadingSpaces(string content)
    {
        string[] lines = content.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].TrimStart();
        }
        return string.Join("\n", lines);
    }
    private static CCSGameRules? GetGameRules()
    {
        try
        {
            var gameRulesEntities = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules");
            return gameRulesEntities.First().GameRules;
        }
        catch
        {
            return null;
        }
    }
    public static bool IsWarmup()
    {
        return GetGameRules()?.WarmupPeriod ?? false;
    }
    public static void CreateDefaultWeaponsJson(string jsonFilePath)
    {
        if (!File.Exists(jsonFilePath))
        {
            var configData = new Dictionary<string, Dictionary<string, object>>
            {
                ["MVP_1"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center_Bottom"] = true,
                    ["Message_Center_Bottom_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Дурной Вкус",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/badtaste.vsnd_c",
                    ["Sound_Path_2"] = "sounds/GoldKingZ/MVP/badtaste2.vsnd_c"
                },
                ["MVP_2"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Center_Bottom"] = true,
                    ["Message_Center_Bottom_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "History",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/history.vsnd_c"
                },
                ["MVP_3"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center_Bottom"] = true,
                    ["Message_Center_Bottom_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "RATATA",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/ratata.vsnd_c"
                },
                ["MVP_4"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center_Bottom"] = true,
                    ["Message_Center_Bottom_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "SWAT",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/swat.vsnd_c"
                },
                ["MVP_5"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center_Bottom"] = true,
                    ["Message_Center_Bottom_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Killin On Demand",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/demand.vsnd_c"
                },
                ["MVP_6"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Untitled 13",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/untitled13.vsnd_c"
                },
                ["MVP_7"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Paranoia",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/paranoia.vsnd_c"
                },
                ["MVP_8"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Lord Pretty Flacko",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/lordprettyflacko.vsnd_c",
                    ["Sound_Path_2"] = "sounds/GoldKingZ/MVP/lordprettyflacko2.vsnd_c"
                },
                ["MVP_9"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Who Got Beef With Me",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/beef.vsnd_c"
                },
                ["MVP_10"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Two Twelve Subwoofer",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/twotwelvesubwoofer.vsnd_c"
                },
                ["MVP_11"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Custom_Message"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Ты и Я",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/uandme.vsnd_c"
                },
                ["MVP_12"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Custom_Message"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Separate ways",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/Separate.vsnd_c",
                    ["Sound_Path_2"] = "sounds/GoldKingZ/MVP/Separate2.vsnd_c"
                },
                ["MVP_13"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Custom_Message"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "My Soldiers",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/soldiers.vsnd_c",
                    ["Sound_Path_2"] = "sounds/GoldKingZ/MVP/soldiers2.vsnd_c"
                },
                ["MVP_14"] = new Dictionary<string, object>
                {
                    ["CanBePreview"] = true,
                    ["Custom_Message"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "What Is That Melody",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/melody.vsnd_c"
                },
                ["MVP_15"] = new Dictionary<string, object>
                {
                    ["VIP"] = true,
                    ["CanBePreview"] = true,
                    ["Custom_Message"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "9mm (VIP)",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/9mm.vsnd_c"
                },
                ["MVP_16"] = new Dictionary<string, object>
                {
                    ["VVIP"] = true,
                    ["CanBePreview"] = true,
                    ["Custom_Message"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "Go Hard Huh (VVIP)",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/gohardhuh.vsnd_c"
                },
                ["MVP_17"] = new Dictionary<string, object>
                {
                    ["ADMIN"] = true,
                    ["HIDDEN"] = true,
                    ["CanBePreview"] = true,
                    ["Custom_Message"] = true,
                    ["Message_Chat"] = true,
                    ["Message_Center"] = true,
                    ["Message_Center_InSecs"] = 10,
                    ["Message_Center_Bottom"] = true,
                    ["Message_Center_Bottom_InSecs"] = 10,
                    ["MVP_Kit_Name"] = "☠ Final Message ☠ (ADMIN)",
                    ["Sound_Path_1"] = "sounds/GoldKingZ/MVP/finalmessage.vsnd_c",
                    ["Sound_Path_2"] = "sounds/GoldKingZ/MVP/finalmessage2.vsnd_c"
                }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = System.Text.Json.JsonSerializer.Serialize(configData, options);

            json = "// Download https://github.com/Source2ZE/MultiAddonManager  With Gold KingZ WorkShop \n// https://steamcommunity.com/sharedfiles/filedetails/?id=3244740528\n// mm_extra_addons 3244740528\n// You Can Find WorkShop Path Sound In  https://github.com/oqyh/cs2-MVP-Sounds-GoldKingZ/blob/main/sounds/Gold%20KingZ%20WorkShop%20Sounds.txt\n// Info: \n// 'CanBePreview' = Make Item PreviewAble Before Select\n// 'VIP','VVIP','ADMIN' = Depend Who Access To The Items in config.json\n// 'HIDDEN' = Will Make Only Who Has Access 'VIP','VVIP','ADMIN' Can See The Item\n// 'Message_Chat' = Print Chat In Lang 'now.playing.chat'\n// 'Message_Center' = Print Center In Lang 'now.playing.centre'\n// 'Message_Center_Bottom' = Print Center Bottom In Lang 'now.playing.centre.bottom'\n// 'Custom_Message' = Will Switch All Messages To Separate Print In Lang 'MVP_X.now.playing' Depend MVP Kit Name\n// 'MVP_Kit_Name' = Music Kit Display Name\n// 'Sound_Path_X' = Music Path Start With 1 Put As Many As You Like  And It Will Play Randomly With No Duplicates \n\n" + json;

            File.WriteAllText(jsonFilePath, json);
        }
    }
    public static void CreateDefaultWeaponsJson2(string jsonFilePath)
    {
        if (!File.Exists(jsonFilePath))
        {
            var configData = new Dictionary<string, object>
            {
                {"MySqlHost", "your_mysql_host"},
                {"MySqlDatabase", "your_mysql_database"},
                {"MySqlUsername", "your_mysql_username"},
                {"MySqlPassword", "your_mysql_password"},
                {"MySqlPort", 3306}
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = System.Text.Json.JsonSerializer.Serialize(configData, options);

            File.WriteAllText(jsonFilePath, json);
        }
    }
    public class PersonData
    {
        public ulong PlayerSteamID { get; set; }
        public string? MusicKit { get; set; }
        public bool Client_Mute_MVP { get; set; }
        public DateTime DateAndTime { get; set; }
    }
    public static void SaveToJsonFile(ulong PlayerSteamID, string MusicKit, bool Client_Mute_MVP, DateTime DateAndTime)
    {
        string Fpath = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/MVP-Sounds-GoldKingZ/Cookies/");
        string Fpathc = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/MVP-Sounds-GoldKingZ/Cookies/MVP_Sounds_Cookies.json");
        try
        {
            if (!Directory.Exists(Fpath))
            {
                Directory.CreateDirectory(Fpath);
            }

            if (!File.Exists(Fpathc))
            {
                File.WriteAllText(Fpathc, "[]");
            }

            List<PersonData> allPersonsData;
            string jsonData = File.ReadAllText(Fpathc);
            allPersonsData = JsonConvert.DeserializeObject<List<PersonData>>(jsonData) ?? new List<PersonData>();

            if (MusicKit == Configs.Shared.StringLocalizer!["menu.disabled"])
            {
                allPersonsData.RemoveAll(p => p.PlayerSteamID == PlayerSteamID);
            }
            else
            {
                PersonData existingPerson = allPersonsData.Find(p => p.PlayerSteamID == PlayerSteamID)!;

                if (existingPerson != null)
                {
                    existingPerson.MusicKit = MusicKit;
                    existingPerson.Client_Mute_MVP = Client_Mute_MVP;
                    existingPerson.DateAndTime = DateAndTime;
                }
                else
                {
                    PersonData newPerson = new PersonData
                    {
                        PlayerSteamID = PlayerSteamID,
                        MusicKit = MusicKit,
                        Client_Mute_MVP = Client_Mute_MVP,
                        DateAndTime = DateAndTime
                    };
                    allPersonsData.Add(newPerson);
                }
            }

            allPersonsData.RemoveAll(p => (DateTime.Now - p.DateAndTime).TotalDays > Configs.GetConfigData().MVP_AutoRemovePlayerCookieOlderThanXDays);

            string updatedJsonData = JsonConvert.SerializeObject(allPersonsData, Formatting.Indented);
            try
            {
                File.WriteAllText(Fpathc, updatedJsonData);
            }
            catch
            {
                // Handle exception
            }
        }
        catch
        {
            // Handle exception
        }
    }

    public static PersonData RetrievePersonDataById(ulong targetId)
    {
        string Fpath = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/MVP-Sounds-GoldKingZ/Cookies/");
        string Fpathc = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/MVP-Sounds-GoldKingZ/Cookies/MVP_Sounds_Cookies.json");
        try
        {
            if (File.Exists(Fpathc))
            {
                string jsonData = File.ReadAllText(Fpathc);
                List<PersonData> allPersonsData = JsonConvert.DeserializeObject<List<PersonData>>(jsonData) ?? new List<PersonData>();

                PersonData targetPerson = allPersonsData.Find(p => p.PlayerSteamID == targetId)!;

               
                if (targetPerson != null && (DateTime.Now - targetPerson.DateAndTime<= TimeSpan.FromDays(Configs.GetConfigData().MVP_AutoRemovePlayerCookieOlderThanXDays)))
                {
                    return targetPerson;
                }
                else if (targetPerson != null)
                {
                    allPersonsData.Remove(targetPerson);
                    string updatedJsonData = JsonConvert.SerializeObject(allPersonsData, Formatting.Indented);
                    try
                    {
                        File.WriteAllText(Fpathc, updatedJsonData);
                    }
                    catch
                    {
                        // Handle exception
                    }
                }
                
                
            }
        }
        catch
        {
            // Handle exception
        }
        return new PersonData();
    }
    
}