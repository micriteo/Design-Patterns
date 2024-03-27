using Google.Cloud.Firestore;
using Google.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.Commands
{
    //Change of plans. New categories collection with an updateed file of categories inside.
    internal class DeleteCategoryC : DBCommand
    {
        //Fields
        private string _categoryName;

        //Setter for _categoryName
        public void setName(string name)
        {
            this._categoryName = name;
        }

        //Delete Category in Watchables table 
        public async void deleteCategoryWatchables()
        {
            Query query = _db.Collection("watchables").WhereArrayContains("Category", this._categoryName);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Documents.Count > 0)
            {
                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> data = documentSnapshot.ToDictionary();
                        List<string> arrayEntries;

                        // Retrieve  current categories
                        arrayEntries = ((List<object>)documentSnapshot.GetValue<List<object>>("Category")).Select(x => x.ToString()).ToList();

                        // Add the new categories to th elist
                        arrayEntries.Remove(this._categoryName);

                        // Update the _category field
                        data["Category"] = arrayEntries;

                        DocumentReference delRef = documentSnapshot.Reference;
                        await delRef.UpdateAsync(data);
                        break;
                    }
                }
            }
        }

        //Execite method inherited from DBCommand
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

                // Add the new categories to the list
                arrayEntries.Remove(this._categoryName);

                // Update the _category field
                data["Categories"] = arrayEntries;
                await docRef.UpdateAsync(data);
                deleteCategoryWatchables();
            }

        }
    }
}
