using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public interface Watchable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //For the image
        public string ImageUrl { get; set; }
        void watchable(string name, string description, string imageUrl);
    }
}
