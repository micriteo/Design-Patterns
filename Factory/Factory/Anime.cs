using Factory.Factory;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    [FirestoreData(ConverterType = typeof(Converter<Anime>))]
    public class Anime:Watchable
    {
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string ImageUrl { get; set; }
        //[FirestoreProperty]
       // public string Type { get; set; }

        public Anime()
        {

        }

        public Anime(string name, string description, string imageUrl)
        {
            this.watchable(this.Name, this.Description, this.ImageUrl);
        }

        public void watchable(string name, string description, string imageUrl)
        {
            this.Name = name;
            this.Description = description;
            this.ImageUrl = imageUrl;
            //this.Type = "anime";
            //throw new NotImplementedException();
        }
    }
}
