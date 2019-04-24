using UnityEngine;

namespace RiskOfShame
{
    public class AlwaysSprint : MonoBehaviour
    {
        private void Update()
        {
            var localUser = RoR2.LocalUserManager.GetFirstLocalUser();
            var controller = localUser.cachedMasterController;
            var body = controller.master.GetBody();
            if (body && !body.isSprinting && !localUser.inputPlayer.GetButton("Sprint"))
                controller.SetField("sprintInputPressReceived", true);
        }
    }
}
