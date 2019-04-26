using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfShame
{
    public class AdvancedTooltips : MonoBehaviour
    {
        void OnEnable()
        {
            var itemDefs = typeof(RoR2.ItemCatalog).GetField<RoR2.ItemDef[]>("itemDefs");
            foreach (var itemDef in itemDefs)
                itemDef.pickupToken = itemDef.descriptionToken;
        }
        RoR2.CharacterBody Body;
        RoR2.Inventory Inventory;
        void Update()
        {
            var localUser = RoR2.LocalUserManager.GetFirstLocalUser();
            if (localUser == null || localUser.cachedMasterController == null || localUser.cachedMasterController.master == null) return;
            var master = localUser.cachedMasterController.master;
            Body = master.GetBody();
            Inventory = master.inventory;
            var huds = typeof(RoR2.UI.HUD).GetField<List<RoR2.UI.HUD>>("instancesList");
            foreach (var hud in huds)
            {
                var items = hud.itemInventoryDisplay.GetField<List<RoR2.UI.ItemIcon>>("itemIcons");
                foreach (var item in items)
                {
                    var def = RoR2.ItemCatalog.GetItemDef(item.GetField<RoR2.ItemIndex>("itemIndex"));
                    var index = item.GetField<RoR2.ItemIndex>("itemIndex");
                    var count = item.GetField<Int32>("itemCount");
                    item.tooltipProvider.overrideBodyText = RoR2.Language.GetString(def.descriptionToken) + GetExtraData(index, count);
                }
            }
        }
        Double Clover(Double orig, Int32 cloverCount)
        {
            return (1f - Math.Pow((1f - orig / 100.0f), cloverCount + 1)) * 100.0f;
        }
        String GetExtraData(RoR2.ItemIndex index, Int32 count)
        {
            var cloverCount = Inventory.GetItemCount(RoR2.ItemIndex.Clover);
            switch (index)
            {
                case RoR2.ItemIndex.None:
                    break;
                case RoR2.ItemIndex.Syringe:
                    return "<br><br>From Syringes : " + RoR2.Util.GenerateColoredString(count * 15 + "%", Color.green) +
                        "<br>Total Attack Speed Increase : " + RoR2.Util.GenerateColoredString(((Body.attackSpeed - 1) * 100).ToString() + "%", Color.green);
                case RoR2.ItemIndex.Bear:
                    var bearChance = (1f - 1f / (0.15f * count + 1f)) * 100f;
                    return "<br><br>Chance to Block : " + RoR2.Util.GenerateColoredString(bearChance.ToString("0.###") + "%", Color.green) +
                          "<br><br>Picking up another : " + RoR2.Util.GenerateColoredString(((1f - 1f / (0.15f * (count + 1f) + 1f)) * 100f).ToString("0.###") + "%", Color.green);
                case RoR2.ItemIndex.Behemoth:
                    return "<br><br>Actual Radius (tooltip is incorrect) : " + RoR2.Util.GenerateColoredString((1.5f + 2.5f * count) + "m", Color.green) +
                         "<br>Damage : " + RoR2.Util.GenerateColoredString((Body.damage * 0.6f).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.Missile:
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(0.1f, cloverCount).ToString("0.###"), Color.green) +
                         "<br>Damage : " + RoR2.Util.GenerateColoredString((Body.damage * 3.0f * count).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.ExplodeOnDeath:
                    return "<br><br>Radius : " + RoR2.Util.GenerateColoredString((12f + 2.4f * (count - 1)) + "m", Color.green) +
                         "<br>Damage : " + RoR2.Util.GenerateColoredString((Body.damage * 3.5f * (1f + (count - 1) * 0.8f)).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.Dagger:
                    return "<br><br>3 Daggers<br>Damage (each), can crit : " + RoR2.Util.GenerateColoredString((Body.damage * 1.5f).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.Tooth:
                    return "<br><br>Healing : " + RoR2.Util.GenerateColoredString((4 * count).ToString("0.###"), Color.green) +
                        "<br>Size? : " + RoR2.Util.GenerateColoredString(Math.Pow(count, 0.25f).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.CritGlasses:
                    return "<br><br>From Glasses : " + RoR2.Util.GenerateColoredString((0.1f * count).ToString("0.###"), Color.green) +
                        "<br>Total Crit : " + RoR2.Util.GenerateColoredString((Body.crit).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.Hoof:
                    return "<br><br>From Hoofs : " + RoR2.Util.GenerateColoredString((0.14f * count).ToString("0.###"), Color.green) +
                        "<br>Total Move Speed : " + RoR2.Util.GenerateColoredString((Body.moveSpeed).ToString("0.###"), Color.green) + "<br><br>Quiet foot steps...";
                case RoR2.ItemIndex.Feather:
                    return "<br><br>Total Extra Jumps : " + RoR2.Util.GenerateColoredString((Body.maxJumpCount - 1).ToString(), Color.green);
                case RoR2.ItemIndex.AACannon:
                    break;
                case RoR2.ItemIndex.ChainLightning:
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(0.25f, cloverCount).ToString("0.###"), Color.green) +
                         "<br>Damage : " + RoR2.Util.GenerateColoredString((Body.damage * 0.8f).ToString("0.###"), Color.green) +
                         "<br>Bounces : " + RoR2.Util.GenerateColoredString((2 * count).ToString(), Color.green) +
                         "<br>Range : " + RoR2.Util.GenerateColoredString((2 * count).ToString() + "m", Color.green);
                case RoR2.ItemIndex.PlasmaCore:
                    break;
                case RoR2.ItemIndex.Seed:
                    return "<br><br>Heal on Crit : " + RoR2.Util.GenerateColoredString(count.ToString("0.###"), Color.green) +
                        "<br>Crit Chance : " + RoR2.Util.GenerateColoredString(Body.crit.ToString("0.###"), Color.green);
                case RoR2.ItemIndex.Icicle:
                    return "<br><br>Damage : " + RoR2.Util.GenerateColoredString((Body.damage * (0.5f + 0.5f * count) * 0.25f).ToString("0.###"), Color.green) +
                         "<br>Time : 5s per kill, stacks";
                case RoR2.ItemIndex.GhostOnKill:
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(0.1f, cloverCount).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.Mushroom:
                    return "<br><br>Radius : " + RoR2.Util.GenerateColoredString((1.5f + 1.5f * count) + "m", Color.green) +
                         "<br>Healing : " + RoR2.Util.GenerateColoredString((0.0225f + 0.0225f * count).ToString("0.###") + "%/s", Color.green) +
                         "<br>Of target, not owner";
                case RoR2.ItemIndex.Crowbar:
                    return "<br><br>Multiplier : " + RoR2.Util.GenerateColoredString((1.5f + 0.3f * (count - 1)).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.LevelBonus:
                    break;
                case RoR2.ItemIndex.AttackSpeedOnCrit:
                    return "<br><br>Crit Chance : " + RoR2.Util.GenerateColoredString(Body.crit.ToString("0.###"), Color.green) +
                        "<br>Total Attack Speed : " + RoR2.Util.GenerateColoredString(((Body.attackSpeed - 1) * 100).ToString() + "%", Color.green) +
                        "<br>Duration : 2s per crit";
                case RoR2.ItemIndex.BleedOnHit:
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString((Clover(0.15f * count, cloverCount) * 100.0f).ToString("0.###") + "%", Color.green) +
                        "<br>Damage : tbd, weird math" +
                        "<br>Duration : 3s";
                case RoR2.ItemIndex.SprintOutOfCombat:
                    return "<br><br>Multipler : " + RoR2.Util.GenerateColoredString((0.3f * count).ToString("0.###"), Color.green) +
                        "<br>Total Move Speed : " + RoR2.Util.GenerateColoredString((Body.moveSpeed).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.FallBoots:
                    return "<br><br>Cooldown : " + RoR2.Util.GenerateColoredString((EntityStates.Headstompers.HeadstompersCooldown.baseDuration / count).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.CooldownOnCrit:
                    break;
                case RoR2.ItemIndex.WardOnLevel:
                    return "<br><br>Radius : " + RoR2.Util.GenerateColoredString((8f + 8f * (count)) + "m", Color.green);
                case RoR2.ItemIndex.Phasing:
                    return "<br><br>Based on enemy damage, affected by clover" +
                        "<br> Duration, Speed : " + RoR2.Util.GenerateColoredString((1.5f + 1.5f * (count)).ToString(), Color.green);
                case RoR2.ItemIndex.HealOnCrit:
                    return "Healing : " + RoR2.Util.GenerateColoredString((4 * 4 * count).ToString(), Color.green) +
                        "<br>Crit : " + RoR2.Util.GenerateColoredString((5 * count).ToString("0.###") + "%", Color.green) +
                        "<br>Total Crit : " + RoR2.Util.GenerateColoredString(Body.crit.ToString("0.###") + "%", Color.green);
                case RoR2.ItemIndex.HealWhileSafe:
                    return "<br><br>Regen : " + RoR2.Util.GenerateColoredString((2.5f + (count - 1) * 1.5f).ToString("0.###"), Color.green) +
                        "<br>Total Regen : " + RoR2.Util.GenerateColoredString(Body.regen.ToString("0.###") + "%", Color.green);
                case RoR2.ItemIndex.TempestOnKill:
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(0.25f, cloverCount).ToString("0.###"), Color.green) +
                        "<br>Duration : " + RoR2.Util.GenerateColoredString((2f + 6f * count).ToString("0.###") + "s", Color.green);
                case RoR2.ItemIndex.PersonalShield:
                    return "<br><br>Shield : " + RoR2.Util.GenerateColoredString((25 * count).ToString("0.###"), Color.green) +
                        "<br>Total Shield : " + RoR2.Util.GenerateColoredString(Body.maxShield.ToString("0.###") + "s", Color.green);
                case RoR2.ItemIndex.EquipmentMagazine:
                    var cd = Math.Pow(0.85f, count);
                    cd *= Math.Pow(0.5f, Inventory.GetItemCount(RoR2.ItemIndex.AutoCastEquipment));
                    return "<br><br>Cooldown : " + RoR2.Util.GenerateColoredString((1 - cd).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.NovaOnHeal:
                    return "";
                case RoR2.ItemIndex.ShockNearby:
                    return "<br><br>Clover affects crit<br>Bounces : " + RoR2.Util.GenerateColoredString((2 * count).ToString(), Color.green);
                case RoR2.ItemIndex.Infusion:
                    return "<br><br>Bonus : " + RoR2.Util.GenerateColoredString(Inventory.infusionBonus.ToString(), Color.green) + " / " + RoR2.Util.GenerateColoredString((100 * count).ToString(), Color.green);
                case RoR2.ItemIndex.WarCryOnCombat:
                    break;
                case RoR2.ItemIndex.Clover:
                    break;
                case RoR2.ItemIndex.Medkit:
                    return "Healing : " + RoR2.Util.GenerateColoredString((10 * count).ToString(), Color.green);
                case RoR2.ItemIndex.Bandolier:
                    var bando = (1f - 1f / Mathf.Pow((float)(count + 1), 0.33f));
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(bando, cloverCount).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.BounceNearby:
                    var meathook = 1f - 100f / (100f + 20f * count);
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(meathook, cloverCount).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.IgniteOnKill:
                    return "";
                case RoR2.ItemIndex.PlantOnHit:
                    break;
                case RoR2.ItemIndex.StunChanceOnHit:
                    var stun = 1f - 1f / (0.05f * count + 1f);
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(stun, cloverCount).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.Firework:
                    return "";
                case RoR2.ItemIndex.LunarDagger:
                    return "";
                case RoR2.ItemIndex.GoldOnHit:
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(.30f, cloverCount).ToString("0.###"), Color.green) +
                        "<br>Gold : " + RoR2.Util.GenerateColoredString((2f * count * RoR2.Run.instance.difficultyCoefficient).ToString("0.###"), Color.yellow);
                case RoR2.ItemIndex.MageAttunement:
                    break;
                case RoR2.ItemIndex.WarCryOnMultiKill:
                    return "";
                case RoR2.ItemIndex.BoostHp:
                    break;
                case RoR2.ItemIndex.BoostDamage:
                    break;
                case RoR2.ItemIndex.ShieldOnly:
                    return "";
                case RoR2.ItemIndex.AlienHead:
                    cd = Math.Pow(0.75f, count);
                    return "<br><br>Cooldown : " + RoR2.Util.GenerateColoredString((1 - cd).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.Talisman:
                    return "";
                case RoR2.ItemIndex.Knurl:
                    break;
                case RoR2.ItemIndex.BeetleGland:
                    break;
                case RoR2.ItemIndex.BurnNearby:
                    break;
                case RoR2.ItemIndex.CritHeal:
                    break;
                case RoR2.ItemIndex.CrippleWardOnLevel:
                    break;
                case RoR2.ItemIndex.SprintBonus:
                    return "<br><br>Total Move Speed : " + RoR2.Util.GenerateColoredString((Body.moveSpeed).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.SecondarySkillMagazine:
                    break;
                case RoR2.ItemIndex.StickyBomb:
                    return "<br><br>Chance : " + RoR2.Util.GenerateColoredString(Clover(2.5f + 2.5f * count, cloverCount).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.TreasureCache:
                    var totalWeight = (80f + 20f * count + Math.Pow(count, 2f)) / 100.0f;
                    var totalWeight2 = (80f + 20f * (count + 1) + Math.Pow(count + 1, 2f)) / 100.0f;
                    return "<br><br>Every stage will have a rusted lockbox." +
                        "<br>Chance for White : " + RoR2.Util.GenerateColoredString((80f / totalWeight).ToString("0.###") + "%", Color.green) +
                        "<br>Chance for Green : " + RoR2.Util.GenerateColoredString((20f * count / totalWeight).ToString("0.###") + "%", Color.green) +
                        "<br>Chance for Red : " + RoR2.Util.GenerateColoredString((Math.Pow(count, 2) / totalWeight).ToString("0.###") + "%", Color.green) +
                        "<br><br>If you get one more key..." +
                        "<br>Chance for White : " + RoR2.Util.GenerateColoredString((80f / totalWeight2).ToString("0.###") + "%", Color.green) +
                        "<br>Chance for Green : " + RoR2.Util.GenerateColoredString((20f * (count + 1) / totalWeight2).ToString("0.###") + "%", Color.green) +
                        "<br>Chance for Red : " + RoR2.Util.GenerateColoredString((Math.Pow(count + 1, 2) / totalWeight2).ToString("0.###") + "%", Color.green);
                case RoR2.ItemIndex.BossDamageBonus:
                    break;
                case RoR2.ItemIndex.SprintArmor:
                    break;
                case RoR2.ItemIndex.IceRing:
                    return "<br><br>Chance (shares proc with Kjaro's Band) : " + RoR2.Util.GenerateColoredString(Clover(8f, cloverCount).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.FireRing:
                    return "<br><br>Chance (shares proc with Runald's Band) : " + RoR2.Util.GenerateColoredString(Clover(8f, cloverCount).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.SlowOnHit:
                    break;
                case RoR2.ItemIndex.ExtraLife:
                    break;
                case RoR2.ItemIndex.ExtraLifeConsumed:
                    break;
                case RoR2.ItemIndex.UtilitySkillMagazine:
                    break;
                case RoR2.ItemIndex.HeadHunter:
                    break;
                case RoR2.ItemIndex.KillEliteFrenzy:
                    break;
                case RoR2.ItemIndex.RepeatHeal:
                    break;
                case RoR2.ItemIndex.Ghost:

                    //"<br><br>Spooky sound on kill, spooky ghost...";
                    break;
                case RoR2.ItemIndex.HealthDecay:
                    break;
                case RoR2.ItemIndex.AutoCastEquipment:
                    cd = Math.Pow(0.85f, count);
                    cd *= Math.Pow(0.5f, Inventory.GetItemCount(RoR2.ItemIndex.AutoCastEquipment));
                    return "<br><br>Cooldown : " + RoR2.Util.GenerateColoredString((1 - cd).ToString("0.###"), Color.green);
                case RoR2.ItemIndex.IncreaseHealing:
                    break;
                case RoR2.ItemIndex.JumpBoost:
                    break;
                case RoR2.ItemIndex.DrizzlePlayerHelper:
                    break;
                case RoR2.ItemIndex.Count:
                    break;
                default:
                    break;
            }
            return "";
        }
    }
}
