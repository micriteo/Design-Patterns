using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Factory
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=DESKTOP-P1UFSEM;Database=test;Integrated Security=true;TrustServerCertificate=True";
            Show show = new Show("NameA", "DescriptionA");
            Anime anime = new Anime("AnimeN","AnimeD");
            Movie movie = new Movie("MovieN", "MovieD");
            myButton.Content = "Clicked";
            //label1.Text = show.getName();
            //label2.Text = show.getDescription();
            //label3.Text = anime.getName();
            //label4.Text = anime.getDescription();
            string retrieveDataQuery = @"
            USE test;
            SELECT * FROM [Table];";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = new SqlCommand(retrieveDataQuery, connection))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                         label5.Text = $"Name: {reader["Name"]}, Description: {reader["Description"]}";
                                        //label6.Text = movie.getDescription();
                                        // Console.WriteLine($"Name: {reader["Name"]}, Description: {reader["Description"]}");
                        }
                                }
                            }
                        }
            //label5.Text = movie.getName();
           //label6.Text = movie.getDescription();
            //Debug.WriteLine(show.getName + " " + show.getDescription);

        }
    }
}
