using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using System.Security.Cryptography.X509Certificates;

namespace MyWatchList.Model.Commands
{
    internal abstract class DBCommand : MyWatchList.Interfaces.ICommand
    {
        //Fields
        protected readonly FirestoreDb _db;
        protected readonly StorageClient _storage;
        protected readonly string _bucketName;
        protected string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        protected string _bucketLink;
        protected string _name;
        protected string _description;
        protected string _category;
        protected string _type;
        protected string _docRef;
        protected Action _callback;

        //Constructor
        public DBCommand()
        {

            //Get the soluton directory(the actual directory of the project with all the files)
            string solutionDirectory = Directory.GetParent(_baseDir).Parent.Parent.Parent.Parent.Parent.Parent.FullName;

            //We need the JSON file to authenticate the app with the Firestore and the bucket
            string filePath = Path.Combine(solutionDirectory, "designpatterns-98314-firebase-adminsdk-z4r47-f1741e07bf.json");

            //We need the images folder to store the images in the bucket, they are deleted once they are _uploaded
            string imagesPath = Path.Combine(solutionDirectory, "images");

            //If the images folder is created already we don't need to create it again
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }
            //The credentials are set to this for the Firestore, _filePath variable in this case is tied to our json authentication file
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);

            //Bucket storage in Firebase
            this._storage = StorageClient.Create();
            this._bucketName = "designpatterns-98314.appspot.com";

            //Firestore database (Project ID, you find it in the settings of Firebase)
            this._db = FirestoreDb.Create("designpatterns-98314");
        }

        //Execute method
        public abstract void execute();

        //Add watchable command
        public static void executeAddWatchableCommand(string name, string description, List<string> categories, string type, string filePath, string imageName)
        {
            var command = new AddWatchableC(name, description, categories, type, filePath, imageName);
            command.execute();
        }

        //Retrieve categories command
        public static async Task<List<string>> executeRetrieveCategoriesCommand()
        {
            var command = new RetrieveCategoryC();
            command.execute();
            return command.GetCategories();
        }

    }
}
