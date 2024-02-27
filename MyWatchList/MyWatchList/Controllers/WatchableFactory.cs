using ObserverDesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverDesignPatterns.Controller
{
    internal abstract class WatchableFactory
    {
        public abstract Watchable createWatchable();
    }
}
