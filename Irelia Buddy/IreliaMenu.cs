using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Irelia_Buddy
{


    class IreliaMenu
    {
        public static Menu IreliaMainMenu = MainMenu.AddMenu("Irelia Buddy", "memes.main");

        public static Menu ComboMenu = IreliaMainMenu.AddSubMenu("Combo", "memes.combo");

        public static Menu HarassMenu = IreliaMainMenu.AddSubMenu("Harass", "memes.harass");

        public static Menu LaneClearMenu = IreliaMainMenu.AddSubMenu("LaneClear", "memes.laneclear");

        public static Menu DrawingsMenu = IreliaMainMenu.AddSubMenu("Drawings", "memes.drawings");

        public static Menu MiscMenu = IreliaMainMenu.AddSubMenu("Misc", "memes.misc");

        public static void Initialize()
        {
            ComboMenu.AddGroupLabel("COMBO");
            ComboMenu.AddGroupLabel("Q settings");
            ComboMenu.Add("combo.q", new CheckBox("Use Q on enemy"));
            ComboMenu.Add("combo.q.minrange", new Slider("   Minimum range to Q enemy", 450, 0, 650));
            ComboMenu.AddLabel("                                                                                                                                                               .", 4);
            ComboMenu.Add("combo.q.undertower", new Slider("   Q enemy under tower only if their health % under", 40));
            ComboMenu.AddLabel("                                                                                                                                                               .", 4);
            ComboMenu.Add("combo.q.lastsecond", new CheckBox("Use Q to target always before W buff ends (range doesnt matter)"));
            ComboMenu.AddLabel("                                                                                                                                                               .", 4);
            ComboMenu.Add("combo.q.gc", new CheckBox("Use Q to gapclose (killable minions)"));
            ComboMenu.AddSeparator(12);
            ComboMenu.AddGroupLabel("W settings");
            ComboMenu.Add("combo.w", new CheckBox("Use W"));
            ComboMenu.AddSeparator(12);
            ComboMenu.AddGroupLabel("E settings");
            ComboMenu.Add("combo.e", new CheckBox("Use E"));
            ComboMenu.AddLabel("                                                                                                                                                               .", 4);
            ComboMenu.Add("combo.e.logic", new CheckBox("   advanced logic"));
            ComboMenu.AddSeparator(12);
            ComboMenu.AddGroupLabel("R settings");
            ComboMenu.Add("combo.r", new CheckBox("Use R"));
            ComboMenu.AddLabel("                                                                                                                                                               .", 4);
            ComboMenu.Add("combo.r.weave", new CheckBox("   sheen synergy"));
            ComboMenu.AddLabel("                                                                                                                                                               .", 4);
            ComboMenu.Add("combo.r.selfactivated", new CheckBox("   only if self activated", false));
            ComboMenu.AddSeparator(12);
            ComboMenu.AddGroupLabel("Extra");
            ComboMenu.Add("combo.items", new CheckBox("Use items"));
            ComboMenu.Add("combo.ignite", new CheckBox("Use ignite if combo killable"));

            HarassMenu.AddGroupLabel("HARASS");
            HarassMenu.AddGroupLabel("Q settings");
            HarassMenu.Add("harass.q", new CheckBox("Use Q on enemy"));
            HarassMenu.Add("harass.q.minrange", new Slider("   Minimum range to Q enemy", 450, 0, 650));
            HarassMenu.AddLabel("                                                                                                                                                               .", 4);
            HarassMenu.Add("harass.q.undertower", new Slider("   Q enemy under tower only if their health % under", 40));
            HarassMenu.AddLabel("                                                                                                                                                               .", 4);
            HarassMenu.Add("harass.q.lastsecond", new CheckBox("Use Q to target always before W buff ends (range doesnt matter)"));
            HarassMenu.AddLabel("                                                                                                                                                               .", 4);
            HarassMenu.Add("harass.q.gc", new CheckBox("Use Q to gapclose (killable minions)"));
            HarassMenu.AddSeparator(12);
            HarassMenu.AddGroupLabel("W settings");
            HarassMenu.Add("harass.w", new CheckBox("Use W"));
            HarassMenu.AddSeparator(12);
            HarassMenu.AddGroupLabel("E settings");
            HarassMenu.Add("harass.e", new CheckBox("Use E"));
            HarassMenu.AddLabel("                                                                                                                                                               .", 4);
            HarassMenu.Add("harass.e.logic", new CheckBox("   advanced logic"));
            HarassMenu.AddSeparator(12);
            HarassMenu.AddGroupLabel("R settings");
            HarassMenu.Add("harass.r", new CheckBox("Use R"));
            HarassMenu.AddLabel("                                                                                                                                                               .", 4);
            HarassMenu.Add("harass.r.weave", new CheckBox("   sheen synergy"));
            HarassMenu.AddLabel("                                                                                                                                                               .", 4);
            HarassMenu.Add("harass.r.selfactivated", new CheckBox("   only if self activated", false));

            LaneClearMenu.Add("laneclear.q", new CheckBox("LaneClear Q"));
            LaneClearMenu.AddSeparator(12);
            LaneClearMenu.Add("laneclear.r", new CheckBox("LaneClear R - SOON", false));
            LaneClearMenu.Add("laneclear.r.minions", new Slider("Minimum minions hit", 3, 1, 6));
            LaneClearMenu.Add("laneclear.mana", new Slider("Mana manager (%)", 40, 1));

            DrawingsMenu.Add("drawings.q", new CheckBox("Draw Q"));
            DrawingsMenu.AddLabel("                                                                                                                                                               .", 4);
            DrawingsMenu.Add("drawings.e", new CheckBox("Draw E"));
            DrawingsMenu.AddLabel("                                                                                                                                                               .", 4);
            DrawingsMenu.Add("drawings.r", new CheckBox("Draw R"));

            MiscMenu.Add("misc.ks.q", new CheckBox("Killsteal Q"));
            MiscMenu.Add("misc.ks.e", new CheckBox("Killsteal E"));
            MiscMenu.Add("misc.ks.r", new CheckBox("Killsteal R"));
            MiscMenu.Add("misc.ag.e", new CheckBox("Anti-Gapclose E"));
            MiscMenu.Add("misc.interrupt", new CheckBox("Stun interruptable spells"));
            MiscMenu.Add("misc.stunundertower", new CheckBox("Stun enemy with tower aggro - soon"));
        }
    }
}