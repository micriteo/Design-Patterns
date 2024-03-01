using MyWatchList.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model
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
            }
            else
            {
                return false;
            }
            return true;
        }

        //Notifies all subscribers about the specific show
        public bool NotifySubscribers(Iwatchable watchable)
        {
            foreach (Subscriber subscriber in subscribers)
            {
                subscriber.Remove(watchable);
            }
            return true;
        }
    }
}
