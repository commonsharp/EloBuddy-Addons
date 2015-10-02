namespace Lasthit_Marker
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Color = System.Drawing.Color;

    internal class Program
    {
        #region Static Fields

        private static readonly AIHeroClient Player = ObjectManager.Player;

        #endregion

        #region Methods

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var minion in
                EntityManager.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Position.To2D(), 1500)
                    .Where(m => m.IsValidTarget() && !m.IsDead))
            {
                var health = Prediction.Health.GetPrediction(
                    minion,
                    (int) ((int)(ObjectManager.Player.AttackCastDelay * 1000) - 100 + Game.Ping / 2 + 1000 *
                    (int)ObjectManager.Player.Distance(minion) /
                    (Player.CombatType == GameObjectCombatType.Melee || Player.ChampionName == "Azir"
                    ? float.MaxValue
                    : Player.BasicAttack.MissileSpeed)));

                var damage = ObjectManager.Player.GetAutoAttackDamage(minion, true);
                var killable = health <= damage;
                var barpos = minion.HPBarPosition;
                var barwidth = minion.BaseSkinName.Contains("Siege") ? 70 : 63;
                var offset = (barwidth / (minion.MaxHealth / damage));
                offset = offset < barwidth ? offset : barwidth;
                Drawing.DrawLine(
                    new Vector2(barpos.X + 25 + offset, barpos.Y + 17),
                    new Vector2(barpos.X + 25 + offset, barpos.Y + 24),
                    1,
                    killable ? Color.Green : Color.White);

                if (killable)
                {
                    Drawing.DrawCircle(minion.Position, minion.BoundingRadius * 2, Color.Green);
                }

                else if (health <= damage * 2)
                {
                    Drawing.DrawCircle(minion.Position, minion.BoundingRadius * 2, Color.Red);
                }
            }
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Drawing.OnEndScene += Drawing_OnEndScene;

            Chat.Print("[EloBuddy]<font color='#FFFFFF'>:  >></font> <font color='#79BAEC'> Lasthit Marker </font> <font color='#FFFFFF'><<</font>");
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        #endregion
    }
}