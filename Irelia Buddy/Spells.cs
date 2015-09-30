using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace Irelia_Buddy
{
    using EloBuddy.SDK.Enumerations;

    class Spells
    {
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Skillshot R { get; private set; }
        public static SpellSlot Ignite { get; private set; }
        public static Item Youmuu { get; private set; }
        public static Item Cutlass { get; private set; }
        public static Item Blade { get; private set; }
        public static Item Tiamat { get; private set; }
        public static Item Hydra { get; private set; }


        public static void Initialize()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 650);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 350);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Linear, 0, 1600, 65);

            Ignite = ObjectManager.Player.GetSpellSlotFromName("summonerdot");
            Youmuu = new Item(3142, Q.Range);
            Cutlass = new Item(3144, 450f);
            Blade = new Item(3153, 450f);
            Tiamat = new Item(3077, 400f);
            Hydra = new Item(3074, 400f);
        }
    }
}
