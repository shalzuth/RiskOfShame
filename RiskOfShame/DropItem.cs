using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfShame
{
    public class TooltipClick : MonoBehaviour, IPointerClickHandler
    {
        public RoR2.LocalUser User;
        public RoR2.Inventory Inventory;
        public RoR2.UI.ItemIcon Item;
        public RoR2.ItemIndex ItemIndex;
        public void OnPointerClick(PointerEventData eventData)
        {
            Inventory.RemoveItem(ItemIndex, 1);
            RoR2.PickupDropletController.CreatePickupDroplet(new RoR2.PickupIndex(ItemIndex), User.cachedBody.transform.position + Vector3.up * 1.5f, Vector3.up * 20f + User.cachedBody.transform.forward * 2f);
            //var localUser = RoR2.LocalUserManager.GetFirstLocalUser();
            //RoR2.Chat.SendBroadcastChat(new RoR2.Chat.UserChatMessage { sender = localUser.cachedMasterObject, text = ItemIndex + " dropped!" });
        }
    }
    public class DropItem : MonoBehaviour
    {
        private void Update()
        {
            var huds = typeof(RoR2.UI.HUD).GetStaticField<List<RoR2.UI.HUD>>("instancesList");
            foreach (var hud in huds)
            {
                var items = hud.itemInventoryDisplay.GetField<List<RoR2.UI.ItemIcon>>("itemIcons");
                foreach (var item in items)
                {
                    var itemGo = item.transform.gameObject;
                    if (itemGo.GetComponent<TooltipClick>() == null)
                    {
                        var ttc = item.transform.gameObject.AddComponent<TooltipClick>();
                        ttc.User = hud.localUserViewer;
                        ttc.Item = item;
                        ttc.ItemIndex = item.GetField<RoR2.ItemIndex>("itemIndex");
                        ttc.Inventory = hud.itemInventoryDisplay.GetField<RoR2.Inventory>("inventory");
                    }
                }
            }
        }
    }
}
