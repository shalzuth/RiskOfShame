using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiskOfShame
{
    public class RemoveEmptyObjects : MonoBehaviour
    {
        public class RemoveEmptyObject : MonoBehaviour
        {
            void Update()
            {
                var entityStateMachine = gameObject.GetComponent<RoR2.EntityStateMachine>();
                if (entityStateMachine && entityStateMachine.state.GetType() == typeof(EntityStates.Barrel.Opened))
                {
                    UnityEngine.Networking.NetworkServer.Destroy(gameObject);
                    Destroy(gameObject);
                }
            }
        }
        void AddRemoveEmptyObjectBehavior(RoR2.SceneDirector obj)
        {
            var purchasables = Object.FindObjectsOfType<RoR2.PurchaseInteraction>();
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
        void OnEnable()
        {
            AddRemoveEmptyObjectBehavior(null);
            RoR2.SceneDirector.onPostPopulateSceneServer += AddRemoveEmptyObjectBehavior;
        }
        void OnDisable()
        {
            RoR2.SceneDirector.onPostPopulateSceneServer -= AddRemoveEmptyObjectBehavior;
            var removers = Object.FindObjectsOfType<RemoveEmptyObject>();
            foreach (var remover in removers)
                Destroy(remover);
        }
    }
}
