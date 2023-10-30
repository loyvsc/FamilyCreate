using FamilyCreate.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class EventTable : IDBSet<Event>
    {
        private MySqlConnection _connection;
        public EventTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }
        public string CreateTableQuery { get; set; }
            = "CREATE TABLE Events (ID INT NOT NULL AUTO_INCREMENT," +
            "RODID int not null,placeid int not null, startdate date, enddate date,text varchar(300), PRIMARY KEY(ID));";

        public void Add(Event item) => App.DatabaseContext?.Query("INSERT INTO " +
            $"Events (rodid,placeid,startdate,enddate,text) VALUES ({item.RodID},{item.PlaceID},'{item.StartDate.ToMySQLDateString()}','{item.EndDate.ToMySQLDateString()}','{item.Text}');");

        public void Remove(Event item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Events WHERE ID = {id}");

        public Event ElementAt(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Events WHERE ID = {id};";
                var reader = command.ExecuteReader();
                Event pers = ReadValue(reader);
                _connection.CloseAsync().Wait();
                return pers;
            }
        }

        public List<Event> Select(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                List<Event> list = new List<Event>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
                _connection.CloseAsync().Wait();
                return list;
            }
        }

        public List<Event> ToList() => Select("SELECT * FROM Events;");

        public void Update(Event item) =>
            App.DatabaseContext!.Query($"UPDATE Event SET RodID = {item.RodID}, PlaceID = {item.PlaceID}," +
                $"StartDate = '{item.StartDate.ToMySQLDateString()}',EndDate ='{item.EndDate.ToMySQLDateString()}',Text='{item.Text}' WHERE ID = {item.ID};");

        private Event ReadValue(MySqlDataReader reader) =>
            new Event(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetDateTime(3),
                reader.GetDateTime(4), reader.GetString(5));
    }
}
