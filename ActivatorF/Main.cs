using System;
using ActivatorF.DamageEngine;
using ActivatorF.Items;
using ActivatorF.Summoner_Spells;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy;

namespace MainMenuActivator
{
    public class MainActivator
    {
        public static Menu Menu;
        public MainActivator()
        {
            Load();
        }
        private void Load()
        {
            Loading.OnLoadingComplete += Game_OnGameLoad;
        }

        private void Game_OnGameLoad(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Activator F", "activatorMenu");
            Menu.AddGroupLabel("Activator F");
            Menu.AddLabel("Activator F was first created under the name ActivatorBuddy by Fluxy (thank you!).");
            Menu.AddSeparator();
            Menu.AddLabel("Activator F was edited and continued by Maxhyt, fixed by test");
            Menu.AddSeparator(30);
            Menu.AddLabel("If you found any NEW bugs or new ideas for this addon, please leave a comment under my topic.");
            Menu.AddSeparator(40);
            Menu.AddLabel("Thank you all for using and supporting this addon :D", 30);
            Menu.AddSeparator(60);
            //System.Version version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            Menu.AddLabel("Version: 2016.03.11" /*+ version*/, 30);
            ItemManager.Init();
            SummonerSpells.Init();
            Engine.Init();
        }
    }
}