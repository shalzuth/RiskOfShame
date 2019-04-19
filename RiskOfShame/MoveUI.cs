using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RiskOfShame
{
    public class MoveUI : MonoBehaviour
    {
        class UIDrag : MonoBehaviour, IDragHandler
        {
            public bool ApplyToParent = false;
            public bool ApplyToGrandParent = false;
            public void OnDrag(PointerEventData eventData)
            {
                if (ApplyToParent)
                    transform.parent.localPosition += (Vector3)eventData.delta;
                else if (ApplyToGrandParent)
                    transform.parent.parent.localPosition += (Vector3)eventData.delta;
                else
                    transform.localPosition += (Vector3)eventData.delta;
            }
        }
        List<string> MakeRaycastable = new List<string>
        {
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomLeftCluster/BarRoots",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomLeftCluster/BarRoots/LevelDisplayCluster",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomLeftCluster/BarRoots/HealthbarRoot",

            "HUDSimple(Clone)/MainContainer/MainUIArea/UpperRightCluster/TimerRoot",
            "HUDSimple(Clone)/MainContainer/MainUIArea/UpperRightCluster/TimerRoot/TimerPanel",
            "HUDSimple(Clone)/MainContainer/MainUIArea/UpperRightCluster/TimerRoot/SetDifficultyPanel/DifficultyIcon",
            "HUDSimple(Clone)/MainContainer/MainUIArea/UpperRightCluster/TimerRoot/DifficultyBar",
            "HUDSimple(Clone)/MainContainer/MainUIArea/UpperRightCluster/TimerRoot/ObjectivePanel",

            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/Outline",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/AltEquipmentSlot",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/EquipmentSlot",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/Skill1Root",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/Skill2Root",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/Skill3Root",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/Skill4Root",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/SprintCluster",
            "HUDSimple(Clone)/MainContainer/MainUIArea/BottomRightCluster/Scaler/InventoryCluster",

            "HUDSimple(Clone)/MainContainer/MainUIArea/UpperLeftCluster",
            "HUDSimple(Clone)/MainContainer/MainUIArea/UpperLeftCluster/MoneyRoot",
            "HUDSimple(Clone)/MainContainer/MainUIArea/UpperLeftCluster/LunarCoinRoot",

            "HUDSimple(Clone)/MainContainer/MainUIArea/TopCenterCluster",
            "HUDSimple(Clone)/MainContainer/MainUIArea/TopCenterCluster/ItemInventoryDisplay",
            "HUDSimple(Clone)/MainContainer/MainUIArea/TopCenterCluster/BossHealthBarRoot",

            "HUDSimple(Clone)/MainContainer/MainUIArea/LeftCluster/AllyCardContainer",

            "HUDSimple(Clone)/MainContainer/MainUIArea/RightCluster/ContextNotification/ContextDisplay",

            "HUDSimple(Clone)/MainContainer/MainUIArea/ScoreboardPanel/Container/StripContainer/ScoreboardStrip(Clone)/LongBackground",
        };
        void Update()
        {
            foreach (var child in MakeRaycastable)
            {
                var obj = GameObject.Find(child);
                var image = obj.GetComponent<Image>();
                if (image != null)
                {
                    image.raycastTarget = true;
                }
                if (obj.GetComponent<GraphicRaycaster>() == null)
                {
                    var ttc = obj.gameObject.AddComponent<GraphicRaycaster>();
                }
                if (obj.GetComponent<UIDrag>() == null)
                {
                    var ttc = obj.gameObject.AddComponent<UIDrag>();
                    if (child.EndsWith("Scaler/Outline"))
                    {
                        obj.SetActive(true);
                        ttc.ApplyToParent = true;
                    }
                    if (child.EndsWith("LongBackground"))
                    {
                        ttc.ApplyToGrandParent = true;
                    }
                }
            }
        }
    }
}