using Google.Cloud.Firestore;
using Microsoft.UI.Xaml;
using MyWatchList.Interfaces;
using ObserverDesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

namespace MyWatchList.Model.Commands
{
    internal class AddShowC : DBCommand
    {
        private string filePath;
        private string imageName;

        public AddShowC(string name, string description, string category, string filePath, string imageName)
        {
            this.name = name;
            this.description = description;
            this.category = category;
            this.filePath = filePath;
            this.imageName = imageName;
        }

        public override async void execute()
        {
            var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(this.filePath)}%2F{Uri.EscapeDataString(this.imageName)}?alt=media"; //from the abstract class

            Show show = new Show { Name = this.name, Description = this.description, ImageUrl = url, Category = this.category };

            CollectionReference watchableNode = _db.Collection("watchables");
            DocumentReference showRef = watchableNode.Document(show.Name);
            await showRef.SetAsync(new Converter<Show>().ToFirestore(show));
        }
    }
}
