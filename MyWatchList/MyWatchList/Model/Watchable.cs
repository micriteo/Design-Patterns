using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverDesignPatterns.Model
{
    public interface Watchable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        void watchable(string name, string description, string imageUrl, string category);
    }
}
