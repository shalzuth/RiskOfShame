using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiskOfShame
{
    public class EnemySurvivors : MonoBehaviour
    {
        String[] Survivors = new String[]
        {
            "Bandit",
            "HAND",
            "Beetle",
            "BeetleGuard",
            "Bison",
            "MegaDrone",
            "Golem",
            "GreaterWisp",
            "HermitCrab",
            "ImpBoss",
            "Imp",
            "LemurianBruiser",
            "Lemurian",
            "Titan"
        };
        List<RoR2.SurvivorDef> initial;
        void Awake()
        {
            initial = typeof(RoR2.SurvivorCatalog).GetStaticField<RoR2.SurvivorDef[]>("survivorDefs").ToList();
        }
        void OnDisable()
        {
            var survivorDefs = typeof(RoR2.SurvivorCatalog).GetStaticField<RoR2.SurvivorDef[]>("survivorDefs");
            int i = 0;
            foreach(var def in initial)
                survivorDefs[i++] = def;
        }
        Int32 ii = 0;
        void OnEnable()
        {
            ///charSelect.enabled = false;
           /* var survivorDefs = typeof(RoR2.SurvivorCatalog).GetStaticField<RoR2.SurvivorDef[]>("survivorDefs");
            for(int i = 0; i < 6; i++)
            {
                if (ii + i >= Survivors.Length)
                    continue;
                var characterObject = UnityEngine.Object.Instantiate(RoR2.BodyCatalog.FindBodyPrefab(Survivors[ii + i] + "Body"));
                UnityEngine.Object.DontDestroyOnLoad(characterObject);
                characterObject.SetActive(false);
                var newSurvivorDef = new RoR2.SurvivorDef
                {
                    bodyPrefab = characterObject,
                    displayPrefab = Resources.Load<GameObject>("Prefabs/CharacterDisplays/" + Survivors[ii + i] + "Display"),
                    descriptionToken = Survivors[ii + i].ToUpper() + "_DESCRIPTION",
                    primaryColor = new Color(0.423529416f, 0.819607854f, 0.917647064f),
                    unlockableName = ""
                };
                survivorDefs[i++] = newSurvivorDef;
            }
            ii += 6;
            if (ii >= Survivors.Length)
                ii = 0;*/

            /*
            var newSurvivorDef = new RoR2.SurvivorDef
            {
                bodyPrefab = RoR2.BodyCatalog.FindBodyPrefab("MercBody"),
                displayPrefab = Resources.Load<GameObject>("Prefabs/CharacterDisplays/MercDisplay"),
                descriptionToken = "MERC_DESCRIPTION",
                primaryColor = new Color(0.423529416f, 0.819607854f, 0.917647064f),
                unlockableName = "Characters.Mercenary"
            };*/
            //typeof(RoR2.SurvivorCatalog).SetStaticField("survivorMaxCount", 8);
            /*typeof(RoR2.SurvivorCatalog).Invoke("RegisterSurvivor", newSurvivorDef);
            var survivors = RoR2.ViewablesCatalog.FindNode("Survivors");
            var newSurvivor = new RoR2.ViewablesCatalog.Node(RoR2.SurvivorIndex.Engineer.ToString(), false, survivors);
            newSurvivor.shouldShowUnviewed = ((RoR2.UserProfile a) => true);//

            var newSurvivorOrder = RoR2.SurvivorCatalog.idealSurvivorOrder.ToList();
            newSurvivorOrder.Add((RoR2.SurvivorIndex)8);
            RoR2.SurvivorCatalog.idealSurvivorOrder = newSurvivorOrder.ToArray();
            */
            //var charSelect = UnityEngine.Object.FindObjectOfType<RoR2.CharacterSelectBarController>();
            //charSelect.Invoke("Build");
            /*
            var survivorIconControllers = charSelect.GetField<RoR2.UI.UIElementAllocator<RoR2.UI.SurvivorIconController>>("survivorIconControllers");
            var elements = survivorIconControllers.elements;
            elements[elements.Count - 1].survivorIndex = RoR2.SurvivorIndex.Count;*/
        }
    }
}