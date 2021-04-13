using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace WeatherNotification.Utils
{
    public class TraySystem
    {
        public static Window window;
        private static NotifyIcon notifyIcon;

        public static void SetTray()
        {
            notifyIcon = new NotifyIcon();

            notifyIcon.Icon = Properties.Resources.icon;
            notifyIcon.Text = "미세먼지 알림이";
            notifyIcon.ContextMenu = GetContextMenu();
            notifyIcon.MouseDoubleClick += ViewItem_Click;
            notifyIcon.Visible = true;
        }

        public static void HideTray()
        {
            notifyIcon.Visible = false;
            notifyIcon.Icon = null;
        }

        private static ContextMenu GetContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            MenuItem viewItem = new MenuItem("View");
            MenuItem exitItem = new MenuItem("Exit");

            viewItem.Click += ViewItem_Click;
            exitItem.Click += ExitItem_Click;

            menu.MenuItems.Add(viewItem);
            menu.MenuItems.Add(exitItem);

            return menu;
        }

        private static void ViewItem_Click(object sender, System.EventArgs e)
        {
            window.Visibility = Visibility.Visible;
            HideTray();
        }

        private static void ExitItem_Click(object sender, System.EventArgs e)
        {
            HideTray();
            Environment.Exit(0);
        }

        //public static async void ShowNotification(string title, string message, ToolTipIcon icon)
        //{
        //    await Task.Run(() => notifyIcon.ShowBalloonTip(5000, title, message, icon));
        //}
    }
}
