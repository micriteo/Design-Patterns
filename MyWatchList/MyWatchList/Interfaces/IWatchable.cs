using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Interfaces
{
    //Watchable properties 
    public interface IWatchable
    {
        //Fields
        public string Name { get; set; }
        public string Description { get; set; }
        List<string> Category { get; set; }
        public string ImageUrl { get; set; }
        void watchable(string name, string description, List<string> category, string imageUrl);
        //Remove _category from the _category list
        public bool removeCategory(string name);
    }
}
