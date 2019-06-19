using RestoreMonarchy.Pickpocket.Helpers;
using RestoreMonarchy.Pickpocket.Models;
using Rocket.API.Serialisation;
using Rocket.Core;
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
        private bool IsInitialized = false;

        public void Initialize(UnturnedPlayer pickpocket, UnturnedPlayer victim, PickpocketPlugin pluginInstance)
        {
            this.Pickpocket = pickpocket;
            this.Victim = victim;
            this.pluginInstance = pluginInstance;
            this.Timer = new Timer(pluginInstance.Configuration.Instance.PickpocketTime * 1000);
            Timer.AutoReset = false;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Disposed += Timer_Disposed;
            Timer.Start();

            this.IsInitialized = true;
        }

        void FixedUpdate()
        {
            if (IsInitialized)
            {
                Player victimPlayer = RaycastHelper.GetPlayerFromHits(Pickpocket.Player,
                        RaycastHelper.RaycastAll(new Ray(Pickpocket.Player.look.aim.position, Pickpocket.Player.look.aim.forward), 5f, RayMasks.PLAYER_INTERACT));

                if (victimPlayer != Victim.Player)
                {
                    Timer.Dispose();
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UnturnedChat.Say(Pickpocket.CSteamID, pluginInstance.Translate("SUCCESS", Victim.CharacterName));
            if (pluginInstance.Configuration.Instance.NotifyVictimOnSuccess)
                UnturnedChat.Say(Victim.CSteamID, pluginInstance.Translate("NOTIFY_SUCCESS", Pickpocket.CharacterName));

            System.Random random = new System.Random();

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
                UnturnedChat.Say(Pickpocket.CSteamID, pluginInstance.Translate("NOTHING", Victim.CharacterName));
            else
            {
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
                                inventoryItem.Item.interactableItem.asset.itemName, inventoryItem.Item.item.id, Victim.CharacterName), Color.red);
                        }
                    });
                }
            }

            Destroy(this);
        }


        private void Timer_Disposed(object sender, EventArgs e)
        {
            UnturnedChat.Say(Pickpocket.CSteamID, pluginInstance.Translate("FAIL", Victim.CharacterName));
            if (pluginInstance.Configuration.Instance.NotifyVictimOnFail)
                UnturnedChat.Say(Victim.CSteamID, pluginInstance.Translate("NOTIFY_FAIL", Pickpocket.CharacterName));
            
            Destroy(this);
        }

        void OnDestroy()
        {
            Timer.Elapsed -= Timer_Elapsed;
            Timer.Disposed -= Timer_Disposed;
        }

    }
}
