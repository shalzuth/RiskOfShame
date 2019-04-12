using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class RemoveEmptyObject : MonoBehaviour
    {
        void Update()
        {
            var entityStateMachine = gameObject.GetComponent<RoR2.EntityStateMachine>();
            if (entityStateMachine.state.GetType() == typeof(EntityStates.Barrel.Opened))
            {
                UnityEngine.Networking.NetworkServer.Destroy(gameObject);
                Destroy(gameObject);
            }
        }
    }
    public class RemoveEmptyObjects : MonoBehaviour
    {
        void OnEnable()
        {
            var purchasables = RoR2.PurchaseInteraction.readOnlyInstancesList;//Object.FindObjectsOfType<RoR2.PurchaseInteraction>();
            foreach (var purchase in purchasables)
            {
                var reo = purchase.gameObject.GetComponent<RemoveEmptyObject>();
                if (reo == null)
                    purchase.gameObject.AddComponent<RemoveEmptyObject>();
            }
            var barrels = Object.FindObjectsOfType<RoR2.BarrelInteraction>();
            foreach (var barrel in barrels)
            {
                var reo = barrel.gameObject.GetComponent<RemoveEmptyObject>();
                if (reo == null)
                    barrel.gameObject.AddComponent<RemoveEmptyObject>();
            }
        }
        void OnDisable()
        {
            var removers = Object.FindObjectsOfType<RemoveEmptyObject>();
            foreach (var remover in removers)
            {
                Destroy(remover);
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
