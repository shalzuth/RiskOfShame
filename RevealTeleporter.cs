using UnityEngine;

namespace RiskOfShame
{
    public class RevealTeleporter : MonoBehaviour
    {
        GameObject prefab;
        GameObject teleporterPositionIndicator;
        private void Start()
        {
            prefab = Resources.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator");
            OnEnable();
        }
        private void OnEnable()
        {
            if (prefab == null)
                return;
            teleporterPositionIndicator = UnityEngine.Object.Instantiate<GameObject>(prefab, RoR2.TeleporterInteraction.instance.transform.position, Quaternion.identity);
            teleporterPositionIndicator.GetComponent<RoR2.PositionIndicator>().targetTransform = RoR2.TeleporterInteraction.instance.transform;
            teleporterPositionIndicator.GetComponent<RoR2.UI.ChargeIndicatorController>().isCharged = true;
        }
        private void OnDisable()
        {
            GameObject.Destroy(teleporterPositionIndicator);
        }
    }
}
