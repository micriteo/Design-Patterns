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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Windows;
using Microsoft.UI.Xaml.Media.Imaging;
using ObserverDesignPatterns.Model;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyWatchList
{

    //Mimics values from the firebase for testing
    public class Item
    {
        //fields
        public List<string> Category { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }

        //constructor
        public Item(List<string> category, string description, string name, string type, string imageUrl)
        {
            this.Category = category;
            this.Description = description;
            this.Name = name;
            this.Type = type;
            this.ImageUrl = imageUrl;
        }
    }

    //will be the data structure used to display everything properly
    public class GroupedItems
    {
        public string Category { get; set; }
        public List<Item> Items = new List<Item>();

        public GroupedItems(string category)
        {
            Category = category;
        }

        public void addItem(Item item)
        {
            Items.Add(item);
        }

        public void removeItem(Item item)
        {
            Items.Remove(item);
        }
    }

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        //declared outside MainWindow so it can be used as a binding
        public ObservableCollection<GroupedItems> GroupedItemsCollection { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();

            //a list of items made to mimic DB info
            var items = new List<Item>
            {
                //Test animes, all containing mystery and one other category for testing
                new Item(new List<string> {"Mystery","High School"}, "High schoolers mess with weird monsters", "Bakemonogatari", "Anime", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\bakemonogatari.jpg"),
                new Item(new List<string> {"Mystery", "Detective" }, "Detectives solve crimes through entering minds", "Id:Invaded", "Anime", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\IdInvaded.jpg"),
                new Item(new List<string> {"Mystery", "Detective" }, "Ginko travels ancient japan solving problems related to monsters called mushi", "Mushishi", "Anime", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Mushishi.jpg"),

                //Movies new List<string> 
                new Item(new List<string> {"Action", "Gangster" }, "Gaijin becomes yakuza and is a little too good at it", "The Outsider", "Movie", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\TheOutsider.png"),
                new Item(new List<string> {"Comedy", "Fantasy" }, "Ogre is forced out of his swamp and secretly enjoys it", "Shrek", "Movie", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Shrek.jpg"),
                new Item(new List<string> {"Action", "Comedy" }, "Secret force behind german lines plans to assassinate Hitler", "Inglorious Basterds", "Movie", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\InglouriousBasterds.jpg"),

                //Series new List<string> 
                new Item(new List<string> {"Comedy", "Superhero" }, "Super heroes arent as heroic and great as they seem...", "The Boys", "Series", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\TheBoys.jpg"),
                new Item(new List<string> {"Comedy", "Supernatural" }, "Two brothers hunt ghosts and try to find their father", "Supernatural", "Series", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Supernatural.jpg"),
                new Item(new List<string> {"Educational", "History" }, "Documentary about the history of the Roman Empire", "The Boys", "Series", @"C:\Users\Dimi\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\AncientRome.jpg"),
            };

            //get categories from items and place in field that  remembers the names
            List<string> CategoryList = RegisterCategories(items);

            //using sorted categories and items add them to a object suitable for display (a collection of groupedItems)
            GroupedItemsCollection = new ObservableCollection<GroupedItems>(OrderItemsByCategory(CategoryList, items));

        }

        //A Method that takes all the items and makes a list of shows from it which can be used to make the categories show in the UI
        public List<string> RegisterCategories(List<Item> items)
        {
            //temp list
            List<string> categories = new List<string>();

            //loops all items
            foreach (Item item in items)
            {
                //loops all categories within an item
                foreach ( String category in item.Category)
                {
                    //if the category list is not null and the list does not contain the category already
                    if(item.Category != null && !categories.Contains(category))
                    {
                        //add the category to the list
                        categories.Add(category);
                    } else
                    {
                        //do nothing
                    }
                }
            }
            //sort the categories alphabetically
            categories.Sort();

            return categories;
        }

        //takes list of categories and adds all shows with that category to a list
        public List<GroupedItems> OrderItemsByCategory(List<string> categories, List<Item> items)
        {
            //create grouped item list
            var ItemsByCat = new List<GroupedItems>();

            //loop through all categories
            foreach(string category in categories)
            {
                //create grouped item
                var groupedItem = new GroupedItems(category);

                //loop through all items and check if they belong to that category
                foreach(Item item in items)
                {
                    //loop through all items categories
                    foreach(string itemCategory in item.Category)
                    {
                        //if in category add to list and end iterations
                        if(itemCategory == category)
                        {
                            groupedItem.addItem(item);
                            continue;
                        }
                    }
                }
                //display category only if it actually has contents
                if(groupedItem.Items.Count > 0)
                {
                    ItemsByCat.Add(groupedItem);
                }
            }

            return ItemsByCat;
        }
    }
}
