using MyWatchList.Interfaces;
using ObserverDesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model
{
    public class Publisher
    {
        List<IWatchable> subscribers = new List<IWatchable>();

        //adds the subscriber to the subscriber list granted it is not null
        public bool Subscribe(IWatchable subscriber)
        {
            if (subscriber != null && !subscribers.Contains(subscriber))
            {
                subscribers.Add(subscriber);
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool Unsubscribe(IWatchable subscriber)
        {
            if (subscriber != null)
            {
                subscribers.Remove(subscriber);
            }
            else
            {
                return false;
            }
            return true;
        }

        //Notifies all subscribers about the specific show
        public bool NotifySubscribers(string categoryName)
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.removeCategory(categoryName);
            }
            return true;
        }
    }
}
