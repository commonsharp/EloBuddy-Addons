using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ActivatorF.Summoner_Spells;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using MainMenuActivator;
using ActivatorF.DamageEngine;

namespace ActivatorF.Items
{
    internal static class ItemManager
    {
        public static Menu PotionsMenu, OffensiveMenu, Cleansers, DefenceMenu;
        private static EloBuddy.SDK.Item _qss, _merc, _dervish, _zhonya, _wooglets, _mikael, _fotm, _lotis, _seraphs, _seraph2;
        private static string[] champsnomana = { "Aatrox", "DrMundo", "Mordekaiser", "Vladimir", "Zac", "Akali", "Kennen", "LeeSin", "Shen", "Zed", "Garen", "Gnar", "Katarina", "RekSai", "Renekton", "Rengar", "Riven", "Rumble", "Shyvana", "Tryndamere", "Yasuo" };

        public static Dictionary<BuffType, string> BuffTypes = new Dictionary<BuffType, string>
        {
            {BuffType.Stun, "stunActivator"},
            {BuffType.Polymorph, "polymorphActivator"},
            {BuffType.Snare, "rootActivator"},
            {BuffType.Slow, "slowActivator"},
            {BuffType.Knockup, "knockupActivator"},
            {BuffType.Taunt, "tauntActivator"},
            {BuffType.Fear, "fearActivator"}
        };

        public static Item[] Items =
        {
            new Item("botrk", 450, CastType.Targeted, ItemId.Blade_of_the_Ruined_King, ItemType.Offensive),
            new Item("cutlass", 450, CastType.Targeted, ItemId.Bilgewater_Cutlass, ItemType.Offensive),
            new Item("tiamat", 250, CastType.SelfCast, ItemId.Tiamat_Melee_Only, ItemType.Offensive, true),
            new Item("ravenhydra", 250, CastType.SelfCast, ItemId.Ravenous_Hydra_Melee_Only, ItemType.Offensive, true),
            new Item("titanichydra", 250, CastType.SelfCast, ItemId.Titanic_Hydra, ItemType.Offensive, true),
            new Item("gunblade", 700, CastType.Targeted, ItemId.Hextech_Gunblade, ItemType.Offensive),
            new Item("ghostblade", 1500, CastType.SelfCast, ItemId.Youmuus_Ghostblade, ItemType.Offensive),
            new Item("refillpot", int.MaxValue, CastType.SelfCast, (ItemId) 2031, ItemType.Healing, false,
                "ItemCrystalFlask"),
            new Item("corruptpot", int.MaxValue, CastType.SelfCast, (ItemId) 2033, ItemType.Healing, false,
                "ItemDarkCrystalFlask"),
            new Item("corruptpot", int.MaxValue, CastType.SelfCast, (ItemId) 2033, ItemType.ManaRestore, false,
                "ItemDarkCrystalFlask"),
            new Item("huntersPot", int.MaxValue, CastType.SelfCast, (ItemId) 2032, ItemType.Healing, false,
                "ItemCrystalFlaskJungle"),
            new Item("huntersPot", int.MaxValue, CastType.SelfCast, (ItemId) 2032, ItemType.ManaRestore, false,
                "ItemCrystalFlaskJungle"),
            new Item("healthPotion", int.MaxValue, CastType.SelfCast, ItemId.Health_Potion, ItemType.Healing, false,
                "RegenerationPotion"),
            new Item("biscuitPotion", int.MaxValue, CastType.SelfCast, (ItemId) 2010, ItemType.Healing, false,
                "ItemMiniRegenPotion"),
        };

        public static Spell.Active Cleanse;

        public static List<Item> ActiveItems = new List<Item>();

