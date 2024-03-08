using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    namespace Factory
    {
        internal class Converter<T> : IFirestoreConverter<T> where T : Watchable, new()
        {
            public T FromFirestore(object value)
            {
                Dictionary<string, object> values = (Dictionary<string, object>)value;
                T watchable = new T();
                watchable.watchable(values["Name"].ToString(), values["Description"].ToString(), values["ImageUrl"].ToString());
                return watchable;
            }

            public object ToFirestore(T value)
            {
                return new Dictionary<string, object>
        {
            { "Type", typeof(T).Name },
            { "Name", value.Name },
            { "Description", value.Description },
            { "ImageUrl", value.ImageUrl }
        };
            }
        }


        //Firestore can not take in the interface so we have to convert it with the dictionary
        //Working for Show only (without generics in the watchable interface)
        /*
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
        }*/
    }
}

