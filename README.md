# [CS2] MVP-Sounds-GoldKingZ (1.0.5)

### Custom MVP Sounds (Custom MVP Sounds + Vips)

![mainmenu2](https://github.com/oqyh/cs2-MVP-Sounds-GoldKingZ/assets/48490385/aa764efe-1405-4736-be35-01f66b26d68f)


![mvp](https://github.com/oqyh/cs2-MVP-Sounds-GoldKingZ/assets/48490385/485ad4a0-3d3e-476e-adad-4d825061c579)


## .:[ Dependencies ]:.
[Metamod:Source (2.x)](https://www.sourcemm.net/downloads.php/?branch=master)

[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases)

[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)

[MySqlConnector](https://www.nuget.org/packages/MySqlConnector)

## .:[ Configuration ]:.

> [!CAUTION]
> Config Located In ..\addons\counterstrikesharp\plugins\MVP-Sounds-GoldKingZ\config\config.json                                           
>

```json
{
  //Enable MySql? Located In MVP-Sounds-GoldKingZ/config/MySql_Settings.json
  "MVP_UseMySql": false,

  //true: Will Disable All Mvps
  //false: Will Disable Only Who Have Custom MVP
  "MVP_ForceDisableDefaultMVP_ToAll": false,

  //Change MVP Display Menu From Chat To Centre?
  "MVP_ChangeMVPMenuFromChatToCentre": true,

  //Toggle In Game To Show MVP 
  "MVP_InGameMenu": "!mvp,!mvps,!mvpsound",

  //First/Default Depend SteamID64 If He Have Music Kit Selected, This Will Get Ignored
  "MVP_DefaultMusicKitPerSteam": {
    "76561198206086993": "MVP_2",
    "76561198974936845": "MVP_3"
  },

  //Only Allow These Groups To Have Access To MVP_InGameMenu (Making Empty "" Means Everyone Has Access) [ex of groups: "@css/root,@css/admin,#css/admin"]
  "MVP_OnlyAllowTheseGroupsCanMVP": "",

  //Only Allow These Groups To Disable MVP Client Side (Making Empty "" Means Everyone Has Access) [ex of groups: "@css/root,@css/admin,#css/admin"]
  "MVP_OnlyAllowTheseGroupsCanToggleOffMVP": "@css/root,@css/admin,@css/vip,#css/admin,#css/vip",

  //Auto Delete Inactive Players Cookies Older Than X Days plugins/MVP-Sounds-GoldKingZ/Cookies/MVP_Sounds_Cookies.json
  "MVP_AutoRemovePlayerCookieOlderThanXDays": 7,

  //Auto Delete Inactive Players Cookies Older Than X Days In MySql
  "MVP_AutoRemovePlayerMySqlOlderThanXDays": 7,
}
```


## .:[ Configuration MVP ]:.

> [!CAUTION]
> Config Located In ..\addons\counterstrikesharp\plugins\MVP-Sounds-GoldKingZ\config\MVP_Settings.json                                          
>

> [!NOTE]
> Download https://github.com/Source2ZE/MultiAddonManager  With Gold KingZ WorkShop                                                                                                                               
> https://steamcommunity.com/sharedfiles/filedetails/?id=3244740528                                                                                                                             
> mm_extra_addons 3244740528                                                                                                                        
> You Can Find WorkShop Path Sounds In  https://github.com/oqyh/cs2-MVP-Sounds-GoldKingZ/blob/main/sounds/Gold%20KingZ%20WorkShop%20Sounds.txt                                                                                                                           
                                                                                                                       
```
"CanBePreview" = Make Item PreviewAble Before Select

"FLAGS" = Depend Who Access To The Items [ex of groups: @css/root,@css/admin,#css/admin] 

"HIDDEN" = Will Make Only Who Has Access "FLAGS" Can See The Item

"Message_Chat" = Print Chat In Lang "now.playing.chat"

"Message_Center" = Print Center In Lang "now.playing.centre"

"Message_Center_Bottom" = Print Center Bottom In Lang "now.playing.centre.bottom"

"Custom_Message" = Will Switch All Messages To Separate Print In Lang "MVP_X.now.playing" Depend MVP Kit Name

"MVP_Kit_Name" = Music Kit Display Name

"Sound_Path_X" = Music Path Start With 1 Put As Many As You Like  And It Will Play Randomly With No Duplicates 
```


![colors](https://github.com/oqyh/cs2-MVP-Sounds-GoldKingZ/assets/48490385/ba02c700-8e0b-4ebe-bc28-103b796c0b2e)



## .:[ Language ]:.
```json
{
	//==========================
	//        Colors
	//==========================
	//{Yellow} {Gold} {Silver} {Blue} {DarkBlue} {BlueGrey} {Magenta} {LightRed}
	//{LightBlue} {Olive} {Lime} {Red} {Purple} {Grey}
	//{Default} {White} {Darkred} {Green} {LightYellow}
	//==========================
	//        Other
	//==========================
	//<br> = Next Line On Center Hud 
	//{nextline} = Print On Next Line
	//==========================
	
	
	"player.not.allowed": "{green}Gold KingZ {grey}| {darkred}MVP Menu Is For {lime}VIPS {darkred}Only",
	"player.disabled.not.allowed": "{green}Gold KingZ {grey}| {darkred}Disable MVP Is For {lime}VIPS {darkred}Only",
	"player.not.allowed.musickit.flag": "{green}Gold KingZ {grey}| This Music Kit For {darkred}Vips Only",
	"player.preview": "{green}Gold KingZ {grey}| Playing {purple}{0} {grey}For You Only",
	"player.musickit.selected": "{green}Gold KingZ {grey}| You Select Music Kit {purple}{0}",
	"player.musickit.disabled": "{green}Gold KingZ {grey}| MVP Music Now {darkred}Disabled",
	"player.musickit.enabled": "{green}Gold KingZ {grey}| MVP Music Now {lime}Enabled",
	"player.musickit.remove": "{green}Gold KingZ {grey}| Your Custom MVP Sounds is Now {darkred}Removed",
	
	
	"now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
	"now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <font color='white'>Playing <font color='green'>{1} </font>",
	"now.playing.centre.bottom": "{0} Is MVP Of The Match! Playing   {1}",
	
	"MVP_11.now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP Of The Match !!!!!!! {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
	"MVP_11.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/meandyou.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
	
	"MVP_12.now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP Of The Match !!!!!!! {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
	"MVP_12.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/king.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
	
	"MVP_13.now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP Of The Match !!!!!!! {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
	"MVP_13.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/soldiers.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
	
	"MVP_14.now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP Of The Match !!!!!!! {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
	"MVP_14.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/melody.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
	
	"MVP_15.now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP Of The Match !!!!!!! {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
	"MVP_15.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/skull1.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
	
	"MVP_16.now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP Of The Match !!!!!!! {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
	"MVP_16.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/9mm.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
	
	"MVP_17.now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP Of The Match !!!!!!! {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
	"MVP_17.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/skull2.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
	"MVP_17.now.playing.centre.bottom": "{0} Is MVP Of The Match! Playing   {1}",
	
	"menu.music": ".:[ Music Menu ]:.",
	"menu.are.you.sure": "You Want [ {0} ] ?",
	"menu.answer.yes": "Yes",
	"menu.answer.no": "No, Preview It First",
	"menu.back": "-> Back",
	"menu.disabled": "Disabled All MVP Sounds",
	"menu.enabled": "Enabled All MVP Sounds",
	"menu.remove": "Remove Your Custom MVP Sound"
}
```


## .:[ Change Log ]:.
```
(1.0.5)
-Remove MVP_VVipMusicKit 
-Remove MVP_AdminMusicKit 
-Remove "VVIP" To Music Kit
-Remove "ADMIN" To Music Kit
-Added "FLAGS" To Music Kit

(1.0.4)
-Fix Sync Music To All
-Added MVP_VVipMusicKit 
-Added MVP_AdminMusicKit 
-Added MVP_DefaultMusicKitPerSteam 
-Added MVP_OnlyAllowTheseGroupsCanToggleOffMVP
-Added "HIDDEN" To Music Kit
-Added "VVIP" To Music Kit
-Added "ADMIN" To Music Kit
-Added Extra Music In Workshop 3244740528

(1.0.3)
-Fix Crash
-Fix Message Centre and  Centre Bottom Not Shown To Dead People

(1.0.2)
-Added MVP_AutoRemovePlayerMySqlOlderThanXDays
-Added Back In Menu "menu.back"

(1.0.1)
-Added MVP_ChangeMVPMenuFromChatToCentre

(1.0.0)
-Initial Release
```

## .:[ Donation ]:.

If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/oQYh)
