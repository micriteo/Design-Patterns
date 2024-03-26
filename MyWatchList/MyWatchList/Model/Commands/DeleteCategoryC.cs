using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.Commands
{
    //Change of plans. New categories collection with an updateed file of categories inside.
    internal class DeleteCategoryC : DBCommand
    {
        public string _name; //in this case the category name

        public DeleteCategoryC(string name)
        {
            setName(name);
        }

        public void setName(string name)
        {
            this._name = name;
        }

        public override async void execute()
        {
            DocumentReference docRef = _db.Collection("categories").Document("Categories");
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                List<string> arrayEntries;

                // Retrieve  current categories
                arrayEntries = ((List<object>)snapshot.GetValue<List<object>>("Categories")).Select(x => x.ToString()).ToList();

                // Add the new categories to th elist
                arrayEntries.Remove(this._name);

                // Update the category field
                data["Categories"] = arrayEntries;


                //ADD NULL CHECK
                await docRef.UpdateAsync(data);
            }

        }
    }
}
