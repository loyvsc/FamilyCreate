using FamilyCreate.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyCreate.Database
{
    public class FilesTable : IDBSet<File>
    {
        private readonly MySqlConnection _connection;
        public FilesTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }

        public string CreateTableQuery => "CREATE TABLE Files (ID INT NOT NULL AUTO_INCREMENT, data LONGBLOB not null, Name varchar(200) not null, Extension varchar(50) not null, PRIMARY KEY(ID));";

        public void Add(File item)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"INSERT INTO Files (data,Extension,Name) VALUES (?data,?extens,?name);", _connection))
            {
                command.Parameters.Add("data", MySqlDbType.LongBlob).Value = item.FileText;
                command.Parameters.Add("extens", MySqlDbType.VarChar).Value = item.Extension;
                command.Parameters.Add("name", MySqlDbType.VarChar).Value = item.Name;

                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public File ElementAt(int id) => Select($"SELECT * FROM FILES WHERE ID = {id};")[0];

        public void Remove(File item) => RemoveAt(item.ID);

        public void RemoveAt(int id) =>
            App.DatabaseContext.Query($"DELETE FROM FILES WHERE ID ={id};");

        public List<File> Select(string query)
        {
            List<File> files = new List<File>();
            _connection.Open();
            using (MySqlCommand com = new MySqlCommand(query, _connection))
            {
                var reader = com.ExecuteReader();
                while (reader.Read())
                {
                    files.Add(ReadValue(reader));
                }
            }
            _connection.Close();
            return files;
        }

        public List<File> ToList() => Select("SELECT * FROM FILES;");

        public void Update(File item)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"UPDATE FILES SET data = ?data, Extension= ?extens, Name = ?name WHERE ID = {item.ID};", _connection))
            {
                command.Parameters.Add("data", MySqlDbType.LongBlob).Value = item.FileText;
                command.Parameters.Add("extens", MySqlDbType.VarChar).Value = item.Extension;
                command.Parameters.Add("name", MySqlDbType.VarChar).Value = item.Name;

                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        private File ReadValue(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            byte[] data = (byte[])reader["data"];
            string name = reader.GetString(2);
            string extension = reader.GetString(3);
            return new File(id, name, data, extension);
        }
    }
}
