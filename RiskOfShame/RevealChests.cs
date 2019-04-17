using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class ChestRevealer : MonoBehaviour
    {
        public GameObject Lock;
        void Awake()
        {
            Lock = Object.Instantiate<GameObject>(RoR2.TeleporterInteraction.instance.lockPrefab, transform.position, Quaternion.identity);
            UnityEngine.Networking.NetworkServer.Spawn(Lock);
        }
        void Update()
        {
            var purchase = gameObject.GetComponent<RoR2.PurchaseInteraction>();
            if (purchase != null && !purchase.available)
            {
                Destroy(Lock);
                UnityEngine.Networking.NetworkServer.Destroy(Lock);
            }
            var barrel = gameObject.GetComponent<RoR2.BarrelInteraction>();
            if (barrel != null && barrel.Networkopened)
            {
                Destroy(Lock);
                UnityEngine.Networking.NetworkServer.Destroy(Lock);
            }
        }
        void OnDisable()
        {
            Destroy(Lock);
            UnityEngine.Networking.NetworkServer.Destroy(Lock);
        }
    }
    public class RevealChests : MonoBehaviour
    {
        private void OnEnable()
        {
            var purchasables = Object.FindObjectsOfType<RoR2.PurchaseInteraction>();
            foreach (var purchase in purchasables)
            {
                var revealer = purchase.gameObject.GetComponent<ChestRevealer>();
                if (revealer == null && purchase.available)
                {
                    purchase.gameObject.AddComponent<ChestRevealer>();
                }
            }
            var barrels = Object.FindObjectsOfType<RoR2.BarrelInteraction>();
            foreach (var barrel in barrels)
            {
                var revealer = barrel.gameObject.GetComponent<ChestRevealer>();
                if (revealer == null && !barrel.Networkopened)
                {
                    barrel.gameObject.AddComponent<ChestRevealer>();
                }
            }
        }
        private void OnDisable()
        {
            var revealers = Object.FindObjectsOfType<ChestRevealer>();
            foreach(var revealer in revealers)
            {
                Destroy(revealer.Lock);
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
