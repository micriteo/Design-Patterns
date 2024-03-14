using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using MyWatchList.Interfaces;

namespace MyWatchList.Model
{
    internal class Converter<T> : IFirestoreConverter<T> where T : IWatchable, new()
    {
        public T FromFirestore(object value)
        {
            Dictionary<string, object> values = (Dictionary<string, object>)value;
            T watchable = new T();
            watchable.watchable(values["Name"].ToString(), values["Description"].ToString(), values["ImageUrl"].ToString(), values["Category"].ToString());
            return watchable;
        }

        public object ToFirestore(T value)
        {
            return new Dictionary<string, object>
             {
            { "Type", typeof(T).Name }, //we need this for retrieval to know the type also which type we edit in the edit functions
            {"Category", value.Category },
            { "Name", value.Name },
            { "Description", value.Description },
            { "ImageUrl", value.ImageUrl }
             };
        }
    }
}
