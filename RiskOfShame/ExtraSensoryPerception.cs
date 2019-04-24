using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiskOfShame
{
    public class ExtraSensoryPerception : MonoBehaviour
    {
        public static class ResourcesCached
        {
            public static Dictionary<string, Object> resourceCache = new Dictionary<string, Object>();
            public static T Load<T>(string path) where T : Object
            {
                if (!resourceCache.ContainsKey(path))
                    resourceCache[path] = Resources.Load<T>(path);
                return (T)resourceCache[path];
            }
        }
        List<RoR2.PurchaseInteraction> PurchaseInteractions = new List<RoR2.PurchaseInteraction>();
        List<RoR2.BarrelInteraction> Barrels = new List<RoR2.BarrelInteraction>();
        List<RoR2.PressurePlateController> SecretButtons = new List<RoR2.PressurePlateController>();
        void DumpInteractables(RoR2.SceneDirector obj)
        {
            PurchaseInteractions = GameObject.FindObjectsOfType<RoR2.PurchaseInteraction>().ToList();
            Barrels = GameObject.FindObjectsOfType<RoR2.BarrelInteraction>().ToList();
            SecretButtons = GameObject.FindObjectsOfType<RoR2.PressurePlateController>().ToList();
        }
        void OnEnable()
        {
            DumpInteractables(null);
            RoR2.SceneDirector.onPostPopulateSceneServer += DumpInteractables;
        }
        void OnDisable()
        {
            RoR2.SceneDirector.onPostPopulateSceneServer -= DumpInteractables;
        }
        void DrawIcon(string iconPath, Vector3 position, int scale = 75)
        {
            var screenPos = Camera.main.WorldToScreenPoint(position);
            if (screenPos.z <= 0)
                return;
            var distance = (Camera.main.transform.position - position).magnitude;
            var size = scale * 100 / (100 + distance);
            try
            {
                var icon = ResourcesCached.Load<Texture>(iconPath);
                GUI.DrawTexture(new Rect(screenPos.x - size / 2, Screen.height - screenPos.y - size, size, size), icon);
            }
            catch
            {
                GUI.Label(new Rect(screenPos.x - size / 2, Screen.height - screenPos.y - size, size, size), iconPath);
            }
        }
        string GetPickupItemPatch(RoR2.PickupIndex pickupIndex)
        {
            return pickupIndex.value < (int)RoR2.ItemIndex.Count ?
                RoR2.ItemCatalog.GetItemDef((RoR2.ItemIndex)pickupIndex.value).pickupIconPath :
                RoR2.EquipmentCatalog.GetEquipmentDef((RoR2.EquipmentIndex)pickupIndex.value - (int)RoR2.ItemIndex.Count).pickupIconPath;
        }
        void OnGUI()
        {
            foreach (var purchase in PurchaseInteractions)
            {
                if (!purchase.available)
                    continue;
                var itemPath = "";
                if (purchase.gameObject.name.Contains("NewtStatue"))
                    itemPath = "Textures/MiscIcons/texShrineIconOutlined";
                var chest = purchase.gameObject.GetComponent<RoR2.ChestBehavior>();
                var shop = purchase.gameObject.GetComponent<RoR2.ShopTerminalBehavior>();
                var blood = purchase.gameObject.GetComponent<RoR2.ShrineBloodBehavior>();
                var boss = purchase.gameObject.GetComponent<RoR2.ShrineBossBehavior>();
                var chance = purchase.gameObject.GetComponent<RoR2.ShrineChanceBehavior>();
                var combat = purchase.gameObject.GetComponent<RoR2.ShrineCombatBehavior>();
                var healing = purchase.gameObject.GetComponent<RoR2.ShrineHealingBehavior>();
                var order = purchase.gameObject.GetComponent<RoR2.ShrineRestackBehavior>();
                var summon = purchase.gameObject.GetComponent<RoR2.SummonMasterBehavior>();
                if (chest)
                {
                    var pickupIndex = chest.GetField<RoR2.PickupIndex>("dropPickup");
                    if (pickupIndex == RoR2.PickupIndex.none)
                    {
                        itemPath = "Textures/MiscIcons/texInventoryIcon";
                        if (purchase.gameObject.name.Contains("LunarChest"))
                            DrawIcon("Textures/ItemIcons/bg/texLunarBGIcon", purchase.transform.position, 80);
                        else if (purchase.gameObject.name.Contains("Chest1"))
                            DrawIcon("Textures/ItemIcons/bg/texTier1BGIcon", purchase.transform.position, 80);
                        else if (purchase.gameObject.name.Contains("Chest1Stealthed") || purchase.gameObject.name.Contains("Lockbox"))
                            itemPath = "Textures/MiscIcons/texLootIconOutlined";
                        else if (purchase.gameObject.name.Contains("Chest2"))
                            DrawIcon("Textures/ItemIcons/bg/texTier2BGIcon", purchase.transform.position, 80);
                        else if (purchase.gameObject.name.Contains("GoldChest"))
                            DrawIcon("Textures/ItemIcons/bg/texTier3BGIcon", purchase.transform.position, 80);
                        else if (purchase.gameObject.name.Contains("EquipmentBarrel"))
                            DrawIcon("Textures/ItemIcons/bg/texEquipmentbgIcon", purchase.transform.position, 80);
                        else if (purchase.gameObject.name.Contains("HumanFan"))
                            continue;
                    }
                    else
                        itemPath = GetPickupItemPatch(pickupIndex);
                }
                else if (shop)
                    itemPath = GetPickupItemPatch(shop.GetField<RoR2.PickupIndex>("pickupIndex"));
                else if (blood)
                    itemPath = "Textures/ShrineSymbols/" + blood.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                else if (boss)
                    itemPath = "Textures/ShrineSymbols/" + boss.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                else if (chance)
                    itemPath = "Textures/ShrineSymbols/" + chance.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                else if (combat)
                    itemPath = "Textures/ShrineSymbols/" + combat.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                else if (healing)
                    itemPath = "Textures/ShrineSymbols/" + healing.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                else if (order)
                    itemPath = "Textures/ShrineSymbols/" + order.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                else if (summon)
                    itemPath = "Textures/BodyIcons/" + summon.masterPrefab.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponent<RoR2.CharacterBody>().portraitIcon.name;
                DrawIcon(itemPath, purchase.transform.position);
            }
            foreach (var barrel in Barrels)
                if (!barrel.Networkopened)
                    DrawIcon("Textures/MiscIcons/texRuleBonusStartingMoney", barrel.transform.position);
            foreach (var secret in SecretButtons)
                DrawIcon("Textures/BuffIcons/texBuffSlow25Icon", secret.transform.position);
            if (RoR2.TeleporterInteraction.instance)
                DrawIcon("Textures/MiscIcons/texTeleporterIcon", RoR2.TeleporterInteraction.instance.transform.position);
        }
    }
}
