using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverDesignPatterns.Model
{
    internal interface Watchable
    {
        String Name { get; set; }
        String Description { get; set; }
        Byte Image {  get; set; }
    }
}
