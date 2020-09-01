[![Version](https://img.shields.io/github/release/RestoreMonarchyPlugins/Pickpocket.svg)](https://github.com/RestoreMonarchyPlugins/Pickpocket/releases) ![Discord](https://discordapp.com/api/guilds/520355060312440853/widget.png)
## Pickpocket - RocketMod 4 Plugin
* Let your players steal items from others instead of killing
* Pickpocket must hover the cursor on the victim as long as it's configured to rob him 
* Manage who can rob and who cannot be robbed with bypass permission: `bypass.pickpocket`    
* You can set an alert for the police in the config, if pocket theft was committed

See the preview video of how plugin works on [Youtube](https://youtu.be/O_NKTCZmKEg)

You can buy custom extension features to this plugin just message us on [VK](https://vk.com/pluginsrestoremonarchy) or [Discord](https://discord.gg/yBztk3w)

There's also more advanced version of this plugin available to buy at [ImperialPlugins.com](https://imperialplugins.com/Products/ProductDetails/162)

### Default Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<PickpocketConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <PickpocketTime>2</PickpocketTime>
  <PickpocketCooldown>30</PickpocketCooldown>
  <NotifyVictimOnSuccess>false</NotifyVictimOnSuccess>
  <NotifyVictimOnFail>true</NotifyVictimOnFail>
  <UsePermissions>false</UsePermissions>
  <NotifyPolice>false</NotifyPolice>
  <PoliceGroupId>police</PoliceGroupId>
  <MaxDistance>5</MaxDistance>
  <UseBypass>true</UseBypass>
  <MessageColor>yellow</MessageColor>
</PickpocketConfiguration>
```

### Default Translation
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="SUCCESS" Value="You successfully robbed {0}" />
  <Translation Id="NOTIFY_SUCCESS" Value="You were robbed by {0}!" />
  <Translation Id="FAIL" Value="You failed to rob {0}" />
  <Translation Id="NOTIFY_FAIL" Value="{0} tried to rob you!" />
  <Translation Id="NOTHING" Value="{0} had nothing to steal!" />
  <Translation Id="COOLDOWN" Value="You have to wait {0} seconds before you can pickpocket again" />
  <Translation Id="NOTIFY_POLICE" Value="{0} stole {1}({2}) from {3}" />
  <Translation Id="BYPASS" Value="This player cannot be robbed" />
</Translations>
```
