using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyWatchList.Controllers;
using static MyWatchList.Controllers.MainPage;
using MyWatchList.Interfaces;

namespace MyWatchList.Views
{
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            this.InitializeComponent();
            this.DataContext = Watchable;
        }

        public IWatchable Watchable { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is IWatchable watchableItem)
            {
                Watchable = watchableItem;
                //Converting the list of categories into a string (yay that's cool) and also splitting them with ,
                var categoriesString = string.Join(", ", Watchable.Category);
                WatchableCategoryTextBlock.Text = categoriesString;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
