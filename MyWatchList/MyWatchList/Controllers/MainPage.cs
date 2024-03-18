using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyWatchList.Interfaces;
using MyWatchList.Model.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyWatchList.Controllers
{
    public sealed partial class MainPage : Page
    {
        private RetrieveC _retrieveC;
        private DeleteC _deleteC;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeFirestore();
        }

        private void InitializeFirestore()
        {
            _retrieveC = new RetrieveC(dataReceived);
            _retrieveC.execute();
            _deleteC = new DeleteC();
        }

        private void dataReceived(List<IWatchable> dataList)
        {
            populateLv(dataList);
        }

        private void populateLv(List<IWatchable> dataList)
        {
            List<ActionItem> actionItems = new List<ActionItem>();
            foreach (var watchable in dataList)
            {
                actionItems.Add(new ActionItem
                {
                    Title = watchable.Name,
                    ImageSource = watchable.ImageUrl
                });
            }
            ActionListView.ItemsSource = actionItems;
        }

        public class ActionItem
        {
            public string Title { get; set; }
            public string ImageSource { get; set; }
        }

        private void CreateCategory_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Use the Frame to navigate to the CreateCategory page
            MainFrame.Navigate(typeof(CreateCategory));
        }

        private void AddShow_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(AddShow));
        }

        private void EditShow_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(EditShow));
        }

        private async void DeleteShow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ActionItem actionItem)
            {
                _deleteC.SetDocRef(actionItem.Title);
                await _deleteC.Delete();

                //Refresh the ListView (thank god)
                _retrieveC.execute();
            }
        }







    }
}