        public static void Init()
        {
            _qss = new EloBuddy.SDK.Item(ItemId.Quicksilver_Sash);
            _merc = new EloBuddy.SDK.Item(ItemId.Mercurial_Scimitar);
            _dervish = new EloBuddy.SDK.Item(ItemId.Dervish_Blade);
            _zhonya = new EloBuddy.SDK.Item(ItemId.Zhonyas_Hourglass);
            _wooglets = new EloBuddy.SDK.Item(ItemId.Wooglets_Witchcap);
            _mikael = new EloBuddy.SDK.Item(ItemId.Mikaels_Crucible);
            _fotm = new EloBuddy.SDK.Item(ItemId.Face_of_the_Mountain);
            _lotis = new EloBuddy.SDK.Item(ItemId.Locket_of_the_Iron_Solari);
            _seraphs = new EloBuddy.SDK.Item((ItemId)3040);
            _seraph2 = new EloBuddy.SDK.Item((ItemId)3048);

            var menu = MainActivator.Menu;

            OffensiveMenu = menu.AddSubMenu("Offensive Items", "offItems");
            OffensiveMenu.AddGroupLabel("Offensive Items");
            OffensiveMenu.AddLabel("(Activates With Combo)");
            OffensiveMenu.AddLabel("Blade Of The Ruined King");
            OffensiveMenu.Add("botrkManager", new CheckBox("Blade Of The Ruined King"));
            OffensiveMenu.Add("botrkManagerMinMeHP", new Slider("Self HP %", 70, 1));
            OffensiveMenu.Add("botrkManagerMinEnemyHP", new Slider("Enemy HP %", 99, 1));
            OffensiveMenu.AddLabel("Cutlass");
            OffensiveMenu.Add("cutlassManager", new CheckBox("Cutlass"));
            OffensiveMenu.Add("cutlassManagerMinMeHP", new Slider("Self HP %", 70,1));
            OffensiveMenu.Add("cutlassManagerMinEnemyHP", new Slider("Enemy HP %", 99,1));

            if (Player.Instance.IsMelee)
            {
                OffensiveMenu.AddLabel("Tiamat");
                OffensiveMenu.Add("tiamatManager", new CheckBox("Use Tiamat"));
                OffensiveMenu.Add("tiamatManagerMinMeHP", new Slider("Self HP %", 99 ,1));
                OffensiveMenu.Add("tiamatManagerMinEnemyHP", new Slider("Enemy HP %", 99, 1));
                OffensiveMenu.AddLabel("Hydra");
                OffensiveMenu.Add("ravenhydraManager", new CheckBox("Use Ravenous Hydra"));
                OffensiveMenu.Add("ravenhydraManagerMinMeHP", new Slider("Self HP %", 99, 1));
                OffensiveMenu.Add("ravenhydraManagerMinEnemyHP", new Slider("Enemy HP %", 99, 1));
                OffensiveMenu.Add("titanichydraManager", new CheckBox("Use Titanic Hydra"));
                OffensiveMenu.Add("titanichydraManagerMinMeHP", new Slider("Self HP %", 99, 1));
                OffensiveMenu.Add("titanichydraManagerMinEnemyHP", new Slider("Enemy HP %", 99, 1));
            }

            OffensiveMenu.AddLabel("Gunblade");
            OffensiveMenu.Add("gunbladeManager", new CheckBox("Use Gunblade"));
            OffensiveMenu.Add("gunbladeManagerMinMeHP", new Slider("Self HP %", 99, 1));
            OffensiveMenu.Add("gunbladeManagerMinEnemyHP", new Slider("Enemy HP %", 99, 1));
            OffensiveMenu.AddLabel("GhostBlade");
            OffensiveMenu.Add("ghostbladeManager", new CheckBox("Use GhostBlade"));
            OffensiveMenu.Add("ghostbladeManagerMinMeHP", new Slider("Self HP %", 70, 1));
            OffensiveMenu.Add("ghostbladeManagerMinEnemyHP", new Slider("Enemy HP %", 99, 1));

            PotionsMenu = menu.AddSubMenu("Potions", "potions");
            PotionsMenu.AddGroupLabel("Potion Items");
            PotionsMenu.Add("healthPotionManager", new CheckBox("Health Potion"));
            PotionsMenu.Add("healthPotionManagerMinMeHP", new Slider("HP %", 65));
            PotionsMenu.Add("biscuitPotionManager", new CheckBox("Biscuit"));
            PotionsMenu.Add("biscuitPotionManagerMinMeHP", new Slider("HP %", 65));
            PotionsMenu.Add("refillpotManager", new CheckBox("Refillable Potion"));
            PotionsMenu.Add("refillpotManagerMinMeHP", new Slider("HP %", 65));
            PotionsMenu.Add("corruptpotManager", new CheckBox("Corrupt Potion"));
            PotionsMenu.Add("corruptpotManagerMinMeHP", new Slider("HP %", 60));
            /*if (Player.Instance.ChampionName == champsnomana.FirstOrDefault())*/ PotionsMenu.Add("corruptpotManagerMinMeMana", new Slider("Mana %", 30));
            PotionsMenu.Add("huntersPotManager", new CheckBox("Hunter's Potion"));
            PotionsMenu.Add("huntersPotManagerMinMeHP", new Slider("HP %", 60));
            /*if (Player.Instance.ChampionName == champsnomana.FirstOrDefault())*/ PotionsMenu.Add("huntersPotManagerMinMeMana", new Slider("Mana %", 30));

            Cleansers = menu.AddSubMenu("Cleansers", "cleansers");
            Cleansers.Add("useCleansers", new CheckBox("Use Cleansers"));
            Cleansers.AddGroupLabel("Cleansers Settings");
            Cleansers.Add("polymorphActivator", new CheckBox("PolyMorph"));
            Cleansers.Add("stunActivator", new CheckBox("Stun"));
            Cleansers.Add("tauntActivator", new CheckBox("Taunt"));
            Cleansers.Add("knockupActivator", new CheckBox("Knock-up"));
            Cleansers.Add("fearActivator", new CheckBox("Fear"));
            Cleansers.Add("rootActivator", new CheckBox("Root"));
            Cleansers.Add("slowActivator", new CheckBox("Slow"));
            Cleansers.AddSeparator();
            Cleansers.AddLabel("Cleanse Items / Summoner Spell");
            Cleansers.Add("cleanserscombo", new CheckBox("Combo Only", false));
            Cleansers.Add("mikaelsCleanser", new CheckBox("Mikael's Cruicble"));
            Cleansers.Add("mercurialScimitarCleanser", new CheckBox("Mercurial Scimitar"));
            Cleansers.Add("quicksilverSashCleanser", new CheckBox("Quicksilver Sash"));
            if (Game.MapId == GameMapId.CrystalScar)
                Cleansers.Add("dervishCleanser", new CheckBox("Dervish Blade"));
            Cleansers.Add("summonerSpellCleanse", new CheckBox("Summoner Cleanse"));
            Cleansers.AddSeparator();
            Cleansers.AddLabel("Humanizer");
            Cleansers.Add("qsshuman", new Slider("Delay QSS (Miliseconds)", 400, 0, 2000));
            Cleanse = SummonerSpells.HasSpell("summonerboost") ? new Spell.Active(Player.Instance.GetSpellSlotFromName("summonerboost"), int.MaxValue) : null;

            DefenceMenu = MainActivator.Menu.AddSubMenu("Defensive Items", "defmenuactiv");

            DefenceMenu.AddGroupLabel("Shield/Heal Items (self)");
            DefenceMenu.Add("Seraphs_Embrace", new CheckBox("Serahph's Embrace"));

            DefenceMenu.AddGroupLabel("Shield/Heal Items (ally/self)");
            DefenceMenu.Add("Mikaels_Crucible_Heal", new CheckBox("Mikaels Crucible"));
            DefenceMenu.AddLabel("Locket of the Iron Solari");
            DefenceMenu.Add("Locket_of_the_Iron_Solari", new CheckBox("Locket of the Iron Solari"));
            DefenceMenu.AddSeparator(0);
            DefenceMenu.Add("Locket_of_the_Iron_Solari_ally", new CheckBox("Ally"));
            DefenceMenu.Add("Locket_of_the_Iron_Solari_self", new CheckBox("Self"));
            DefenceMenu.AddLabel("Face of the Mountain");
            DefenceMenu.Add("Face_of_the_Mountain", new CheckBox("Face of the Mountain"));
            DefenceMenu.AddSeparator(0);
            DefenceMenu.Add("Face_of_the_Mountain_ally", new CheckBox("Ally"));
            DefenceMenu.Add("Face_of_the_Mountain_self", new CheckBox("Self"));

            DefenceMenu.AddGroupLabel("Cleanse Items (Dangerous Spells)");
            DefenceMenu.Add("Quicksilver_Sash", new CheckBox("Quicksilver Sash"));
            DefenceMenu.Add("Dervish_Blade", new CheckBox("Dervish Blade"));
            DefenceMenu.Add("Mercurial_Scimitar", new CheckBox("Mercurial Scimitar"));
            DefenceMenu.Add("Mikaels_Crucible_Cleanse", new CheckBox("Mikaels Crucible"));

            DefenceMenu.AddGroupLabel("Zhonyas Items");
            DefenceMenu.Add("Zhonyas_Hourglass", new CheckBox("Zhonyas Hourglass"));
            DefenceMenu.Add("Wooglets_Witchcap", new CheckBox("Wooglets Witchcap"));

            DefenceMenu.AddGroupLabel("Dangerous Spells");
            foreach (var dangerousSpell in DangerousSpells.Spells.Where(a => EntityManager.Heroes.Enemies.Any(b => b.Hero == a.Champion)))
            {
                DefenceMenu.Add(dangerousSpell.Champion.ToString() + dangerousSpell.Slot,
                    new CheckBox(dangerousSpell.Champion + ": " + dangerousSpell.Slot + (dangerousSpell.IsCleanseable ? " (Cleanseable)" : ""), dangerousSpell.Stats));
            }

            Game.OnTick += Game_OnTick;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            
            if(Player.Instance.Hero == Champion.Riven) Chat.Print("Tiamat/Hydra Disabled For Riven", Color.LimeGreen);
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !args.SData.IsAutoAttack() || !(args.Target is AIHeroClient) || args.Target != Orbwalker.LastTarget || Player.Instance.Hero == Champion.Riven) return;

