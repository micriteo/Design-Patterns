using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Windows;
using Microsoft.UI.Xaml.Media.Imaging;
using MyWatchList.Model.Commands;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace MyWatchList.Controllers
{
    public sealed partial class AddShow : Page
    {
        private AddShowC addShow;
        public AddShow()
        {
            this.InitializeComponent();
            this.addShow = new AddShowC();
        }

        private void submitBtn(object sender, RoutedEventArgs e)
        {
            //AddShowC addShow = new AddShowC(sName.Text, sDescription.Text, cBCat.SelectedItem.ToString());
            this.addShow.execute(sName.Text, sDescription.Text, ((ComboBoxItem)cBCat.SelectedItem).Content.ToString());
            submitStatus.Text = "Show added!";
        }

        private void imageBtn(object sender, RoutedEventArgs e)
        {
           this.addShow.imgUpload(imgConverter);
            //UpdateLayout();
            //imgConverter();
            //CoverImage.Source = new BitmapImage(new Uri(this.addShow.getBucketLink()));

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void imgConverter()
        {
            string imageUrl = this.addShow.getBucketLink();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(imageUrl, UriKind.Absolute);
            CoverImage.Source = bitmapImage;
            CoverImage.UpdateLayout();
        }

    }
}