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
        private string bucketLink;
        /*public AddShowC()
        {
            string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
            this.bucketLink = "ms-appx:///Assets/Images/aot.jpg";
        }*/

        public override async void imgUpload(Action callback)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Window currentWindow = Window.Current ?? new Window();
            nint windowHandle = WindowNative.GetWindowHandle(currentWindow);
            InitializeWithWindow.Initialize(picker, windowHandle);

            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                string imgName = Path.GetFileName(file.Path);
                this.imageName = imgName;

                // Copy the selected file to the images folder
                string solutionFolderPath = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
                string imagesFolderPath = Path.Combine(solutionFolderPath, "images");


                StorageFolder imagesFolder = await StorageFolder.GetFolderFromPathAsync(imagesFolderPath);
                await file.CopyAsync(imagesFolder, imgName, NameCollisionOption.ReplaceExisting);

                string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
                string filePath = Path.Combine(solutionDirectory, "images/", imgName);
                var image = filePath;

                using (var fileStream = File.OpenRead(filePath)) //Otherwise we can't delete the file at the end cause it's still in use by the thread, so it closes the FS
                {
                    await _storage.UploadObjectAsync(this._bucketName, image, null, fileStream);

                }
                string filePathUrl = Path.Combine(solutionDirectory, "images");
                this.filePath = filePathUrl;

                if (!string.IsNullOrEmpty(imgName))
                {
                    var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(filePathUrl)}%2F{Uri.EscapeDataString(imgName)}?alt=media";
                    this.bucketLink = url;
                    File.Delete(filePath);
                    callback();
                }
            }
        }

         public override async void execute(string name, string description, string category)
        {
            /*
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Window currentWindow = Window.Current ?? new Window();
            nint windowHandle = WindowNative.GetWindowHandle(currentWindow);
            InitializeWithWindow.Initialize(picker, windowHandle);

            var file = await picker.PickSingleFileAsync();
            */

            //if (file != null)
            //{
                //string imgName = Path.GetFileName(file.Path);

                // Copy the selected file to the images folder
                /*
                string imagesFolderPath = Path.Combine(Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName, "images");
                StorageFolder imagesFolder = await StorageFolder.GetFolderFromPathAsync(imagesFolderPath);
                await file.CopyAsync(imagesFolder, imgName, NameCollisionOption.ReplaceExisting);

                string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
                string filePath = Path.Combine(solutionDirectory, "images/", imgName);
                var image = filePath;

                using (var fileStream = File.OpenRead(filePath)) //Otherwise we can't delete the file at the end cause it's still in use by the thread, so it closes the FS
                {
                    await _storage.UploadObjectAsync(this._bucketName, image, null, fileStream);

                }
                string filePathUrl = Path.Combine(solutionDirectory, "images");
                */
                //if (!string.IsNullOrEmpty(imgName))
                //{
                    var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(this.filePath)}%2F{Uri.EscapeDataString(this.imageName)}?alt=media";
                    //this.bucketLink = url;

                    Show show = new Show { Name = name, Description = description, ImageUrl = url, Category = category };
                    //Anime anime = new Anime { Name = "AnimeN", Description = "AnimeD", ImageUrl = url, Category = "Comedy" };
                    //Movie movie = new Movie { Name = "MovieN", Description = "MovieD", ImageUrl = url, Category = "Drama" };

                    CollectionReference watchableNode = _db.Collection("watchables");
                    DocumentReference showRef = watchableNode.Document(show.Name);
                    //DocumentReference movieRef = watchableNode.Document(movie.Name);
                    //DocumentReference animeRef = watchableNode.Document(anime.Name);

                    //Find a solution so you can upload watchables of any type to Firestore
                    await showRef.SetAsync(new Converter<Show>().ToFirestore(show));
                    //await movieRef.SetAsync(new Converter<Movie>().ToFirestore(movie));
                    //await animeRef.SetAsync(new Converter<Anime>().ToFirestore(anime));
                    //File.Delete(filePath);
                //}
            //}
        }

            public string getBucketLink()
            {
                return this.bucketLink;
            }

    }
}
