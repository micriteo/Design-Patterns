using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using MyWatchList.Model.Commands;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.UI.Xaml.Navigation;

namespace MyWatchList.Controllers
{
    public sealed partial class EditShow : Page
    {
        //Instance fields
        private RetrieveCategoryC _retrieveCategoryC;
        private ImageUploadC _imageUpload;
        private bool _uploaded = false;
        private List<string> _selectedCategories = new List<string>();
        private string _docRef;

        //Constructor
        public EditShow()
        {
            this.InitializeComponent();
            this._imageUpload = new ImageUploadC();
            this._retrieveCategoryC = new RetrieveCategoryC();
            SubscribeToCheckBoxEvents();
            PopulateCategoriesComboBox();
        }

        /* 
         * This is to grab the docRef from the MainPage when you press on the watchable.
         * We need the document reference (an ID) to know which watchable we want to edit.
         */
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is string docRef)
            {
                this._docRef = docRef;
            }
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
            List<string> categories = await this._retrieveCategoryC.GetCategories();

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
            if (string.IsNullOrEmpty(this._docRef))
            {
                submitStatus.Text = "ERROR The document reference is missing !";
            } 
            else
            {
                if (!string.IsNullOrEmpty(this._docRef))
                {
                    var editCommand = new EditC();
                    editCommand.DocRef = this._docRef;

                    if (!string.IsNullOrEmpty(sName.Text))
                    {
                        editCommand.Name = sName.Text;
                    }

                    if (!string.IsNullOrEmpty(sDescription.Text))
                    {
                        editCommand.Description = sDescription.Text;
                    }

                    if (this._selectedCategories.Count > 0)
                    {
                        editCommand.Categories.AddRange(_selectedCategories);
                    }

                    if (cBType.SelectedItem != null)
                    {
                        editCommand.Type = ((ComboBoxItem)cBType.SelectedItem).Content.ToString();
                    }

                    if (!string.IsNullOrEmpty(_imageUpload.filePath) && !string.IsNullOrEmpty(_imageUpload.imageName))
                    {
                        editCommand.FilePath = _imageUpload.filePath;
                        editCommand.ImageName = _imageUpload.imageName;
                    }
                    else
                    {
                        submitStatus.Text = "ERROR Image picker canceled selection ! Select the image again !";
                    }
                    

                    editCommand.execute();
                }

                submitStatus.Text = "Edits made !";
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

