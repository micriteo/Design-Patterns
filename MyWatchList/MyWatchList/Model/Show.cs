using Google.Cloud.Firestore;
using MyWatchList.Interfaces;
using System.Collections.Generic;

namespace ObserverDesignPatterns.Model
{
    public class Show : IWatchable
    {
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string ImageUrl { get; set; }
        [FirestoreProperty]
        public List<string> Category { get; set; }

        public Show() { }

        public Show(string name, string description, List<string> category, string imageUrl) 
        {
            this.watchable(name, description, category, imageUrl);
        }

        public void watchable(string name, string description, List<string> category, string imageUrl) 
        {
            this.Name = name;
            this.Description = description;
            this.Category = category;
            this.ImageUrl = imageUrl;
        }
    }
}
