﻿using AndromedaStudio.Data.Classes;
using System.Windows;
using System.Windows.Controls;

namespace AndromedaStudio.Data.Controls.HeadToolsPanel.Pages
{
    public partial class Notification : Page
    {
        public Notification()
        {
            InitializeComponent();

            if(Database.NotificationsManager.Notifications.Count > 0)
            {
                Height = double.NaN;
                NotificationBorder.Visibility = Visibility.Visible;
                Database.MainWindow.NotificationButton.New = false;
                foreach (Notifications.Notification item in Database.NotificationsManager.Notifications)
                {
                    var obj = new Controls.Notification
                    {
                        Content = item.Caption,
                        Description = item.Description,
                        Icon = item.Icon
                    };
                    Notifications.Children.Add(obj);
                }
            }
        }

        private void Menu_Select(object sender, RoutedEventArgs e)
        {
            Classes.Menu.SetPage(sender);
            Classes.HeadTools.HideContent();
        }
    }
}
