using Google.Cloud.Firestore;
using Microsoft.UI.Dispatching;
using MyWatchList.Interfaces;
using ObserverDesignPatterns.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.Commands
{
    internal class RetrieveShowC : DBCommand
    {
        private Action<List<IWatchable>> _watchablesReceived;

        public RetrieveShowC(Action<List<IWatchable>> onDataReceived)
        {
            _watchablesReceived = onDataReceived;
        }

        //Retrieve Shows
        public async Task<List<IWatchable>> RetrieveWatchables()
        {
            List<IWatchable> dataList = new List<IWatchable>();
            try
            {
                CollectionReference collectionRef = _db.Collection("watchables");
                QuerySnapshot snapshot = await collectionRef.GetSnapshotAsync();
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.Exists)
                    {
                        Dictionary<string, object> data = document.ToDictionary();
                        string type = data["Type"].ToString();
                        IWatchable watchable = null;
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }
            return dataList;
        }

        public override async void execute()
        {
            var dataList = await RetrieveWatchables();
            _watchablesReceived?.Invoke(dataList);
        }
    }
}
