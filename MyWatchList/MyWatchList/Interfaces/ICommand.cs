using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Interfaces
{
    internal interface ICommand
    {
        //We need the JSON file to authenticate the app with the Firestore and the bucket
       void execute();
    }
}
