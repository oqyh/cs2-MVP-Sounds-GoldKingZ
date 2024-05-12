using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Localization;

namespace MVP_Sounds_GoldKingZ.Config
{
    public static class Configs
    {
        public static class Shared {
            public static string? CookiesModule { get; set; }
            public static IStringLocalizer? StringLocalizer { get; set; }
        }
        
        private static readonly string ConfigDirectoryName = "config";
        private static readonly string ConfigFileName = "config.json";
        private static readonly string jsonFilePath = "MVP_Settings.json";
        private static readonly string jsonFilePath2 = "MySql_Settings.json";
        private static string? _jsonFilePath;
        private static string? _jsonFilePath2;
        private static string? _configFilePath;
        private static ConfigData? _configData;

        private static readonly JsonSerializerOptions SerializationOptions = new()
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        public static bool IsLoaded()
        {
            return _configData is not null;
        }

        public static ConfigData GetConfigData()
        {
            if (_configData is null)
            {
                throw new Exception("Config not yet loaded.");
            }
            
            return _configData;
        }

        public static ConfigData Load(string modulePath)
        {
            var configFileDirectory = Path.Combine(modulePath, ConfigDirectoryName);
            if(!Directory.Exists(configFileDirectory))
            {
                Directory.CreateDirectory(configFileDirectory);
            }
            _jsonFilePath = Path.Combine(configFileDirectory, jsonFilePath);
            Helper.CreateDefaultWeaponsJson(_jsonFilePath);

            _jsonFilePath2 = Path.Combine(configFileDirectory, jsonFilePath2);
            Helper.CreateDefaultWeaponsJson2(_jsonFilePath2);

            _configFilePath = Path.Combine(configFileDirectory, ConfigFileName);
            if (File.Exists(_configFilePath))
            {
                _configData = JsonSerializer.Deserialize<ConfigData>(File.ReadAllText(_configFilePath), SerializationOptions);
            }
            else
            {
                _configData = new ConfigData();
            }

            if (_configData is null)
            {
                throw new Exception("Failed to load configs.");
            }

            SaveConfigData(_configData);
            
            return _configData;
        }

        private static void SaveConfigData(ConfigData configData)
        {
            if (_configFilePath is null)
            {
                throw new Exception("Config not yet loaded.");
            }
            string json = JsonSerializer.Serialize(configData, SerializationOptions);

            json = "// Download https://github.com/Source2ZE/MultiAddonManager  With Gold KingZ WorkShop \n// https://steamcommunity.com/sharedfiles/filedetails/?id=3244740528\n// mm_extra_addons 3244740528\n// You Can Find WorkShop Path Sound In  https://github.com/oqyh/cs2-MVP-Sounds-GoldKingZ/blob/main/sounds/Gold%20KingZ%20WorkShop%20Sounds.txt \n\n" + json;
            File.WriteAllText(_configFilePath, json);
        }

        public class ConfigData
        {
            public bool MVP_UseMySql { get; set; }
            public bool MVP_ForceDisableDefaultMVP_ToAll { get; set; }
            public bool MVP_ChangeMVPMenuFromChatToCentre { get; set; }
            public string MVP_InGameMenu { get; set; }
            public string MVP_VipMusicKit { get; set; }
            public string MVP_OnlyAllowTheseGroupsCanMVP { get; set; }
            public int MVP_AutoRemovePlayerCookieOlderThanXDays { get; set; }
            public int MVP_AutoRemovePlayerMySqlOlderThanXDays { get; set; }
            public string empty { get; set; }
            public string Information_For_You_Dont_Delete_it { get; set; }
            
            public ConfigData()
            {
                MVP_UseMySql = false;
                MVP_ForceDisableDefaultMVP_ToAll = false;
                MVP_ChangeMVPMenuFromChatToCentre = true;
                MVP_InGameMenu = "!mvp,!mvps,!mvpsound";
                MVP_VipMusicKit = "@css/root,@css/admin,@css/vip,#css/admin,#css/vip";
                MVP_OnlyAllowTheseGroupsCanMVP = "";
                MVP_AutoRemovePlayerCookieOlderThanXDays = 7;
                MVP_AutoRemovePlayerMySqlOlderThanXDays = 7;
                empty = "-----------------------------------------------------------------------------------";
                Information_For_You_Dont_Delete_it = " Vist  [https://github.com/oqyh/cs2-MVP-Sounds-GoldKingZ/tree/main?tab=readme-ov-file#-configuration-] To Understand All Above";
            }
        }
    }
}