using MyWatchList.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWatchList.Model.DbCommands
{
    internal class AddShow : Icommand
    {
        private string showName;
        private string showDescription;
        private string connectionString;

        public AddShow(string showName, string showDescription, string connectionString)
        {
            this.showName = showName;
            this.showDescription = showDescription;
            this.connectionString = connectionString;
        }

        public void execute()
        {
            string query = $"INSERT INTO Shows (Name, Description, Type) VALUES ('{showName}', '{showDescription}')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Added show '{showName}' with description '{showDescription}'");
                }
                else
                {
                    Console.WriteLine($"Failed to add show '{showName}'");
                }
            }
        }
    }
}
