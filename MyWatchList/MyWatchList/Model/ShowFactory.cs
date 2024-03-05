using ObserverDesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model
{
    internal class ShowFactory : WatchableFactory
    {
        public override Watchable createWatchable()
        {
            return null;
        }
    }
}
