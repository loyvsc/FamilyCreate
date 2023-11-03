using FamilyCreate.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class PlaceTable : IDBSet<Place>
    {
        private readonly MySqlConnection _connection;
        public PlaceTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }
        public string CreateTableQuery { get; set; }
            = "CREATE TABLE Places (ID INT NOT NULL AUTO_INCREMENT," +
            "Latitude varchar(50), Longitude varchar(50), NAME VARCHAR(100) NOT NULL, Description varchar(500), PRIMARY KEY(ID));";

        public void Add(Place item) => App.DatabaseContext?.Query($"INSERT INTO Places (latitude,longitude,name,description) VALUES ('{item.Latitude}','{item.Longitude}','{item.Name}','{item.Description}');");

        public void Remove(Place item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Places WHERE ID = {id}");

        public Place ElementAt(int id)
        {
            _connection.Open();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Places WHERE ID = {id};";
                var reader = command.ExecuteReader();
                reader.Read();
                Place pers = new Place();
                if (reader.HasRows)
                {
                    pers = ReadValue(reader);
                }
                _connection.Close();
                return pers;
            }
        }

        public int GetIDByItem(Place item)
        {
            _connection.OpenAsync().Wait();
            int id = -1;
            using (MySqlCommand command = new MySqlCommand($"SELECT ID FROM Places WHERE Latitude = '{item.Latitude}' and " +
                $"Longitude='{item.Longitude}' and Name = '{item.Name}' and Description = '{item.Description}';", _connection))
            {
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    id = reader.GetInt32(0);
                }
            }
            _connection.CloseAsync().Wait();
            return id;
        }

        public List<Place> Select(string query)
        {
            List<Place> list = new List<Place>();
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
            }
            _connection.Close();
            return list;
        }

        public List<Place> ToList() => Select("SELECT * FROM Places;");

        public void Update(Place item) =>
            App.DatabaseContext!.Query($"UPDATE Places SET Latitude = '{item.Latitude}', Longitude = '{item.Longitude}'," +
                $"Name = '{item.Name}',Description = '{item.Description}' WHERE ID = {item.ID};");

        private Place ReadValue(MySqlDataReader reader) =>
            new Place(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
    }
}
