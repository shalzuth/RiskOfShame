using UnityEngine;
namespace RiskOfShame
{
    public class RailgunnerReload : MonoBehaviour
    {
        void Update()
        {
            for (int i = 0; i < RoR2.LocalUserManager.GetFirstLocalUser().cachedBody.skillLocator.skillSlotCount; i++)
            {
                var skillAtIndex = RoR2.LocalUserManager.GetFirstLocalUser().cachedBody.skillLocator.GetSkillAtIndex(i);
                if (skillAtIndex)
                {
                    var railgunSkillDef = skillAtIndex.skillDef as RoR2.Skills.RailgunSkillDef;
                    if (railgunSkillDef) skillAtIndex.skillInstanceData.GetField<RoR2.EntityStateMachine>("reloadStateMachine").state.SetField("boostGracePeriod", 100.0f);
                }
            }
        }
    }
}
