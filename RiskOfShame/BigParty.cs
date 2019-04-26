using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class BigParty : MonoBehaviour
    {
        List<UnityEngine.GameObject> Locks = new List<GameObject>();
        private void OnEnable()
        {
            typeof(RoR2.RoR2Application).SetField("maxPlayers", 16);
            UnityEngine.Networking.NetworkManager.singleton.maxConnections = 16;
        }
        private void OnDisable()
        {
            typeof(RoR2.RoR2Application).SetField("maxPlayers", 4);
            UnityEngine.Networking.NetworkManager.singleton.maxConnections = 4;
        }
    }
}
