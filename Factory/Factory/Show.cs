using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core.AnimationMetrics;

namespace Factory
{
    [FirestoreData(ConverterType = typeof(Converter<Show>))]
    public class Show : Watchable
    {
        //public String _name;
        //public String _description;

        [FirestoreProperty]
        public String Name { get; set; }
        [FirestoreProperty]
        public String Description { get; set; }

        public Show() { }

        public Show(String name, String description) { 
             this.watchable(this.Name, this.Description);
        }
        public void watchable(String name, String description)
        {
                this.Name = name;
                this.Description = description;
            //throw new NotImplementedException();
        }


    }
}
