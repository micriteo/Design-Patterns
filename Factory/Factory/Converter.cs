using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    //Firestore can not take in the interface so we have to convert it with the dictionary
    internal class Converter:IFirestoreConverter<Show>
    {
        public Show FromFirestore(object value)
        {
            Dictionary<string, object> values = (Dictionary<string, object>)value;
            Show show = new Show();
            show.watchable(values["Name"].ToString(), values["Description"].ToString());
            return show;
        }

        public object ToFirestore(Show value)
        {
            return new Dictionary<string, object>
            {
                { "Name", value.Name },
                { "Description", value.Description }
            };
        }
    }
}

