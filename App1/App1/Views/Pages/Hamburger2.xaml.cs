using App1.Services;
using App1.ViewModels;
using App1.Views.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1.Templates.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Hamburger2 : Page
    {
        Hamburger2ViewModel hamburger2ViewModel;
        public Hamburger2()
        {
            this.InitializeComponent();
            Shares.Data.Hamburger2 = this;
            Shares.Data.RequestedThemeList.Add(root);
            Shares.Data.TitleBar = g_title;
            Shares.Data.Tb_Header = tb_header;
            Shares.Data.G_Frames = g_frames;
            Shares.Data.SplitView = SplitView;
            Shares.Data.G_Pane = g_pane;
            Shares.Data.ListView_PageList = lv_pages;
            hamburger2ViewModel = new Hamburger2ViewModel();
        }
        public void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            hamburger2ViewModel.HamburgerButton_Click(SplitView);
        }

        public void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hamburger2ViewModel.ListView_SelectionChanged(sender as ListView);
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            new SettingsViewModel().ThemeInit();
            hamburger2ViewModel.Start();
            root.SizeChanged += (ss, ee) => { hamburger2ViewModel.WindowEx_SizeChanged(ee.NewSize.Width); };
        }
    }
}
