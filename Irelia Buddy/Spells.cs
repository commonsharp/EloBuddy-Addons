using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

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
        public static Item Hydra1 { get; private set; }
        public static Item Hydra2 { get; private set; }
        public static Item Corruptpot { get; private set; }
        public static Item Randuin { get; private set; }

        public static void Initialize()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 650);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 425);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Linear, 260, 1600, 120);
            //Chat.Print("init");
            Ignite = Player.Instance.GetSpellSlotFromName("summonerdot");
            Youmuu = new Item(3142, 650f);   //Youmuus_Ghostblade
            Cutlass = new Item(3144, 450f);     //Bilgewater_Cutlass
            Blade = new Item(3153, 450f);       //Blade_of_the_Ruined_King
            Tiamat = new Item(3077, 400f);      //Tiamat_Melee_Only
            Hydra1 = new Item(3074, 400f);      //Ravenous_Hydra
            Hydra2 = new Item(3748, 385f);      //Titanic_Hydra
            Corruptpot = new Item(2033, 650f);  //Corrupting_Potion
            Randuin = new Item(ItemId.Randuins_Omen, 500f); //Randuins_Omen
        }
    }
}