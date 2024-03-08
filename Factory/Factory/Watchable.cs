using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public interface Watchable
    {
        //Put here the FIRESTORE PROPERTY
        public string Name { get; set; }
        public string Description { get; set; }
        //public string Type { get; set; }
        //For the image
        public string ImageUrl { get; set; }
        void watchable(string name, string description, string imageUrl);
    }
}
