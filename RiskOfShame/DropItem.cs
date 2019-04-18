using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RiskOfShame
{
    public class DropItem : MonoBehaviour
    {
        class TooltipClick : MonoBehaviour, IPointerClickHandler
        {
            public RoR2.ItemIndex ItemIndex;
            public void OnPointerClick(PointerEventData eventData)
            {
                var user = RoR2.LocalUserManager.GetFirstLocalUser();
                RoR2.Chat.SendBroadcastChat(new RoR2.Chat.UserChatMessage { sender = user.currentNetworkUser.gameObject, text = RoR2.Language.GetString(new RoR2.PickupIndex(ItemIndex).GetPickupNameToken()) + " dropped!" });
                user.cachedBody.inventory.RemoveItem(ItemIndex, 1);
                RoR2.PickupDropletController.CreatePickupDroplet(new RoR2.PickupIndex(ItemIndex), user.cachedBody.transform.position + Vector3.up * 1.5f, Vector3.up * 20f + user.cachedBody.transform.forward * 2f);
                Destroy(this);
            }
        }
        void Update()
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
                        ttc.ItemIndex = item.GetField<RoR2.ItemIndex>("itemIndex");
                    }
                }
            }
        }
    }
}
