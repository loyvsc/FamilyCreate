using FamilyCreate.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class SourceTable : IDBSet<Source>
    {
        private readonly MySqlConnection _connection;
        public SourceTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }

        public string CreateTableQuery { get; set; } = "CREATE TABLE Sources" +
            "(ID INT NOT NULL AUTO_INCREMENT, TreeID INT NOT NULL, TEXT VARCHAR(300), AddDate DATE, PRIMARY KEY(ID));";

        public void Add(Source item) => App.DatabaseContext?.Query
            ($"INSERT INTO Sources (TreeID, Text, AddDate) VALUES ({item.TreeID},'{item.Text}','{item.AddDate.ToMySQLDateString()}');");

        public void Remove(Source item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Sources WHERE ID = {id}");

        public Source ElementAt(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Sources WHERE ID = {id};";
                var reader = command.ExecuteReader();
                Source pers = ReadValue(reader);
                _connection.CloseAsync().Wait();
                return pers;
            }
        }

        public List<Source> Select(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                var list = new List<Source>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
                _connection.CloseAsync().Wait();
                return list;
            }
        }

        public List<Source> ToList() => Select("SELECT * FROM Sources;");
        public void Update(Source item) =>
            App.DatabaseContext!.Query($"UPDATE Sources SET TreeID = {item.TreeID}, Text = {item.Text} WHERE ID = {item.ID};");

        private Source ReadValue(MySqlDataReader reader) =>
            new Source(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3));
    }
}
