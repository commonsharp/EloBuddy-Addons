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
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron",
            "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug",  "Sru_Crab", "TT_Spiderboss",
            "TTNGolem", "TTNWolf", "TTNWraith"
        };
        
        private static string[] Smites = new[] { "s5_summonersmiteplayerganker", "s5_summonersmiteduel", "itemsmiteaoe", "s5_summonersmitequick", "summonersmite" };

        public static void SetSmiteSlot()
        {
            if (Smites.Contains(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1).Name))
                SummonerSpells.Smite = new Spell.Targeted(SpellSlot.Summoner1, 500);
            else if (Smites.Contains(ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2).Name))
                SummonerSpells.Smite = new Spell.Targeted(SpellSlot.Summoner2, 500);
        }


        public static int GetSmiteDamage()
        {
            int level = ObjectManager.Player.Level;
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
