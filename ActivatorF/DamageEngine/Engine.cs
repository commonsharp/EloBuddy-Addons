using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MainMenuActivator;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using EloBuddy.SDK.Enumerations;

namespace ActivatorF.DamageEngine
{
    internal static class Engine
    {
        public static Menu DamageEngine;
        public static bool TakeAA;
        public static bool TakeSS;
        public static bool TakeSpell;
        public static bool TakeDangerSpell;

        public static List<SpellSlot> SpellSlots = new List<SpellSlot>()
        {
            SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R
        };
        public static List<MissileClient> Missile = new List<MissileClient>();

        public static int LastSpellCast;
        public static GameObject miss;

        public static void Init()
        {
            DamageEngine = MainActivator.Menu.AddSubMenu("Damage Engine (Advanced)", "damageengineactiv");

            DamageEngine.AddGroupLabel("Danger Settings");
            DamageEngine.AddLabel("HP Tracking");
            DamageEngine.Add("HPDangerSlider", new Slider("HP % For Danger", 15));
            DamageEngine.Add("EnemiesDangerSlider", new Slider("Enemies Around", 1, 0, 5));
            DamageEngine.Add("EnemiesDangerRange", new Slider("Range", 850, 1, 2000));
            DamageEngine.AddLabel("Damage Tracking");
            DamageEngine.Add("TrackDamage", new CheckBox("Track Incoming Damage"));
            DamageEngine.AddGroupLabel("Engine Settings");
            DamageEngine.Add("ConsiderSpells", new CheckBox("Consider Spells"));
            DamageEngine.Add("ConsiderSkillshots", new CheckBox("Consider Skillshots"));
            DamageEngine.Add("ConsiderTargeted", new CheckBox("Consider Targeted"));
            DamageEngine.Add("ConsiderAttacks", new CheckBox("Consider Basic Attacks"));
            DamageEngine.Add("ConsiderMinions", new CheckBox("Consider Non-Champions"));
            DamageEngine.AddGroupLabel("Missile Check (BETA - TESTING)");
            DamageEngine.Add("missilecheck", new CheckBox("Enable Missile Check"));
            DamageEngine.AddSeparator(-5);
            DamageEngine.Add("missilerun", new CheckBox("Only use Items if can run away", false));
            DamageEngine.AddLabel("Only if you can Flash out or can run out of missile range, defence items will be active. It needs more logic so don't tick unless you want to try.");

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += Obj_AI_Base_OnBasicAttack;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            var caster = sender as MissileClient;
            if (caster == null || !caster.Target.IsMe) return;
            Missile.Remove(caster);
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var caster = sender as MissileClient;
            if (caster == null || !caster.Target.IsMe) return;
            Missile.Add(caster);
        }

        private static void Obj_AI_Base_OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsAlly || !DamageEngine["TrackDamage"].Cast<CheckBox>().CurrentValue || !DamageEngine["ConsiderAttacks"].Cast<CheckBox>().CurrentValue || (sender.IsMinion() && !DamageEngine["ConsiderMinions"].Cast<CheckBox>().CurrentValue)) return;
            if (args.Target.IsMe)
            {
                TakeAA = true;
                Core.DelayAction(() => TakeAA = false, 80);
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var caster = sender as AIHeroClient;
            if (caster == null || caster.IsAlly || !SpellSlots.Contains(args.Slot) || !DamageEngine["ConsiderSpells"].Cast<CheckBox>().CurrentValue || !DamageEngine["TrackDamage"].Cast<CheckBox>().CurrentValue) return;
            foreach (var target in EntityManager.Heroes.Allies)
            {
                if (!args.Target.IsMe) return;

                var dangerSpell =
                        DangerousSpells.Spells.FirstOrDefault(
                            a => a.Champion == caster.Hero && args.Slot == a.Slot && Items.ItemManager.DefenceMenu[a.Champion.ToString() + a.Slot].Cast<CheckBox>().CurrentValue);
                if (dangerSpell != null)
                {
                    
                    var project = Player.Instance.Position.To2D().ProjectOn(args.Start.To2D(), args.End.To2D());

                    if (project.IsOnSegment &&
                        project.SegmentPoint.Distance(Player.Instance.Position) <=
                        args.SData.CastRadius + Player.Instance.BoundingRadius + 30)
                    {
                        Core.DelayAction(() => TakeDangerSpell = true, dangerSpell.BonusDelay);
                        Core.DelayAction(() => TakeDangerSpell = false, 80);
                        return;
                    }
                    TakeSS = true;
                    Core.DelayAction(() => TakeSS = false, 80);
                }

                if (args.Target != null && args.Target.IsMe && DamageEngine["ConsiderTargeted"].Cast<CheckBox>().CurrentValue || DamageEngine["ConsiderSkillshots"].Cast<CheckBox>().CurrentValue && args.End != Vector3.Zero && args.End.Distance(target) < 200)
                {
                    TakeSpell = true;
                    Core.DelayAction(() => TakeSpell = false, 80);
                    if (dangerSpell != null && TakeSpell == true)
                    {
                        TakeDangerSpell = true;
                        Core.DelayAction(() => TakeDangerSpell = false, 80);
                    }
                }
            }
        }

        public static bool InDanger(this AIHeroClient target)
        {
            if (Player.Instance.IsInShopRange()) return false;
            if (TakeDangerSpell) return true;
            if (TakeAA || TakeSpell || TakeSS)
            {
                if (target.HealthPercent <= DamageEngine["HPDangerSlider"].Cast<Slider>().CurrentValue)
                {
                    if (target.CountEnemiesInRange(DamageEngine["EnemiesDangerRange"].Cast<Slider>().CurrentValue) >= DamageEngine["EnemiesDangerSlider"].Cast<Slider>().CurrentValue) return true;
                    else return false;
                }
                else return false;
            }
            return false;
        }
        public static bool InMissileDanger(this AIHeroClient target)
        {
            if (DamageEngine["missilecheck"].Cast<CheckBox>().CurrentValue)
            {
                if (DamageEngine["missilerun"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var caster in Missile.Where(a => Player.Instance.IsInRange(a, 775)))
                    {
                        Spell.Skillshot _flash = new Spell.Skillshot(Player.Instance.GetSpellSlotFromName("summonerflash"), 425, SkillShotType.Linear);
                        if ((!Player.Instance.IsInRange(caster, 375) && Missile.Count > 0 && _flash.IsReady()) || (!_flash.IsReady() && Missile.Count > 0 && !Player.Instance.IsInRange(caster, 775 - Player.Instance.MoveSpeed))) return true;
                    }
                }
                else
                if (Missile.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}