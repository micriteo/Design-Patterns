using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Interfaces
{
    public interface IWatchable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        //For the image
        public string ImageUrl { get; set; }
        void watchable(string name, string description, string imageUrl, string category);
    }
}
