using UnityEngine;

namespace RiskOfShame
{
    public class RevealTeleporter : MonoBehaviour
    {
        class TeleporterIndicator : MonoBehaviour
        {
            GameObject teleporterPositionIndicator;
            private void OnEnable()
            {
                var prefab = Resources.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator");
                teleporterPositionIndicator = UnityEngine.Object.Instantiate<GameObject>(prefab, RoR2.TeleporterInteraction.instance.transform.position, Quaternion.identity);
                teleporterPositionIndicator.GetComponent<RoR2.PositionIndicator>().targetTransform = RoR2.TeleporterInteraction.instance.transform;
                teleporterPositionIndicator.GetComponent<RoR2.UI.ChargeIndicatorController>().isCharged = true;
            }
            private void OnDisable()
            {
                if (teleporterPositionIndicator)
                    GameObject.Destroy(teleporterPositionIndicator);
            }
        }
        void Update()
        {
            var teleporter = RoR2.TeleporterInteraction.instance;
            if (teleporter == null)
                return;
            var poi = teleporter.gameObject.GetComponent<TeleporterIndicator>();
            if (poi == null)
                poi = teleporter.gameObject.AddComponent<TeleporterIndicator>();
        }
        void OnDisable()
        {
            var teleporter = RoR2.TeleporterInteraction.instance;
            if (teleporter == null)
                return;
            var poi = teleporter.gameObject.GetComponent<TeleporterIndicator>();
            if (poi)
                Destroy(poi);
        }
    }
}
