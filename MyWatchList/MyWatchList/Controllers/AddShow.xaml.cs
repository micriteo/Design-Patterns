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
        //Instance fields
        private ImageUploadC _imageUpload;
        private bool _uploaded = false;
        private List<string> _selectedCategories = new List<string>();

        //Constructor
        public AddShow()
        {
            this.InitializeComponent();
            this._imageUpload = new ImageUploadC();
            SubscribeToCheckBoxEvents();
            PopulateCategoriesComboBox();
        }

        /*
         * Method to subscribe to checkbox events (Categories combobx)
         * If the item is a combobox item subscribe it respectively to Checked and Unchecked events
         */
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

        //Populate the combobox of categories with the one from the FirestoreDB(categories table)
        private async void PopulateCategoriesComboBox()
        {
            List<string> categories = await DBCommand.executeRetrieveCategoriesCommand();

            //Clear combobox items
            cBCat.Items.Clear();

            //Add the categories with checkboxes in the combobox 
            foreach (var category in categories)
            {
                var checkBox = new CheckBox { Content = category, Tag = category };
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Unchecked;
                cBCat.Items.Add(checkBox);
            }
        }

        //Event handler if the checkbox is checked
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                this._selectedCategories.Add(checkBox.Tag.ToString());
            }
        }

        //Event handler if the checkbox is unchecked
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                this._selectedCategories.Remove(checkBox.Tag.ToString());
            }
        }

        //Submit button
        private async void submitBtn(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(sName.Text) || string.IsNullOrEmpty(sDescription.Text))
            {
                submitStatus.Text = "ERROR Make sure to complete _name and _description ! ";
            }
            else if (_selectedCategories.Count == 0 || cBType.SelectedItem == null)
            {
                submitStatus.Text = "ERROR Make sure fill check the categories and select the _type ! ";
            }
            else if (!_uploaded)
            {
                submitStatus.Text = "ERROR Upload an image of _type jpg or png !";
            }
            else if (string.IsNullOrEmpty(this._imageUpload._filePath) || string.IsNullOrEmpty(this._imageUpload._imageName))
            {
                submitStatus.Text = "ERROR Image picker canceled selection ! Select the image again !";
            }
            else
            {
                this._selectedCategories.Add("All Shows");
                DBCommand.executeAddWatchableCommand(sName.Text, sDescription.Text, _selectedCategories, ((ComboBoxItem)cBType.SelectedItem).Content.ToString(), this._imageUpload._filePath, this._imageUpload._imageName);
                submitStatus.Text = "Show added!";
            }
        }

        //Image button (aka Filepicker button)
        private async void imageBtn(object sender, RoutedEventArgs e)
        {
            if (this._uploaded)
            {
                this._uploaded = false;
            }

            this._imageUpload.setCallback(imgConverter);
            await this._imageUpload.imgUpload();
            this._uploaded = true;
        }

        //Back button
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        /*
         * Image converter to update the preview panel (cover image) on the page and converting it to a BitmapIamge.
         */
        private void imgConverter()
        {
            string imageUrl = this._imageUpload.getBucketLink();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(imageUrl, UriKind.Absolute);
            CoverImage.Source = bitmapImage;
            CoverImage.UpdateLayout();
        }
    }
}

