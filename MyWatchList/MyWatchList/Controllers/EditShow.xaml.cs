using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using MyWatchList.Model.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyWatchList.Controllers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditShow : Page
    {
        //private EditC editShow;
        private ImageUploadC imageUpload;
        private bool uploaded = false;
        private string _docRef;

        public EditShow()
        {
            this.InitializeComponent();
            this.imageUpload = new ImageUploadC();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is string docRef)
            {
                _docRef = docRef; 
            }
        }

        private async void submitBtn(object sender, RoutedEventArgs e)
        {
            // Use _docRef as needed
            if (!string.IsNullOrEmpty(_docRef))
            {
                Debug.WriteLine($"Submitting with docRef: {_docRef}");

                var editCommand = new EditC();
                editCommand.DocRef = _docRef;

                if (!string.IsNullOrEmpty(sName.Text))
                    editCommand.Name = sName.Text;

                if (!string.IsNullOrEmpty(sDescription.Text))
                    editCommand.Description = sDescription.Text;

                if (cBCat.SelectedItem != null)
                    editCommand.Category = ((ComboBoxItem)cBCat.SelectedItem).Content.ToString();

                if (cBType.SelectedItem != null)
                    editCommand.Type = ((ComboBoxItem)cBType.SelectedItem).Content.ToString();

                if (!string.IsNullOrEmpty(imageUpload.filePath) && !string.IsNullOrEmpty(imageUpload.imageName))
                {
                    editCommand.FilePath = imageUpload.filePath;
                    editCommand.ImageName = imageUpload.imageName;
                }

                editCommand.execute();
            }
            else
            {
                Debug.WriteLine("No docRef received!");
            }
        }


        private async void imageBtn(object sender, RoutedEventArgs e)
        {
            if (uploaded)
            {
                uploaded = false;
            }
            this.imageUpload.setCallback(imgConverter);
            await this.imageUpload.imgUpload();
            uploaded = true;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void imgConverter()
        {
            string imageUrl = this.imageUpload.getBucketLink();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(imageUrl, UriKind.Absolute);
            CoverImage.Source = bitmapImage;
            CoverImage.UpdateLayout();
        }
    }
}
