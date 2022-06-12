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
        List<GameObject> VultureEggs = new List<GameObject>();
        void DumpInteractables(RoR2.SceneDirector obj)
        {
            PurchaseInteractions = GameObject.FindObjectsOfType<RoR2.PurchaseInteraction>().ToList();
            Barrels = GameObject.FindObjectsOfType<RoR2.BarrelInteraction>().ToList();
            SecretButtons = GameObject.FindObjectsOfType<RoR2.PressurePlateController>().ToList();
            VultureEggs = GameObject.FindObjectsOfType<RoR2.CharacterDeathBehavior>().ToList().FindAll(co=>co.gameObject.name.Contains("VultureEggBody")).Select(co=>co.gameObject).ToList();
        }
        void OnEnable()
        {
            Material chamsMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy
            };
            chamsMaterial.SetInt("_SrcBlend", 5);
            chamsMaterial.SetInt("_DstBlend", 10);
            chamsMaterial.SetInt("_Cull", 0);
            chamsMaterial.SetInt("_ZTest", 8); // Render through walls.
            chamsMaterial.SetInt("_ZWrite", 0);
            chamsMaterial.SetColor("_Color", Color.green);
            DumpInteractables(null);
            //foreach(var interactable in PurchaseInteractions)
            //foreach (Renderer renderer in interactable.GetComponentsInChildren<Renderer>())
                //renderer.material = chamsMaterial;
            RoR2.SceneDirector.onPostPopulateSceneServer += DumpInteractables;
        }
        void OnDisable()
        {
            RoR2.SceneDirector.onPostPopulateSceneServer -= DumpInteractables;
        }
        void DrawIcon(string iconPath, Vector3 position, int scale = 75)
        {
            var screenPos = Camera.main.WorldToScreenPoint(position);
            if (screenPos.z <= 0) return;
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
        void DrawIcon(Texture icon, Vector3 position, int scale = 50)
        {
            var screenPos = Camera.main.WorldToScreenPoint(position);
            if (screenPos.z <= 0)
                return;
            var distance = (Camera.main.transform.position - position).magnitude;
            var size = scale * 100 / (100 + distance);
            try
            {
                GUI.DrawTexture(new Rect(screenPos.x - size / 2, Screen.height - screenPos.y - size, size, size), icon);
            }
            catch
            {
                GUI.Label(new Rect(screenPos.x - size / 2, Screen.height - screenPos.y - size, size, size), icon.name);
            }
        }
        void OnGUI()
        {
            foreach (var purchase in PurchaseInteractions)
            {
                if (purchase == null || purchase.transform == null || purchase.transform.position == null || purchase.gameObject == null || purchase.gameObject.name == null) continue;
                var screenPos = Camera.main.WorldToScreenPoint(purchase.transform.position);
                if (screenPos.z <= 0) continue;
                GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 200, 80), purchase.gameObject.name);

                if (!purchase.available) continue;
                var itemPath = "";
                var chest = purchase.gameObject.GetComponent<RoR2.ChestBehavior>();
                if (chest)
                {
                    //var pickupIndex = chest.GetField<RoR2.PickupIndex>("dropPickup");
                    //if (pickupIndex == RoR2.PickupIndex.none)
                    {
                        //itemPath = "Textures/MiscIcons/texInventoryIcon";
                        if (purchase.gameObject.name.Contains("LunarChest")) itemPath = "Textures/ItemIcons/bg/texLunarBGIcon";
                        else if (purchase.gameObject.name.Contains("Chest1Stealthed") || purchase.gameObject.name.Contains("Lockbox")) itemPath = "Textures/MiscIcons/texLootIconOutlined";
                        else if (purchase.gameObject.name.Contains("Chest1")) itemPath = "Textures/ItemIcons/bg/texTier1BGIcon";
                        else if (purchase.gameObject.name.Contains("Chest2")) itemPath = "Textures/ItemIcons/bg/texTier2BGIcon";
                        else if (purchase.gameObject.name.Contains("CategoryChestDamage")) itemPath = "Textures/MiscIcons/texAttackIcon";
                        else if (purchase.gameObject.name.Contains("CategoryChestUtility")) itemPath = "Textures/BuffIcons/texBuffGenericShield";
                        else if (purchase.gameObject.name.Contains("CategoryChestHealing")) itemPath = "Textures/MiscIcons/texCriticallyHurtIcon";
                        else if (purchase.gameObject.name.Contains("GoldChest")) itemPath = "Textures/ItemIcons/bg/texTier3BGIcon";
                        else if (purchase.gameObject.name.Contains("EquipmentBarrel")) itemPath = "Textures/ItemIcons/bg/texEquipmentbgIcon";
                        else if (purchase.gameObject.name.Contains("HumanFan")) continue;
                    }
                    //else itemPath =  "Textures/ItemIcons/" + RoR2.PickupCatalog.GetPickupDef(pickupIndex).iconTexture.name;
                }
                else
                {
                    var shop = purchase.gameObject.GetComponent<RoR2.ShopTerminalBehavior>();
                    var blood = purchase.gameObject.GetComponent<RoR2.ShrineBloodBehavior>();
                    var boss = purchase.gameObject.GetComponent<RoR2.ShrineBossBehavior>();
                    var chance = purchase.gameObject.GetComponent<RoR2.ShrineChanceBehavior>();
                    var combat = purchase.gameObject.GetComponent<RoR2.ShrineCombatBehavior>();
                    var healing = purchase.gameObject.GetComponent<RoR2.ShrineHealingBehavior>();
                    var order = purchase.gameObject.GetComponent<RoR2.ShrineRestackBehavior>();
                    var summon = purchase.gameObject.GetComponent<RoR2.SummonMasterBehavior>();
                    var casino = purchase.gameObject.GetComponent<RoR2.RouletteChestController>();
                    var seer = purchase.gameObject.GetComponent<RoR2.SeerStationController>();
                    //if (shop) GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y + 50, 200, 80), RoR2.PickupCatalog.GetPickupDef(shop.GetField<RoR2.PickupIndex>("pickupIndex")).iconTexture.name);
                    if (shop) itemPath = "Textures/ItemIcons/" + RoR2.PickupCatalog.GetPickupDef(shop.GetField<RoR2.PickupIndex>("pickupIndex")).iconTexture.name;
                    else if (blood) itemPath = "Textures/ShrineSymbols/" + blood.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                    else if (boss) itemPath = "Textures/ShrineSymbols/" + boss.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                    else if (chance) itemPath = "Textures/ShrineSymbols/" + chance.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                    else if (combat) itemPath = "Textures/ShrineSymbols/" + combat.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                    else if (healing) itemPath = "Textures/ShrineSymbols/" + healing.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                    else if (order) itemPath = "Textures/ShrineSymbols/" + order.symbolTransform.GetComponent<MeshRenderer>().material.mainTexture.name;
                    else if (summon) itemPath = "Textures/BodyIcons/" + summon.masterPrefab.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponent<RoR2.CharacterBody>().portraitIcon.name;
                    else if (casino) itemPath = "Textures/MiscIcons/texMysteryIcon";
                    else if (seer) GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y + 15, 200, 80), RoR2.SceneCatalog.GetSceneDef(seer.NetworktargetSceneDefIndex).baseSceneName);
                }
                if (purchase.gameObject.name.Contains("NewtStatue")) itemPath = "Textures/MiscIcons/texShrineIconOutlined";
                if (purchase.gameObject.name.Contains("Duplicator")) DrawIcon("Textures/MiscIcons/texInventoryIcon", purchase.transform.position, 80);
                if (itemPath != "") DrawIcon(itemPath, purchase.transform.position);
            }
            foreach (var barrel in Barrels) if (!barrel.Networkopened) DrawIcon("Textures/MiscIcons/texRuleBonusStartingMoney", barrel.transform.position);
            foreach (var secret in SecretButtons) DrawIcon("Textures/BuffIcons/texBuffSlow25Icon", secret.transform.position);
            if (RoR2.TeleporterInteraction.instance) DrawIcon("Textures/MiscIcons/texTeleporterIcon", RoR2.TeleporterInteraction.instance.transform.position);
            foreach (var egg in VultureEggs.FindAll(go=>go != null && go.activeInHierarchy && go.transform != null && go.transform.position != null)) DrawIcon("Textures/ItemIcons/texParentEggIcon", egg.transform.position);
        }
    }
}
