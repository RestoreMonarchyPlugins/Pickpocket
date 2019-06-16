using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using System;
using Rocket.Unturned.Player;
using Rocket.Core;
using Rocket.API;
using SDG.Unturned;
using RestoreMonarchy.Pickpocket.Components;
using UnityEngine;
using RestoreMonarchy.Pickpocket.Helpers;
using System.Linq;

namespace RestoreMonarchy.Pickpocket
{
    public class PickpocketPlugin : RocketPlugin<PickpocketConfiguration>
    {
        protected override void Load()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEvents_OnPlayerUpdateGesture;            
        }

        private void UnturnedPlayerEvents_OnPlayerUpdateGesture(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if (gesture == UnturnedPlayerEvents.PlayerGesture.Point)
            {                
                RocketPlayer rocketPlayer = new RocketPlayer(player.Id);
                if (rocketPlayer.HasPermission("pickpocket"))
                {
                    Player victimPlayer = RaycastHelper.GetPlayerFromHits(player.Player,
                        RaycastHelper.RaycastAll(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), 5f, RayMasks.PLAYER_INTERACT));

                    if (victimPlayer != null)
                    {
                        UnturnedPlayer victim = UnturnedPlayer.FromPlayer(victimPlayer);
                        if (victim.HasPermission("bypass.pickpocket"))
                            return;

                        PickpocketComponent comp = player.Player.gameObject.AddComponent<PickpocketComponent>();
                        comp.Initialize(player, victim, this);
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
                    {"NOTHING","{0} had nothing to steal!"}
                };
            }
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= UnturnedPlayerEvents_OnPlayerUpdateGesture;
        }
    }
}
