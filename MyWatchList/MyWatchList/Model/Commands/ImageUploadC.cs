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
        //Fields
        public string _filePath;
        public string _imageName;

        //Image Upload method (FilePicker)
        public async Task<(string, string)> imgUpload()
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
                this._imageName = imgName;

                // Copy the selected file to the images folder
                string solutionFolderPath = Directory.GetParent(_baseDir).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
                string imagesFolderPath = Path.Combine(solutionFolderPath, "images");


                StorageFolder imagesFolder = await StorageFolder.GetFolderFromPathAsync(imagesFolderPath);
                await file.CopyAsync(imagesFolder, imgName, NameCollisionOption.ReplaceExisting);

                string solutionDirectory = Directory.GetParent(_baseDir).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
                string filePath = Path.Combine(solutionDirectory, "images/", imgName);
                var image = filePath;

                //Otherwise we can't delete the file at the end cause it's still in use by the thread, so it closes the FP
                using (var fileStream = File.OpenRead(filePath))
                {
                    await _storage.UploadObjectAsync(this._bucketName, image, null, fileStream);

                }
                string filePathUrl = Path.Combine(solutionDirectory, "images");
                this._filePath = filePathUrl;

                if (!string.IsNullOrEmpty(imgName))
                {
                    var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(filePathUrl)}%2F{Uri.EscapeDataString(imgName)}?alt=media";
                    this._bucketLink = url;
                    File.Delete(filePath);
                    runCallback();
                }
            }

            return (_filePath, _imageName);
        }

        //Execute method inherited from DBCommand
        public override async void execute()
        {
            (_filePath, _imageName) = await imgUpload();
        }

        //Callback setter 
        public void setCallback(Action callback)
        {
            this._callback = callback;
        }

        //Run the _callback
        public void runCallback()
        {
            this._callback();
        }

        //DBCommand _bucketLink
        public string getBucketLink()
        {
            return this._bucketLink;
        }
    }
}
