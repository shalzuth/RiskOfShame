using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class AdvancedTooltips : MonoBehaviour
    {
        private void Start()
        {
            var itemDefs = typeof(RoR2.ItemCatalog).GetStaticField<RoR2.ItemDef[]>("itemDefs");
            foreach(var itemDef in itemDefs)
                itemDef.pickupToken = itemDef.descriptionToken;
        }
        private void Update()
        {
            var huds = typeof(RoR2.UI.HUD).GetStaticField<List<RoR2.UI.HUD>>("instancesList");
            foreach (var hud in huds)
            {
                var items = hud.itemInventoryDisplay.GetField<List<RoR2.UI.ItemIcon>>("itemIcons");
                foreach (var item in items)
                {
                    var itemDef = RoR2.ItemCatalog.GetItemDef(item.GetField<RoR2.ItemIndex>("itemIndex"));
                    item.tooltipProvider.bodyToken = itemDef.descriptionToken;
                }
            }
        }
    }
}
