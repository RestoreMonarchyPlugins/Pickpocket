using RestoreMonarchy.Pickpocket.Helpers;
using RestoreMonarchy.Pickpocket.Models;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace RestoreMonarchy.Pickpocket.Components
{
    public class PickpocketComponent : MonoBehaviour
    {
        private PickpocketPlugin pluginInstance;
        public Timer Timer { get; set; }
        public UnturnedPlayer Pickpocket { get; set; }
        public UnturnedPlayer Victim { get; set; }

        public void Initialize(UnturnedPlayer pickpocket, UnturnedPlayer victim, PickpocketPlugin pluginInstance)
        {
            this.pluginInstance = pluginInstance;
            Pickpocket = pickpocket;
            Victim = victim;            
            Timer = new Timer(pluginInstance.Configuration.Instance.PickpocketTime * 1000);
            Timer.AutoReset = false;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        void FixedUpdate()
        {
            if (Timer != null)
            {
                Player victimPlayer = RaycastHelper.GetPlayerFromHits(Pickpocket.Player, pluginInstance.Configuration.Instance.MaxDistance);

                if (victimPlayer != Victim.Player)
                {
                    UnturnedChat.Say(Pickpocket.CSteamID, pluginInstance.Translate("FAIL", Victim.CharacterName), pluginInstance.MessageColor);
                    if (pluginInstance.Configuration.Instance.NotifyVictimOnFail)
                        UnturnedChat.Say(Victim.CSteamID, pluginInstance.Translate("NOTIFY_FAIL", Pickpocket.CharacterName), pluginInstance.MessageColor);

                    Destroy(this);
                    Timer.Dispose();
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TaskDispatcher.QueueOnMainThread(() =>
            {
                UnturnedChat.Say(Pickpocket.CSteamID, pluginInstance.Translate("SUCCESS", Victim.CharacterName), pluginInstance.MessageColor);
                if (pluginInstance.Configuration.Instance.NotifyVictimOnSuccess)
                    UnturnedChat.Say(Victim.CSteamID, pluginInstance.Translate("NOTIFY_SUCCESS", Pickpocket.CharacterName), pluginInstance.MessageColor);

                List<InventoryItem> items = new List<InventoryItem>();
                for (byte page = 0; page < 6; page++)
                {
                    for (byte i = 0; i < Victim.Inventory.items[page].getItemCount(); i++)
                    {
                        if (Victim.Inventory.items[page].getItem(i) != null)
                            items.Add(new InventoryItem(Victim.Inventory.items[page].getItem(i), page, i));
                    }
                }

                if (items.Count <= 0)
                    UnturnedChat.Say(Pickpocket.CSteamID, pluginInstance.Translate("NOTHING", Victim.CharacterName), pluginInstance.MessageColor);
                else
                {
                    System.Random random = new System.Random();
                    var inventoryItem = items[random.Next(items.Count)];
                    Victim.Inventory.removeItem(inventoryItem.Page, inventoryItem.Index);
                    Pickpocket.Inventory.forceAddItem(inventoryItem.Item.item, true);

                    if (pluginInstance.Configuration.Instance.NotifyPolice)
                    {
                        RocketPermissionsGroup group = R.Permissions.GetGroup(pluginInstance.Configuration.Instance.PoliceGroupId);
                        Provider.clients.ForEach(client =>
                        {
                            if (group.Members.Contains(client.playerID.steamID.m_SteamID.ToString()) || client.isAdmin)
                            {
                                UnturnedChat.Say(client.playerID.steamID, pluginInstance.Translate("NOTIFY_POLICE", Pickpocket.CharacterName,
                                    inventoryItem.Item.interactableItem.asset.itemName, inventoryItem.Item.item.id, Victim.CharacterName), pluginInstance.MessageColor);
                            }
                        });
                    }
                }

                Destroy(this);
            });
        }

        void OnDestroy()
        {
            Timer.Elapsed -= Timer_Elapsed;
        }
    }
}
