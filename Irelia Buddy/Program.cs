using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace Irelia_Buddy
{
    class Program
    {
        private static readonly AIHeroClient Player = ObjectManager.Player;
        private static int rcount;
        private static readonly float[] FervorofBattle = { 0.9f, 1.67f, 2.44f, 3.21f, 3.98f, 4.75f, 5.52f, 6.29f, 7.06f, 7.83f, 8.6f, 9.37f, 10.14f, 10.91f, 11.68f, 12.45f, 13.22f, 13.99f };
        private static readonly float[] QDefault = { 20, 50, 80, 110, 140 };
        private static readonly double[] WDefault = { 15, 30, 45, 60, 75 };
        private static readonly float[] EDefault = { 80, 120, 160, 200, 240 };
        private static readonly float[] RDefault = { 80, 120, 160 };
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.BaseSkinName != "Irelia") return;

            IreliaMenu.Initialize();
            Spells.Initialize();
            Bootstrap.Init(null);

            Game.OnUpdate += OnTick;
            Orbwalker.OnPreAttack += OnPreAttack;
            Drawing.OnDraw += OnDraw;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
            //Obj_AI_Base.OnAggro += OnAggro; // find equivalent
            Obj_AI_Base.OnProcessSpellCast += (sender, eventArgs) => //causes crash
            {
                if (!sender.IsMe) return;
                if (eventArgs.SData.Name == Spells.E.Name)
                    Orbwalker.ResetAutoAttack();//Core.DelayAction( ()=> Orbwalker.ResetAutoAttack(), (int)(Math.Round(Player.AttackCastDelay, 3) * 1000));

                if (eventArgs.SData.Name == Spells.Q.Name)
                    Orbwalker.ResetAutoAttack();//Core.DelayAction( ()=> Orbwalker.ResetAutoAttack(), (int)(Math.Round(Player.AttackCastDelay, 3) * 1000));

            };
            Orbwalker.OnPostAttack += (unit, target) =>
            {
                if (IreliaMenu.ComboMenu["combo.items"].Cast<CheckBox>().CurrentValue && target != null && Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
                {
                    if (Spells.Hydra1.IsReady())
                        Spells.Hydra1.Cast();

                    if (Spells.Tiamat.IsReady())
                        Spells.Tiamat.Cast();
                }
            };
            Hacks.RenderWatermark = false;
        }

        static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!IreliaMenu.MiscMenu["misc.interrupt"].Cast<CheckBox>().CurrentValue) return;
            if (sender.IsValidTarget(Spells.E.Range) && Player.HealthPercent <= sender.HealthPercent)
                if (Spells.E.IsReady())
                    Spells.E.Cast(sender);
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!IreliaMenu.MiscMenu["misc.ag.e"].Cast<CheckBox>().CurrentValue) return;
            if (sender.IsValidTarget(Spells.E.Range) && e.End.Distance(Player.ServerPosition) <= Spells.E.Range
                && Player.HealthPercent <= sender.HealthPercent)
            {
                if (Spells.E.IsReady())
                    Spells.E.Cast(sender);
            }
        }

        //private static bool Selected()
        //{
        //    if (!IreliaMenu.ComboMenu["force.target"].Cast<CheckBox>().CurrentValue) return false;

        //    var target = (Obj_AI_Base)Hud.SelectedTarget;
        //    float range = IreliaMenu.ComboMenu[("force.target.range")].Cast<Slider>().CurrentValue;
        //    if (target == null || target.IsDead || target.IsZombie) return false;

        //    return !(Player.Distance(target) > range);
        //}

        //private static Obj_AI_Base GetTarget(float range)
        //{
        //    return (Selected() ? (Obj_AI_Base)Hud.SelectedTarget : TargetSelector.GetTarget(range, DamageType.Physical));
        //}


        private static void OnDraw(EventArgs args)
        {
            //Drawing.DrawCircle(new Vector3(Player.Position.X + IreliaMenu.IreliaMainMenu["X"].Cast<Slider>().CurrentValue, Player.Position.Y + IreliaMenu.IreliaMainMenu["Y"].Cast<Slider>().CurrentValue, Player.Position.Z + IreliaMenu.IreliaMainMenu["Z"].Cast<Slider>().CurrentValue), 70, Color.Yellow);
            if (IreliaMenu.DrawingsMenu["drawings.q"].Cast<CheckBox>().CurrentValue)
            {
                if (Spells.Q.IsReady()) new Circle { Color = Color.FloralWhite, Radius = Spells.Q.Range }.Draw(Player.Position);
                else if (Spells.Q.IsOnCooldown) new Circle { Color = Color.Maroon, Radius = Spells.Q.Range }.Draw(Player.Position);
            }

            if (IreliaMenu.DrawingsMenu["drawings.e"].Cast<CheckBox>().CurrentValue)
            {
                if (Spells.E.IsReady()) new Circle { Color = Color.FloralWhite, Radius = Spells.E.Range }.Draw(Player.Position);
                else if (Spells.E.IsOnCooldown) new Circle { Color = Color.Maroon, Radius = Spells.E.Range }.Draw(Player.Position);
            }

            if (IreliaMenu.DrawingsMenu["drawings.r"].Cast<CheckBox>().CurrentValue)
            {
                if (Spells.R.IsReady()) new Circle { Color = Color.FloralWhite, Radius = Spells.R.Range }.Draw(Player.Position);
                else if (Spells.R.IsOnCooldown) new Circle { Color = Color.Maroon, Radius = Spells.R.Range }.Draw(Player.Position);
            }

        }

        private static void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                target != null && target.Type == Player.Type && target.IsValidTarget())
            {
                if (IreliaMenu.ComboMenu["combo.w"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady()) Spells.W.Cast();
                if (IreliaMenu.ComboMenu["combo.items"].Cast<CheckBox>().CurrentValue)
                {
                    if (Spells.Corruptpot.IsReady() && !Player.HasBuff("ItemDarkCrystalFlask") && Player.HealthPercent <= 95)
                        Spells.Corruptpot.Cast();
                    if (Spells.Hydra2.IsReady())
                        Spells.Hydra2.Cast();
                }
            }

            if (IreliaMenu.HarassMenu["harass.w"].Cast<CheckBox>().CurrentValue &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) &&
                target != null &&
                target.Type == Player.Type &&
                target.IsValidTarget())
                    if (Spells.W.IsReady())
                        Spells.W.Cast();
        }

        private static void OnTick(EventArgs args)
        {
            Killsteal();
            /*if (IreliaMenu.MiscMenu["misc.items.randuin"].Cast<Slider>().CurrentValue != 0)
                if (Spells.Randuin.IsReady() && Player.CountEnemiesInRange(500f) >= IreliaMenu.ComboMenu["combo.items.randuin"].Cast<Slider>().CurrentValue)
                    Spells.Randuin.Cast();//Core.DelayAction(() => Spells.Randuin.Cast(), new Random(DateTime.Now.Millisecond * (int)(Game.CursorPos.X + Player.Position.Y)).Next(5, 75));*/
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Clear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                /*foreach (var buffInstance in TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical).Buffs)
                {
                    Chat.Print("Type : " + buffInstance.Type.ToString());
                    Chat.Print("Name : " + buffInstance.Name);
                    Chat.Print("Display : " + buffInstance.DisplayName);
                    Chat.Print("SourceName : " + buffInstance.SourceName);
                    Chat.Print("Count : " + buffInstance.Count.ToString());
                    //masterylordsdecreecooldown;
                    //masterylordsdecreestacks;
                }*/
                Flee();
            }
            RCount();
        }

        private static void Flee()
        {
            if (Spells.E.IsReady() && IreliaMenu.FleeMenu["flee.e"].Cast<CheckBox>().CurrentValue)
            {
                var etarget =
                    EntityManager.Heroes.Enemies
                        .Where(
                            enemy =>
                                enemy.IsValidTarget() && Player.Distance(enemy.Position) <= Spells.E.Range)
                        .OrderBy(e => e.Distance(Player));

                if (etarget.FirstOrDefault() != null)
                    Spells.E.Cast(etarget.FirstOrDefault());
            }
            if (Spells.R.IsReady() && IreliaMenu.FleeMenu["flee.r"].Cast<CheckBox>().CurrentValue)
            {
                var rtarget =
                    EntityManager.Heroes.Enemies
                        .Where(
                            enemy =>
                                enemy.IsValidTarget() && Player.Distance(enemy.Position) <= Spells.R.Range)
                        .OrderBy(e => e.Distance(Player));
                if (rtarget.FirstOrDefault() == null) goto WALK;
                var rprediction = Prediction.Position.PredictLinearMissile(rtarget.FirstOrDefault(), Spells.R.Range,
                    Spells.R.Width, Spells.R.CastDelay, Spells.R.Speed, int.MaxValue);

                Spells.R.Cast(rprediction.CastPosition);
            }
            if (Spells.Q.IsReady() && IreliaMenu.FleeMenu["flee.q"].Cast<CheckBox>().CurrentValue)
            {
                var target =
                    EntityManager.Heroes.Enemies
                        .Where(
                            enemy =>
                                enemy.IsValidTarget() && Player.Distance(enemy.Position) <= Spells.R.Range)
                        .LastOrDefault(e => e.Distance(Player) <= Spells.R.Range);

                if (target == null) goto WALK;
                
                var qminion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Game.CursorPos, Spells.Q.Range)
                    .Where(
                        m =>
                        m.IsValidTarget() &&
                        m.Distance(target) > Player.Distance(target))
                    .FirstOrDefault(m => m.Distance(Player) <= Spells.Q.Range);
                var qmonster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Game.CursorPos, Spells.Q.Range)
                    .Where(
                        m =>
                        m.IsValidTarget() &&
                        m.Distance(target) > Player.Distance(target))
                    .FirstOrDefault(m => m.Distance(Player) <= Spells.Q.Range);
                if (qminion != null && qmonster != null)
                    Spells.Q.Cast(qminion.Distance(Player) <= qmonster.Distance(Player) ? qmonster : qminion);
                else if (qminion != null)
                    Spells.Q.Cast(qminion);
                else if (qmonster != null)
                    Spells.Q.Cast(qmonster);
            }
            WALK:
            Orbwalker.MoveTo(Game.CursorPos);
        }

        private static void Clear()
        {
            if (Player.ManaPercent <= IreliaMenu.LaneClearMenu["laneclear.mana"].Cast<Slider>().CurrentValue && Player.HasBuff("ireliatranscendentbladesspell") && rcount >= 1) goto castr;
            if (Player.ManaPercent <= IreliaMenu.LaneClearMenu["laneclear.mana"].Cast<Slider>().CurrentValue) return;
            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Spells.Q.Range);
            if (minion.FirstOrDefault() == null) return;
            var qminion =
                minion.FirstOrDefault(
                    m =>
                        m.Distance(Player) <= Spells.Q.Range &&
                        m.Health <= QDamage(m) + ExtraWDamage() + SheenDamage(m) + IreliaMenu.MiscMenu["misc.qminion"].Cast<Slider>().CurrentValue &&
                        m.IsValidTarget());

            if (Spells.Q.IsReady() && IreliaMenu.LaneClearMenu["laneclear.q"].Cast<CheckBox>().CurrentValue && qminion != null && !Orbwalker.IsAutoAttacking)
            {
                if(Spells.W.IsReady() && IreliaMenu.LaneClearMenu["laneclear.w"].Cast<CheckBox>().CurrentValue) Spells.W.Cast();
                Spells.Q.Cast(qminion);
            }
            if (Spells.W.IsReady() && IreliaMenu.LaneClearMenu["laneclear.w"].Cast<CheckBox>().CurrentValue && minion.FirstOrDefault(m => m.IsMinion && m.Distance(Player) <= Player.AttackRange ) != null )
            {
                Spells.W.Cast();
            }
            castr: //왜일까 goto 대신 if로 대체하고 return 처리하니까 밑의 로직이 실행안됨
            var rminions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Spells.R.Range);
            if (Spells.R.IsReady() && IreliaMenu.LaneClearMenu["laneclear.r"].Cast<CheckBox>().CurrentValue && rminions.ToList().Count != 0)
            {
                var location = EntityManager.MinionsAndMonsters.GetLineFarmLocation(rminions,Spells.R.Width,(int)Spells.R.Range,Player.ServerPosition.To2D());//Spells.R.GetLineFarmLocation(rminions);

                if (location.HitNumber >=
                    IreliaMenu.LaneClearMenu["laneclear.r.minions"].Cast<Slider>().CurrentValue)
                    Spells.R.Cast(location.CastPosition);
            }
        }

        private static void Killsteal()
        {
            foreach (
                var enemy in
                    EntityManager.Heroes.Enemies.Where(e => e.Distance(Player) <= Spells.R.Range && e.IsValidTarget() && !e.IsInvulnerable))
            {
                if (Spells.Q.IsReady() && IreliaMenu.MiscMenu["misc.ks.q"].Cast<CheckBox>().CurrentValue &&
                    Spells.E.IsReady() && IreliaMenu.MiscMenu["misc.ks.e"].Cast<CheckBox>().CurrentValue &&
                    EDamage(enemy) + QDamage(enemy) + ExtraWDamage() + SheenDamage(enemy) >=
                            enemy.Health)
                {
                    if (enemy.Distance(Player) <= Spells.Q.Range && enemy.Distance(Player) > Spells.E.Range)
                    {
                        Spells.Q.Cast(enemy);
                        var enemy1 = enemy;
                        Core.DelayAction(() => Spells.E.Cast(enemy1), (int)(1000 * Player.Distance(enemy) / 2200));
                    }
                    else if (enemy.Distance(Player) <= Spells.Q.Range)
                    {
                        Spells.E.Cast(enemy);
                        var enemy1 = enemy;
                        Core.DelayAction(() => Spells.Q.Cast(enemy1), 250);
                    }

                }

                if (IreliaMenu.MiscMenu["misc.ks.q"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady() &&
                    QDamage(enemy) + ExtraWDamage() + SheenDamage(enemy)>= enemy.Health &&
                    enemy.Distance(Player) <= Spells.Q.Range)
                {
                    Spells.Q.Cast(enemy);
                    return;
                }

                if (IreliaMenu.MiscMenu["misc.ks.e"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() &&
                    EDamage(enemy) >= enemy.Health && enemy.Distance(Player) <= Spells.E.Range)
                {
                    Spells.E.Cast(enemy);
                    return;
                }

                if (IreliaMenu.MiscMenu["misc.ks.r"].Cast<CheckBox>().CurrentValue && Spells.R.IsReady() &&
                    RDamage(enemy) >= enemy.Health)
                {
                    Spells.R.Cast(enemy);
                }

            }
        }

        private static void RCount()
        {
            if (rcount == 0 && Spells.R.IsReady())
                rcount = 4;

            if (!Spells.R.IsReady() & rcount != 0)
                rcount = 0;

            if(Player.HasBuff("ireliatranscendentbladesspell"))/*foreach (
                var buff in
                    Player.Buffs.Where(b => b.Name == "ireliatranscendentbladesspell" && b.IsValid))*/
            {
                rcount = Player.GetBuffCount("ireliatranscendentbladesspell");//buff.Count;
            }
        }

        private static bool UnderTheirTower(Obj_AI_Base target)
        {
            var tower =
                ObjectManager
                    .Get<Obj_AI_Turret>()
                    .FirstOrDefault(turret => turret != null && turret.Distance(target) <= 775 && turret.IsValid && turret.Health > 0 && !turret.IsAlly);

            return tower != null;
        }

        private static void Combo()
        {
            var gctarget = TargetSelector.GetTarget(Spells.Q.Range * 2.5f, DamageType.Physical);
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
            if (gctarget == null) return;
            if (!target.IsValidTarget()) return;
            var qminion =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Spells.Q.Range + 350)
                    .Where(
                        m =>
                        m.IsValidTarget()
                        && Prediction.Health.GetPrediction(m, 1000 * (int)(m.Distance(Player) / 2200))
                        <= QDamage(m) + ExtraWDamage() + SheenDamage(m) + IreliaMenu.MiscMenu["misc.qminion"].Cast<Slider>().CurrentValue)
                    .OrderBy(m => m.Distance(gctarget))
                    .FirstOrDefault();

            if (Spells.Q.IsReady())
            {
                if (IreliaMenu.ComboMenu["combo.q.gc"].Cast<CheckBox>().CurrentValue && qminion != null &&
                    gctarget.Distance(Player) >= Player.GetAutoAttackRange(gctarget) &&
                    qminion.Distance(gctarget) <= Player.Distance(gctarget) &&
                    qminion.Distance(Player) <= Spells.Q.Range)
                {
                    Spells.Q.Cast(qminion);
                }

                if (IreliaMenu.ComboMenu["combo.q"].Cast<CheckBox>().CurrentValue && target != null &&
                    target.Distance(Player) <= Spells.Q.Range &&
                    Player.Distance(target) >=
                    IreliaMenu.ComboMenu["combo.q.minrange"].Cast<Slider>().CurrentValue)
                {
                    if (UnderTheirTower(target))
                        if (target.HealthPercent >=
                            IreliaMenu.ComboMenu["combo.q.undertower"].Cast<Slider>().CurrentValue) return;

                    if (IreliaMenu.ComboMenu["combo.w"].Cast<CheckBox>().CurrentValue)
                        Spells.W.Cast();

                    if (qminion != null && qminion.Distance(target) <= Player.Distance(target)) return;

                    Spells.Q.Cast(target);
                }

                if (IreliaMenu.ComboMenu["combo.q"].Cast<CheckBox>().CurrentValue &&
                    IreliaMenu.ComboMenu["combo.q.lastsecond"].Cast<CheckBox>().CurrentValue && target != null)
                {
                    var buff = Player.Buffs.FirstOrDefault(b => b.Name == "ireliahitenstylecharged" && b.IsValid);
                    if (buff != null && buff.EndTime - Game.Time <= (Player.Distance(target) / 2200 + .500 + Player.AttackCastDelay) && !Orbwalker.IsAutoAttacking)
                    {
                        if (UnderTheirTower(target))
                            if (target.HealthPercent >=
                                IreliaMenu.ComboMenu["combo.q.undertower"].Cast<Slider>().CurrentValue) return;

                        Spells.Q.Cast(target);
                    }
                }
            }

            if (Spells.E.IsReady() && IreliaMenu.ComboMenu["combo.e"].Cast<CheckBox>().CurrentValue && target != null)
            {
                if (IreliaMenu.ComboMenu["combo.e.logic"].Cast<CheckBox>().CurrentValue &&
                    target.Distance(Player) <= Spells.E.Range)
                {
                    if (target.HealthPercent >= Player.HealthPercent && !Player.IsDashing())
                    {
                        Spells.E.Cast(target);
                    }
                    else if (!target.IsAttackingPlayer && !Player.IsDashing() && target.Distance(Player) >= Spells.E.Range * .5)
                    {
                        Spells.E.Cast(target);
                    }
                }
                else if (target.Distance(Player) <= Spells.E.Range)
                {
                    Spells.E.Cast(target);
                }
            }

            if (Spells.R.IsReady() && IreliaMenu.ComboMenu["combo.r"].Cast<CheckBox>().CurrentValue && !IreliaMenu.ComboMenu["combo.r.selfactivated"].Cast<CheckBox>().CurrentValue)
            {
                if (IreliaMenu.ComboMenu["combo.r.weave"].Cast<CheckBox>().CurrentValue)
                {
                    if (Item.HasItem((int)ItemId.Sheen, Player) && Item.CanUseItem((int)ItemId.Sheen) || Item.HasItem((int)ItemId.Trinity_Force, Player) && Item.CanUseItem((int)ItemId.Trinity_Force))
                        if (target != null && !Player.HasBuff("sheen") &&
                            target.Distance(Player) <= Spells.R.Range)
                        {
                            Spells.R.Cast(target);
                        }   
                }
                else if (!IreliaMenu.ComboMenu["combo.r.weave"].Cast<CheckBox>().CurrentValue && target != null)
                {
                    Spells.R.Cast(target);
                    // Set to Q range because we are already going to combo them at this point most likely, no stupid long range R initiations
                }
            }
            else if (Spells.R.IsReady() && IreliaMenu.ComboMenu["combo.r"].Cast<CheckBox>().CurrentValue && IreliaMenu.ComboMenu["combo.r.selfactivated"].Cast<CheckBox>().CurrentValue && rcount <= 3)
            {
                if (IreliaMenu.ComboMenu["combo.r.weave"].Cast<CheckBox>().CurrentValue)
                {
                    if (Item.HasItem((int)ItemId.Sheen, Player) && Item.CanUseItem((int)ItemId.Sheen) || Item.HasItem((int)ItemId.Trinity_Force, Player) && Item.CanUseItem((int)ItemId.Trinity_Force))
                        if (target != null && !Player.HasBuff("sheen") &&
                            target.Distance(Player) <= Spells.R.Range)
                        {
                            Spells.R.Cast(target);
                        }
                }
                else if (!IreliaMenu.ComboMenu["combo.r.weave"].Cast<CheckBox>().CurrentValue && target != null)
                {
                    Spells.R.Cast(target);
                    // Set to Q range because we are already going to combo them at this point most likely, no stupid long range R initiations
                }
            }

            if (IreliaMenu.ComboMenu["combo.ignite"].Cast<CheckBox>().CurrentValue && target != null && Spells.Ignite != SpellSlot.Unknown)
            {
                if (Player.Distance(target) <= 600 && ComboDamage(target) >= target.Health)
                    Player.Spellbook.CastSpell(Spells.Ignite, target);
            }

            if (IreliaMenu.ComboMenu["combo.items"].Cast<CheckBox>().CurrentValue && target != null)
            {   
                if (Spells.Youmuu.IsReady() && target.IsValidTarget(Spells.Q.Range))
                {
                    Spells.Youmuu.Cast();
                }

                if (Player.Distance(target) <= 450 && Spells.Cutlass.IsReady())
                {
                    Spells.Cutlass.Cast(target);
                }

                if (Player.Distance(target) <= 450 && Spells.Blade.IsReady())
                {
                    Spells.Blade.Cast(target);
                }
            }
        }
        private static void Harass()
        {
            var gctarget = TargetSelector.GetTarget(Spells.Q.Range * 2.5f, DamageType.Physical);
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
            if (gctarget == null) return;
            if (!target.IsValidTarget()) return;
            var qminion =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Spells.Q.Range + 350)
                    .Where(
                        m =>
                        m.IsValidTarget()
                        && Prediction.Health.GetPrediction(m, (int)(m.Distance(Player) / 2200))
                        <= QDamage(m) + ExtraWDamage() + SheenDamage(m) - IreliaMenu.MiscMenu["misc.qminion"].Cast<Slider>().CurrentValue)
                    .OrderBy(m => m.Distance(gctarget))
                    .FirstOrDefault();

            if (Spells.Q.IsReady())
            {
                if (IreliaMenu.HarassMenu["harass.q.gc"].Cast<CheckBox>().CurrentValue && qminion != null &&
                    gctarget.Distance(Player) >= Player.GetAutoAttackRange(gctarget) &&
                    qminion.Distance(gctarget) <= Player.Distance(gctarget) &&
                    qminion.Distance(Player) <= Spells.Q.Range)
                {
                    Spells.Q.Cast(qminion);
                }

                if (IreliaMenu.HarassMenu["harass.q"].Cast<CheckBox>().CurrentValue && target != null &&
                    target.Distance(Player) <= Spells.Q.Range &&
                    Player.Distance(target) >=
                    IreliaMenu.HarassMenu["harass.q.minrange"].Cast<Slider>().CurrentValue)
                {
                    if (UnderTheirTower(target))
                        if (target.HealthPercent >=
                            IreliaMenu.HarassMenu["harass.q.undertower"].Cast<Slider>().CurrentValue) return;

                    if (IreliaMenu.HarassMenu["harass.w"].Cast<CheckBox>().CurrentValue)
                        Spells.W.Cast();

                    if (qminion != null && qminion.Distance(target) <= Player.Distance(target)) return;

                    Spells.Q.Cast(target);
                }

                if (IreliaMenu.HarassMenu["harass.q"].Cast<CheckBox>().CurrentValue &&
                    IreliaMenu.HarassMenu["harass.q.lastsecond"].Cast<CheckBox>().CurrentValue && target != null)
                {
                    var buff = Player.Buffs.FirstOrDefault(b => b.Name == "ireliahitenstylecharged" && b.IsValid);
                    if (buff != null && buff.EndTime - Game.Time <= (Player.Distance(target) / 2200 + .500 + Player.AttackCastDelay) && !Orbwalker.IsAutoAttacking)
                    {
                        if (UnderTheirTower(target))
                            if (target.HealthPercent >=
                                IreliaMenu.HarassMenu["harass.q.undertower"].Cast<Slider>().CurrentValue) return;

                        Spells.Q.Cast(target);
                    }
                }
            }

            if (Spells.E.IsReady() && IreliaMenu.HarassMenu["harass.e"].Cast<CheckBox>().CurrentValue && target != null)
            {
                if (IreliaMenu.HarassMenu["harass.e.logic"].Cast<CheckBox>().CurrentValue &&
                    target.Distance(Player) <= Spells.E.Range)
                {
                    if (target.HealthPercent >= Player.HealthPercent && !Player.IsDashing())
                    {
                        Spells.E.Cast(target);
                    }
                    else if (!target.IsAttackingPlayer && !Player.IsDashing() && target.Distance(Player) >= Spells.E.Range * .5)
                    {
                        Spells.E.Cast(target);
                    }
                }
                else if (target.Distance(Player) <= Spells.E.Range)
                {
                    Spells.E.Cast(target);
                }
            }

            if (Spells.R.IsReady() && IreliaMenu.HarassMenu["harass.r"].Cast<CheckBox>().CurrentValue && !IreliaMenu.HarassMenu["harass.r.selfactivated"].Cast<CheckBox>().CurrentValue)
            {
                if (IreliaMenu.HarassMenu["harass.r.weave"].Cast<CheckBox>().CurrentValue)
                {
                    if (Item.HasItem((int)ItemId.Sheen, Player) && Item.CanUseItem((int)ItemId.Sheen) || Item.HasItem((int)ItemId.Trinity_Force, Player) && Item.CanUseItem((int)ItemId.Trinity_Force))
                        if (target != null && !Player.HasBuff("sheen") &&
                            target.Distance(Player) <= Spells.R.Range)
                        {
                            Spells.R.Cast(target);
                        }
                }
                else if (!IreliaMenu.HarassMenu["harass.r.weave"].Cast<CheckBox>().CurrentValue && target != null)
                {
                    Spells.R.Cast(target);
                }
            }
            else if (Spells.R.IsReady() && IreliaMenu.HarassMenu["harass.r"].Cast<CheckBox>().CurrentValue && IreliaMenu.HarassMenu["harass.r.selfactivated"].Cast<CheckBox>().CurrentValue && rcount <= 3)
            {
                if (IreliaMenu.HarassMenu["harass.r.weave"].Cast<CheckBox>().CurrentValue)
                {
                    if (Item.HasItem((int)ItemId.Sheen, Player) && Item.CanUseItem((int)ItemId.Sheen) || Item.HasItem((int)ItemId.Trinity_Force, Player) && Item.CanUseItem((int)ItemId.Trinity_Force))
                        if (target != null && !Player.HasBuff("sheen") &&
                            target.Distance(Player) <= Spells.R.Range)
                        {
                            Spells.R.Cast(target);
                        }
                }
                else if (!IreliaMenu.HarassMenu["harass.r.weave"].Cast<CheckBox>().CurrentValue && target != null)
                {
                    Spells.R.Cast(target);
                }
            }
        }


        private static void JungleClear()
        {
            if (Player.ManaPercent <= IreliaMenu.JungleClearMenu["jungleclear.mana"].Cast<Slider>().CurrentValue) return;
            var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, Spells.Q.Range).Where(m => m.IsValidTarget() && m.IsMonster);
            if (monster.FirstOrDefault() == null) return;
            string[] Legendary = { "TT_Spiderboss", "SRU_Baron", "SRU_Dragon", "SRU_RiftHerald" };
            var emonster = monster.Where(m => m.IsValidTarget(Spells.E.Range) && !m.Name.Contains("Mini") && !Legendary.Any(l => m.Name.StartsWith(l))).FirstOrDefault(m => Player.HealthPercent <= m.HealthPercent);
            if (Spells.E.IsReady() && emonster != null && IreliaMenu.JungleClearMenu["jungleclear.e"].Cast<CheckBox>().CurrentValue)
                Spells.E.Cast(emonster);
            if (IreliaMenu.JungleClearMenu["jungleclear.mini"].Cast<CheckBox>().CurrentValue)
            {
                var mini = monster.Where(m => m.Name.Contains("Mini"));
                if (mini.FirstOrDefault(m => m.Distance(Player) <= Player.AttackRange ) != null)
                {
                    Orbwalker.ForcedTarget = mini.FirstOrDefault();
                    monster = mini;
                }
            }
            if (Spells.Q.IsReady() || Spells.W.IsReady())
            {
                if (IreliaMenu.JungleClearMenu["jungleclear.q"].Cast<CheckBox>().CurrentValue && !IreliaMenu.JungleClearMenu["jungleclear.lq"].Cast<CheckBox>().CurrentValue)
                {
                    var wqmonster = monster.Where(m => m.IsValidTarget(Spells.Q.Range)).FirstOrDefault(m => 75 <= m.HealthPercent);
                    if (Spells.W.IsReady() && IreliaMenu.JungleClearMenu["jungleclear.w"].Cast<CheckBox>().CurrentValue) Spells.W.Cast();
                    if (Spells.Q.IsReady() && wqmonster != null && !wqmonster.Name.Contains("Mini")) Spells.Q.Cast(wqmonster);
                }
                else if (Spells.W.IsReady() && IreliaMenu.JungleClearMenu["jungleclear.w"].Cast<CheckBox>().CurrentValue)
                {
                    if (monster.FirstOrDefault(m => m.IsValidTarget(Player.AttackRange)) != null) Spells.W.Cast();
                }
            }
            var qmonster =
                monster.FirstOrDefault(
                    m =>
                        m.Distance(Player) <= Spells.Q.Range &&
                        m.Health <= QDamage(m) + ExtraWDamage() + SheenDamage(m) + IreliaMenu.MiscMenu["misc.qminion"].Cast<Slider>().CurrentValue &&
                        m.IsValidTarget());

            if (Spells.Q.IsReady() && IreliaMenu.JungleClearMenu["jungleclear.q"].Cast<CheckBox>().CurrentValue && qmonster != null )
            {
                Spells.Q.Cast(qmonster);
            }
        }


        public static float ComboDamage(Obj_AI_Base hero) // Thanks honda
        {
            var result = 0d;

            if (Spells.Q.IsReady())
            {
                result += QDamage(hero) + ExtraWDamage() + SheenDamage(hero);
            }
            if (Spells.W.IsReady() || Player.HasBuff("ireliahitenstylecharged"))
            {
                result += (ExtraWDamage() + Player.CalculateDamageOnUnit(hero, DamageType.Physical, Player.TotalAttackDamage)) * 3; // 3 autos
            }
            if (Spells.E.IsReady())
            {
                result += EDamage(hero);
            }
            if (Spells.R.IsReady())
            {
                result += RDamage(hero);
            }

            return (float)result;
        }

        private static double SheenDamage(Obj_AI_Base target) // Thanks princer007 for the basic idea
        {
            var result = 0d;
            foreach (var item in Player.InventoryItems)
                switch ((int)item.Id)
                {
                    case 3025: //Iceborn Gauntlet
                        if (Item.CanUseItem((int)ItemId.Iceborn_Gauntlet))
                            result += Player.CalculateDamageOnUnit(target, DamageType.Physical, Player.BaseAttackDamage * 1.25f );
                        break;
                    case 3057: //Sheen
                        if (Item.CanUseItem((int)ItemId.Sheen))
                            result += Player.CalculateDamageOnUnit(target, DamageType.Physical, Player.BaseAttackDamage);
                                //Player.GetItemDamage(target, ItemId.Sheen);
                        break;
                    case 3078: //Triforce
                        if (Item.CanUseItem((int)ItemId.Trinity_Force))
                            result += Player.CalculateDamageOnUnit(target, DamageType.Physical, Player.BaseAttackDamage * 2);
                                //Player.GetItemDamage(target, ItemId.Trinity_Force);
                        break;
                    case 3134: //Serrated Dirk
                        if (Player.HasBuff("Serrated"))
                            result += Player.CalculateDamageOnUnit(target, DamageType.Physical, 15);
                        break;
                }
            if (target.GetType().ToString() == "EloBuddy.AIHeroClient")
                if (Player.HasBuff("MasteryOnHitDamageStacker")) //Rip 5.24 (0.9f + 0.42f * Player.Level), Now 6.1 (1.04f + 6.16f * Player.Level) 
                    result += Player.CalculateDamageOnUnit(target, DamageType.Physical, FervorofBattle[Player.Level - 1] * Player.GetBuffCount("MasteryOnHitDamageStacker"));
            return result;
        }

        private static double ExtraWDamage()
        {
            // tried some stuff with if buff == null but the damage will be enough then cast W and it worked.. but meh, idk will look at later

            var extra = 0d;
            //var buff = Player.Buffs.FirstOrDefault(b => b.Name == "ireliahitenstylecharged" && b.IsValid);
            //if (buff != null) // && buff.EndTime > (1000 * Player.Distance(target) / 2200))
            if (Player.HasBuff("ireliahitenstylecharged"))
            {
                extra += WDefault[Spells.W.Level - 1];
            }
            return extra;
        }
        private static double QDamage(Obj_AI_Base target)
        {
            return Spells.Q.IsReady()
                ? Player.CalculateDamageOnUnit(
                    target,
                    DamageType.Physical,
                    QDefault[Spells.Q.Level - 1]
                    + Player.TotalAttackDamage)
                : 0d;
        }

        private static double EDamage(Obj_AI_Base target)
        {
            return Spells.E.IsReady()
                ? Player.CalculateDamageOnUnit(
                    target,
                    DamageType.Magical,
                    EDefault[Spells.E.Level - 1]
                    + .5f * Player.TotalMagicalDamage)
                : 0d;
        }

        private static double RDamage(Obj_AI_Base target)
        {
            return Spells.R.IsReady()
                ? Player.CalculateDamageOnUnit(
                    target,
                    DamageType.Physical,
                    (RDefault[Spells.R.Level - 1]
                    + .5f * Player.TotalMagicalDamage
                    + .6f * Player.FlatPhysicalDamageMod
                    ) * rcount)
                : 0d;
        }
    }
}