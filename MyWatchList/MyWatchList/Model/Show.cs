using Google.Cloud.Firestore;
using MyWatchList.Interfaces;
using System.Collections.Generic;

namespace ObserverDesignPatterns.Model
{
    public class Show : IWatchable
    {
        //Fields (gotta mentione FirestoreProperty to let it know what is a firestore property)
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string ImageUrl { get; set; }
        [FirestoreProperty]
        public List<string> Category { get; set; }

        //Empty constructor to let Firestore build
        public Show() { }

        //Constructor
        public Show(string name, string description, List<string> category, string imageUrl)
        {
            this.watchable(name, description, category, imageUrl);
        }

        //Watchable method inherited from IWatchable
        public void watchable(string name, string description, List<string> category, string imageUrl)
        {
            this.Name = name;
            this.Description = description;
            this.Category = category;
            this.ImageUrl = imageUrl;
        }

        //Remove the category inherited from IWatchable
        public bool removeCategory(string name)
        {
            if (name != null)
            {
                Category.Remove(name);
                return true;
            }
            return false;
        }
    }
}
