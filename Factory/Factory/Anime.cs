using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    [FirestoreData(ConverterType = typeof(Converter<Movie>))]
    public class Anime:Watchable
    {
        [FirestoreProperty]
        public String Name { get; set; }
        [FirestoreProperty]
        public String Description { get; set; }

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
