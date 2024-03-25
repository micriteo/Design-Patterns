using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.Commands
{
    internal class RetrieveCategoryC : DBCommand
    {
        public async Task<List<string>> GetCategories()
        {
            List<string> categories = new List<string>();

            try
            {
                DocumentReference docRef = _db.Collection("categories").Document("Categories");
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

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
               //For testing
            }

            return categories;
        }

        public override void execute()
        {
            GetCategories();
        }
    }
}
