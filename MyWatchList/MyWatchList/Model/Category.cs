using MyWatchList.Model;
using MyWatchList.Model.Interfaces;
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
        private List<Iwatchable> WatchableList;
        private string Name {  get; set; }

        public Category() { }

        public bool RemoveWatchable(Iwatchable watchable)
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

        public bool Remove(Iwatchable watchable)
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
        public bool AddWatchable(Iwatchable watchable)
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
