using FamilyCreate.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class RodsTable : IDBSet<Rod>
    {
        private readonly MySqlConnection _connection;
        public RodsTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }
        public string CreateTableQuery { get; set; } =
            "CREATE TABLE Rods (ID INT NOT NULL AUTO_INCREMENT, TreeID int not null," +
            "NAME VARCHAR(100) NOT NULL, PRIMARY KEY(ID));";

        public void Add(Rod item) => App.DatabaseContext?.Query($"INSERT INTO Rods (TreeID,NAME) VALUES ({item.TreeID},'{item.Name}');");

        public void Remove(Rod item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Rods WHERE ID = {id}");

        public Rod ElementAt(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Rods WHERE ID = {id};";
                var reader = command.ExecuteReader();
                Rod pers = ReadValue(reader);
                _connection.CloseAsync().Wait();
                return pers;
            }
        }

        public List<Rod> Select(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                List<Rod> list = new List<Rod>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
                _connection.CloseAsync().Wait();
                return list;
            }
        }

        public List<Rod> ToList() => Select("SELECT * FROM Rods;");

        public void Update(Rod item) =>
            App.DatabaseContext!.Query($"UPDATE Rods SET TreeID = {item.TreeID}, Name = '{item.Name}' WHERE ID = {item.ID};");

        private Rod ReadValue(MySqlDataReader reader) =>
            new Rod(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
    }
}
