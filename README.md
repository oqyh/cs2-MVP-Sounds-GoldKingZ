# [CS2] MVP-Sounds-GoldKingZ (1.0.0)

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

  //Toggle In Game To Show MVP 
  "MVP_InGameMenu": "!mvp,!mvps,!mvpsound",

  //VIP Music Kit in MVP_Settings.json "VIP"
  "MVP_VipMusicKit": "@css/root,@css/admin,@css/vip,#css/admin,#css/vip",

  //Only Allow These Groups To Have Access To MVP_InGameMenu (Making Empty "" Means Everyone Has Access) [ex of groups: "@css/root,@css/admin,#css/admin"]
  "MVP_OnlyAllowTheseGroupsCanMVP": "",

  //Auto Delete Inactive Players Cookies Older Than X Days plugins/MVP-Sounds-GoldKingZ/Cookies/MVP_Sounds_Cookies.json
  "MVP_AutoRemovePlayerCookieOlderThanXDays": 7,
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
  "VIP" : Make Item Vip Only (For Who In "MVP_VipMusicKit" in config.json)

  "CanBePreview" : Make Item Can Be Preview Before Selected
  
  "Custom_Message" : Switch To MVP_X.now.playing.centre For Custom Message
  "Message_Chat" : Message In Chat "now.playing.chat"
  "Message_Center" : Message In Center  "now.playing.centre"
  "Message_Center_Bottom" : Message In Center Bottom "now.playing.centre.bottom"

  "Message_Center_InSecs" : How Much Show "Message_Center" In Secs
  "Message_Center_Bottom_InSecs" : How Much Show "Message_Center_Bottom" In Secs 

  "MVP_Kit_Name" : Music Kit Name
  
  "Sound_Path_X" : You CaN Add Multiple Sounds In One Music Kit
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
  "player.not.allowed.musickit": "{green}Gold KingZ {grey}| This Music Kit For {darkred}Vips Only",
  "player.preview": "{green}Gold KingZ {grey}| Playing {purple}{0} {grey}For You Only",
  "player.musickit.selected": "{green}Gold KingZ {grey}| You Select Music Kit {purple}{0}",
  "player.musickit.disabled": "{green}Gold KingZ {grey}| MVP Music Kit Now {darkred}Disabled",
  
  
  "now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
  "now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <font color='white'>Playing <font color='green'>{1} </font>",
  "now.playing.centre.bottom": "{0} Is MVP Of The Match! Playing   {1}",
  
  "MVP_2.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/skull1.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
  
  "MVP_12.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/9mm.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
  
  "MVP_13.now.playing.chat": "{green}Gold KingZ {grey}| {lightblue}{0} {grey} Is MVP Of The Match !!!!!!! {nextline}{green}Gold KingZ {grey}| Now Playing {purple}{1}",
  "MVP_13.now.playing.centre": "<font color='purple'>{0} <font color='white'>Is MVP Of The Match! <br> <img src='https://raw.githubusercontent.com/oqyh/cs2-MVP-Sounds-GoldKingZ/main/Resources/skull2.gif' class=''> <br> <br> <font color='white'>Playing <font color='green'>{1} </font>",
  "MVP_13.now.playing.centre.bottom": "{0} Is MVP Of The Match! Playing   {1}",
  
  "menu.music": ".:[ Music Menu ]:.",
  "menu.are.you.sure": "You Want [ {0} ] ?",
  "menu.answer.yes": "Yes",
  "menu.answer.no": "No, Preview It First",
  "menu.disabled": "Disabled/Remove MVP"
}
```


## .:[ Change Log ]:.
```
(1.0.0)
-Initial Release
```

## .:[ Donation ]:.

If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/oQYh)
