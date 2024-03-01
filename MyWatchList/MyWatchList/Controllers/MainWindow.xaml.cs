using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyWatchList.Model.DbCommands;
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
        string DBFILENAME = "Database1.mdf";


        public MainWindow()
        {
            this.InitializeComponent();

            string workingDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\..\\..\\"));
            string filePath = workingDirectory + DBFILENAME;

            string connectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + filePath + ";Integrated Security=True";
            Console.WriteLine(connectionstring);

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                string query = "INSERT INTO Watchable(name, description, type) Values (@name, @description, @type)";

               AddShow addshow = new AddShow ("Attack on Titan", "A show about titans", connectionstring);

                addshow.execute();

                connection.Close();
            }
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }
    }
}
