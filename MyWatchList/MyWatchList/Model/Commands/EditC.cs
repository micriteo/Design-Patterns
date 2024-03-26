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
        public List<string> Categories { get; set; }
        public string Type { get; set; }
        public string FilePath { get; set; }
        public string ImageName {get; set; }
        public string DocRef { get; set; }

        public EditC()
        {
            this.Categories = new List<string>();
        }

        public override async void execute()
        {
            Query query = _db.Collection("watchables").WhereEqualTo("Name", this.DocRef);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    DocumentReference docRef = documentSnapshot.Reference;
                    Dictionary<string, object> data = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(this.Name))
                    {
                        data["Name"] = this.Name;
                    }

                    if (!string.IsNullOrEmpty(this.Description))
                    {
                        data["Description"] = this.Description;
                    }

                    if (!string.IsNullOrEmpty(this.FilePath) && !string.IsNullOrEmpty(this.ImageName))
                    {
                        var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(this.FilePath)}%2F{Uri.EscapeDataString(this.ImageName)}?alt=media";
                        data["ImageUrl"] = url;
                    }

                    
                    
                        // Updating it within the document
                        data["Category"] = this.Categories;
                    
                    
                    //ADD NULL CHECK
                    await docRef.UpdateAsync(data);
                }
            }
        }
    }
}
