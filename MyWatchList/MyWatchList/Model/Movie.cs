using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWatchList.Model.Interfaces;

namespace ObserverDesignPatterns.Model
{
    internal class Movie : Iwatchable
    {
        public Movie(string name, string description, byte image) {
            this.Name = name;
            this.Description = description;
            this.Image = image;
        }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Image { get; set; }
    }
}
