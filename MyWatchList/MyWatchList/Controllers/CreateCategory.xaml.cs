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

namespace MyWatchList.Controllers
{
    public sealed partial class CreateCategory : Page
    {
        public CreateCategory()
        {
            this.InitializeComponent();
        }

        private async void submitBtn(object sender, RoutedEventArgs e)
        {
            var addCategoryCommand = new AddCategoryC(CategoryName.Text);
            //addCategoryCommand.Name = CategoryName.Text;
            addCategoryCommand.execute();
            Status.Text = "Category Added !";
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}