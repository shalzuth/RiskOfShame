using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class RevealChests : MonoBehaviour
    {
        List<UnityEngine.GameObject> Locks = new List<GameObject>();
        private void OnEnable()
        {
            Locks.Clear();
            var purchasables = UnityEngine.Object.FindObjectsOfType<RoR2.PurchaseInteraction>();
            foreach (var purchase in purchasables)
            {
                if (purchase.available)
                    Locks.Add(UnityEngine.Object.Instantiate<GameObject>(RoR2.TeleporterInteraction.instance.lockPrefab, purchase.transform.position, Quaternion.identity));
            }
            var barrels = UnityEngine.Object.FindObjectsOfType<RoR2.BarrelInteraction>();
            foreach (var barrel in barrels)
            {
                if (!barrel.Networkopened)
                    Locks.Add(UnityEngine.Object.Instantiate<GameObject>(RoR2.TeleporterInteraction.instance.lockPrefab, barrel.transform.position, Quaternion.identity));
            }
        }
        private void OnDisable()
        {
            foreach(var go in Locks)
                GameObject.Destroy(go);
        }
    }
}
