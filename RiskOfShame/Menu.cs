using System;
using System.Reflection;
using UnityEngine;

namespace RiskOfShame
{
    public class Menu : MonoBehaviour
    {
        public static int size = 18;
        public static Boolean CursorIsVisible()
        {
            foreach (var mpeventSystem in RoR2.UI.MPEventSystem.readOnlyInstancesList)
                if (mpeventSystem.isCursorVisible)
                    return true;
            return false;
        }
        void OnGUI()
        {
#if DEBUG
            GUI.Label(new Rect(0, 0, 800, 25), "init : " + GetType().Namespace);
#else
            GUI.Label(new Rect(0, 0, 200, 25), "Risk of Shame Version " + Assembly.GetExecutingAssembly().GetName().Version);
#endif
            if (CursorIsVisible())
            {
                int menuOptions = 0;
                foreach (var mono in Loader.BaseObject.GetComponents<MonoBehaviour>())
                {
                    if (mono.GetType().Name == "Controller" || mono.GetType().Name == "Menu")
                        continue;
                    mono.enabled = GUI.Toggle(new Rect(5, 125 + menuOptions++ * size, 150, size + 5), mono.enabled, mono.GetType().Name);
                }
                var unload = GUI.Toggle(new Rect(5, 125 + menuOptions++ * size, 150, size + 5), false, "Unload");
                if (unload)
                    GameObject.Destroy(Loader.BaseObject);
            }
        }
    }
}