using Factory.Factory;
using Google.Cloud.Firestore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    [FirestoreData(ConverterType = typeof(Converter<Movie>))]
    public class Movie:Watchable
    {

        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string ImageUrl { get; set; }
        //[FirestoreProperty]
        //public string Type { get; set; }

        public Movie()
        {

        }

        public Movie(string name, string description, string imageUrl)
        {
            this.watchable(this.Name, this.Description, this.ImageUrl);
        }

        public void watchable(string name, string description, string imageUrl)
        {
            this.Name = name;
            this.Description = description;
            this.ImageUrl = imageUrl;
            //this.Type = "movie";
            //throw new NotImplementedException();
        }


        //string connectionString = "Server=DESKTOP-P1UFSEM;Database=test;Integrated Security=true;TrustServerCertificate=True";
        /*string insertDataQuery = @"
         USE test;
         INSERT INTO [Table] (name, description)
         VALUES (@_name, @_description);";

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
             connection.Open();
             using (SqlCommand command = new SqlCommand(insertDataQuery, connection))
             {
                 command.Parameters.AddWithValue("@_name", this._name);
                 command.Parameters.AddWithValue("@_description", this._description);
                 command.ExecuteNonQuery();
             }
         }
        */



        //throw new NotImplementedException();

    }
}
