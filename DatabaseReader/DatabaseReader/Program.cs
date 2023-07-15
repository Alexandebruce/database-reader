using Npgsql;
using System;
using System.Data;
using System.Threading;

namespace DatabaseReader
{
    internal class Program
    {
        private static readonly string connectionString = "Server=postgres;Port=5432;Database=postgres;User Id=postgres;SearchPath=test_schema;Password=mysecretpassword;";

        static void Main(string[] args)
        {
            Console.WriteLine("Let's started");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM test_table;";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string name = reader.GetString("name");
                            string surname = reader.GetString("surname");
                            string patronymic = reader.GetString("patronymic");
                            string comment = reader.GetString("comment");
                            bool deleted = reader.GetBoolean("deleted");

                            Console.WriteLine($"Readed user: Surname:{surname}, Name:{name}, Patronymic:{patronymic}, Comment:{comment}, Deleted:{deleted}");
                        }
                    }
                }

                connection.Close();
            }

            Console.WriteLine("Finished reading");

            for (;;)
            {
                Console.WriteLine("It's alive");
                Thread.Sleep(10000);
            }
        }
    }
}