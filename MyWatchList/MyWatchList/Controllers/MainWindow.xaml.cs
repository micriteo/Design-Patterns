using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyWatchList
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        string connectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dimit\source\repos\micriteo\Design-Patterns\MyWatchList\MyWatchList\Database1.mdf;Integrated Security=True";

        public MainWindow()
        {
            this.InitializeComponent();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                string query = "INSERT INTO Watchable(name, description, type) Values (@name, @description, @type)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", "aot");
                command.Parameters.AddWithValue("@description", "big man fight");
                command.Parameters.AddWithValue("@type", "anime");

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }
    }
}
