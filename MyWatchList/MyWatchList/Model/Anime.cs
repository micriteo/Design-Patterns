using Google.Cloud.Firestore;
using MyWatchList.Interfaces;
using MyWatchList.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverDesignPatterns.Model
{
    public class Anime : IWatchable
    {
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string ImageUrl { get; set; }
        [FirestoreProperty]
        public List<string> Category { get; set; }

        public Anime() { }

        public Anime(string name, string description, List<string> category, string imageUrl)
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
