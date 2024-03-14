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
    internal class AddShow : DBCommand
    {
         public override async void execute(string name, string description, string imageUrl, string category)
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

                // Copy the selected file to the images folder
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

                if (!string.IsNullOrEmpty(imgName))
                {
                    var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(filePathUrl)}%2F{Uri.EscapeDataString(imgName)}?alt=media";

                    Show show = new Show { Name = "ShowN", Description = "ShowD", ImageUrl = url, Category = "Action" };
                    Anime anime = new Anime { Name = "AnimeN", Description = "AnimeD", ImageUrl = url, Category = "Comedy" };
                    Movie movie = new Movie { Name = "MovieN", Description = "MovieD", ImageUrl = url, Category = "Drama" };

                    CollectionReference watchableNode = _db.Collection("watchables");
                    DocumentReference showRef = watchableNode.Document(show.Name);
                    DocumentReference movieRef = watchableNode.Document(movie.Name);
                    DocumentReference animeRef = watchableNode.Document(anime.Name);

                    await showRef.SetAsync(new Converter<Show>().ToFirestore(show));
                    await movieRef.SetAsync(new Converter<Movie>().ToFirestore(movie));
                    await animeRef.SetAsync(new Converter<Anime>().ToFirestore(anime));
                    File.Delete(filePath);
                }
            }
        }

    }
}
