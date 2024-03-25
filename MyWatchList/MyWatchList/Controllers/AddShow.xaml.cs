using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using MyWatchList.Model.Commands;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using System.Linq;

namespace MyWatchList.Controllers
{
    public sealed partial class AddShow : Page
    {
        private RetrieveCategoryC retrieveCategoryC;
        private ImageUploadC imageUpload;
        private bool uploaded = false;
        private List<string> selectedCategories = new List<string>();

        public AddShow()
        {
            this.InitializeComponent();
            this.imageUpload = new ImageUploadC();
            this.retrieveCategoryC = new RetrieveCategoryC();
            SubscribeToCheckBoxEvents();
            PopulateCategoriesComboBox();
        }

        private void SubscribeToCheckBoxEvents()
        {
            foreach (var item in cBCat.Items)
            {
                if (item is ComboBoxItem comboBoxItem)
                {
                    if (comboBoxItem.Content is CheckBox checkBox)
                    {
                        checkBox.Checked += CheckBox_Checked;
                        checkBox.Unchecked += CheckBox_Unchecked;
                    }
                }
            }
        }

        private async void PopulateCategoriesComboBox()
        {
            List<string> categories = await retrieveCategoryC.GetCategories();

            // Clear existing items in ComboBox
            cBCat.Items.Clear();

            // Add categories to the ComboBox with checkboxes
            foreach (var category in categories)
            {
                var checkBox = new CheckBox { Content = category, Tag = category };
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Unchecked;
                cBCat.Items.Add(checkBox);
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                selectedCategories.Add(checkBox.Tag.ToString());
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                selectedCategories.Remove(checkBox.Tag.ToString());
            }
        }

        private async void submitBtn(object sender, RoutedEventArgs e)
        {
            if (!uploaded)
            {
                return;
            }

            var addShowCommand = new AddShowC(sName.Text, sDescription.Text, selectedCategories, ((ComboBoxItem)cBType.SelectedItem).Content.ToString(), imageUpload.filePath, imageUpload.imageName);
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

