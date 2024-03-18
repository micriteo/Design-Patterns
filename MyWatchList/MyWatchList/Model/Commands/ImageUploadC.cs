using Microsoft.UI.Xaml;
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
    internal class ImageUploadC : DBCommand
    {
        public async void imgUpload()
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
                    runCallback();
                }
            }
        }

        public override async void execute()
        {
            imgUpload();
        }

        public void setCallback(Action callback)
        {
            this.callback = callback;
        }

        public void runCallback()
        {
            this.callback();
        }

        public string getBucketLink()
        {
            return this.bucketLink;
        }
    }
}
