using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiskOfShame
{
    public class AimBot : MonoBehaviour
    {
        public bool CursorIsVisible()
        {
            for (int i = 0; i < RoR2.UI.MPEventSystem.readOnlyInstancesList.Count; i++)
            {
                var mpeventSystem = RoR2.UI.MPEventSystem.readOnlyInstancesList[i];
                if (mpeventSystem.isCursorVisible)
                {
                    return true;
                }
            }
            return false;
        }
        void Update()
        {
            if (CursorIsVisible()) return;
            var localUser = RoR2.LocalUserManager.GetFirstLocalUser();
            var controller = localUser.cachedMasterController;
            if (!controller) return;
            var body = controller.master.GetBody();
            if (!body) return;
            var inputBank = body.GetComponent<RoR2.InputBankTest>();
            var aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
            var bullseyeSearch = new RoR2.BullseyeSearch();
            var team = body.GetComponent<RoR2.TeamComponent>();
            bullseyeSearch.teamMaskFilter = RoR2.TeamMask.all;
            bullseyeSearch.teamMaskFilter.RemoveTeam(team.teamIndex);
            bullseyeSearch.filterByLoS = true;
            bullseyeSearch.searchOrigin = aimRay.origin;
            bullseyeSearch.searchDirection = aimRay.direction;
            bullseyeSearch.sortMode = RoR2.BullseyeSearch.SortMode.Distance;
            bullseyeSearch.maxDistanceFilter = float.MaxValue;
            bullseyeSearch.maxAngleFilter = 20f;// ;// float.MaxValue;
            bullseyeSearch.RefreshCandidates();
            var hurtBox = bullseyeSearch.GetResults().FirstOrDefault();
            if (hurtBox) inputBank.aimDirection = hurtBox.transform.position - aimRay.origin;   
        }
    }
}