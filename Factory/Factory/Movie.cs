using Google.Cloud.Firestore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public class Movie:Watchable
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Movie()
        {
            this.watchable(this.Name, this.Description);
        }
        public void watchable(string name, string description)
        {
                this.Name = name;
                this.Description = description;
               

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

        public String getName()
        {
            return this.Name;
        }

        public String getDescription()
        {
            return this.Description;
        }
    }
}
