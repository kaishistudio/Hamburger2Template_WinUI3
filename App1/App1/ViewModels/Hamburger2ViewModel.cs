using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using App1.Views;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls.Primitives;
using App1.Services;
using WinUIEx;
using Windows.Devices.Enumeration;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using App1.Views.Pages;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace App1.ViewModels
{
    internal class Hamburger2ViewModel : ObservableRecipient
    {
        SettingsViewModel settingsViewModel=new SettingsViewModel();
        public string AppName
        {
            get { return SystemInformation.Instance.ApplicationName; }
            set { }
        }
        public Hamburger2ViewModel()
        {

        }
        public void Start()
        {
            UpdateSize();
            UpdateTitleBar();
            Shares.Data.ListView_PageList.SelectedIndex = 0;
            foreach (var p in Shares.Data.List_Pages)
            {
                AddPageToListViewPageList(p.Name, p.IconCode);
            }
            App.AddUserPage("Settings", "\xE115", Symbol.Setting, typeof(SettingsPage));
            SqliteInit();
        }
        public void AddPageToListViewPageList(string name, string icon)
        {
            ListViewItem item = new ListViewItem() { Tag = name };
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock tb1 = new TextBlock()
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                FontSize = 22,
                Text = icon,
                VerticalAlignment = VerticalAlignment.Center
            };
            sp.Children.Add(tb1);
            TextBlock tb2 = new TextBlock()
            {
                Text = name,
                FontSize = 14,
                Margin = new Thickness(20, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            sp.Children.Add(tb2);
            item.Content = sp;
            Shares.Data.ListView_PageList.Items.Add(item);
        }
        public void SetFrame(string tag, Type type)
        {
            if (Shares.Data.Tb_Header != null) Shares.Data.Tb_Header.Text = tag;
            if (GetFrame(tag) == null)
            {
                Frame frame = new() { Tag = tag };
                frame.Navigate(type);
                Shares.Data.G_Frames.Children.Add(frame);
            }
        }
        Frame GetFrame(string tag)
        {
            Frame result = null;
            foreach (var f in Shares.Data.G_Frames.Children)
            {
                if (f.GetType() == typeof(Frame))
                {
                    var frame = f as Frame;
                    if (frame.Tag.ToString() == tag)
                    {
                        frame.Visibility = Visibility.Visible;
                        result = frame;
                    }
                    else
                    {
                        frame.Visibility = Visibility.Collapsed;
                    }
                }
            }
            return result;
        }
        public void WindowEx_SizeChanged(double param)
        {
            UpdateSize();
            UpdateTitleBar();
        }
        public void UpdateTitleBar()
        {
            if (Shares.Data.TitleBar != null && Shares.Data.MainWindow != null)
            {
                Shares.Data.MainWindow.SetTitleBar(Shares.Data.TitleBar);
                Shares.Data.TitleBar.MinWidth = 188;
            }
        }
        public void ListView_SelectionChanged(ListView param)
        {
            if (param.SelectedIndex != -1)
            {
                var SelectedItem = param.SelectedItem as ListViewItem;
                var tag = SelectedItem.Tag.ToString();
                SetFrame(tag, Shares.Data.List_Pages.First(o => o.Name == tag).PageType);
            }
            param.SelectedIndex = -1;
            UpdateSize();
        }
        public void HamburgerButton_Click(SplitView param)
        {
            param.IsPaneOpen = !param.IsPaneOpen;
        }
        public void UpdateSize()
        {
            if (Shares.Data.Hamburger2.ActualWidth > Shares.Data.Hamburger2.ActualHeight)
            {
                Shares.Data.SplitView.DisplayMode = SplitViewDisplayMode.Inline;
                Shares.Data.SplitView.IsPaneOpen = true;
                if (Shares.Data.G_Pane != null) Shares.Data.G_Pane.Background = null;
            }
            else
            {
                Shares.Data.SplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                Shares.Data.SplitView.IsPaneOpen = false;
                if (Shares.Data.G_Pane != null) Shares.Data.G_Pane.Background = GetThemeBackground();
            }
        }
        SolidColorBrush GetThemeBackground()
        {
            var theme = settingsViewModel.GetSavedRequestedTheme();
            switch (theme)
            {
                case ElementTheme.Dark:
                    return new SolidColorBrush(Color.FromArgb(250, 46, 46, 46));
                case ElementTheme.Light:
                    return new SolidColorBrush(Color.FromArgb(250, 211, 211, 211));
                default:
                    return null;
            }
        }
        public async void SqliteInit()
        {
            await SqliteService.CreatOrReadDBByName("Data", Shares.Data.UserDBTableName, Shares.Data.UserDBColumns);
            var t = "";
            foreach (var d in Shares.Data.UserDBColumns)
            {
                t += d + ",";
            }
            if (t.Length > 0) t = t.Substring(0, t.Length - 1);
            for (int i = 0; i < 10; i++)
            {
                SqliteService.EditDBByCommand($@"INSERT INTO {Shares.Data.UserDBTableName} ({t}) VALUES ('{i}','{i * i}');");
            }

        }
    }
}
