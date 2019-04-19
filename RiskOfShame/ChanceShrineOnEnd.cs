using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class ChanceShrineOnEnd : MonoBehaviour
    {
        public class SpawnChanceShrine : MonoBehaviour
        {
            GameObject ChanceShrine;
            GameObject TrySpawnChanceShrineAt(Vector3 pos)
            {
                return RoR2.DirectorCore.instance.TrySpawnObject(Resources.Load<RoR2.SpawnCard>("SpawnCards/InteractableSpawnCard/iscShrineChance"),
                    new RoR2.DirectorPlacementRule
                    {
                        placementMode = RoR2.DirectorPlacementRule.PlacementMode.ApproximateSimple,
                        preventOverhead = true,
                        position = pos
                    },
                    new Xoroshiro128Plus(0));
            }
            void TrySpawnChanceShrine()
            {
                var teleporterPos = RoR2.TeleporterInteraction.instance.transform.position;
                ChanceShrine = TrySpawnChanceShrineAt(new Vector3(teleporterPos.x + 12, teleporterPos.y, teleporterPos.z));
                if (ChanceShrine == null)
                    ChanceShrine = TrySpawnChanceShrineAt(new Vector3(teleporterPos.x - 12, teleporterPos.y, teleporterPos.z));
                if (ChanceShrine == null)
                    ChanceShrine = TrySpawnChanceShrineAt(new Vector3(teleporterPos.x, teleporterPos.y + 12, teleporterPos.z));
                if (ChanceShrine == null)
                    ChanceShrine = TrySpawnChanceShrineAt(new Vector3(teleporterPos.x, teleporterPos.y - 12, teleporterPos.z));
                
            }
            void Update()
            {
                if (RoR2.TeleporterInteraction.instance.isCharged && ChanceShrine == null)
                    TrySpawnChanceShrine();
                if (ChanceShrine)
                {
                    var behavior = ChanceShrine.GetComponent<RoR2.ShrineChanceBehavior>();
                    behavior.SetField("refreshTimer", 0.0f);
                    behavior.SetField("maxPurchaseCount", 200);
                    behavior.SetField("successfulPurchaseCount", 0);
                }
            }
        }
        void Update()
        {
            var teleporter = RoR2.TeleporterInteraction.instance;
            var spawner = teleporter.gameObject.GetComponent<SpawnChanceShrine>();
            if (spawner == null)
                spawner = teleporter.gameObject.AddComponent<SpawnChanceShrine>();
        }
    }
}
