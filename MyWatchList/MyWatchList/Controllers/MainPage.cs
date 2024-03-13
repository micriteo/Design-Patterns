using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static MyWatchList.Controllers.MainWindow;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyWatchList.Controllers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var items = new List<ActionItem>
            {
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg","AOT"),
             };

            ActionListView.ItemsSource = items;
        }

        public class ActionItem
        {
            public ImageSource ImageSource { get; set; }
            public string Title { get; set; }

            public ActionItem(string imageSource, string title)
            {
                ImageSource = new BitmapImage(new Uri(imageSource));
                Title = title;
            }
        }

        private void CreateCategory_Click(object sender, RoutedEventArgs e)
        {
            // Use the Frame to navigate to the CreateCategory page
            MainFrame.Navigate(typeof(CreateCategory));
        }

        private void AddShow_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(AddShow));
        }

        private void EditShow_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(EditShow));
        }
    }
}
