using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyWatchList.Interfaces;
using MyWatchList.Model;
using MyWatchList.Model.Commands;
using MyWatchList.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MyWatchList.Controllers
{

    //will be the data structure used to display everything properly
    public class GroupedWatchables
    {
        public string Category { get; set; }
        public List<IWatchable> Watchables = new List<IWatchable>();

        public GroupedWatchables(string category)
        {
            Category = category;
        }

        public void addWatchable(IWatchable watchable)
        {
            Watchables.Add(watchable);
        }

        public void removeWatchable(IWatchable watchable)
        {
            Watchables.Remove(watchable);
        }
    }

    public sealed partial class MainPage : Page
    {
        //Fields
        private RetrieveShowC _retrieveC;
        private DeleteC _deleteC;
        private DeleteCategoryC _deleteCategoryC;
        private EditC _editC;

        //Fields
        //declared outside MainWindow so it can be used as a binding
        public ObservableCollection<GroupedWatchables> GroupedItemsCollection { get; set; }
        public List<IWatchable> Watchables { get; set; }
        public Publisher publisher { get; set; }

        //Constructor
        public MainPage()
        {
            this.InitializeComponent();
            this._retrieveC = new RetrieveShowC(dataReceived);
            this._retrieveC.execute();
            this._deleteC = new DeleteC();
            this._deleteCategoryC = new DeleteCategoryC();
            this._editC = new EditC();
            publisher = new Publisher();
        }

        //Populate the ListView with the watchables
        private void dataReceived(List<IWatchable> dataList)
        {
            populateLv(dataList);
        }

        private void populateLv(List<IWatchable> dataList)
        {
            //get categories from items and place in field that  remembers the names
            List<string> CategoryList = RegisterCategories(dataList);

            //using sorted categories and items add them to a object suitable for display (a collection of groupedItems)
            GroupedItemsCollection = new ObservableCollection<GroupedWatchables>(OrderItemsByCategory(CategoryList, dataList));

            CategoryListView.ItemsSource = GroupedItemsCollection;
        }

        //Create Category 
        private void CreateCategory_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(CreateCategory));
        }

        //Delete Category
        private async void DeleteCategory(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is GroupedWatchables grpWatchable)
            {
                //notify the watchables about the removal of the _category
                publisher.NotifySubscribers(grpWatchable.Category);

                //remove the _category from the _category list
                GroupedItemsCollection.Remove(grpWatchable);

                _deleteCategoryC.setName(grpWatchable.Category);
                _deleteCategoryC.execute();
            }
        }

        //Add Show
        private void AddShow_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(AddShow));
        }

        //Delete Show (we want to check if it is a watchable and then delete it based on the _docRef)
        private async void DeleteShow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is IWatchable watchable)
            {
                //get the watchable out of the list as it is removed and we dont need it in the list anymore
                publisher.Unsubscribe(watchable);

                _deleteC.SetDocRef(watchable.Name);
                await _deleteC.Delete();

                //Refresh the ListView (thank god)
                _retrieveC.execute();
            }
        }

        //Edit Show (we want to check if it is a watchable and then send us to the editPage alongside with it to grab the doc ref)
        private async void EditShow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is IWatchable watchable)
            {
                //_editC.SetDocRef(actionItem.Title);
                MainFrame.Navigate(typeof(EditShow), watchable.Name); // Pass the _docRef to EditShow
            }
        }

        //Action Item (it is the watchable, we want to check if it is a watchable and then send us to the infoPage alongside with it to grab the doc ref)
        private void ActionItem_Click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is IWatchable watchableItem)
            {
                MainFrame.Navigate(typeof(InfoPage), watchableItem);
            }
        }

        //A Method that takes all the items and makes a list of shows from it which can be used to make the categories show in the UI
        public List<string> RegisterCategories(List<IWatchable> watchables)
        {
            //temp list
            List<string> categories = new List<string>();

            //loops all items
            foreach (IWatchable item in watchables)
            {
                //loops all categories within an item
                foreach (String category in item.Category)
                {
                    //if the _category list is not null and the list does not contain the _category already
                    if (item.Category != null && !categories.Contains(category))
                    {
                        //add the _category to the list
                        categories.Add(category);
                    }
                    else
                    {
                        //do nothing
                    }
                }
            }
            //sort the categories alphabetically
            categories.Sort();

            return categories;
        }

        //takes list of categories and adds all shows with that _category to a list
        public List<GroupedWatchables> OrderItemsByCategory(List<string> categories, List<IWatchable> watchable)
        {
            //create grouped item list
            var WatchablesByCat = new List<GroupedWatchables>();

            //loop through all categories
            foreach (string category in categories)
            {
                //create grouped item
                var groupedItem = new GroupedWatchables(category);

                //loop through all items and check if they belong to that _category
                foreach (IWatchable watchables in watchable)
                {
                    //loop through all items categories
                    foreach (string itemCategory in watchables.Category)
                    {
                        //if in _category add to list and end iterations
                        if (itemCategory == category)
                        {
                            groupedItem.addWatchable(watchables);
                            publisher.Subscribe(watchables);
                            continue;
                        }
                    }
                }
                //display _category only if it actually has contents
                if (groupedItem.Watchables.Count > 0)
                {
                    WatchablesByCat.Add(groupedItem);
                }
            }

            return WatchablesByCat;
        }
    }
}
