using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class Movie:Watchable
    {
        private String _name;
        private String _description;
        public Movie(string name, string description)
        {
            this.watchable(name, description);
        }
        public void watchable(string name, string description)
        {
            if (name == null || description == null)
            {
                throw new ArgumentException("Can not leave name null !");
            }
            else
            {
                this._name = name;
                this._description = description;
                string connectionString = "Server=DESKTOP-P1UFSEM;Database=test;Integrated Security=true;TrustServerCertificate=True";
                string insertDataQuery = @"
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


            }
            //throw new NotImplementedException();
        }

        public String getName()
        {
            return this._name;
        }

        public String getDescription()
        {
            return this._description;
        }
    }
}
