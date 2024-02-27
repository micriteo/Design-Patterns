using ObserverDesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverDesignPatterns.Controller
{
    internal interface Subscriber
    {
        bool Remove(Watchable watchable) 
        {
            return false;
        }
    }
}
