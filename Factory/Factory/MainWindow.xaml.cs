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
using Google.Api.Gax.ResourceNames;


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
            RetrieveData();
        }



        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            //Image upload; TEST THIS TO BE MOVED TO A DIFFERENT METHOD AND CALLED WITH var imageUrl = await UploadImage("<path-to-your-image-file>");
            //Google cloud (Firebase bucket) for uploading the image to the bucket and also saving it into the file

            //Image path
            string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
            //string filePath = Path.Combine(solutionDirectory, "images/car.jpg");
            string filePath = Path.Combine(solutionDirectory, "images/car.jpg");
            var image = filePath;
            //Uploading the file to the bucket (afterwards we tie it to the object in the collection)
            using var fileStream = File.OpenRead(filePath);
            await _storage.UploadObjectAsync(this._bucketName, image, null, fileStream);
            string filePathUrl = Path.Combine(solutionDirectory, "images");
            //We need the public URL of the uploaded file to tie it to the object in the collection
            //THIS LINK IS TRANSLATED FROM GS TO ACCESS IT VIA THE CLOUD ! ADD ?alt=media at the END TO CHANGE THE ENCODING TO IMAGE !!!!
            var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(filePathUrl)}%2F{Uri.EscapeDataString("car.jpg")}?alt=media";
            //https://firebasestorage.googleapis.com/v0/b/{this._bucketname}/o/{image}

            //string connectionString = "Server=DESKTOP-P1UFSEM;Database=test;Integrated Security=true;TrustServerCertificate=True";
            Show show = new Show { Name = "ShowN", Description = "ShowD", ImageUrl = url };
            Show show2 = new Show { Name = "ShowN2", Description = "ShowD2", ImageUrl = url };
            Anime anime = new Anime { Name = "AnimeN", Description = "AnimeD" };
            Movie movie = new Movie { Name = "MovieN", Description = "MovieD" };
            CollectionReference watchableNode = _db.Collection("watchables");
            DocumentReference docRef = watchableNode.Document(show.Name);
            DocumentReference docRef2 = watchableNode.Document(show2.Name);

            await docRef.SetAsync(show);
            await docRef2.SetAsync(show2);
            myButton.Content = "Sent data";

        }

        public void RetrieveData()
        {
            try
            {
                CollectionReference collectionRef = _db.Collection("watchables");
                collectionRef.Listen(snapshot =>
                {
                    List<Watchable> dataList = new List<Watchable>();
                    foreach (DocumentSnapshot document in snapshot.Documents)
                    {
                        if (document.Exists)
                        {
                            Dictionary<string, object> data = document.ToDictionary();
                            Show show = new Show()
                            {
                                Name = data["Name"].ToString(),
                                Description = data["Description"].ToString(),
                                ImageUrl = data["ImageUrl"].ToString()
                            };
                            dataList.Add(show);
                        }
                    }
                    //To do something with the UI thread (because blabla it does not want to run it on the thread)
                    DispatcherQueue.TryEnqueue(() =>
                    {
                       lV.ItemsSource = dataList;
                    });
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }
        }



    }
}
