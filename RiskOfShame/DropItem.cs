using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace RiskOfShame
{
    public class DropItem : MonoBehaviour
    {
        const Int16 HandleId = 72;
        class DropItemPacket : MessageBase
        {
            public GameObject Player;
            public RoR2.ItemIndex ItemIndex;
            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(Player);
                writer.Write((UInt16)ItemIndex);
            }

            public override void Deserialize(NetworkReader reader)
            {
                Player = reader.ReadGameObject();
                ItemIndex = (RoR2.ItemIndex)reader.ReadUInt16();
            }
        }
        static void SendDropItem(GameObject player, RoR2.ItemIndex itemIndex)
        {
            NetworkServer.SendToAll(HandleId, new DropItemPacket
            {
                Player = player,
                ItemIndex = itemIndex
            });
        }
        [RoR2.Networking.NetworkMessageHandler(msgType = HandleId, client = true)]
        static void HandleDropItem(NetworkMessage netMsg)
        {
            var dropItem = netMsg.ReadMessage<DropItemPacket>();
            var body = dropItem.Player.GetComponent<RoR2.CharacterBody>();
            body.inventory.RemoveItem(dropItem.ItemIndex, 1);
            RoR2.PickupDropletController.CreatePickupDroplet(new RoR2.PickupIndex(dropItem.ItemIndex), body.transform.position + Vector3.up * 1.5f, Vector3.up * 20f + body.transform.forward * 2f);
        }
        class TooltipClick : MonoBehaviour, IPointerClickHandler
        {
            public void OnPointerClick(PointerEventData eventData)
            {
                var user = RoR2.LocalUserManager.GetFirstLocalUser(); var icon = gameObject.GetComponent<RoR2.UI.ItemIcon>();
                var itemIndex = icon.GetField<RoR2.ItemIndex>("itemIndex");
                RoR2.Chat.SendBroadcastChat(new RoR2.Chat.UserChatMessage { sender = user.currentNetworkUser.gameObject, text = RoR2.Language.GetString(new RoR2.PickupIndex(itemIndex).GetPickupNameToken()) + " dropped!" });
                SendDropItem(user.cachedBody.gameObject, itemIndex);
            }
        }
        void Update()
        {
            var networkClient = NetworkClient.allClients.FirstOrDefault();
            if (networkClient != null)
                networkClient.RegisterHandlerSafe(HandleId, HandleDropItem);
            var huds = typeof(RoR2.UI.HUD).GetField<List<RoR2.UI.HUD>>("instancesList");
            foreach (var hud in huds)
            {
                var items = hud.itemInventoryDisplay.GetField<List<RoR2.UI.ItemIcon>>("itemIcons");
                foreach (var item in items)
                {
                    var itemGo = item.transform.gameObject;
                    if (itemGo.GetComponent<TooltipClick>() == null)
                        item.transform.gameObject.AddComponent<TooltipClick>();
                }
            }
        }
        void OnDisable()
        {
            var huds = typeof(RoR2.UI.HUD).GetField<List<RoR2.UI.HUD>>("instancesList");
            foreach (var hud in huds)
            {
                var items = hud.itemInventoryDisplay.GetField<List<RoR2.UI.ItemIcon>>("itemIcons");
                foreach (var item in items)
                {
                    var itemGo = item.transform.gameObject;
                    var ttc = itemGo.GetComponent<TooltipClick>();
                    if (ttc != null)
                        Destroy(ttc);
                }
            }
        }
    }
}
