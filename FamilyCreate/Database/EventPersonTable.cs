using FamilyCreate.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class EventPersonTable : IDBSet<EventPerson>
    {
        private readonly MySqlConnection _connection;
        public EventPersonTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }
        public string CreateTableQuery { get; set; } =
            "CREATE TABLE EventPersons (ID INT NOT NULL AUTO_INCREMENT," +
            "eventid int NOT NULL, personid int not null, PRIMARY KEY(ID));";

        public void Add(EventPerson item)
            => App.DatabaseContext?.Query($"INSERT INTO EventActors (eventid,personid) VALUES ({item.EventID},{item.PersonID});");

        public void Remove(EventPerson item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM EventActors WHERE ID = {id}");

        public EventPerson ElementAt(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM EventActors WHERE ID = {id};";
                var reader = command.ExecuteReader();
                EventPerson pers = ReadValue(reader);
                _connection.CloseAsync().Wait();
                return pers;
            }
        }

        public List<EventPerson> Select(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                List<EventPerson> list = new List<EventPerson>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
                _connection.CloseAsync().Wait();
                return list;
            }
        }

        public List<EventPerson> ToList() => Select("SELECT * FROM EventActors;");

        public void Update(EventPerson item) =>
            App.DatabaseContext!.Query($"UPDATE EventPersons SET EventID = {item.EventID}, PersonID = {item.PersonID} WHERE ID = {item.ID};");

        private EventPerson ReadValue(MySqlDataReader reader) => new EventPerson(reader.GetInt32(0), reader.GetInt32(2), reader.GetInt32(1));
    }
}