using ObserverDesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverDesignPatterns.Controller
{
    internal class Publisher
    {
        List<Subscriber> subscribers;

        //adds the subscriber to the subscriber list granted it is not null
        public bool Subscribe(Subscriber subscriber)
        {
            if (subscriber != null)
            {
                subscribers.Add(subscriber);
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool Unsubscribe(Subscriber subscriber)
        {
            if (subscriber != null)
            {
                subscribers.Remove(subscriber);
            } else
            {
                return false;
            }
            return true;
        }

        //Notifies all subscribers about the specific show
        public bool NotifySubscribers(Watchable watchable)
        {
            foreach(Subscriber subscriber in subscribers)
            {
                subscriber.Remove(watchable);
            }
            return true;
        }
    }
}
