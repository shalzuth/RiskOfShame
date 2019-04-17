using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class LootRevealer : MonoBehaviour
    {
        public string Display;
        void LateUpdate()
        {
            var holo = gameObject.GetComponent<RoR2.Hologram.HologramProjector>();
            holo.displayDistance = 10000.0f;
            var holoContent = holo.GetField<GameObject>("hologramContentInstance").GetComponent<RoR2.CostHologramContent>();
            holoContent.targetTextMesh.text = Display;
            holoContent.targetTextMesh.fontSize = 20.0f;
        }
        void OnDisable()
        {
            var holo = gameObject.GetComponent<RoR2.Hologram.HologramProjector>();
            holo.displayDistance = 15.0f;
        }
    }
    public class RevealLoot : MonoBehaviour
    {
        private void OnEnable()
        {
            var purchasables = Object.FindObjectsOfType<RoR2.PurchaseInteraction>();
            foreach (var purchase in purchasables)
            {
                var chest = purchase.gameObject.GetComponent<RoR2.ChestBehavior>();
                if (chest != null)
                {
                    if (chest.gameObject.GetComponent<LootRevealer>() == null)
                    {
                        var lr = chest.gameObject.AddComponent<LootRevealer>();
                        lr.Display = RoR2.Language.GetString(chest.GetField<RoR2.PickupIndex>("dropPickup").GetPickupNameToken());
                    }
                }
                var shop = purchase.gameObject.GetComponent<RoR2.ShopTerminalBehavior>();
                if (shop != null)
                {
                    if (shop.gameObject.GetComponent<LootRevealer>() == null)
                    {
                        var lr = shop.gameObject.AddComponent<LootRevealer>();
                        lr.Display = RoR2.Language.GetString(shop.GetField<RoR2.PickupIndex>("pickupIndex").GetPickupNameToken());
                    }
                }
            }
            var multishops = GameObject.FindObjectsOfType<RoR2.MultiShopController>();
            foreach(var multishop in multishops)
            {
                if (multishop.gameObject.GetComponent<LootRevealer>() == null)
                {
                    var lr = multishop.gameObject.AddComponent<LootRevealer>();
                    var terminals = multishop.GetField<GameObject[]>("terminalGameObjects");
                    foreach (var terminal in terminals)
                    {
                        var i = terminal.GetComponent<RoR2.ShopTerminalBehavior>();
                        lr.Display += RoR2.Language.GetString(i.NetworkpickupIndex.GetPickupNameToken());
                    }
                }
            }
        }
        private void OnDisable()
        {
            var revealers = Object.FindObjectsOfType<LootRevealer>();
            foreach (var revealer in revealers)
            {
                Destroy(revealer);
            }
        }
        int InitialStage = RoR2.Run.instance.stageClearCount;
        void Update()
        {
            if (InitialStage != RoR2.Run.instance.stageClearCount && RoR2.LocalUserManager.GetFirstLocalUser().cachedBody.isSprinting)
            {
                InitialStage = RoR2.Run.instance.stageClearCount;
                OnEnable();
            }
        }
    }
}
