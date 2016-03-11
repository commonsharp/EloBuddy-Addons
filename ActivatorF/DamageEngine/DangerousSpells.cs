using System.Collections.Generic;
using EloBuddy;

namespace ActivatorF.DamageEngine
{

    public class DangerousSpells
    {
        public static List<DangerousSpell> Spells = new List<DangerousSpell>()
        {
            new DangerousSpell(Champion.Aatrox, SpellSlot.Q, false, 250),
            new DangerousSpell(Champion.Ahri, SpellSlot.E, false, 250),
            new DangerousSpell(Champion.Amumu, SpellSlot.R, true),
            new DangerousSpell(Champion.Annie, SpellSlot.R, true),
            new DangerousSpell(Champion.Ashe, SpellSlot.R, true, 250),
            new DangerousSpell(Champion.Azir, SpellSlot.R, true),
            new DangerousSpell(Champion.Bard, SpellSlot.Q, false, 350),
            new DangerousSpell(Champion.Blitzcrank, SpellSlot.Q, true, 350),
            new DangerousSpell(Champion.Brand, SpellSlot.R, true),
            new DangerousSpell(Champion.Braum, SpellSlot.R, true, 50),
            new DangerousSpell(Champion.Caitlyn, SpellSlot.R, true, 800) {IsCleanseable = true},
            new DangerousSpell(Champion.Cassiopeia, SpellSlot.R, true, 50),
            new DangerousSpell(Champion.Chogath, SpellSlot.R, true),
            new DangerousSpell(Champion.Darius, SpellSlot.R, true, 250) {IsCleanseable = true},
            new DangerousSpell(Champion.Draven, SpellSlot.R, true, 400),
            new DangerousSpell(Champion.Ekko, SpellSlot.R, true),
            new DangerousSpell(Champion.Elise, SpellSlot.E, false, 150),
            new DangerousSpell(Champion.Evelynn, SpellSlot.R, false),
            new DangerousSpell(Champion.Ezreal, SpellSlot.R, true, 800),
            new DangerousSpell(Champion.FiddleSticks, SpellSlot.R, true, 1000),
            new DangerousSpell(Champion.Fiora, SpellSlot.R, true, 300),
            new DangerousSpell(Champion.Fizz, SpellSlot.R, true, 150) {IsCleanseable = true},
            new DangerousSpell(Champion.Galio, SpellSlot.R, true),
            new DangerousSpell(Champion.Garen, SpellSlot.R, true),
            new DangerousSpell(Champion.Gnar, SpellSlot.R, true),
            new DangerousSpell(Champion.Gragas, SpellSlot.R, true),
            new DangerousSpell(Champion.Graves, SpellSlot.R, true),
            new DangerousSpell(Champion.Hecarim, SpellSlot.R, true, 350),
            new DangerousSpell(Champion.Illaoi, SpellSlot.R, true),
            new DangerousSpell(Champion.JarvanIV, SpellSlot.R, true),
            new DangerousSpell(Champion.Jax, SpellSlot.E, false, 350),
            new DangerousSpell(Champion.Jinx, SpellSlot.R, true, 600),
            new DangerousSpell(Champion.Kalista, SpellSlot.E, false),
            new DangerousSpell(Champion.Karthus, SpellSlot.R, true, 2800),
            new DangerousSpell(Champion.Kassadin, SpellSlot.R, false),
            new DangerousSpell(Champion.Katarina, SpellSlot.R, true),
            new DangerousSpell(Champion.Kennen, SpellSlot.R, true, 150),
            new DangerousSpell(Champion.Leblanc, SpellSlot.R, true),
            new DangerousSpell(Champion.LeeSin, SpellSlot.R, true),
            new DangerousSpell(Champion.Leona, SpellSlot.R, true),
            new DangerousSpell(Champion.Lissandra, SpellSlot.R, true),
            new DangerousSpell(Champion.Lux, SpellSlot.R, true, 200),
            new DangerousSpell(Champion.Lux, SpellSlot.Q, false, 350),
            new DangerousSpell(Champion.Malphite, SpellSlot.R, true),
            new DangerousSpell(Champion.Malzahar, SpellSlot.R, true),
            new DangerousSpell(Champion.MissFortune, SpellSlot.R, true, 150),
            new DangerousSpell(Champion.MonkeyKing, SpellSlot.R, true),
            new DangerousSpell(Champion.Mordekaiser, SpellSlot.R, true, 650) {IsCleanseable = true},
            new DangerousSpell(Champion.Morgana, SpellSlot.R, true, 2800),
            new DangerousSpell(Champion.Nami, SpellSlot.R, true, 700),
            new DangerousSpell(Champion.Nami, SpellSlot.Q, false, 250),
            new DangerousSpell(Champion.Nautilus, SpellSlot.R, true),
            new DangerousSpell(Champion.Nocturne, SpellSlot.R, true, 500) {IsCleanseable = true},
            new DangerousSpell(Champion.Nunu, SpellSlot.R, true, 2800),
            new DangerousSpell(Champion.Olaf, SpellSlot.E, false, 350),
            new DangerousSpell(Champion.Orianna, SpellSlot.R, true),
            new DangerousSpell(Champion.Poppy, SpellSlot.R, true),
            new DangerousSpell(Champion.Riven, SpellSlot.R, true),
            new DangerousSpell(Champion.Rumble, SpellSlot.R, true, 150),
            new DangerousSpell(Champion.Sejuani, SpellSlot.R, true),
            new DangerousSpell(Champion.Skarner, SpellSlot.R, true),
            new DangerousSpell(Champion.Sona, SpellSlot.R, true),
            new DangerousSpell(Champion.Syndra, SpellSlot.R, true),
            new DangerousSpell(Champion.Talon, SpellSlot.R, true),
            new DangerousSpell(Champion.Tristana, SpellSlot.R, true),
            new DangerousSpell(Champion.Trundle, SpellSlot.R, false, 350),
            new DangerousSpell(Champion.Urgot, SpellSlot.R, true, 1000) {IsCleanseable = true},
            new DangerousSpell(Champion.Varus, SpellSlot.R, true),
            new DangerousSpell(Champion.Veigar, SpellSlot.R, true),
            new DangerousSpell(Champion.Velkoz, SpellSlot.R, true, 150) {IsCleanseable = true},
            new DangerousSpell(Champion.Vi, SpellSlot.R, true, 150),
            new DangerousSpell(Champion.Viktor, SpellSlot.R, true),
            new DangerousSpell(Champion.Vladimir, SpellSlot.R, true) {IsCleanseable = true},
            new DangerousSpell(Champion.Warwick, SpellSlot.R, true) {IsCleanseable = true},
            new DangerousSpell(Champion.Yasuo, SpellSlot.R, true),
            new DangerousSpell(Champion.Zed, SpellSlot.R, true, 4000) {IsCleanseable = true},
            new DangerousSpell(Champion.Zyra, SpellSlot.R, true, 250)
        }; 
    }

    public class DangerousSpell
    {
        public DangerousSpell(Champion champ, SpellSlot slot, bool stat, int bonusDelay = 0)
        {
            Slot = slot;
            Champion = champ;
            Stats = stat;
            BonusDelay = bonusDelay;
        }

        public SpellSlot Slot;
        public Champion Champion;
        public int BonusDelay;
        public bool Stats;
        public bool IsCleanseable = false;
    }
}
