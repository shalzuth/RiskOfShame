using UnityEngine;

namespace RiskOfShame
{
    public class SkipCurrentLevel : MonoBehaviour
    {
        private void OnEnable()
        {
            RoR2.Stage.instance.BeginAdvanceStage(RoR2.Run.instance.nextStageScene);
            enabled = false;
        }
    }
}
