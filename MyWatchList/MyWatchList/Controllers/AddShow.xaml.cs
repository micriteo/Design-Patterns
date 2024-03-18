using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using MyWatchList.Model.Commands;
using Microsoft.UI.Xaml.Media.Imaging;

namespace MyWatchList.Controllers
{
    public sealed partial class AddShow : Page
    {
        private AddShowC addShow;
        private ImageUploadC imageUpload;
        private bool uploaded = false;

        public AddShow()
        {
            this.InitializeComponent();
            this.imageUpload = new ImageUploadC();
        }

        private async void submitBtn(object sender, RoutedEventArgs e)
        {
            if (!uploaded)
            {
                return;
            }

            var addShowCommand = new AddShowC(sName.Text, sDescription.Text, cBCat.SelectedItem.ToString(), imageUpload.filePath, imageUpload.imageName);
            addShowCommand.execute();
            submitStatus.Text = "Show added!";
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
