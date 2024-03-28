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
    internal class AddWatchableC : DBCommand
    {
        //Fields
        private string filePath;
        private string imageName;
        private List<string> categories;

        //Constructor
        public AddWatchableC(string name, string description, List<string> categories, string type, string filePath, string imageName)
        {
            this._name = name;
            this._description = description;
            this.categories = categories;
            this._type = type;
            this.filePath = filePath;
            this.imageName = imageName;
        }

        //Execute method inherited from DBCommand
        public override async void execute()
        {
            CollectionReference watchableNode = _db.Collection("watchables");
            /*
             * The url is made in this way to access the file once in the bucket. 
             * This is the public url.
             * ?alt=media tells it to give us the image
             */
            var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(this.filePath)}%2F{Uri.EscapeDataString(this.imageName)}?alt=media";

            if (this._type == "Show")
            {
                Show show = new Show { Name = this._name, Description = this._description, ImageUrl = url, Category = this.categories };
                DocumentReference showRef = watchableNode.Document(show.Name);
                await showRef.SetAsync(new Converter<Show>().ToFirestore(show));
            }
            else if (this._type == "Anime")
            {
                Anime anime = new Anime { Name = this._name, Description = this._description, ImageUrl = url, Category = this.categories };
                DocumentReference animeRef = watchableNode.Document(anime.Name);
                await animeRef.SetAsync(new Converter<Anime>().ToFirestore(anime));
            }
            else if (this._type == "Movie")
            {
                Movie movie = new Movie { Name = this._name, Description = this._description, ImageUrl = url, Category = this.categories };
                DocumentReference movieRef = watchableNode.Document(movie.Name);
                await movieRef.SetAsync(new Converter<Movie>().ToFirestore(movie));
            }
        }
    }
}
