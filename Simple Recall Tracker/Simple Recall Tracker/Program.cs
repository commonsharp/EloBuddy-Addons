using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Simple_Recall_Tracker
{
    class Program
    {
        private static Menu main;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            main = MainMenu.AddMenu("RecallTracker", "simple.recall.tracker");
            main.AddGroupLabel("Simple Recall Tracker");
            main.AddLabel("by Legos");
            main.AddSeparator();
            main.Add("enabled", new CheckBox("Enabled"));

            Chat.Print("[EloBuddy]<font color='#FFFFFF'>:  >></font> <font color='#79BAEC'> Simple Recall Tracker </font> <font color='#FFFFFF'><<</font>");
            Teleport.OnTeleport += Teleport_OnTeleport;
        }

        private static string FormatTime(double time)
        {
            var t = TimeSpan.FromSeconds(time);
            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        private static void Teleport_OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            if (sender.Team == Player.Instance.Team || !main["enabled"].Cast<CheckBox>().CurrentValue) return;

            if (args.Status == TeleportStatus.Start)
            {     
                Chat.Print("<font color='#ffffff'>[" + FormatTime(Game.Time) + "]</font> " + sender.BaseSkinName + " has <font color='#00ff66'>started</font> recall.");
            }

            if (args.Status == TeleportStatus.Abort)
            {         
                Chat.Print("<font color='#ffffff'>[" + FormatTime(Game.Time) + "]</font> " + sender.BaseSkinName + " has <font color='#ff0000'>aborted</font> recall.");
            }

            if (args.Status == TeleportStatus.Finish)
            {
                Chat.Print("<font color='#ffffff'>[" + FormatTime(Game.Time) + "]</font> " + sender.BaseSkinName + " has <font color='#fdff00'>finished</font> recall.");
            }
        }
    }
}
