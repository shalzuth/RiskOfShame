using UnityEngine;
using System.Linq;

namespace RiskOfShame
{
    public class SkipCurrentLevel : MonoBehaviour
    {
        private void OnEnable()
        {
            //RoR2.Stage.instance.BeginAdvanceStage(RoR2.Run.instance.nextStageScene);
            RoR2.Stage.instance.BeginAdvanceStage(RoR2.SceneCatalog.allStageSceneDefs.First(s=>s.baseSceneName == "shipgraveyard"));
            enabled = false;
        }
    }
}
