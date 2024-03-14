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
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using WinRT.Interop;
using Factory.Factory;


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
            //Get the soluton directory(the actual directory of the project with all the files)
            string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
            //We need the JSON file to authenticate the app with the Firestore and the bucket
            string filePath = Path.Combine(solutionDirectory, "designpatterns-98314-firebase-adminsdk-z4r47-f1741e07bf.json");
            //We need the images folder to store the images in the bucket, they are deleted once they are uploaded
            string imagesPath = Path.Combine(solutionDirectory, "images");
            //If the images folder is created already we don't need to create it again
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }
            //The credentials are set to this for the Firestore, filePath variable in this case is tied to our json authentication file
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            //Bucket storage in Firebase
            this._storage = StorageClient.Create();
            //Firebase bucket name
            /*
             * ! IMPORTANT !
             * Remove gs:// from the link (bucket name) once for the bucket name and twice for the files in the bucket.
             * Later in the code I will explain why to do so for files. On short gs:// it's the internal Firebase link and it's not accessible from the outside.
             */
            this._bucketName = "designpatterns-98314.appspot.com";
            //Firestore database (Project ID, you find it in the settings of Firebase)
            this._db = FirestoreDb.Create("designpatterns-98314");
            //Retrieve data from the database (Firestore) method (explained within the method)
            RetrieveData();
        }



        //BUTTONS UI
        //Insert data into the database (Firestore) button
        private void LoadBC(object sender, RoutedEventArgs e)
        {
            InsertFS();

        }

        //Delete data from the database (Firestore) button
        private void DeleteBC(object sender, RoutedEventArgs e)
        {
            DeleteFS();
        }

        //Edit data from the database (Firestore) button
        private void EditBC(object sender, RoutedEventArgs e)
        {
            EditFS();
        }

        //METHODS

        //INSERT into Firestore method
        private async void InsertFS()
        {
            //Image path
            string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
            //string filePath = Path.Combine(solutionDirectory, "images/car.jpg");
            string filePath = Path.Combine(solutionDirectory, "images/car.jpg");
            var image = filePath;
            //Uploading the file to the bucket afterwards we tie it to the object in the collection)
            //using var fileStream = File.OpenRead(filePath);
            using (var fileStream = File.OpenRead(filePath)) //Otherwise we can't delete the file at the end cause it's still in use by the thread, so it closes the FS
            {
                await _storage.UploadObjectAsync(this._bucketName, image, null, fileStream);

            }
            string filePathUrl = Path.Combine(solutionDirectory, "images");
            //We need the public URL of the uploaded file to tie it to the object in the collection
            //THIS LINK IS TRANSLATED FROM GS TO ACCESS IT VIA THE CLOUD ! ADD ?alt=media at the END TO CHANGE THE ENCODING TO IMAGE !!!!
            var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(filePathUrl)}%2F{Uri.EscapeDataString("car.jpg")}?alt=media";
            //https://firebasestorage.googleapis.com/v0/b/{this._bucketname}/o/{image}

            //string connectionString = "Server=DESKTOP-P1UFSEM;Database=test;Integrated Security=true;TrustServerCertificate=True";
            Show show = new Show { Name = "ShowN", Description = "ShowD", ImageUrl = url, Category = "Action" };
            Anime anime = new Anime { Name = "AnimeN", Description = "AnimeD", ImageUrl = url, Category = "Comedy" };
            Movie movie = new Movie { Name = "MovieN", Description = "MovieD", ImageUrl = url, Category = "Drama" };

            CollectionReference watchableNode = _db.Collection("watchables");
            DocumentReference showRef = watchableNode.Document(show.Name);
            DocumentReference movieRef = watchableNode.Document(movie.Name);
            DocumentReference animeRef = watchableNode.Document(anime.Name);

            await showRef.SetAsync(new Converter<Show>().ToFirestore(show));
            await movieRef.SetAsync(new Converter<Movie>().ToFirestore(movie));
            await animeRef.SetAsync(new Converter<Anime>().ToFirestore(anime));

            loadB.Content = "Sent data";
            File.Delete(filePath);
        }

        //Retrieve data from the database (Firestore) method (attached to no button, connected to the ListView)
        private void RetrieveData()
        {
            /*
                My nemesis, retrieving data into the UI (ListView in this case). Sweat and tears were shed for this method.
                The method is simple, it listens to the collection "watchables" and for each document in the collection it creates an object of the type of the document (Show, Movie, Anime).
                The object is then added to the dataList and the ListView is set to the dataList.
                The method is attached to the ListView in the XAML file (UI file).
                The method is also attached to the constructor of the MainWindow class so it's called when the window is created.
                IMPORTANT, why you need each object definded in the foreach loop ?
                ListView can't take in the interface (Watchable) so it needs the actual object (and it's proprieties) to be able to display it.
             */
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
                            string type = data["Type"].ToString();
                            Watchable watchable = null;
                            switch (type)
                            {
                                case "Show":
                                    watchable = new Converter<Show>().FromFirestore(data);
                                    break;
                                case "Movie":
                                    watchable = new Converter<Movie>().FromFirestore(data);
                                    break;
                                case "Anime":
                                    watchable = new Converter<Anime>().FromFirestore(data);
                                    break;
                            }
                            if (watchable != null)
                            {
                                dataList.Add(watchable);
                            }
                        }
                    }
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


        //Delete from Firestore document (in a given collection by name field) method
        public async void DeleteFS()
        {
            Query query = _db.Collection("watchables").WhereEqualTo("Name", tId.Text);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Documents.Count() > 0)
            {
                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        DocumentReference delRef = documentSnapshot.Reference;
                        await delRef.DeleteAsync();
                        deleteB.Content = "Deleted !";
                    }
                }
            }
            else
            {
                deleteB.Content = "ID NOT FOUND !!!";
            }
        }

        //Edit Firestore document (in a given collection by name field) method
        public async void EditFS()
        {

            /*
             * Well same as insert and only that we convert the data to object as above and that it uses FileOpenPicker (the image picker window).
             */
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Window currentWindow = Window.Current ?? new Window();
            nint windowHandle = WindowNative.GetWindowHandle(currentWindow);
            InitializeWithWindow.Initialize(picker, windowHandle);

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string imgName = Path.GetFileName(file.Path);

                // Copy the selected file to the images folder
                string imagesFolderPath = Path.Combine(Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName, "images");
                StorageFolder imagesFolder = await StorageFolder.GetFolderFromPathAsync(imagesFolderPath);
                await file.CopyAsync(imagesFolder, imgName, NameCollisionOption.ReplaceExisting);

                string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
                string filePath = Path.Combine(solutionDirectory, "images/", imgName);
                var image = filePath;

                using (var fileStream = File.OpenRead(filePath)) //Otherwise we can't delete the file at the end cause it's still in use by the thread, so it closes the FS
                {
                    await _storage.UploadObjectAsync(this._bucketName, image, null, fileStream);

                }
                string filePathUrl = Path.Combine(solutionDirectory, "images");

                Query query = _db.Collection("watchables").WhereEqualTo("Name", tId.Text);
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    if (documentSnapshot.Exists && !string.IsNullOrEmpty(imgName))
                    {
                        // Get the document reference
                        DocumentReference docRef = documentSnapshot.Reference;
                        var url = $"https://firebasestorage.googleapis.com/v0/b/{this._bucketName}/o/{Uri.EscapeDataString(filePathUrl)}%2F{Uri.EscapeDataString(imgName)}?alt=media";
                        Dictionary<string, object> data = documentSnapshot.ToDictionary();
                        data["Name"] = tEditName.Text;
                        data["Description"] = tEditDescription.Text;
                        data["ImageUrl"] = url;
                        data["Category"] = tEditCategory.Text;
                        await docRef.UpdateAsync(data);
                        File.Delete(filePath);
                    }
                }
            }

        }
    }
}
