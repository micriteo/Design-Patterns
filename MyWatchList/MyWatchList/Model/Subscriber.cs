using MyWatchList.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model
{
    internal interface Subscriber
    {
        bool Remove(Iwatchable watchable)
        {
            return false;
        }
    }
}
