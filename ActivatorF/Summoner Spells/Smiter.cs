using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace ActivatorF.Summoner_Spells
{
    class Smiter
    {
        public static readonly string[] SmiteableUnits =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron", "SRU_RiftHerald",
            "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug",  "Sru_Crab", "TT_Spiderboss",
            "TTNGolem", "TTNWolf", "TTNWraith"
        };
        
        private static string[] Smites = new[] { "s5_summonersmiteplayerganker", "s5_summonersmiteduel", "itemsmiteaoe", "s5_summonersmitequick", "summonersmite" };

        public static void SetSmiteSlot()
        {
            if (Smites.Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name))
                SummonerSpells.Smite = new Spell.Targeted(SpellSlot.Summoner1, 570);
            else if (Smites.Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name))
                SummonerSpells.Smite = new Spell.Targeted(SpellSlot.Summoner2, 570);
        }


        public static int GetSmiteDamage()
        {
            int level = Player.Instance.Level;
            int[] smitedamage =
            {
                20*level + 370,
                30*level + 330,
                40*level + 240,
                50*level + 100
            };
            return smitedamage.Max();
        }
    }
}
