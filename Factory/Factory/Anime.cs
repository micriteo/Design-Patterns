using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public class Anime:Watchable
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Anime()
        {
            this.watchable(this.Name, this.Description);
        }
        public void watchable(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            //throw new NotImplementedException();
        }
    }
}
