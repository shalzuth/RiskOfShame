using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfShame
{
    public class ChatNotified : MonoBehaviour { }
    public class ItemNotification : MonoBehaviour
    {
        private void Update()
        {
            var droppedItems = Object.FindObjectsOfType<RoR2.PickupDropletController>();
            foreach (var item in droppedItems)
            {
                var itemGo = item.transform.gameObject;
                if (itemGo.GetComponent<ChatNotified>() == null)
                {
                    itemGo.AddComponent<ChatNotified>();
                    RoR2.Chat.AddMessage(RoR2.Language.GetString(item.NetworkpickupIndex.GetPickupNameToken()) + " dropped!");
                    //var localUser = RoR2.LocalUserManager.GetFirstLocalUser();
                    //RoR2.Chat.SendBroadcastChat(new RoR2.Chat.UserChatMessage { sender = localUser.cachedMasterObject, text = item.NetworkpickupIndex + " dropped!" });
                }
            }
        }
    }
}
