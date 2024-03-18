using Google.Cloud.Firestore;
using ObserverDesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;

namespace MyWatchList.Model.Commands
{
    internal class EditC : DBCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string FilePath { get; set; }
        public string ImageName {get; set; }
        public string DocRef { get; set; }

        /*public void SetDocRef(string docRef)
        {
            _docRef = docRef;
        }*/

        public override async void execute()
        {
            Query query = _db.Collection("watchables").WhereEqualTo("Name", this.DocRef);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    // Get the document reference
                    DocumentReference docRef = documentSnapshot.Reference;
                    var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(this.FilePath)}%2F{Uri.EscapeDataString(this.ImageName)}?alt=media"; //from the abstract class
                    Dictionary<string, object> data = documentSnapshot.ToDictionary();
                    data["Name"] = this.Name; 
                    data["Description"] = this.Description; 
                    data["ImageUrl"] = url;
                    data["Category"] = this.Category;
                    await docRef.UpdateAsync(data);
                }
            }
        }
    }
}
