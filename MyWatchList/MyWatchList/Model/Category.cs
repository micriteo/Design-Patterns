using ObserverDesignPatterns.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverDesignPatterns.Model
{
    internal class Category : Subscriber
    {
        private List<Watchable> WatchableList;
        private string Name {  get; set; }

        public Category() { }

        public bool RemoveWatchable(Watchable watchable)
        {
            if(watchable != null)
            {
                WatchableList.Remove(watchable);
            } else
            {
                return false;
            }
            return true;
        }

        public bool Remove(Watchable watchable)
        {
            if(watchable != null && WatchableList.Contains(watchable)) 
            {
                WatchableList.Remove(watchable);
            } else
            {
                return false;
            }
            return true;
        }

        //adds things like shows, animes to the category
        public bool AddWatchable(Watchable watchable)
        {
            if(watchable != null)
            {
                WatchableList.Add(watchable);
            } else
            { 
                return false; 
            }   
            return true;
        }
    }
}
