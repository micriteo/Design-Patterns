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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyWatchList
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var items = new List<ActionItem>
            {
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),
                new ActionItem("ms-appx:///Assets/Images/aot.jpg"),


            };

            ActionListView.ItemsSource = items;
        }

        public class ActionItem
        {
            public ImageSource ImageSource { get; set; }
            //public string Title { get; set; }

            public ActionItem(string imageSource)
            {
                ImageSource = new BitmapImage(new Uri(imageSource));
                //Title = title;
            }
        }

    }
}
