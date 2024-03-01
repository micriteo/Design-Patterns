using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class Movie:Watchable
    {
        private String _name;
        private String _description;
        public Movie(string name, string description)
        {
            this.watchable(name, description);
        }
        public void watchable(string name, string description)
        {
            if (name == null || description == null)
            {
                throw new ArgumentException("Can not leave name null !");
            }
            else
            {
                this._name = name;
                this._description = description;
            }
            //throw new NotImplementedException();
        }

        public String getName()
        {
            return this._name;
        }

        public String getDescription()
        {
            return this._description;
        }
    }
}
