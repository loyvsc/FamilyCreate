using FamilyCreate.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public static partial class Extensions
    {
        public static string ToMySQLDateString(this DateTime date) => $"{date.Year}-{date.Month}-{date.Day}";
    }

    public class PersonsTable : IDBSet<Person>
    {
        private readonly MySqlConnection _connection;
        public PersonsTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }
        public string CreateTableQuery { get; set; } = "CREATE TABLE Persons (ID int AUTO_INCREMENT," +
            "RODID int NOT NULL," +
            "Name varchar(50) not null, Surname varchar(50) not null,patronomyc varchar(50) not null," +
            "borndate datetime(6) not null," +
            "bornplaceid int," +
            "Deathdate datetime(6), deathplaceid int, ismale tinyint(1) not null, fatherid int, motherid int, " +
            "primary key (id));";

        public void Add(Person item)
        {
            string deathParams = (item.DeathDate != null && item.DeathPlaceID != -1) ? "DEATHDATE,DEATHPLACEID," : "";
            string deathValues = (item.DeathDate != null && item.DeathPlaceID != -1) ? $"'{item.DeathDate?.ToMySQLDateString()}',{item.DeathPlaceID}," : "";

            string fatmatParams = (item.FatherID != -1 && item.MotherID != -1) ? ",FATHERID,MOTHERID" : "";
            string fatmatValues = (item.FatherID != -1 && item.MotherID != -1) ? $",{item.FatherID},{item.MotherID}" : "";

            App.DatabaseContext?.Query("INSERT INTO Persons (RODID, NAME, SURNAME, PATRONOMYC," +
        $"BORNDATE,BORNPLACEID,{deathParams}ISMALE{fatmatParams}) VALUES (" +
        $"{item.RodID},'{item.Name}','{item.Surname}','{item.Patronomyc}','{item.BornDate?.ToMySQLDateString()}',{item.BornPlaceID}," +
        $"{deathValues} {item.IsMale}{fatmatValues});");
        }

        public Person ElementAt(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Persons WHERE ID = {id};";
                MySqlDataReader reader = command.ExecuteReader();
                Person pers = ReadValue(reader);
                _connection.CloseAsync().Wait();
                return pers;
            }
        }

        public void Remove(Person item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Persons WHERE ID = {id};");

        public List<Person> Select(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                List<Person> list = new List<Person>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
                _connection.CloseAsync().Wait();
                return list;
            }
        }

        public List<Person> ToList() => Select("SELECT * FROM Persons;");

        public void Update(Person item) =>
            App.DatabaseContext!.Query($"UPDATE Persons SET Name = '{item.Name}', Surname = '{item.Surname}'," +
                $"Patronomyc = '{item.Patronomyc}',borndate = '{item.BornDate}',bornplaceid={item.BornPlaceID}," +
                $"deathdate = '{item.DeathDate}',deathplaceid={item.DeathPlaceID},ismale={(item.IsMale ? 1 : 0)}," +
                $"fatherid = {item.FatherID}, motherid = {item.MotherID} WHERE ID = {item.ID};");

        private Person ReadValue(MySqlDataReader reader) =>
            new Person(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3),
                reader.GetString(4), reader.GetDateTime(5), reader.GetInt32(6), (reader.IsDBNull(7) ? null : reader.GetDateTime(7)),
                reader.IsDBNull(8) ? null : reader.GetInt32(8), reader.GetBoolean(9), (reader.IsDBNull(10) ? 0 : reader.GetInt32(10)), reader.IsDBNull(11) ? 0 : reader.GetInt32(11));
    }
}
