using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RiskOfShame
{
    public class Testing : MonoBehaviour
    {
        T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            var type = original.GetType();
            var copy = destination.AddComponent(type);
            var fields = type.GetFields(Extensions.flags);
            foreach (var field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            var props = type.GetProperties(Extensions.flags);
            foreach (var prop in props)
            {
                prop.SetValue(copy, prop.GetValue(original));
            }
            return copy as T;
        }
        public void recurseobj(StringBuilder sb, GameObject obj, int level)
        {
            sb.AppendLine(new string(' ', level * 2) + obj.name + " : " + obj.GetInstanceID());
            var components = obj.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                sb.AppendLine(new string(' ', level * 2) + obj.name + " component : " + component.GetType());
                if (component.GetType() == typeof(RoR2.EntityStateMachine))
                {
                    sb.AppendLine(new string(' ', level * 2) + obj.name + " state : " + ((RoR2.EntityStateMachine)component).state);
                }
            }
            var num = obj.transform.childCount;
            for (int i = 0; i < num; i++)
            {
                var childObj = obj.transform.GetChild(i);
                sb.AppendLine(new string(' ', level * 2) + obj.name + " child : " + childObj.name);
                recurseobj(sb, childObj.gameObject, level + 1);
                if (childObj.name == "Option, Damage Numbers")
                {
                    //var go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs\\ButtonSelection"), obj.transform);
                    //var go = (GameObject)GameObject.Instantiate(childObj.gameObject);

                    var newButton = new GameObject("new button");
                    newButton.transform.position = new Vector3(childObj.position.x, childObj.position.y, childObj.position.z);
                    newButton.transform.localScale = new Vector3(childObj.localScale.x, childObj.localScale.y, childObj.localScale.z);
                    newButton.transform.parent = obj.transform;
                    CopyComponent(childObj.GetComponent<UnityEngine.UI.Image>(), newButton);
                    CopyComponent(childObj.GetComponent<RoR2.UI.SelectableDescriptionUpdater>(), newButton);
                    CopyComponent(childObj.GetComponent<UnityEngine.UI.LayoutElement>(), newButton);
                    CopyComponent(childObj.GetComponent<RoR2.UI.SkinControllers.PanelSkinController>(), newButton);
                    /*var Image = childObj.GetComponent<UnityEngine.UI.Image>();
                    var newImage = newButton.AddComponent<UnityEngine.UI.Image>();
                    newImage.sprite = Image.sprite;
                    newImage.material = Image.material;
                    var SelectableDescriptionUpdater = childObj.GetComponent<RoR2.UI.SelectableDescriptionUpdater>();
                    var newSelectableDescriptionUpdater = newButton.AddComponent<RoR2.UI.SelectableDescriptionUpdater>();
                    newSelectableDescriptionUpdater.selectableDescriptionToken = SelectableDescriptionUpdater.selectableDescriptionToken;
                    newSelectableDescriptionUpdater.languageTextMeshController = SelectableDescriptionUpdater.languageTextMeshController;
                    var layout = childObj.GetComponent<UnityEngine.UI.LayoutElement>();
                    var newLayout = newButton.AddComponent<UnityEngine.UI.LayoutElement>();
                    newLayout.minHeight = layout.minHeight;
                    newLayout.minWidth = layout.minWidth;
                    newLayout.preferredHeight = layout.preferredHeight;
                    newLayout.preferredWidth = layout.preferredWidth;
                    newLayout.flexibleHeight = layout.flexibleHeight;
                    newLayout.flexibleWidth = layout.flexibleWidth;
                    newLayout.ignoreLayout = layout.ignoreLayout;
                    newLayout.layoutPriority = layout.layoutPriority;
                    var PanelSkinController = childObj.GetComponent<RoR2.UI.SkinControllers.PanelSkinController>();
                    var newPanelSkinController = newButton.AddComponent<RoR2.UI.SkinControllers.PanelSkinController>();
                    newPanelSkinController.panelType = PanelSkinController.panelType;
                    newButton.transform.parent = obj.transform;
                    newButton.transform.position = new Vector3(childObj.position.x, childObj.position.y, childObj.position.z);
                    newButton.transform.localScale = new Vector3(childObj.localScale.x, childObj.localScale.y, childObj.localScale.z);*/

                    /*
                    var cornerRect = childObj.GetChild(0);
                    var newCornerRect = new GameObject("CornerRect");
                    newCornerRect.transform.SetParent(newButton.transform);
                    var CornerRectImage = cornerRect.GetComponent<UnityEngine.UI.Image>();
                    var newCornerRectImage = newCornerRect.AddComponent<UnityEngine.UI.Image>();
                    newCornerRectImage.sprite = CornerRectImage.sprite;
                    var cornerRectLayout = cornerRect.GetComponent<UnityEngine.UI.LayoutElement>();
                    var newCornerRectLayout = newCornerRect.AddComponent<UnityEngine.UI.LayoutElement>();
                    newCornerRectLayout.minHeight = cornerRectLayout.minHeight;
                    newCornerRectLayout.minWidth = cornerRectLayout.minWidth;                    
                    newCornerRectLayout.preferredHeight = cornerRectLayout.preferredHeight;
                    newCornerRectLayout.preferredWidth = cornerRectLayout.preferredWidth;

                    var text = childObj.GetChild(1);
                    var newText = new GameObject("Text, Name");
                    newText.transform.SetParent(newButton.transform);
                    var LanguageTextMeshController = text.GetComponent<RoR2.UI.LanguageTextMeshController>();
                    var newLanguageTextMeshController = newText.AddComponent<RoR2.UI.LanguageTextMeshController>();
                    var t = text.GetField<UnityEngine.UI.Text>("text");
                    t.text = "Droppable Items";
                    newLanguageTextMeshController.SetField("text", t);
                    var tm = text.GetField("textMesh");
                    tm.SetField("text", "Droppable Items");
                    newLanguageTextMeshController.SetField("textMesh", tm);
                    var tmp = text.GetField<TMPro.TextMeshPro>("textMeshPro");
                    tmp.text = "Droppable Items";
                    newLanguageTextMeshController.SetField("textMeshPro", tmp);
                    var tmpu = text.GetField<TMPro.TextMeshProUGUI>("textMeshProUGui");
                    tmpu.text = "Droppable Items";
                    newLanguageTextMeshController.SetField("textMeshProUGui", tmpu);*/

                }
            }
        }
        static bool False()
        {
            return false;
        }
        private void OnEnable()
        {
            /*RoR2.RuleCatalog.allCategoryDefs.FirstOrDefault(c => c.displayToken == "RULE_HEADER_ARTIFACTS").emptyTipToken =null;
            RoR2.RuleCatalog.allCategoryDefs.FirstOrDefault(c => c.displayToken == "RULE_HEADER_ITEMS").hiddenTest = new Func<bool>(False);
            RoR2.RuleCatalog.allCategoryDefs.FirstOrDefault(c => c.displayToken == "RULE_HEADER_EQUIPMENT").hiddenTest = new Func<bool>(False);
            RoR2.RuleCatalog.allCategoryDefs.FirstOrDefault(c => c.displayToken == "RULE_HEADER_MISC").hiddenTest = new Func<bool>(False);*/

            RoR2.LocalUserManager.GetFirstLocalUser().cachedMasterController.master.money = 1000;
            RoR2.LocalUserManager.GetFirstLocalUser().cachedBody.baseMoveSpeed = 40.0f;
            RoR2.LocalUserManager.GetFirstLocalUser().cachedBody.baseJumpCount = 5;

            RoR2.LocalUserManager.GetFirstLocalUser().cachedBody.healthComponent.godMode= true;
            RoR2.LocalUserManager.GetFirstLocalUser().cachedBody.baseDamage = 10000;

            /*var sb = new StringBuilder();
            var chests = UnityEngine.Object.FindObjectsOfType<RoR2.ChestBehavior>();
            foreach (var obj in chests)
            {
                sb.AppendLine(obj.GetField<RoR2.PickupIndex>("dropPickup").GetPickupNameToken());
                sb.AppendLine(RoR2.Language.GetString(obj.GetField<RoR2.PickupIndex>("dropPickup").GetPickupNameToken()));
                //recurseobj(sb, obj.gameObject, 0);
            }
            File.WriteAllText(@"GOs.txt", sb.ToString());*/
            /*var sb = new StringBuilder();
            var rootobj = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var obj in rootobj)
            {
                recurseobj(sb, obj, 0);
            }
            File.WriteAllText(@"GOs.txt", sb.ToString());*/
            //var cam = UnityEngine.Object.FindObjectOfType<RoR2.UICamera>();
            //var cam = UnityEngine.Object.FindObjectOfType<RoR2.UI.ItemInventoryDisplay>();

            // typeof(RoR2.Console).SetStaticField("maxMessages", new RoR2.ConVar.IntConVar("max_messages", RoR2.ConVarFlags.Archive, "250", "Maximum number of messages that can be held in the console log."));
            /*var sb = new StringBuilder();
            var stateInfoList = RoR2.RoR2Application.instance.stateManager.GetField("stateInfoList").GetList();
            foreach (var s in stateInfoList)
            {
                var vals = s.GetField("stateFieldList").GetList();
                var stateType = s.GetField<EntityStates.SerializableEntityStateType>("serializedType").stateType;
                sb.AppendLine(stateType.ToString());
                foreach(var val in vals)
                {
                    sb.AppendLine(stateType + " : " + val.GetField("_fieldName") + " : " + val.GetField("valueAsSystemObject"));
                }
            }
            File.WriteAllText(@"EntityStates.txt", sb.ToString());*/
        }
        private void OnGUI()
        {

        }
    }
}
