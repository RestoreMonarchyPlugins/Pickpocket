using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.API;
using SDG.Unturned;
using RestoreMonarchy.Pickpocket.Components;
using UnityEngine;
using RestoreMonarchy.Pickpocket.Helpers;
using System.Collections.Generic;
using System;
using Rocket.Unturned.Chat;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.Pickpocket
{
    public class PickpocketPlugin : RocketPlugin<PickpocketConfiguration>
    {
        public Dictionary<string, DateTime> Cooldowns = new Dictionary<string, DateTime>();

        protected override void Load()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEvents_OnPlayerUpdateGesture;
            Logger.Log($"{Assembly.GetName().Name} has been loaded!");
            Logger.Log($"Version: {Assembly.GetName().Version}");
            Logger.Log($"Made by MCrow");
        }

        private void UnturnedPlayerEvents_OnPlayerUpdateGesture(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if (gesture == UnturnedPlayerEvents.PlayerGesture.Point)
            {
                 

                RocketPlayer rocketPlayer = new RocketPlayer(player.Id);
                if (!Configuration.Instance.UsePermissions || rocketPlayer.HasPermission("pickpocket"))
                {
                    Player victimPlayer = RaycastHelper.GetPlayerFromHits(player.Player,
                        RaycastHelper.RaycastAll(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), 5f, RayMasks.PLAYER_INTERACT));

                    if (victimPlayer != null)
                    {
                        if (Cooldowns.TryGetValue(player.Id, out DateTime expireDate) && expireDate > DateTime.Now)
                        {
                            UnturnedChat.Say(player.CSteamID, Translate("COOLDOWN", System.Math.Truncate((expireDate - DateTime.Now).TotalSeconds)));
                        }
                        else
                        {
                            if (expireDate != null)
                                Cooldowns.Remove(player.Id);

                            UnturnedPlayer victim = UnturnedPlayer.FromPlayer(victimPlayer);
                            if (victim.HasPermission("bypass.pickpocket"))
                                return;

                            PickpocketComponent comp = player.Player.gameObject.AddComponent<PickpocketComponent>();
                            comp.Initialize(player, victim, this);
                            Cooldowns.Add(player.Id, DateTime.Now.AddSeconds(Configuration.Instance.PickpocketCooldown));
                        }                        
                    }
                }             
            }            
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"SUCCESS","You successfully robbed {0}"},
                    {"NOTIFY_SUCCESS","You were robbed by {0}!"},
                    {"FAIL","You failed to rob {0}"},
                    {"NOTIFY_FAIL","{0} tried to rob you!"},
                    {"NOTHING","{0} had nothing to steal!"},
                    {"COOLDOWN","You have to wait {0} seconds before you can pickpocket again"},
                    {"NOTIFY_POLICE","{0} stole {1}({2}) from {3}"}
                };
            }
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= UnturnedPlayerEvents_OnPlayerUpdateGesture;
        }
    }
}
