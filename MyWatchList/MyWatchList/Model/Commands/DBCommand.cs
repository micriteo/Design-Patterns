using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using System.Security.Cryptography.X509Certificates;

namespace MyWatchList.Model.Commands
{
    internal abstract class DBCommand : MyWatchList.Interfaces.ICommand
    {
        protected readonly FirestoreDb _db;
        protected readonly StorageClient _storage;
        protected readonly string _bucketName;
        protected string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        protected string bucketLink;
        protected string filePath;
        protected string imageName;

        public DBCommand()
        {

            //Get the soluton directory(the actual directory of the project with all the files)
            string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
            //We need the JSON file to authenticate the app with the Firestore and the bucket
            string filePath = Path.Combine(solutionDirectory, "designpatterns-98314-firebase-adminsdk-z4r47-f1741e07bf.json");
            //We need the images folder to store the images in the bucket, they are deleted once they are uploaded
            string imagesPath = Path.Combine(solutionDirectory, "images");
            //If the images folder is created already we don't need to create it again
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }
            //The credentials are set to this for the Firestore, filePath variable in this case is tied to our json authentication file
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            //Bucket storage in Firebase
            this._storage = StorageClient.Create();
            //Firebase bucket name
            /*
             * ! IMPORTANT !
             * Remove gs:// from the link (bucket name) once for the bucket name and twice for the files in the bucket.
             * Later in the code I will explain why to do so for files. On short gs:// it's the internal Firebase link and it's not accessible from the outside.
             */
            this._bucketName = "designpatterns-98314.appspot.com";
            //Firestore database (Project ID, you find it in the settings of Firebase)
            this._db = FirestoreDb.Create("designpatterns-98314");
        }

        public abstract void execute(string name, string description, string category);
        public async void imgUpload(Action callback)
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
        public string getBucketLink()
        {
            return this.bucketLink;
        }
    }
}
