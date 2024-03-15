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
                new Item(new List<string> {"Mystery","High School"}, "High schoolers mess with weird monsters", "Bakemonogatari", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\bakemonogatari.jpg"),
                new Item(new List<string> {"Mystery", "Detective" }, "Detectives solve crimes through entering minds", "Id:Invaded", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\IdInvaded.jpg"),
                new Item(new List<string> {"Mystery", "Detective" }, "Ginko travels ancient japan solving problems related to monsters called mushi", "Mushishi", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Mushishi.jpg"),
                new Item(new List<string> {"Drama", "High School"}, "Kid who is really good at shogi, questions life", "3-gatsu no Lion", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\SangatsuNoLion.jpg"),
                new Item(new List<string> {"Action","Fantasy"}, "Horny chainsaw person simps for boss", "Chainsaw Man", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\ChainsawMan.jpg"),
                new Item(new List<string> {"Action", "High School" }, "Trying to take over the world with mechs and mind control", "Code Geass: Hangyaku no Lelouch", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\CodeGeassHangyakuNoLelouch.jpg"),
                new Item(new List<string> {"Survival", "Comedy" }, "Genius attempts to any% speedrun modern society", "Dr. Stone", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\DrStone.jpg"),
                new Item(new List<string> {"Action", "Comedy", "History"}, "Japanese soldier hunts for gold with ainu", "Golden Kamuy", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\GoldenKamuy.jpg"),
                new Item(new List<string> {"Thriller","Gambling"}, "Man Suffers from gambling addiction and suffers the consequences", "Inuyashiki ", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Gyakkyou.jpg"),
                new Item(new List<string> {"Action", "Comedy", "Mecha" }, "Teen and Old Man gain super battle bodies from aliens", "Inuyashiki ", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Inuyashiki.jpg"),
                new Item(new List<string> {"Action", "Adventure", "Comedy", "History" }, "Ninja, Gabimaru tries to escape funny island with Daoism", "Jigokuraku", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Jigokuraku.jpg"),
                new Item(new List<string> {"Action", "History", "Vampire"}, "Jojo hunts funny vampire using sunlight power", "JoJo no Kimyou na Bouken", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Jojo1.jpg"),
                new Item(new List<string> {"Action","Historical", "Drama", "Psychological", "Detective"}, "Japanese man chases down crazy german orphan to prove his innocence", "Monster", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Monster.jpg"),
                new Item(new List<string> {"Adventure", "Fantasy", "Comedy" }, "Deaf child wanders countryside to save kingdom", "Ousama Ranking", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\OusamaRanking.jpg"),
                new Item(new List<string> {"Drama", "Fantasy", "Action" }, "Idiot takes all wrong choices and finds out", "Re:Zero kara Hajimeru Isekai Seikatsu", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\ReZero.jpg"),
                new Item(new List<string> {"Action", "Drama", "History"}, "Child helps fathers murderer to murder fathers murderer", "Vinland Saga", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\VinlandSaga.jpg"),
                new Item(new List<string> {"Action", "Comedy", "Food", "High School"}, "Teen attempts to pass culinary school", "Shokugeki no Souma", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\ShokugekiNoSoma.jpg"),
                new Item(new List<string> {"Fantasy", "Comedy", "Food"}, "Adventurers make food in endless dungeon", "Dungeon Meshi", "Anime", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\DungeonMeshi.jpg"),

                //Movies new List<string> 
                new Item(new List<string> {"Action", "Gangster" }, "Gaijin becomes yakuza and is a little too good at it", "The Outsider", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\TheOutsider.png"),
                new Item(new List<string> {"Comedy", "Fantasy" }, "Ogre is forced out of his swamp and secretly enjoys it", "Shrek", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Shrek.jpg"),
                new Item(new List<string> {"Action", "Comedy" }, "Secret force behind german lines plans to assassinate Hitler", "Inglorious Basterds", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\InglouriousBasterds.jpg"),
                new Item(new List<string> {"Action", "Fantasy" }, "Children wander into closet world", "The Chronicles of Narnia", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Narnia.jpg"),
                new Item(new List<string> {"Thriller", "Horror", "Psychological", "Sigma" }, "Life of a corporatist in early 90s", "American Psycho", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\AmericanPsycho.jpg"),
                new Item(new List<string> {"Action", "Comedy", "Fantasy", "Sci-fi" }, "2 jedis save planet", "Episode I: The Phantom Menace", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\StarWars.jpg"),
                new Item(new List<string> {"History", "Drama" }, "He became death, destroyer of worlds", "Oppenheimer", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Oppenheimer.jpg"),
                new Item(new List<string> {"Comedy", "Survival", "Action" }, "Crazy person drags corpse of daniel radcliffe on an adventure", "Swiss Army Man", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\SwissArmyMan.jpg"),
                new Item(new List<string> {"Action", "Comedy" }, "Deadly samurai wanders japan and defeats foes without killing", "Rurouni Kenshin", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\RurouniKenshin.jpg"),
                new Item(new List<string> {"Comedy", "Food" }, "Man let rat cook", "Ratatouille", "Movie", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Ratatouille.jpg"),

                //Series new List<string> 
                new Item(new List<string> {"Comedy", "Superhero" }, "Super heroes arent as heroic and great as they seem...", "The Boys", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\TheBoys.jpg"),
                new Item(new List<string> {"Comedy", "Supernatural" }, "Two brothers hunt ghosts and try to find their father", "Supernatural", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Supernatural.jpg"),
                new Item(new List<string> {"Educational", "History" }, "Documentary about the history of the Roman Empire", "Fall of Rome", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\AncientRome.jpg"),
                new Item(new List<string> {"Action", "Comedy" }, "Raccoon and Bluejay disobey boss and akmost destroy the world", "Regular Show", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\RegularShow.jpg"),
                new Item(new List<string> {"Gangster", "Drama", "Psychological" }, "Cancer ridden chemistry teacher becomes a cook", "Breaking Bad", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\BreakingBad.jpg"),
                new Item(new List<string> {"Action", "Thriller" }, "Autistic Hacker Man tries to collapse economy", "Mr. Robot", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\MrRobot.jpg"),
                new Item(new List<string> {"Action", "Comedy" }, "Boy with funny rolex fights the same alien over and over", "Ben 10", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Ben10.jpg"),
                new Item(new List<string> {"Comedy", "Fantasy", "Food" }, "Fat Child Learns to Cook", "Chowder", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\Chowder.jpg"),
                new Item(new List<string> {"Educational", "Food", "Horror" }, "Angry brit makes chefs cry", "Kitchen Nightmares", "Series", @"C:\Users\dimit\Source\Repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\testImages\KitchenNightmares.jpg"),
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
