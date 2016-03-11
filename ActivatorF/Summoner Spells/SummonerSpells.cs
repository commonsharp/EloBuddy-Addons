using System;
using System.Drawing;
using System.Linq;
using ActivatorF.DamageEngine;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Enumerations;
using MainMenuActivator;

namespace ActivatorF.Summoner_Spells
{
    internal class SummonerSpells
    {
        private static Spell.Targeted _ignite;
        private static Spell.Targeted _heal;
        private static Spell.Targeted _exhaust;
        private static Spell.Skillshot _poro;
        private static Spell.Skillshot _snowball;
        private static Spell.Active _barrier;
        public static Spell.Targeted Smite;
        private static Menu _summonerMenu;
        private static Menu _smiteMenu;
        private static string[] SmiteNames = new[] { "s5_summonersmiteplayerganker", "itemsmiteaoe", "s5_summonersmitequick", "s5_summonersmiteduel", "summonersmite" };

        public static void Init()
        {
            _summonerMenu = MainActivator.Menu.AddSubMenu("Summoner Spells");
            if (HasSpell("summonerdot"))
            {
                _summonerMenu.AddGroupLabel("Ignite Settings");
                _summonerMenu.Add("useIgnite", new CheckBox("Use Ignite"));
                _summonerMenu.Add("comboOnlyIgnite", new CheckBox("Combo Only"));
                _summonerMenu.Add("drawIngiteRange", new CheckBox("Draw Ignite Range"));
                _summonerMenu.AddSeparator();
                _ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
                Game.OnTick += IgniteEvent;
                Chat.Print("ActivatorF: Ignite Loaded.", Color.LimeGreen);
            }
            if (HasSpell("summonerexhaust"))
            {
                _summonerMenu.AddGroupLabel("Exhaust Settings");
                _summonerMenu.Add("useExhaust", new CheckBox("Use Exhaust"));
                _summonerMenu.Add("comboOnlyExhaust", new CheckBox("Combo Only"));
                _summonerMenu.Add("drawExhaustRange", new CheckBox("Draw Exhaust Range"));
                foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy))
                {
                    _summonerMenu.Add(source.ChampionName + "exhaust",
                        new CheckBox("Exhaust " + source.ChampionName, true));
                }
                _summonerMenu.AddSeparator();
                _exhaust = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerexhaust"), 650);
                Game.OnTick += ExhaustEvent;
                Chat.Print("ActivatorF: Exhaust Loaded.", Color.OrangeRed);
            }
            if (HasSpell("summonerheal"))
            {
                _summonerMenu.AddGroupLabel("Heal Settings");
                _summonerMenu.Add("useHeal", new CheckBox("Use Heal"));
                _summonerMenu.Add("comboOnlyHeal", new CheckBox("Combo Only"));
                _summonerMenu.Add("drawHealRange", new CheckBox("Draw Heal Range"));
                _summonerMenu.AddLabel("Champions");
                foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsAlly && !a.IsMe))
                {
                    _summonerMenu.Add(source.ChampionName + "heal", new CheckBox("Heal " + source.ChampionName, false));
                }
                _summonerMenu.AddSeparator();
                _heal = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerheal"), 850);
                Game.OnTick += HealEvent;
                Chat.Print("ActivatorF: Heal Loaded.", Color.Aqua);
            }
            if (HasSpell("summonerbarrier"))
            {
                _summonerMenu.AddGroupLabel("Barrier Settings");
                _summonerMenu.Add("useBarrier", new CheckBox("Use Barrier"));
                _summonerMenu.Add("comboOnlyBarrier", new CheckBox("Combo Only"));
                _summonerMenu.AddSeparator();
                _barrier = new Spell.Active(ObjectManager.Player.GetSpellSlotFromName("summonerbarrier"), int.MaxValue);
                Game.OnTick += BarrierEvent;
                Chat.Print("ActivatorF: Barrier Loaded.", Color.GreenYellow);
            }
            if (HasSpell("summonersnowball"))
            {
                _summonerMenu.AddGroupLabel("Snowball Settings");
                _summonerMenu.Add("useSnowball", new CheckBox("Auto Mark", true));
                _summonerMenu.Add("useSnowballKS", new CheckBox("Use to last hit champs only", false));
                _summonerMenu.AddLabel("Auto Mark has already included last hit, but success rate is low. So there is a priority for last hit only");
                _summonerMenu.Add("csbrange", new Slider("Customize throw range", 1500, 1, 1600));
                int rsbrange = _summonerMenu["csbrange"].Cast<Slider>().CurrentValue;
                _summonerMenu.Add("drawSnowballRange", new CheckBox("Draw Snowball Range", true));
                _summonerMenu.Add("drawSnowballRRange", new CheckBox("Draw REAL Snowball Range", false));
                _snowball = new Spell.Skillshot(ObjectManager.Player.GetSpellSlotFromName("summonersnowball"), (uint)rsbrange, SkillShotType.Linear, 0, 1500, 60);
                Game.OnTick += SnowballEvent;
                Chat.Print("ActivatorF: Snowball Loaded.", Color.DeepSkyBlue);
            }
            if (HasSpell("summonerporothrow"))
            {
                _summonerMenu.AddGroupLabel("Poro Throw Settings");
                _summonerMenu.Add("usePoro", new CheckBox("Auto Poro Toss"));
                _summonerMenu.Add("usePoroKS", new CheckBox("Use to last hit champs only", false));
                _summonerMenu.AddLabel("Auto Poro Toss has already included last hit, but success rate is low. So there is a priority for last hit only");
                _summonerMenu.Add("cpororange", new Slider("Customize throw range", 2200, 1, 2500));
                int rpororange = _summonerMenu["cpororange"].Cast<Slider>().CurrentValue;
                _summonerMenu.Add("drawPoroRange", new CheckBox("Draw Poro Throwing Range", true));
                _summonerMenu.Add("drawPoroRRange", new CheckBox("Draw REAL Poro Throwing Range", false));
                _poro = new Spell.Skillshot(ObjectManager.Player.GetSpellSlotFromName("summonerporothrow"), (uint)rpororange, SkillShotType.Linear, 0, 1500, 60);
                Game.OnTick += PoroEvent;
                Chat.Print("ActivatorF: Poro Loaded.", Color.DeepSkyBlue);
            }
            if (HasSpell("smite"))
            {
                _smiteMenu = MainActivator.Menu.AddSubMenu("Smite Settings");
                switch (Game.MapId)
                {
                    case GameMapId.SummonersRift:
                    {
                        _smiteMenu.AddGroupLabel("Camps");
                        _smiteMenu.AddLabel("Epics");
                        _smiteMenu.Add("SRU_Baron", new CheckBox("Baron"));
                        _smiteMenu.Add("SRU_Dragon", new CheckBox("Dragon"));
                        _smiteMenu.AddLabel("Buffs");
                        _smiteMenu.Add("SRU_Blue", new CheckBox("Blue"));
                        _smiteMenu.Add("SRU_Red", new CheckBox("Red"));
                        _smiteMenu.AddLabel("Small Camps");
                        _smiteMenu.Add("SRU_Gromp", new CheckBox("Gromp", false));
                        _smiteMenu.Add("SRU_Murkwolf", new CheckBox("Murkwolf", false));
                        _smiteMenu.Add("SRU_Krug", new CheckBox("Krug", false));
                        _smiteMenu.Add("SRU_Razorbeak", new CheckBox("Razerbeak", false));
                        _smiteMenu.Add("Sru_Crab", new CheckBox("Skuttles", false));
                        break;
                    }

                    case GameMapId.TwistedTreeline:
                    {
                        _smiteMenu.Add("TT_Spiderboss", new CheckBox("Vilemaw"));
                        _smiteMenu.Add("TTNGolem", new CheckBox("Golem"));
                        _smiteMenu.Add("TTNWolf", new CheckBox("Wolf"));
                        _smiteMenu.Add("TTNWraith", new CheckBox("Wraith"));
                        break;
                    }
                }
                _smiteMenu.AddSeparator();
                _smiteMenu.Add("drawSmiteRange", new CheckBox("Draw Smite Range"));
                _smiteMenu.Add("smiteActive",
                    new KeyBind("Smite Active (toggle)", true, KeyBind.BindTypes.PressToggle, 'H'));
                _smiteMenu.AddSeparator();
                _smiteMenu.Add("useSlowSmite", new CheckBox("KS with Slow Smite"));
                _smiteMenu.Add("comboWithDuelSmite", new CheckBox("Combo With Duel Smite"));

                if (SmiteNames.Contains(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name))
                {
                    Smite = new Spell.Targeted(SpellSlot.Summoner1, 500);
                }
                if (SmiteNames.Contains(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name))
                {
                    Smite = new Spell.Targeted(SpellSlot.Summoner2, 500);
                }
                Game.OnUpdate += SmiteEvent;
                Chat.Print("ActivatorF: Smite Loaded.", Color.Yellow);
            }
            Drawing.OnDraw += Drawing_OnDraw;
        }

        #region Barrier
        private static void BarrierEvent(EventArgs args)
        {
            if (!_barrier.IsReady() || Player.Instance.IsDead) return;
            if (!_summonerMenu["useBarrier"].Cast<CheckBox>().CurrentValue || _summonerMenu["comboOnlyBarrier"].Cast<CheckBox>().CurrentValue &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                return;
            if (Player.Instance.InDanger() == true)
            {
                _barrier.Cast();
                return;
            }
        }
        #endregion

        #region Smite
        private static void SmiteEvent(EventArgs args)
        {
            Smiter.SetSmiteSlot();
            if (!Smite.IsReady() || Player.Instance.IsDead) return;
            if (_smiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue)
            {
                var unit =
                    EntityManager.MinionsAndMonsters.Monsters
                        .Where(
                            a =>
                                Smiter.SmiteableUnits.Contains(a.BaseSkinName) && a.Health <= Smiter.GetSmiteDamage() &&
                                _smiteMenu[a.BaseSkinName].Cast<CheckBox>().CurrentValue)
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();

                if (unit != null)
                {
                    Smite.Cast(unit);
                    return;
                }
            }
            if (_smiteMenu["useSlowSmite"].Cast<CheckBox>().CurrentValue &&
                Smite.Handle.Name == "s5_summonersmiteplayerganker")
            {
                foreach (
                    var target in
                        EntityManager.Heroes.Enemies
                            .Where(h => h.IsValidTarget(Smite.Range) && h.Health <= 20 + 8 * Player.Instance.Level))
                {
                    Smite.Cast(target);
                    return;
                }
            }
            if (_smiteMenu["comboWithDuelSmite"].Cast<CheckBox>().CurrentValue &&
                Smite.Handle.Name == "s5_summonersmiteduel" &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                foreach (
                    var target in
                        EntityManager.Heroes.Enemies
                            .Where(h => h.IsValidTarget(Smite.Range)).OrderByDescending(TargetSelector.GetPriority))
                {
                    Smite.Cast(target);
                    return;
                }
            }
        }
        #endregion

        #region Heal

        private static void HealEvent(EventArgs args)
        {
            if (!_heal.IsReady() || Player.Instance.IsDead) return;
            if (!_summonerMenu["useHeal"].Cast<CheckBox>().CurrentValue || _summonerMenu["comboOnlyHeal"].Cast<CheckBox>().CurrentValue &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                return;
            foreach (
                var source in
                    from source in
                        ObjectManager.Get<AIHeroClient>().Where(a => a.IsAlly && a.Distance(Player.Instance) < _heal.Range && !a.IsDead)
                    where
                        (source.IsMe || _summonerMenu[source.ChampionName + "heal"].Cast<CheckBox>().CurrentValue) && source.InDanger() == true
                    select source)
            {
                _heal.Cast(source);
                Engine.LastSpellCast = Environment.TickCount + 500;
                return;
            }
        }

        #endregion

        #region Exhaust

        private static void ExhaustEvent(EventArgs args)
        {
            if (!_exhaust.IsReady() || Player.Instance.IsDead) return;
            if (!_summonerMenu["useExhaust"].Cast<CheckBox>().CurrentValue || _summonerMenu["comboOnlyExhaust"].Cast<CheckBox>().CurrentValue &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                return;
            foreach (
                var enemy in
                    EntityManager.Heroes.Enemies
                        .Where(a => a.IsValidTarget(_exhaust.Range))
                        .Where(enemy => _summonerMenu[enemy.ChampionName + "exhaust"].Cast<CheckBox>().CurrentValue))
            {
                if (enemy.IsFacing(Player.Instance))
                {
                    if (!(Player.Instance.HealthPercent < 50)) continue;
                    _exhaust.Cast(enemy);
                    return;
                }
                if (!(enemy.HealthPercent < 50)) continue;
                _exhaust.Cast(enemy);
                return;
            }
        }

        #endregion

        #region Ignite

        private static void IgniteEvent(EventArgs args)
        {
            if (!_ignite.IsReady() || Player.Instance.IsDead) return;
            if (!_summonerMenu["useIgnite"].Cast<CheckBox>().CurrentValue || _summonerMenu["comboOnlyIgnite"].Cast<CheckBox>().CurrentValue &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;
            foreach (
                var source in
                    EntityManager.Heroes.Enemies
                        .Where(
                            a => a.IsValidTarget(_ignite.Range) &&
                                a.Health < 50 + 20 * Player.Instance.Level - (a.HPRegenRate / 5 * 3)))
            {
                _ignite.Cast(source);
                return;
            }
        }

        #endregion

        #region Poro
        private static void PoroEvent(EventArgs args)
        {
            if (!_poro.IsReady() || Player.Instance.IsDead) return;
            if (_summonerMenu["usePoro"].Cast<CheckBox>().CurrentValue)
            {
                var poro1 = ObjectManager.Get<Obj_AI_Base>().OrderBy(a => a.Health)
                .Where(a => a.IsEnemy && !a.IsDead && a.IsValidTarget(_poro.Range) && !a.IsInvulnerable).FirstOrDefault();
                if (poro1.Health <= Player.Instance.Level * 10 + 20 && _poro.GetPrediction(poro1).HitChance >= HitChance.High && _poro.Name != "porothrowfollowupcast")
                {
                    foreach (
                        var source in
                            EntityManager.Heroes.Enemies
                                .Where(a => a.IsValidTarget(_poro.Range) && a.Health < Player.Instance.Level * 10 + 20))
                    {
                        _poro.Cast(source);
                        return;
                    }
                }
                else if (_poro.GetPrediction(poro1).HitChance >= HitChance.High && _poro.Name != "porothrowfollowupcast" && !_summonerMenu["usePoroKS"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (
                        var source in
                            EntityManager.Heroes.Enemies
                                .Where(a => a.IsValidTarget(_poro.Range)))
                    {
                        _poro.Cast(source);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Snowball

        private static void SnowballEvent(EventArgs args)
        {
            if (!_snowball.IsReady() || Player.Instance.IsDead) return;
            if (_summonerMenu["useSnowball"].Cast<CheckBox>().CurrentValue)
            {
                var sb1 = ObjectManager.Get<Obj_AI_Base>().OrderBy(a => a.Health)
                .Where(a => a.IsEnemy && !a.IsDead && a.IsValidTarget(_snowball.Range) && !a.IsInvulnerable).FirstOrDefault();
                if (sb1.Health <= Player.Instance.Level * 5 + 10 && _snowball.GetPrediction(sb1).HitChance >= HitChance.High && _snowball.Name != "snowballfollowupcast")
                {
                    foreach (
                        var source in
                            EntityManager.Heroes.Enemies
                                .Where(a => a.IsValidTarget(_snowball.Range) && a.Health < Player.Instance.Level * 5 + 10))
                    {
                        _snowball.Cast(source);
                        return;
                    }
                }
                else if (_snowball.GetPrediction(sb1).HitChance >= HitChance.High && _snowball.Name != "snowballfollowupcast" && !_summonerMenu["useSnowballKS"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (
                        var source in
                            EntityManager.Heroes.Enemies
                                .Where(a => a.IsValidTarget(_snowball.Range)))
                    {
                        _snowball.Cast(source);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Drawing

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (HasSpell("summonerexhaust") && _summonerMenu["drawExhaustRange"].Cast<CheckBox>().CurrentValue &&
                _exhaust.IsReady())
            {
                Circle.Draw(SharpDX.Color.OrangeRed, _exhaust.Range, Player.Instance.Position);
            }
            if (HasSpell("summonerheal") && _summonerMenu["drawHealRange"].Cast<CheckBox>().CurrentValue &&
                _heal.IsReady())
            {
                Circle.Draw(SharpDX.Color.DeepSkyBlue, _heal.Range, Player.Instance.Position);
            }
            if (HasSpell("smite") && _smiteMenu["drawSmiteRange"].Cast<CheckBox>().CurrentValue && _smiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue)
            {
                Circle.Draw(SharpDX.Color.Yellow, Smite.Range, Player.Instance.Position);
            }
            if (HasSpell("summonerdot") && _summonerMenu["drawIngiteRange"].Cast<CheckBox>().CurrentValue &&
                _ignite.IsReady())
            {
                Circle.Draw(SharpDX.Color.Red, _ignite.Range, Player.Instance.Position);
            }
            if (HasSpell("summonerporothrow") && _summonerMenu["drawPoroRRange"].Cast<CheckBox>().CurrentValue &&
                _poro.IsReady())
            {
                Circle.Draw(SharpDX.Color.DeepSkyBlue, 2500, Player.Instance.Position);
            }
            if (HasSpell("summonersnowball") && _summonerMenu["drawSnowballRange"].Cast<CheckBox>().CurrentValue &&
                _snowball.IsReady())
            {
                Circle.Draw(SharpDX.Color.DeepSkyBlue, _snowball.Range, Player.Instance.Position);
            }
            if (HasSpell("summonerporothrow") && _summonerMenu["drawPoroRange"].Cast<CheckBox>().CurrentValue &&
                _poro.IsReady())
            {
                Circle.Draw(SharpDX.Color.Red, _summonerMenu["cpororange"].Cast<Slider>().CurrentValue, Player.Instance.Position);
            }
            if (HasSpell("summonersnowball") && _summonerMenu["drawSnowballRRange"].Cast<CheckBox>().CurrentValue &&
                _snowball.IsReady())
            {
                Circle.Draw(SharpDX.Color.Red, _summonerMenu["csbrange"].Cast<Slider>().CurrentValue, Player.Instance.Position);
            }
        }

        #endregion

        #region Util

        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        #endregion
    }
}