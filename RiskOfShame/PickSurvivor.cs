using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiskOfShame
{
    public class PickSurvivor : MonoBehaviour
    {
        static Int32 Margin = 5;
        static Int32 CharacterWidth = 400;
        Int32 CharacterSelectId;
        Rect CharacterWindow = new Rect(Screen.width - CharacterWidth - Margin, Margin, CharacterWidth, Screen.height - Margin* 2);
        Vector2 CharacterScrollPos;
        Dictionary<String, Int32> nameToIndexMap = new Dictionary<String, Int32>();
        void Awake()
        {
            nameToIndexMap = typeof(RoR2.BodyCatalog).GetField<Dictionary<String, Int32>>("nameToIndexMap");
            CharacterSelectId = GetHashCode();
        }
        void OnGUI()
        {
            CharacterWindow = GUILayout.Window(CharacterSelectId, CharacterWindow, CharacterWindowMethod, "Character Select");
        }
        void CharacterWindowMethod(Int32 id)
        {
            CharacterScrollPos = GUILayout.BeginScrollView(CharacterScrollPos);
            {
                foreach (var body in nameToIndexMap)
                {
                    if (body.Key.Contains("(Clone)"))
                        continue;
                    if (GUILayout.Button(body.Key.Replace("Body","")))
                    {
                        GameObject newBody = RoR2.BodyCatalog.FindBodyPrefab(body.Key);
                        if (newBody == null)
                            return;
                        var localUser = RoR2.LocalUserManager.GetFirstLocalUser();
                        if (localUser == null || localUser.cachedMasterController == null || localUser.cachedMasterController.master == null) return;
                        var master = localUser.cachedMasterController.master;
                        if (master == null)
                        {
                            var user = ((RoR2.UI.MPEventSystem)UnityEngine.EventSystems.EventSystem.current).localUser;
                            if (user.eventSystem == UnityEngine.EventSystems.EventSystem.current)
                            {
                                if (user.currentNetworkUser == null)
                                    return;
                                user.currentNetworkUser.CallCmdSetBodyPreference(body.Value);
                            }
                            return;
                        }
                        master.bodyPrefab = newBody;
                        master.Respawn(master.GetBody().transform.position, master.GetBody().transform.rotation);
                        enabled = false;
                        return;
                    }
                }
            }
            GUILayout.EndScrollView();
            GUI.DragWindow();
        }
    }
}