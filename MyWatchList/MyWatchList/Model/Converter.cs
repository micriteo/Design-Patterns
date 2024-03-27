using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using MyWatchList.Interfaces;

namespace MyWatchList.Model
{
    internal class Converter<T> : IFirestoreConverter<T> where T : IWatchable, new()
    {
        //Method to ge tthe watchables from Firestore
        public T FromFirestore(object value)
        {
            if (!(value is Dictionary<string, object> values))
            {
                throw new ArgumentException($"Invalid value _type for Firestore conversion: {value?.GetType()?.Name ?? "null"}");
            }

            T watchable = new T();
            watchable.watchable(
                values["Name"].ToString(),
                values["Description"].ToString(),
                ConvertCategory(values["Category"]),
                values["ImageUrl"].ToString());
            return watchable;
        }

        //Convert the category objects to string
        private List<string> ConvertCategory(object categoryObject)
        {
            if (categoryObject == null)
            {
                return new List<string>();
            }

            if (categoryObject is List<object> categoryList)
            {
                return categoryList.Select(x => x.ToString()).ToList();
            }

            throw new ArgumentException($"Invalid _category _type for conversion: {categoryObject.GetType().Name}");
        }

        //Send the watchable to the Firestore 
        public object ToFirestore(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var firestoreData = new Dictionary<string, object>
            {
                { "Type", typeof(T).Name },
                { "Name", value.Name },
                { "Description", value.Description },
                { "ImageUrl", value.ImageUrl }
            };

            if (value.Category != null && value.Category.Count > 0)
            {
                firestoreData["Category"] = value.Category.ToArray();
            }

            return firestoreData;
        }
    }
}
