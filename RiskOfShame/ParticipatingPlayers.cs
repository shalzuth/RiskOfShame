using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiskOfShame
{
    public class ParticipatingPlayers : MonoBehaviour
    {
        void OnEnable()
        {
            var o = new GameObject();
            var n = o.AddComponent<RoR2.PlayerCharacterMasterController>();
            var m = o.AddComponent<RoR2.CharacterMaster>();
            //m.alive = true;
            n.SetField("master", m);
        }
        void FixedUpdate()
        {
            //RoR2.Run.instance.SetField("participatingPlayerCount", 4);
        }
    }
}
