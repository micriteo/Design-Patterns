using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.Commands
{
    internal class RetrieveCategoryC : DBCommand
    {
        // GetCategories method
        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();

            try
            {
                DocumentReference docRef = _db.Collection("categories").Document("Categories");
                DocumentSnapshot snapshot = docRef.GetSnapshotAsync().Result; // Synchronously wait for the result

                if (snapshot.Exists)
                {
                    Dictionary<string, object> data = snapshot.ToDictionary();
                    if (data.TryGetValue("Categories", out object existingArrayField))
                    {
                        categories = ((List<object>)existingArrayField).ConvertAll(x => x.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return categories;
        }

        // Execute method inherited from DBCommand
        public override void execute()
        {
            GetCategories();
        }
    }
}
