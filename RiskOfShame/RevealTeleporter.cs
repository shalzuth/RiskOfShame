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
        int InitialStage = RoR2.Run.instance.stageClearCount;
        void Update()
        {
            if (InitialStage != RoR2.Run.instance?.stageClearCount && RoR2.LocalUserManager.GetFirstLocalUser()?.cachedBody?.isSprinting == true)
            {
                InitialStage = RoR2.Run.instance.stageClearCount;
                OnEnable();
            }
        }
    }
}
