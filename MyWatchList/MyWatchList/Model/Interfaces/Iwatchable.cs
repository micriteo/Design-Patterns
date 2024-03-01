using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.Interfaces
{
    internal interface Iwatchable
    {
        string Name { get; set; }
        string Description { get; set; }
        byte Image { get; set; }
    }
}
