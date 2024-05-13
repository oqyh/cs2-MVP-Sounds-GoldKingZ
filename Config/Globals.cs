using CounterStrikeSharp.API.Core;
using System.Diagnostics;

namespace MVP_Sounds_GoldKingZ;

public class Globals
{
    public static bool Show_Center = false;
    public static bool Show_Center_Bottom = false;
    public static HashSet<string> playedPaths = new HashSet<string>();
    public static Dictionary<ulong, string> Choosed_MVP = new Dictionary<ulong, string>();
    public static Dictionary<ulong, bool> allow_groups = new Dictionary<ulong, bool>();
    public static Dictionary<ulong, bool> vip_Kit = new Dictionary<ulong, bool>();
    public static Dictionary<ulong, bool> vvip_Kit = new Dictionary<ulong, bool>();
    public static Dictionary<ulong, bool> admin_Kit = new Dictionary<ulong, bool>();
    public static Dictionary<ulong, bool> client_mute = new Dictionary<ulong, bool>();
    public static Dictionary<ulong, bool> client_cantoggle = new Dictionary<ulong, bool>();
    public static string MVP_PlayerName = "";
    public static string MVP_KitName = "";
    public static string MVP_Locaraize_Center = "";
    public static string MVP_Locaraize_Center_Bottom = "";
}