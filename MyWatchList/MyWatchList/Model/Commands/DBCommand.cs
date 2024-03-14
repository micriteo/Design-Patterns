using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyWatchList.Model.Commands
{
    internal abstract class DBCommand : MyWatchList.Interfaces.ICommand
    {
        protected readonly FirestoreDb _db;
        protected readonly StorageClient _storage;
        protected readonly string _bucketName;
        protected string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        public DBCommand()
        {

            //Get the soluton directory(the actual directory of the project with all the files)
            string solutionDirectory = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
            //We need the JSON file to authenticate the app with the Firestore and the bucket
            string filePath = Path.Combine(solutionDirectory, "designpatterns-98314-firebase-adminsdk-z4r47-f1741e07bf.json");
            //We need the images folder to store the images in the bucket, they are deleted once they are uploaded
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            //Bucket storage in Firebase
            this._storage = StorageClient.Create();
            this._bucketName = "designpatterns-98314.appspot.com";
            //Firestore database (Project ID, you find it in the settings of Firebase)
            this._db = FirestoreDb.Create("designpatterns-98314");
        }

        public abstract void execute(string name, string description, string imgUrl, string category);
    }
}
