using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class UnlockEverything : MonoBehaviour
    {
        private void OnEnable()
        {
            var unlockables = typeof(RoR2.UnlockableCatalog).GetField<Dictionary<String, RoR2.UnlockableDef>>("nameToDefTable");
            foreach (var unlockable in unlockables)
                RoR2.Run.instance.GrantUnlockToAllParticipatingPlayers(unlockable.Key);
            foreach (var networkUser in RoR2.NetworkUser.readOnlyInstancesList)
                networkUser.AwardLunarCoins(100);
            var achievementManager = RoR2.AchievementManager.GetUserAchievementManager(RoR2.LocalUserManager.GetFirstLocalUser());
            foreach (var achievement in RoR2.AchievementManager.allAchievementDefs)
                achievementManager.GrantAchievement(achievement);
            var profile = RoR2.LocalUserManager.GetFirstLocalUser().userProfile;
            foreach (var survivor in RoR2.SurvivorCatalog.allSurvivorDefs)
            {
                if (profile.statSheet.GetStatValueDouble(RoR2.Stats.PerBodyStatDef.totalTimeAlive, survivor.bodyPrefab.name) == 0.0)
                    profile.statSheet.SetStatValueFromString(RoR2.Stats.PerBodyStatDef.totalTimeAlive.FindStatDef(survivor.bodyPrefab.name), "0.1");
            }
            for (int i = 0; i < 150; i++)
            {
                profile.DiscoverPickup(new RoR2.PickupIndex((RoR2.ItemIndex)i));
                profile.DiscoverPickup(new RoR2.PickupIndex((RoR2.EquipmentIndex)i));
            }
            enabled = false;
        }
    }
}
