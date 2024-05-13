using System.Text;
using MySqlConnector;
using Newtonsoft.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using MVP_Sounds_GoldKingZ.Config;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;

namespace MVP_Sounds_GoldKingZ;

public class MVPSoundsGoldKingZ : BasePlugin
{
    public override string ModuleName => "Custom MVP Sounds (Custom MVP Sounds + Vips)";
    public override string ModuleVersion => "1.0.4";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
    internal static IStringLocalizer? Stringlocalizer;
    private CounterStrikeSharp.API.Modules.Timers.Timer? HUDTimer_Center;
    private CounterStrikeSharp.API.Modules.Timers.Timer? HUDTimer_Center_Bottom;
	

    public override void Load(bool hotReload)
    {
        Configs.Load(ModuleDirectory);
        Stringlocalizer = Localizer;
        Configs.Shared.CookiesModule = ModuleDirectory;
        Configs.Shared.StringLocalizer = Localizer;
        RegisterEventHandler<EventRoundMvp>(OnEventRoundMvp,HookMode.Pre);
        RegisterEventHandler<EventPlayerChat>(OnEventPlayerChat, HookMode.Post);
        RegisterEventHandler<EventPlayerConnectFull>(OnEventPlayerConnectFull);
        RegisterListener<Listeners.OnTick>(OnTick);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
    }
    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;

        HUDTimer_Center?.Kill();
        HUDTimer_Center = null;
        HUDTimer_Center_Bottom?.Kill();
        HUDTimer_Center_Bottom = null;
        
        Globals.Show_Center = false;
        Globals.Show_Center_Bottom = false;

