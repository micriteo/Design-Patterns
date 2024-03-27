using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.Commands
{
    internal class AddCategoryC : DBCommand
    {
        //Fields
        private string _name;

        //Constructor
        public AddCategoryC(string name)
        {
            setName(name);
        }

        //Setter for _categoryName
        public void setName(string name)
        {
            this._name = name;
        }

        //Execute method extended from DBCommand
        public override async void execute()
        {
            DocumentReference docRef = _db.Collection("categories").Document("Categories");
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                List<string> arrayEntries;

                // Check if the array exists already
                if (data.TryGetValue("Categories", out object actualArrayField))
                {
                    // We need to cast each element from the arrayField to string
                    arrayEntries = ((List<object>)actualArrayField).Select(x => x.ToString()).ToList();
                }
                else
                {
                    // If the array does not exist, create a new one
                    arrayEntries = new List<string>();
                }

                // Adding the string to the List<String>
                arrayEntries.Add(this._name);

                // Updating it within the document
                data["Categories"] = arrayEntries;

                // So why UpdateAsync ? We don't want to overwrite the existing data.
                await docRef.UpdateAsync(data);
            }
        }

    }
}
