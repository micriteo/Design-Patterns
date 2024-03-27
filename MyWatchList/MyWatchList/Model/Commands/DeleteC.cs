using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.Commands
{
    using System.Threading.Tasks;

    internal class DeleteC : DBCommand
    {
        //Fields
        private string _docRef;

        //_docRef Setter
        public void SetDocRef(string docRef)
        {
            _docRef = docRef;
        }

        //Delete method
        public async Task Delete()
        {
            if (string.IsNullOrEmpty(_docRef))
            {
                Debug.WriteLine("Document reference is null or empty.");
                return;
            }

            Query query = _db.Collection("watchables").WhereEqualTo("Name", _docRef);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Documents.Count() > 0)
            {
                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        DocumentReference delRef = documentSnapshot.Reference;
                        await delRef.DeleteAsync();
                        Debug.WriteLine($"Document {_docRef} deleted successfully.");
                        //You found the _docRef great exit the loop
                        return;
                    }
                }
            }
        }


        //Execute method inherited from DBCommand
        public override void execute()
        {
            Task.Run(async () => await Delete()).Wait();
        }
    }

}

