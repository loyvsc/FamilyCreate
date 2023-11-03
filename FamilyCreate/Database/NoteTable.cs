using FamilyCreate.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class NoteTable : IDBSet<Note>
    {
        private readonly MySqlConnection _connection;
        public NoteTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }
        public string CreateTableQuery { get; set; }
            = "CREATE TABLE Notes (ID INT NOT NULL AUTO_INCREMENT," +
            "TreeID int NOT NULL, Text varchar(300), AddDate DATE, Name varchar(100) not null, PRIMARY KEY(ID));";

        public void Add(Note item) => App.DatabaseContext?.
            Query($"INSERT INTO Notes (treeid,text,adddate,Name) VALUES ({item.TreeID},'{item.Text}','{item.AddDate.ToMySQLDateString()}','{item.Name}');");

        public void Remove(Note item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Notes WHERE ID = {id}");

        public Note ElementAt(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Notes WHERE ID = {id};";
                var reader = command.ExecuteReader();
                Note pers = ReadValue(reader);
                _connection.CloseAsync().Wait();
                return pers;
            }
        }

        public List<Note> Select(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                var list = new List<Note>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                    list.Add(ReadValue(reader));
                {
                }
                _connection.CloseAsync().Wait();
                return list;
            }
        }

        public List<Note> ToList() => Select("SELECT * FROM Notes;");

        public void Update(Note item) => App.DatabaseContext!.Query($"UPDATE Notes SET TreeID = {item.TreeID}," +
            $"Text='{item.Text}',AddDate='{item.AddDate.ToMySQLDateString()}', Name = '{item.Name}' WHERE ID = {item.ID};");

        private Note ReadValue(MySqlDataReader reader) =>
            new Note(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3),reader.GetString(4));
    }
}
