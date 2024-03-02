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
using Google.Cloud.Firestore;
using Google.Cloud.Storage;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Google.Api;
using Google.Cloud.Storage.V1;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Security.AccessControl;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Factory
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly FirestoreDb _db;
        private readonly StorageClient _storage;
        private readonly string _bucketName;
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        public MainWindow()
        {
            this.InitializeComponent();
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
            string filePath = Path.Combine(solutionDirectory, "designpatterns-98314-firebase-adminsdk-z4r47-f1741e07bf.json");
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            //Bucket storage in Firebase
            this._storage = StorageClient.Create();
            this._bucketName = "designpatterns-98314.appspot.com"; //remove gs:// prefix
            this._db = FirestoreDb.Create("designpatterns-98314");
        }



        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            //Image upload; TEST THIS TO BE MOVED TO A DIFFERENT METHOD AND CALLED WITH var imageUrl = await UploadImage("<path-to-your-image-file>");
            //Google cloud (Firebase bucket) for uploading the image to the bucket and also saving it into the file

            //Image path
            string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
            string filePath = Path.Combine(solutionDirectory, "images/car.jpg");
            var image = filePath;
            //Uploading the file to the bucket (afterwards we tie it to the object in the collection)
            using var fileStream = File.OpenRead(filePath);
            await _storage.UploadObjectAsync(this._bucketName, image, null, fileStream);
            //We need the public URL of the uploaded file to tie it to the object in the collection
            var url = $"https://storage.googleapis.com/{this._bucketName}/{image}";

            //string connectionString = "Server=DESKTOP-P1UFSEM;Database=test;Integrated Security=true;TrustServerCertificate=True";
            Show show = new Show { Name = "ShowN", Description = "ShowD", ImageUrl = url };
            Anime anime = new Anime { Name = "AnimeN", Description = "AnimeD" };
            Movie movie = new Movie { Name = "MovieN", Description = "MovieD" };
            //label1.Text = show.getName();
            //label2.Text = show.getDescription();
            //label3.Text = anime.getName();
            //label4.Text = anime.getDescription();
            /*string retrieveDataQuery = @"
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
                        }*/
            CollectionReference watchableNode = _db.Collection("watchables");
            //DocumentReference docRef = await watchableNode.AddAsync(show);
            DocumentReference docRef = watchableNode.Document(show.Name);

            await docRef.SetAsync(show);
            //await watchableNode.AddAsync(show);
            //await watchableNode.AddAsync(movie);
            //await watchableNode.AddAsync(anime);
            myButton.Content = "Sent data";
            /*var showData = new Dictionary<string, object>
{
                { "name", show.getName() }
            };
                        await watchableNode.AddAsync(showData);

                        var movieData = new Dictionary<string, object>
            {
                { "name", movie.getName() }
            };
                        await watchableNode.AddAsync(movieData);

                        var animeData = new Dictionary<string, object>
            {
                { "name", anime.getName() }
            };
                        await watchableNode.AddAsync(animeData);
            */
            //label5.Text = movie.getName();
            //label6.Text = movie.getDescription();
            //Debug.WriteLine(show.getName + " " + show.getDescription);

        }
    }
}