        return HookResult.Continue;
    }

    public HookResult OnEventPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (@event == null)return HookResult.Continue;
        var player = @event.Userid;

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;
        var playerid = player.SteamID;
        foreach (var kvp in Configs.GetConfigData().MVP_DefaultMusicKitPerSteam)
        {
            if (kvp.Key.ToString() == playerid.ToString())
            {
                string defaultmusickit = kvp.Value;
                if (!Globals.Choosed_MVP.ContainsKey(player.SteamID))
                {
                    Globals.Choosed_MVP.Add(player.SteamID, defaultmusickit);
                }
                if (Globals.Choosed_MVP.ContainsKey(player.SteamID))
                {
                    Globals.Choosed_MVP[player.SteamID] = defaultmusickit;
                }
                break;
            }
        }
        
        if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_OnlyAllowTheseGroupsCanMVP) && Helper.IsPlayerInGroupPermission(player, Configs.GetConfigData().MVP_OnlyAllowTheseGroupsCanMVP))
        {
            if (!Globals.allow_groups.ContainsKey(playerid))
            {
                Globals.allow_groups.Add(playerid, true);
            }
        }

        if(!string.IsNullOrEmpty(Configs.GetConfigData().MVP_VipMusicKit) && Helper.IsPlayerInGroupPermission(player, Configs.GetConfigData().MVP_VipMusicKit))
        {
            if (!Globals.vip_Kit.ContainsKey(playerid))
            {
                Globals.vip_Kit.Add(playerid, true);
            }

        }
        if(!string.IsNullOrEmpty(Configs.GetConfigData().MVP_VVipMusicKit) && Helper.IsPlayerInGroupPermission(player, Configs.GetConfigData().MVP_VVipMusicKit))
        {
            if (!Globals.vvip_Kit.ContainsKey(playerid))
            {
                Globals.vvip_Kit.Add(playerid, true);
            }

        }
        if(!string.IsNullOrEmpty(Configs.GetConfigData().MVP_AdminMusicKit) && Helper.IsPlayerInGroupPermission(player, Configs.GetConfigData().MVP_AdminMusicKit))
        {
            if (!Globals.vip_Kit.ContainsKey(playerid))
            {
                Globals.vip_Kit.Add(playerid, true);
            }
            if (!Globals.vvip_Kit.ContainsKey(playerid))
            {
                Globals.vvip_Kit.Add(playerid, true);
            }
            if (!Globals.admin_Kit.ContainsKey(playerid))
            {
                Globals.admin_Kit.Add(playerid, true);
            }
        }
        if(!string.IsNullOrEmpty(Configs.GetConfigData().MVP_OnlyAllowTheseGroupsCanToggleOffMVP) && Helper.IsPlayerInGroupPermission(player, Configs.GetConfigData().MVP_OnlyAllowTheseGroupsCanToggleOffMVP))
        {
            if (!Globals.client_cantoggle.ContainsKey(playerid))
            {
                Globals.client_cantoggle.Add(playerid, true);
            }
        }


        Helper.PersonData personData = Helper.RetrievePersonDataById(playerid);

        if(!string.IsNullOrEmpty(personData.MusicKit))
        {
            if (!Globals.Choosed_MVP.ContainsKey(player.SteamID))
            {
                Globals.Choosed_MVP.Add(player.SteamID, personData.MusicKit);
            }
            if (Globals.Choosed_MVP.ContainsKey(player.SteamID))
            {
                Globals.Choosed_MVP[player.SteamID] = personData.MusicKit;
            }
        }

        if(personData.Client_Mute_MVP)
        {
            if (!Globals.client_mute.ContainsKey(player.SteamID))
            {
                Globals.client_mute.Add(player.SteamID, true);
            }
            if (Globals.client_mute.ContainsKey(player.SteamID))
            {
                Globals.client_mute[player.SteamID] = true;
            }
        }


        if(Configs.GetConfigData().MVP_UseMySql)
        {
            async Task PerformDatabaseOperationAsync()
            {
                try
                {
                    var connectionSettings = JsonConvert.DeserializeObject<MySqlDataManager.MySqlConnectionSettings>(await File.ReadAllTextAsync(Path.Combine(Path.Combine(ModuleDirectory, "config"), "MySql_Settings.json")));
                    var connectionString = new MySqlConnectionStringBuilder
                    {
                        Server = connectionSettings!.MySqlHost,
                        Port = (uint)connectionSettings.MySqlPort,
                        Database = connectionSettings.MySqlDatabase,
                        UserID = connectionSettings.MySqlUsername,
                        Password = connectionSettings.MySqlPassword
                    }.ConnectionString;

                    using (var connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        var personDataz = await MySqlDataManager.RetrievePersonDataByIdAsync(playerid, connection);
                        if (personDataz.PlayerSteamID != 0)
                        {
                            DateTime personDate = DateTime.Now;
                            if(!string.IsNullOrEmpty(personDataz.MusicKit))
                            {
                                if (!Globals.Choosed_MVP.ContainsKey(player.SteamID))
                                {
                                    Globals.Choosed_MVP.Add(player.SteamID, personDataz.MusicKit);
                                }
                                if (Globals.Choosed_MVP.ContainsKey(player.SteamID))
                                {
                                    Globals.Choosed_MVP[player.SteamID] = personDataz.MusicKit;
                                }
                            }
                            if(personDataz.Client_Mute_MVP)
                            {
                                if (!Globals.client_mute.ContainsKey(player.SteamID))
                                {
                                    Globals.client_mute.Add(player.SteamID, true);
                                }
                                if (Globals.client_mute.ContainsKey(player.SteamID))
                                {
                                    Globals.client_mute[player.SteamID] = true;
                                }
                            }
                            Helper.SaveToJsonFile(player.SteamID, personDataz.MusicKit!, personDataz.Client_Mute_MVP, DateTime.Now);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"======================== ERROR =============================");
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"======================== ERROR =============================");
                }
            }

            Task.Run(PerformDatabaseOperationAsync);
        }

        
        
        return HookResult.Continue;
    }
    public HookResult OnEventPlayerChat(EventPlayerChat @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        var eventplayer = @event.Userid;
        var eventmessage = @event.Text;
        var Player = Utilities.GetPlayerFromUserid(eventplayer);
        

        if (Player == null || !Player.IsValid)return HookResult.Continue;
        
        var PlayerTeam = Player.TeamNum;
        var PlayerSteamID = Player.SteamID;
        Helper.PersonData personData = Helper.RetrievePersonDataById(PlayerSteamID);

        if (string.IsNullOrWhiteSpace(eventmessage)) return HookResult.Continue;
        string trimmedMessageStart = eventmessage.TrimStart();
        string message = trimmedMessageStart.TrimEnd();
        string[] MVPInGameMenu = Configs.GetConfigData().MVP_InGameMenu.Split(',');
        if (MVPInGameMenu.Any(cmd => cmd.Equals(message, StringComparison.OrdinalIgnoreCase)))
        {
            if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_OnlyAllowTheseGroupsCanMVP) && !Globals.allow_groups.ContainsKey(PlayerSteamID))
            {
                if (!string.IsNullOrEmpty(Localizer["player.not.allowed"]))
                {
                    Helper.AdvancedPrintToChat(Player, Localizer["player.not.allowed"]);
                }
                return HookResult.Continue;
            }
            
            try
            {
                string jsonFilePath = Path.Combine(ModuleDirectory, "../../plugins/MVP-Sounds-GoldKingZ/config/MVP_Settings.json");
                string jsonData = File.ReadAllText(jsonFilePath);
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonData);
                if (data == null) return HookResult.Continue;
                IMenu VoteGameModeMenu;
                if (Configs.GetConfigData().MVP_ChangeMVPMenuFromChatToCentre)
                {
                    VoteGameModeMenu = new CenterHtmlMenu(Localizer["menu.music"]);
                }
                else
                {
                    VoteGameModeMenu = new ChatMenu(Localizer["menu.music"]);
                }
                if(personData.Client_Mute_MVP)
                {
                    VoteGameModeMenu.AddMenuOption(Localizer["menu.enabled"], (Player, option) => HandleMenu(Player, option, string.Empty, false, false, false, false));
                }else
                {
                    VoteGameModeMenu.AddMenuOption(Localizer["menu.disabled"], (Player, option) => HandleMenu(Player, option, string.Empty, false, false, false, false));
                }
                VoteGameModeMenu.AddMenuOption(Localizer["menu.remove"], (Player, option) => HandleMenu(Player, option, string.Empty, false, false, false, false));
                foreach (var key in data.Keys)
                {
                    string ChoosenKitKey = data[key]["MVP_Kit_Name"];
                    bool isVIP = data[key].ContainsKey("VIP") && bool.TryParse(data[key]["VIP"].ToString(), out bool vipValue) ? vipValue : false;
                    bool isVVIP = data[key].ContainsKey("VVIP") && bool.TryParse(data[key]["VVIP"].ToString(), out bool vvipValue) ? vvipValue : false;
                    bool isADMIN = data[key].ContainsKey("ADMIN") && bool.TryParse(data[key]["ADMIN"].ToString(), out bool adminValue) ? adminValue : false;
                    bool isPreviewAble = data[key].ContainsKey("CanBePreview") && bool.TryParse(data[key]["CanBePreview"].ToString(), out bool PreviewValue) ? PreviewValue : false;
                    bool isHiddenItem = data[key].ContainsKey("HIDDEN") && bool.TryParse(data[key]["HIDDEN"].ToString(), out bool isHiddenItemValue) ? isHiddenItemValue : false;
                    if(isHiddenItem && isVIP && !Globals.vip_Kit.ContainsKey(Player.SteamID))continue;
                    if(isHiddenItem && isVVIP && !Globals.vvip_Kit.ContainsKey(Player.SteamID))continue;
                    if(isHiddenItem && isADMIN && !Globals.admin_Kit.ContainsKey(Player.SteamID))continue;
                    VoteGameModeMenu.AddMenuOption(ChoosenKitKey, (Player, option) => HandleMenu(Player, option, key, isVIP, isVVIP, isADMIN, isPreviewAble));
                }
                if (Configs.GetConfigData().MVP_ChangeMVPMenuFromChatToCentre)
                {
                    if (VoteGameModeMenu is CenterHtmlMenu centerHtmlMenu)
                    {
                        MenuManager.OpenCenterHtmlMenu(this, Player, centerHtmlMenu);
                    }
                }
                else
                {
                   if (VoteGameModeMenu is ChatMenu chatMenu)
                    {
                        MenuManager.OpenChatMenu(Player, chatMenu);
                    }
                }
                
            }catch{}
        }
        return HookResult.Continue;
    }
    
    private void HandleMenu(CCSPlayerController Player, ChatMenuOption option, string ChoosenKitKey, bool isVIP, bool isVVIP, bool isADMIN, bool isPreviewAble)
    {
        Helper.PersonData personData = Helper.RetrievePersonDataById(Player.SteamID);
        var disabled = option.Text;
        if(disabled == Localizer["menu.enabled"])
        {
            Globals.client_mute.Remove(Player.SteamID);
            personData.Client_Mute_MVP = !personData.Client_Mute_MVP;
            if(personData.Client_Mute_MVP)
            {

            }else
            {
                Helper.SaveToJsonFile(Player.SteamID, personData.MusicKit!, personData.Client_Mute_MVP, DateTime.Now);
            }
            
            if(Configs.GetConfigData().MVP_UseMySql)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var connectionSettings = JsonConvert.DeserializeObject<MySqlDataManager.MySqlConnectionSettings>(await File.ReadAllTextAsync(Path.Combine(Path.Combine(ModuleDirectory, "config"), "MySql_Settings.json")));
                        var connectionString = new MySqlConnectionStringBuilder
                        {
                            Server = connectionSettings!.MySqlHost,
                            Port = (uint)connectionSettings.MySqlPort,
                            Database = connectionSettings.MySqlDatabase,
                            UserID = connectionSettings.MySqlUsername,
                            Password = connectionSettings.MySqlPassword
                        }.ConnectionString;
                        
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            await connection.OpenAsync();
                            await MySqlDataManager.CreatePersonDataTableIfNotExistsAsync(connection);

                            DateTime personDate = DateTime.Now;
                            var personData = Helper.RetrievePersonDataById(Player.SteamID);
                            if (personData.PlayerSteamID != 0)
                            {
                                await MySqlDataManager.SaveToMySqlAsync(Player.SteamID, personData.MusicKit!, false, personDate, connection, connectionSettings);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"======================== ERROR =============================");
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        Console.WriteLine($"======================== ERROR =============================");
                    }
                });
            }
            if (!string.IsNullOrEmpty(Localizer["player.musickit.enabled"]))
            {
                Helper.AdvancedPrintToChat(Player, Localizer["player.musickit.enabled"], option.Text);
            }
            MenuManager.CloseActiveMenu(Player);
            return;
        }
        if(disabled == Localizer["menu.disabled"])
        {
            if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_OnlyAllowTheseGroupsCanToggleOffMVP) && !Globals.client_cantoggle.ContainsKey(Player.SteamID))
            {
                if (!string.IsNullOrEmpty(Localizer["player.disabled.not.allowed"]))
                {
                    Helper.AdvancedPrintToChat(Player, Localizer["player.disabled.not.allowed"]);
                }
                MenuManager.CloseActiveMenu(Player);
                return;
            }
            if (!Globals.client_mute.ContainsKey(Player.SteamID))
            {
                Globals.client_mute.Add(Player.SteamID, true);
            }
            if (Globals.client_mute.ContainsKey(Player.SteamID))
            {
                Globals.client_mute[Player.SteamID] = true;
            }
            
            personData.Client_Mute_MVP = !personData.Client_Mute_MVP;
            if(personData.Client_Mute_MVP)
            {
                Helper.SaveToJsonFile(Player.SteamID, personData.MusicKit!, personData.Client_Mute_MVP, DateTime.Now);
            }else
            {
                
            }
            if(Configs.GetConfigData().MVP_UseMySql)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var connectionSettings = JsonConvert.DeserializeObject<MySqlDataManager.MySqlConnectionSettings>(await File.ReadAllTextAsync(Path.Combine(Path.Combine(ModuleDirectory, "config"), "MySql_Settings.json")));
                        var connectionString = new MySqlConnectionStringBuilder
                        {
                            Server = connectionSettings!.MySqlHost,
                            Port = (uint)connectionSettings.MySqlPort,
                            Database = connectionSettings.MySqlDatabase,
                            UserID = connectionSettings.MySqlUsername,
                            Password = connectionSettings.MySqlPassword
                        }.ConnectionString;
                        
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            await connection.OpenAsync();
                            await MySqlDataManager.CreatePersonDataTableIfNotExistsAsync(connection);

                            DateTime personDate = DateTime.Now;
                            var personData = Helper.RetrievePersonDataById(Player.SteamID);
                            if (personData.PlayerSteamID != 0)
                            {
                                await MySqlDataManager.SaveToMySqlAsync(Player.SteamID, personData.MusicKit!, true, personDate, connection, connectionSettings);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"======================== ERROR =============================");
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        Console.WriteLine($"======================== ERROR =============================");
                    }
                });
            }
            if (!string.IsNullOrEmpty(Localizer["player.musickit.disabled"]))
            {
                Helper.AdvancedPrintToChat(Player, Localizer["player.musickit.disabled"], option.Text);
            }
            MenuManager.CloseActiveMenu(Player);
            return;
        }

        if(disabled == Localizer["menu.remove"])
        {
            Globals.Choosed_MVP.Remove(Player.SteamID);
            
            Helper.SaveToJsonFile(Player.SteamID, disabled, personData.Client_Mute_MVP, DateTime.Now);
            if(Configs.GetConfigData().MVP_UseMySql)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var connectionSettings = JsonConvert.DeserializeObject<MySqlDataManager.MySqlConnectionSettings>(await File.ReadAllTextAsync(Path.Combine(Path.Combine(ModuleDirectory, "config"), "MySql_Settings.json")));
                        var connectionString = new MySqlConnectionStringBuilder
                        {
                            Server = connectionSettings!.MySqlHost,
                            Port = (uint)connectionSettings.MySqlPort,
                            Database = connectionSettings.MySqlDatabase,
                            UserID = connectionSettings.MySqlUsername,
                            Password = connectionSettings.MySqlPassword
                        }.ConnectionString;
                        
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            await MySqlDataManager.RemoveFromMySqlAsync(Player.SteamID, connection, connectionSettings);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while removing data from MySQL: {ex.Message}");
                    }
                });
            }
            if (!string.IsNullOrEmpty(Localizer["player.musickit.remove"]))
            {
                Helper.AdvancedPrintToChat(Player, Localizer["player.musickit.remove"], option.Text);
            }
            MenuManager.CloseActiveMenu(Player);
            return;
        }
        
        if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_VipMusicKit) && !Globals.vip_Kit.ContainsKey(Player.SteamID) && isVIP && !isPreviewAble)
        {
            if (!string.IsNullOrEmpty(Localizer["player.not.allowed.musickit.vip"]))
            {
                Helper.AdvancedPrintToChat(Player, Localizer["player.not.allowed.musickit.vip"]);
            }
            MenuManager.CloseActiveMenu(Player);
            return;
        }
        if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_VVipMusicKit) && !Globals.vvip_Kit.ContainsKey(Player.SteamID) && isVVIP && !isPreviewAble)
        {
            if (!string.IsNullOrEmpty(Localizer["player.not.allowed.musickit.vvip"]))
            {
                Helper.AdvancedPrintToChat(Player, Localizer["player.not.allowed.musickit.vvip"]);
            }
            MenuManager.CloseActiveMenu(Player);
            return;
        }
        if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_AdminMusicKit) && !Globals.admin_Kit.ContainsKey(Player.SteamID) && isADMIN && !isPreviewAble)
        {
            if (!string.IsNullOrEmpty(Localizer["player.not.allowed.musickit.admin"]))
            {
                Helper.AdvancedPrintToChat(Player, Localizer["player.not.allowed.musickit.admin"]);
            }
            MenuManager.CloseActiveMenu(Player);
            return;
        }
        if(isPreviewAble)
        {
            var choosedkit =  option.Text;
            
            IMenu VoteGameModeMenu;
            if (Configs.GetConfigData().MVP_ChangeMVPMenuFromChatToCentre)
            {
                VoteGameModeMenu = new CenterHtmlMenu(Localizer["menu.are.you.sure", choosedkit]);
            }
            else
            {
                VoteGameModeMenu = new ChatMenu(Localizer["menu.are.you.sure", choosedkit]);
            }

            string[] answers = { Localizer["menu.answer.yes"], Localizer["menu.answer.no"]};

            foreach (string answer in answers)
            {
                VoteGameModeMenu.AddMenuOption(answer, (Player, option) => HandleMenu2(Player, option, ChoosenKitKey, isVIP, isVVIP, isADMIN, isPreviewAble, choosedkit));
            }
            VoteGameModeMenu.AddMenuOption(Localizer["menu.back"], (Player, option) => HandleMenu2(Player, option, ChoosenKitKey, isVIP, isVVIP, isADMIN, isPreviewAble, choosedkit));
            if (Configs.GetConfigData().MVP_ChangeMVPMenuFromChatToCentre)
            {
                if (VoteGameModeMenu is CenterHtmlMenu centerHtmlMenu)
                {
                    MenuManager.OpenCenterHtmlMenu(this, Player, centerHtmlMenu);
                }
            }
            else
            {
                if (VoteGameModeMenu is ChatMenu chatMenu)
                {
                    MenuManager.OpenChatMenu(Player, chatMenu);
                }
            }
            return;
        }
        if (!Globals.Choosed_MVP.ContainsKey(Player.SteamID))
        {
            Globals.Choosed_MVP.Add(Player.SteamID, ChoosenKitKey);
        }
        if (Globals.Choosed_MVP.ContainsKey(Player.SteamID))
        {
            Globals.Choosed_MVP[Player.SteamID] = ChoosenKitKey;
        }
        if (!string.IsNullOrEmpty(Localizer["player.musickit.selected"]))
        {
            Helper.AdvancedPrintToChat(Player, Localizer["player.musickit.selected"], option.Text);
        }

        Helper.SaveToJsonFile(Player.SteamID, ChoosenKitKey, personData.Client_Mute_MVP, DateTime.Now);
        MenuManager.CloseActiveMenu(Player);
    }
    private void HandleMenu2(CCSPlayerController Player, ChatMenuOption option, string ChoosenKitKey, bool isVIP, bool isVVIP, bool isADMIN, bool isPreviewAble, string choosedkit)
    {
        Helper.PersonData personData = Helper.RetrievePersonDataById(Player.SteamID);
        if(option.Text == Localizer["menu.answer.yes"])
        {
            if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_VipMusicKit) && !Globals.vip_Kit.ContainsKey(Player.SteamID) && isVIP)
            {
                if (!string.IsNullOrEmpty(Localizer["player.not.allowed.musickit.vip"]))
                {
                    Helper.AdvancedPrintToChat(Player, Localizer["player.not.allowed.musickit.vip"]);
                }
                MenuManager.CloseActiveMenu(Player);
                return;
            }
            if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_VVipMusicKit) && !Globals.vvip_Kit.ContainsKey(Player.SteamID) && isVVIP)
            {
                if (!string.IsNullOrEmpty(Localizer["player.not.allowed.musickit.vvip"]))
                {
                    Helper.AdvancedPrintToChat(Player, Localizer["player.not.allowed.musickit.vvip"]);
                }
                MenuManager.CloseActiveMenu(Player);
                return;
            }
            if (!string.IsNullOrEmpty(Configs.GetConfigData().MVP_AdminMusicKit) && !Globals.admin_Kit.ContainsKey(Player.SteamID) && isADMIN)
            {
                if (!string.IsNullOrEmpty(Localizer["player.not.allowed.musickit.admin"]))
                {
                    Helper.AdvancedPrintToChat(Player, Localizer["player.not.allowed.musickit.admin"]);
                }
                MenuManager.CloseActiveMenu(Player);
                return;
            }
            if (!Globals.Choosed_MVP.ContainsKey(Player.SteamID))
            {
                Globals.Choosed_MVP.Add(Player.SteamID, ChoosenKitKey);
            }
            if (Globals.Choosed_MVP.ContainsKey(Player.SteamID))
            {
                Globals.Choosed_MVP[Player.SteamID] = ChoosenKitKey;
            }
            if (!string.IsNullOrEmpty(Localizer["player.musickit.selected"]))
            {
                Helper.AdvancedPrintToChat(Player, Localizer["player.musickit.selected"], choosedkit);
            }
            Helper.SaveToJsonFile(Player.SteamID, ChoosenKitKey, personData.Client_Mute_MVP, DateTime.Now);
            MenuManager.CloseActiveMenu(Player);
        }else if(option.Text == Localizer["menu.answer.no"])
        {
            string MVPKitChoosen = ChoosenKitKey;
            try
            {
                string jsonPath = Path.Combine(ModuleDirectory, "../../plugins/MVP-Sounds-GoldKingZ/config/MVP_Settings.json");
                string jsonContent = File.ReadAllText(jsonPath);
                dynamic mvpSettings = JsonConvert.DeserializeObject(jsonContent)!;
                var chosenKit = mvpSettings[MVPKitChoosen];
                string kitName = chosenKit["MVP_Kit_Name"];

                List<string> soundPaths = new List<string>();
                int soundPathCount = 0;
                while (chosenKit[$"Sound_Path_{soundPathCount + 1}"] != null)
                {
                    soundPathCount++;
                }

                for (int i = 1; i <= soundPathCount; i++)
                {
                    string soundPathKey = $"Sound_Path_{i}";
                    if (chosenKit[soundPathKey] != null)
                    {
                        string soundPathValue = chosenKit[soundPathKey];
                        soundPaths.Add(soundPathValue);
                    }
                }

                Random rng = new Random();
                string soundPath = soundPaths.FirstOrDefault(path => !Globals.playedPaths.Contains(path))!;
                
                if (Globals.playedPaths.Count == soundPaths.Count)
                {
                    Globals.playedPaths.Clear();
                }
                if (Globals.playedPaths.Count == 0)
                {
                    soundPaths = soundPaths.OrderBy(x => rng.Next()).ToList();
                }
                if (soundPath == null || Globals.playedPaths.Count == 0)
                {
                    soundPath = soundPaths.FirstOrDefault(path => !Globals.playedPaths.Contains(path))!;
                }

                if (soundPath != null)
                {
                    if (!string.IsNullOrEmpty(Localizer["player.preview"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["player.preview"], choosedkit);
                    }
                    Player.ExecuteClientCommand("play " + soundPath);
                    Globals.playedPaths.Add(soundPath);
                }
            }
            catch { }
        }else if(option.Text == Localizer["menu.back"])
        {
            try
            {
                string jsonFilePath = Path.Combine(ModuleDirectory, "../../plugins/MVP-Sounds-GoldKingZ/config/MVP_Settings.json");
                string jsonData = File.ReadAllText(jsonFilePath);
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonData);
                if (data == null) return;
                IMenu VoteGameModeMenu;
                if (Configs.GetConfigData().MVP_ChangeMVPMenuFromChatToCentre)
                {
                    VoteGameModeMenu = new CenterHtmlMenu(Localizer["menu.music"]);
                }
                else
                {
                    VoteGameModeMenu = new ChatMenu(Localizer["menu.music"]);
                }
                if(personData.Client_Mute_MVP)
                {
                    VoteGameModeMenu.AddMenuOption(Localizer["menu.enabled"], (Player, option) => HandleMenu(Player, option, string.Empty, false, false, false, false));
                }else
                {
                    VoteGameModeMenu.AddMenuOption(Localizer["menu.disabled"], (Player, option) => HandleMenu(Player, option, string.Empty, false, false, false, false));
                }
                
                VoteGameModeMenu.AddMenuOption(Localizer["menu.remove"], (Player, option) => HandleMenu(Player, option, string.Empty, false, false, false, false));
                foreach (var key in data.Keys)
                {
                    string ChoosenKitKeyy = data[key]["MVP_Kit_Name"];
                    bool isVIPs = data[key].ContainsKey("VIP") && bool.TryParse(data[key]["VIP"].ToString(), out bool vipValues) ? vipValues : false;
                    bool isVVIPs = data[key].ContainsKey("VVIP") && bool.TryParse(data[key]["VVIP"].ToString(), out bool vvipValues) ? vvipValues : false;
                    bool isADMINs = data[key].ContainsKey("ADMIN") && bool.TryParse(data[key]["ADMIN"].ToString(), out bool adminValues) ? adminValues : false;
                    bool isPreviewAbles = data[key].ContainsKey("CanBePreview") && bool.TryParse(data[key]["CanBePreview"].ToString(), out bool PreviewValues) ? PreviewValues : false;
                    bool isHiddenItems = data[key].ContainsKey("HIDDEN") && bool.TryParse(data[key]["HIDDEN"].ToString(), out bool isHiddenItemValues) ? isHiddenItemValues : false;
                    if(isHiddenItems && isVIPs && !Globals.vip_Kit.ContainsKey(Player.SteamID))continue;
                    if(isHiddenItems && isVVIPs && !Globals.vvip_Kit.ContainsKey(Player.SteamID))continue;
                    if(isHiddenItems && isADMINs && !Globals.admin_Kit.ContainsKey(Player.SteamID))continue;
                    VoteGameModeMenu.AddMenuOption(ChoosenKitKeyy, (Player, option) => HandleMenu(Player, option, key, isVIPs, isVVIPs, isADMINs, isPreviewAbles));
                }
                if (Configs.GetConfigData().MVP_ChangeMVPMenuFromChatToCentre)
                {
                    if (VoteGameModeMenu is CenterHtmlMenu centerHtmlMenu)
                    {
                        MenuManager.OpenCenterHtmlMenu(this, Player, centerHtmlMenu);
                    }
                }
                else
                {
                   if (VoteGameModeMenu is ChatMenu chatMenu)
                    {
                        MenuManager.OpenChatMenu(Player, chatMenu);
                    }
                }
                
            }catch{}
        }
    }

    private HookResult OnEventRoundMvp(EventRoundMvp @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        var Player = @event.Userid;
        if (Player == null || !Player.IsValid)return HookResult.Continue;
        var PlayerSteamID = Player.SteamID;

        if(Configs.GetConfigData().MVP_ForceDisableDefaultMVP_ToAll)
        {   var allplayerz = Helper.GetAllController();
            allplayerz.ForEach(playerz =>
            {
                if (playerz != null && playerz.IsValid && !playerz.IsBot)
                {
                    playerz.MVPs = 0;
                }
            });
        }

        if (Globals.Choosed_MVP.ContainsKey(PlayerSteamID))
        {
            Player.MVPs = 0;
            string MVPKitChoosen = Globals.Choosed_MVP[PlayerSteamID];

            try
            {
                string jsonPath = Path.Combine(ModuleDirectory, "../../plugins/MVP-Sounds-GoldKingZ/config/MVP_Settings.json");
                string jsonContent = File.ReadAllText(jsonPath);
                dynamic mvpSettings = JsonConvert.DeserializeObject(jsonContent)!;
                var chosenKit = mvpSettings[MVPKitChoosen];
                string kitName = chosenKit["MVP_Kit_Name"];
                Globals.MVP_PlayerName = Player.PlayerName;
                Globals.MVP_KitName = kitName;
                bool custommessage = chosenKit["Custom_Message"] != null ? (bool)chosenKit["Custom_Message"] : false;
                bool messageChatEnabled = chosenKit["Message_Chat"] != null ? (bool)chosenKit["Message_Chat"] : false;

                bool messageCenterEnabled = chosenKit["Message_Center"] != null ? (bool)chosenKit["Message_Center"] : false;
                float messageCenterInSecs = chosenKit["Message_Center_InSecs"] != null ? (float)chosenKit["Message_Center_InSecs"] : 10;

                bool messageCenterBottomEnabled = chosenKit["Message_Center_Bottom"] != null ? (bool)chosenKit["Message_Center_Bottom"] : false;
                float messageCenterBottomInSecs = chosenKit["Message_Center_Bottom_InSecs"] != null ? (float)chosenKit["Message_Center_Bottom_InSecs"] : 10;

                List<string> soundPaths = new List<string>();
                int soundPathCount = 0;
                while (chosenKit[$"Sound_Path_{soundPathCount + 1}"] != null)
                {
                    soundPathCount++;
                }

                for (int i = 1; i <= soundPathCount; i++)
                {
                    string soundPathKey = $"Sound_Path_{i}";
                    if (chosenKit[soundPathKey] != null)
                    {
                        string soundPathValue = chosenKit[soundPathKey];
                        soundPaths.Add(soundPathValue);
                    }
                }

                Random rng = new Random();
                string soundPath = soundPaths.FirstOrDefault(path => !Globals.playedPaths.Contains(path))!;
                
                if (Globals.playedPaths.Count == soundPaths.Count)
                {
                    Globals.playedPaths.Clear();
                }
                if (Globals.playedPaths.Count == 0)
                {
                    soundPaths = soundPaths.OrderBy(x => rng.Next()).ToList();
                }
                if (soundPath == null || Globals.playedPaths.Count == 0)
                {
                    soundPath = soundPaths.FirstOrDefault(path => !Globals.playedPaths.Contains(path))!;
                }
                
                var allplayers = Helper.GetAllController();
                allplayers.ForEach(players =>
                {
                    if (players != null && players.IsValid && !players.IsBot && !Globals.client_mute.ContainsKey(players.SteamID))
                    {
                        if (soundPath != null)
                        {
                            
                            if(custommessage)
                            {
                                if(messageChatEnabled)
                                {
                                    if (!string.IsNullOrEmpty(Localizer[MVPKitChoosen + ".now.playing.chat"]))
                                    {
                                        Helper.AdvancedPrintToChat(players, Localizer[MVPKitChoosen + ".now.playing.chat"], Player.PlayerName, kitName);
                                    }
                                }
                                

                                if(messageCenterEnabled)
                                {
                                    Globals.Show_Center = true;
                                    Globals.MVP_Locaraize_Center = Localizer[MVPKitChoosen + ".now.playing.centre", Globals.MVP_PlayerName, Globals.MVP_KitName];
                                    HUDTimer_Center?.Kill();
                                    HUDTimer_Center = null;
                                    HUDTimer_Center = AddTimer(messageCenterInSecs, HUDTimer_Center_Callback, TimerFlags.STOP_ON_MAPCHANGE);
                                }

                                if(messageCenterBottomEnabled)
                                {
                                    Globals.Show_Center_Bottom = true;
                                    Globals.MVP_Locaraize_Center_Bottom = Localizer[MVPKitChoosen + ".now.playing.centre.bottom", Globals.MVP_PlayerName, Globals.MVP_KitName];
                                    HUDTimer_Center_Bottom?.Kill();
                                    HUDTimer_Center_Bottom = null;
                                    HUDTimer_Center_Bottom = AddTimer(messageCenterBottomInSecs, HUDTimer_Center_Bottom_Callback, TimerFlags.STOP_ON_MAPCHANGE);
                                }
                            }else
                            {
                                if(messageChatEnabled)
                                {
                                    if (!string.IsNullOrEmpty(Localizer["now.playing.chat"]))
                                    {
                                        Helper.AdvancedPrintToChat(players, Localizer["now.playing.chat"], Player.PlayerName, kitName);
                                    }
                                }
                                

                                if(messageCenterEnabled)
                                {
                                    Globals.Show_Center = true;
                                    Globals.MVP_Locaraize_Center = Localizer["now.playing.centre", Globals.MVP_PlayerName, Globals.MVP_KitName];
                                    HUDTimer_Center?.Kill();
                                    HUDTimer_Center = null;
                                    HUDTimer_Center = AddTimer(messageCenterInSecs, HUDTimer_Center_Callback, TimerFlags.STOP_ON_MAPCHANGE);
                                }

                                if(messageCenterBottomEnabled)
                                {
                                    Globals.Show_Center_Bottom = true;
                                    Globals.MVP_Locaraize_Center_Bottom = Localizer["now.playing.centre.bottom", Globals.MVP_PlayerName, Globals.MVP_KitName];
                                    HUDTimer_Center_Bottom?.Kill();
                                    HUDTimer_Center_Bottom = null;
                                    HUDTimer_Center_Bottom = AddTimer(messageCenterBottomInSecs, HUDTimer_Center_Bottom_Callback, TimerFlags.STOP_ON_MAPCHANGE);
                                }
                            }
                            players.ExecuteClientCommand("play " + soundPath);
                            Globals.playedPaths.Add(soundPath);
                        }
                    }
                });
                
            }
            catch { }
        }
        return HookResult.Continue;
    }
    private void HUDTimer_Center_Callback()
    {
        Globals.Show_Center = false;
    }
    private void HUDTimer_Center_Bottom_Callback()
    {
        Globals.Show_Center_Bottom = false;
    }
    public void OnTick()
    {
        if(Globals.Show_Center || Globals.Show_Center_Bottom)
        {
            foreach (var player in Helper.GetAllController())
            {
                if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || Globals.client_mute.ContainsKey(player.SteamID)) continue;
                if(Globals.Show_Center)
                {
                    StringBuilder builder = new StringBuilder();
                    string Show_Center = Globals.MVP_Locaraize_Center;
                    builder.AppendLine(Show_Center);
                    builder.AppendLine("</div>");
                    var centerhtml = builder.ToString();
                    player.PrintToCenterHtml(centerhtml);
                }
                if(Globals.Show_Center_Bottom)
                {
                    StringBuilder builder = new StringBuilder();
                    string Show_Center_bottom = Globals.MVP_Locaraize_Center_Bottom;
                    builder.Append(Show_Center_bottom);
                    var centerhtml = builder.ToString();
                    player.PrintToCenter(centerhtml);
                }
            }
        }
    }


    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;
        var player = @event.Userid;
        
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;
        var playerid = player.SteamID;
        Helper.PersonData personData = Helper.RetrievePersonDataById(playerid);
        DateTime personDate = DateTime.Now;

        Globals.allow_groups.Remove(playerid);
        Globals.vip_Kit.Remove(playerid);
        Globals.vvip_Kit.Remove(playerid);
        Globals.admin_Kit.Remove(playerid);
        Globals.Choosed_MVP.Remove(playerid);
        Globals.client_mute.Remove(playerid);
        Globals.client_cantoggle.Remove(playerid);

        if(Configs.GetConfigData().MVP_UseMySql)
        {
            Task.Run(async () =>
            {
                try
                {
                    var connectionSettings = JsonConvert.DeserializeObject<MySqlDataManager.MySqlConnectionSettings>(await File.ReadAllTextAsync(Path.Combine(Path.Combine(ModuleDirectory, "config"), "MySql_Settings.json")));
                    var connectionString = new MySqlConnectionStringBuilder
                    {
                        Server = connectionSettings!.MySqlHost,
                        Port = (uint)connectionSettings.MySqlPort,
                        Database = connectionSettings.MySqlDatabase,
                        UserID = connectionSettings.MySqlUsername,
                        Password = connectionSettings.MySqlPassword
                    }.ConnectionString;
                    
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        await MySqlDataManager.CreatePersonDataTableIfNotExistsAsync(connection);

                        DateTime personDate = DateTime.Now;
                        var personData = Helper.RetrievePersonDataById(playerid);
                        if (personData.PlayerSteamID != 0)
                        {
                            await MySqlDataManager.SaveToMySqlAsync(playerid, personData.MusicKit!, personData.Client_Mute_MVP, personDate, connection, connectionSettings);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"======================== ERROR =============================");
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"======================== ERROR =============================");
                }
            });
        }

        return HookResult.Continue;
    }
    public void OnMapEnd()
    {
        Helper.ClearVariables();
    }
    public override void Unload(bool hotReload)
    {
        Helper.ClearVariables();
    }
}