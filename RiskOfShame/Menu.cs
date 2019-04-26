using System;
using System.Reflection;
using UnityEngine;

namespace RiskOfShame
{
    public class Menu : MonoBehaviour
    {
        static Int32 Margin = 5;
        static Int32 MenuWidth = 150;
        Int32 MenuId;
        Rect MenuWindow = new Rect(Margin, Margin, MenuWidth, 50);
        public static Boolean CursorIsVisible()
        {
            foreach (var mpeventSystem in RoR2.UI.MPEventSystem.readOnlyInstancesList)
                if (mpeventSystem.isCursorVisible)
                    return true;
            return false;
        }
        void Awake()
        {
            MenuId = GetHashCode();
        }
        void OnGUI()
        {
            if (CursorIsVisible())
            {
#if DEBUG
                MenuWindow = GUILayout.Window(MenuId, MenuWindow, MenuMethod, "init : " + GetType().Namespace, GUILayout.ExpandHeight(true));
#else
                MenuWindow = GUILayout.Window(MenuId, MenuWindow, MenuMethod, "Risk of Shame " + Assembly.GetExecutingAssembly().GetName().Version, GUILayout.ExpandHeight(true));
#endif
            }
        }
        void MenuMethod(Int32 id)
        {
            foreach (var mono in Loader.BaseObject.GetComponents<MonoBehaviour>())
            {
                if (mono.GetType() == GetType())
                    continue;
                mono.enabled = GUILayout.Toggle(mono.enabled, mono.GetType().Name);
            }
            var unload = GUILayout.Toggle(false, "Unload");
            if (unload)
                Destroy(Loader.BaseObject);
            GUI.DragWindow();
        }
    }
}