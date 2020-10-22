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
        public Dictionary<string, DateTime> Cooldowns { get; private set; }
        public UnityEngine.Color MessageColor { get; private set; }

        protected override void Load()
        {
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);
            Cooldowns = new Dictionary<string, DateTime>();
            SteamChannel.onTriggerSend += OnTriggerSend;
            Logger.Log($"{Assembly.GetName().Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        private void OnTriggerSend(SteamPlayer player, string name, ESteamCall mode, ESteamPacket type, params object[] arguments)
        {
            if (name == "tellGesture")
            {
                var up = UnturnedPlayer.FromSteamPlayer(player);
                var id = int.Parse(arguments[0].ToString());
                if (id == 8)
                {
                    if (!Configuration.Instance.UsePermissions || up.HasPermission("pickpocket"))
                    {
                        Player victimPlayer = RaycastHelper.GetPlayerFromHits(up.Player, Configuration.Instance.MaxDistance);

                        if (victimPlayer != null)
                        {
                            if (Cooldowns.TryGetValue(up.Id.ToString(), out DateTime expireDate) && expireDate > DateTime.Now)
                            {
                                UnturnedChat.Say(up.CSteamID, Translate("COOLDOWN", System.Math.Truncate((expireDate - DateTime.Now).TotalSeconds)), MessageColor);
                            }
                            else
                            {
                                if (expireDate != null)
                                    Cooldowns.Remove(up.Id);
                                UnturnedPlayer victim = UnturnedPlayer.FromPlayer(victimPlayer);
                                if (victim.HasPermission("bypass.pickpocket"))
                                {
                                    UnturnedChat.Say(up, Translate("BYPASS"), MessageColor);
                                    return;
                                }
                                PickpocketComponent comp = up.Player.gameObject.AddComponent<PickpocketComponent>();
                                comp.Initialize(up, victim, this);
                                Cooldowns.Add(up.Id, DateTime.Now.AddSeconds(Configuration.Instance.PickpocketCooldown));
                            }
                        }
                    }
                }
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"SUCCESS","You successfully robbed {0}"},
            {"NOTIFY_SUCCESS","You were robbed by {0}!"},
            {"FAIL","You failed to rob {0}"},
            {"NOTIFY_FAIL","{0} tried to rob you!"},
            {"NOTHING","{0} had nothing to steal!"},
            {"COOLDOWN","You have to wait {0} seconds before you can pickpocket again"},
            {"NOTIFY_POLICE","{0} stole {1}({2}) from {3}"},
            {"BYPASS", "This player cannot be robbed"}
        };

        protected override void Unload()
        {
            Logger.Log($"{Assembly.GetName().Name} has been unloaded!", ConsoleColor.Yellow);
            SteamChannel.onTriggerSend -= OnTriggerSend;
        }
    }
}