            var target = (AIHeroClient) args.Target;
            if (target == null) return;
                

            foreach (var item in Items)
            {
                if (!item.MeleeOnly) continue;

                var menuItem = OffensiveMenu[item.Name + "Manager"].Cast<CheckBox>();
                var menuItemMe = OffensiveMenu[item.Name + "ManagerMinMeHP"].Cast<Slider>();
                var menuItemEnemy = OffensiveMenu[item.Name + "ManagerMinEnemyHP"].Cast<Slider>();

                if (!target.IsValidTarget() || target.Distance(Player.Instance) > item.Range || !menuItem.CurrentValue || menuItemMe.CurrentValue <= Player.Instance.HealthPercent || menuItemEnemy.CurrentValue <= target.HealthPercent)
                    continue;

                var spellSlot = Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                if (spellSlot == null || !Player.GetSpell(spellSlot.SpellSlot).IsReady) continue;
                Player.CastSpell(spellSlot.SpellSlot);
                return;
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            #region Defence
            foreach (var ally in EntityManager.Heroes.Allies)
            {
                if (Player.Instance.InDanger() == true && !Player.Instance.IsInShopRange() && !Player.Instance.IsDead)
                {
                    if (DefenceMenu["Seraphs_Embrace"].Cast<CheckBox>().CurrentValue && _seraphs.IsReady()) _seraphs.Cast();
                    if (DefenceMenu["Mikaels_Crucible_Heal"].Cast<CheckBox>().CurrentValue && _mikael.IsReady()) _mikael.Cast();
                    if (DefenceMenu["Locket_of_the_Iron_Solari"].Cast<CheckBox>().CurrentValue && _lotis.IsReady() && DefenceMenu["Locket_of_the_Iron_Solari_self"].Cast<CheckBox>().CurrentValue) _lotis.Cast();
                    if (DefenceMenu["Zhonyas_Hourglass"].Cast<CheckBox>().CurrentValue && _zhonya.IsReady()) _zhonya.Cast();
                    if (DefenceMenu["Wooglets_Witchcap"].Cast<CheckBox>().CurrentValue && _wooglets.IsReady()) _wooglets.Cast();
                }

                if (ally.InDanger() == true && !ally.IsInShopRange() && !ally.IsDead && ally.IsTargetable)
                {
                    if (DefenceMenu["Locket_of_the_Iron_Solari"].Cast<CheckBox>().CurrentValue && _lotis.IsReady() && DefenceMenu["Locket_of_the_Iron_Solari_ally"].Cast<CheckBox>().CurrentValue) _lotis.Cast(ally);
                    if (DefenceMenu["Face_of_the_Mountain"].Cast<CheckBox>().CurrentValue && _fotm.IsReady() && DefenceMenu["Face_of_the_Mountain_ally"].Cast<CheckBox>().CurrentValue) _fotm.Cast(ally);
                }
                if (Player.Instance.InMissileDanger() == true && !Player.Instance.IsInShopRange() && !Player.Instance.IsDead)
                {
                    if (DefenceMenu["Seraphs_Embrace"].Cast<CheckBox>().CurrentValue && _seraphs.IsReady()) _seraphs.Cast();
                    if (DefenceMenu["Mikaels_Crucible_Heal"].Cast<CheckBox>().CurrentValue && _mikael.IsReady()) _mikael.Cast();
                    if (DefenceMenu["Locket_of_the_Iron_Solari"].Cast<CheckBox>().CurrentValue && _lotis.IsReady() && DefenceMenu["Locket_of_the_Iron_Solari_self"].Cast<CheckBox>().CurrentValue) _lotis.Cast();
                }
            }
            #endregion
            foreach (var item in Items)
            {
                if (item.MeleeOnly) continue;
                switch (item.ItemType)
                {
                    case ItemType.Offensive:
                    {
                        if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;

                        var target = (Obj_AI_Base) Orbwalker.LastTarget;
                        
                        var menuItem = OffensiveMenu[item.Name + "Manager"].Cast<CheckBox>();
                        var menuItemMe = OffensiveMenu[item.Name + "ManagerMinMeHP"].Cast<Slider>();
                        var menuItemEnemy = OffensiveMenu[item.Name + "ManagerMinEnemyHP"].Cast<Slider>();

                        if (!target.IsValidTarget() || target.Distance(Player.Instance) > item.Range || 
                        (item.MeleeOnly && !Player.Instance.IsMelee) || !menuItem.CurrentValue || menuItemMe.CurrentValue < Player.Instance.HealthPercent || menuItemEnemy.CurrentValue < target.HealthPercent) continue;
                        
                        switch (item.CastType)
                        {
                            case CastType.Targeted:
                            {
                                var spellSlot = Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                                if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                                {
                                    Player.CastSpell(spellSlot.SpellSlot, target);
                                }
                            }
                                break;
                            case CastType.SelfCast:
                            {
                                var spellSlot = Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                                if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                                {
                                    Player.CastSpell(spellSlot.SpellSlot);
                                }
                            }
                                break;
                        }
                    }
                        break;
                    case ItemType.Healing:
                    {
                        var menuItem = PotionsMenu[item.Name + "Manager"].Cast<CheckBox>();
                        var menuItemMe = PotionsMenu[item.Name + "ManagerMinMeHP"].Cast<Slider>();
                        if (Player.Instance.IsInShopRange() || Player.Instance.HasBuff(item.BuffName) || !menuItem.CurrentValue ||
                            menuItemMe.CurrentValue < Player.Instance.HealthPercent) continue;
                        var spellSlot = Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                        if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                        {
                            Player.CastSpell(spellSlot.SpellSlot);
                        }
                    }
                        break;
                    case ItemType.ManaRestore:
                    {
                        if (!(Player.Instance.ChampionName == champsnomana.FirstOrDefault()))
                        {
                            var menuItem = PotionsMenu[item.Name + "Manager"].Cast<CheckBox>();
                            var menuItemMe = PotionsMenu[item.Name + "ManagerMinMeMana"].Cast<Slider>();
                            if (Player.Instance.IsInShopRange() || Player.Instance.HasBuff(item.BuffName) || !menuItem.CurrentValue ||
                                menuItemMe.CurrentValue < Player.Instance.ManaPercent) continue;
                            var spellSlot = Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == item.Id);
                            if (spellSlot != null && Player.GetSpell(spellSlot.SpellSlot).IsReady)
                            {
                                Player.CastSpell(spellSlot.SpellSlot);
                            }
                        }
                            
                    }
                        break;
                }
            }
            if (Cleansers["useCleansers"].Cast<CheckBox>().CurrentValue)
            {
                if (_mikael.IsReady() && Cleansers["mikaelsCleanser"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var buffInstance in Player.Instance.Buffs)
                    {
                        if (BuffTypes.ContainsKey(buffInstance.Type) && Cleansers[BuffTypes[buffInstance.Type]].Cast<CheckBox>().CurrentValue)
                            Core.DelayAction(() => _mikael.Cast(), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                    }
                    foreach (var dangerousSpell in DangerousSpells.Spells.Where(a => EntityManager.Heroes.Enemies.Any(b => b.Hero == a.Champion)))
                    {
                        if (DefenceMenu[dangerousSpell.Champion.ToString() + dangerousSpell.Slot].Cast<CheckBox>().CurrentValue && dangerousSpell.IsCleanseable)
                        {
                            Core.DelayAction(() => _mikael.Cast(), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                        }
                    }
                }
                if (_merc.IsReady() && Cleansers["mercurialScimitarCleanser"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var buffInstance in Player.Instance.Buffs)
                    {
                        if (BuffTypes.ContainsKey(buffInstance.Type) && Cleansers[BuffTypes[buffInstance.Type]].Cast<CheckBox>().CurrentValue)
                            Core.DelayAction(() => _merc.Cast(), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                    }
                    foreach (var dangerousSpell in DangerousSpells.Spells.Where(a => EntityManager.Heroes.Enemies.Any(b => b.Hero == a.Champion)))
                    {
                        if (DefenceMenu[dangerousSpell.Champion.ToString() + dangerousSpell.Slot].Cast<CheckBox>().CurrentValue && dangerousSpell.IsCleanseable)
                        {
                            Core.DelayAction(() => _merc.Cast(), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                        }
                    }
                }
                if (_qss.IsReady() && Cleansers["quicksilverSashCleanser"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var buffInstance in Player.Instance.Buffs)
                    {
                        if (BuffTypes.ContainsKey(buffInstance.Type) && Cleansers[BuffTypes[buffInstance.Type]].Cast<CheckBox>().CurrentValue)
                            Core.DelayAction(() => _qss.Cast(), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                    }
                    foreach (var dangerousSpell in DangerousSpells.Spells.Where(a => EntityManager.Heroes.Enemies.Any(b => b.Hero == a.Champion)))
                    {
                        if (DefenceMenu[dangerousSpell.Champion.ToString() + dangerousSpell.Slot].Cast<CheckBox>().CurrentValue && dangerousSpell.IsCleanseable)
                        {
                            Core.DelayAction(() => _qss.Cast(), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                        }
                    }
                }
                if (_dervish.IsReady() && Cleansers["dervishCleanser"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var buffInstance in Player.Instance.Buffs)
                    {
                        if (BuffTypes.ContainsKey(buffInstance.Type) && Cleansers[BuffTypes[buffInstance.Type]].Cast<CheckBox>().CurrentValue)
                            Core.DelayAction(() => _dervish.Cast(), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                    }
                    foreach (var dangerousSpell in DangerousSpells.Spells.Where(a => EntityManager.Heroes.Enemies.Any(b => b.Hero == a.Champion)))
                    {
                        if (DefenceMenu[dangerousSpell.Champion.ToString() + dangerousSpell.Slot].Cast<CheckBox>().CurrentValue && dangerousSpell.IsCleanseable)
                        {
                            Core.DelayAction(() => _dervish.Cast(), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                        }
                    }
                }
                if (Cleanse != null && Cleanse.IsReady() && Cleansers["summonerSpellCleanse"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var buffInstance in Player.Instance.Buffs)
                    {
                        if (BuffTypes.ContainsKey(buffInstance.Type) && Cleansers[BuffTypes[buffInstance.Type]].Cast<CheckBox>().CurrentValue)
                        {
                            if (BuffTypes.ContainsKey(buffInstance.Type) && Cleansers[BuffTypes[buffInstance.Type]].Cast<CheckBox>().CurrentValue)
                                Core.DelayAction(() => Player.CastSpell(Cleanse.Slot), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                        }
                    }
                    foreach (var dangerousSpell in DangerousSpells.Spells.Where(a => EntityManager.Heroes.Enemies.Any(b => b.Hero == a.Champion)))
                    {
                        if (DefenceMenu[dangerousSpell.Champion.ToString() + dangerousSpell.Slot].Cast<CheckBox>().CurrentValue && dangerousSpell.IsCleanseable)
                        {
                            Core.DelayAction(() => Player.CastSpell(Cleanse.Slot), Cleansers["qsshuman"].Cast<Slider>().CurrentValue);
                        }
                    }
                }
            }
        }

        public static bool HasBuff(this Obj_AI_Base unit, string s)
        {
            return
                unit.Buffs.Any(
                    a =>
                        a.Name.ToLower().Contains(s.ToLower()) || a.DisplayName.ToLower().Contains(s.ToLower()));
        }
    }
}