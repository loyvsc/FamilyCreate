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
            string deathParams = (item.DeathDate != null && item.DeathPlaceID != null) ? "DEATHDATE,DEATHPLACEID," : "";
            string deathValues = (item.DeathDate != null && item.DeathPlaceID != null) ? $"'{item.DeathDate?.ToMySQLDateString()}',{item.DeathPlaceID}," : "";

            string farparams = item.FatherID != -1 ? ",FATHERID" : "";
            string motparams = item.MotherID != -1 ? ",MOTHERID" : "";

            string fatvalues = item.FatherID != -1 ? $",{item.FatherID}" : "";
            string matvalues = item.MotherID != -1 ? $",{item.MotherID}" : "";

            App.DatabaseContext?.Query("INSERT INTO Persons (RODID, NAME, SURNAME, PATRONOMYC," +
        $"BORNDATE,BORNPLACEID,{deathParams}ISMALE{farparams}{motparams}) VALUES (" +
        $"{item.RodID},'{item.Name}','{item.Surname}','{item.Patronomyc}','{item.BornDate?.ToMySQLDateString()}',{item.BornPlaceID}," +
        $"{deathValues} {item.IsMale}{fatvalues}{matvalues});");
        }

        public Person ElementAt(int id)
        {
            Person pers = new Person();
            var con = new MySqlConnection(DatabaseContext.ConnectionString);
            con.Open();
            using (MySqlCommand command = con.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Persons WHERE ID = {id};";
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows) pers = ReadValue(reader);
            }
            con.Close();
            return pers;
        }

        public void Remove(Person item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Persons WHERE ID = {id};");

        public List<Person> Select(string query)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                List<Person> list = new List<Person>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
                _connection.Close();
                return list;
            }
        }

        public List<Person> ToList() => Select("SELECT * FROM Persons;");

        public void Update(Person item)
        {
            _connection.Open();
            string deathplaceid = item.DeathPlaceID == null ? "NULL" : item.DeathPlaceID.ToString();
            string deathdatge = item.DeathDate == null ? "NULL" : $"'{item.DeathDate.Value.ToMySQLDateString()}'";

            string mother = item.MotherID == 0 ? "" : $", motherid = {item.MotherID}";
            string father = item.FatherID == 0 ? "" : $", fatherid = {item.FatherID}";

            using (MySqlCommand command = new MySqlCommand($"UPDATE Persons SET ismale=@ismale, Name = '{item.Name}', Surname = '{item.Surname}'," +
                $"Patronomyc = '{item.Patronomyc}',borndate = '{item.BornDate.Value.ToMySQLDateString()}',bornplaceid={item.BornPlaceID}," +
                $"deathdate = {deathdatge},deathplaceid={deathplaceid}{mother}{father} WHERE ID = {item.ID};", _connection))
            {
                command.Parameters.AddWithValue("@ismale", (item.IsMale ? 1 : 0));
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        private Person ReadValue(MySqlDataReader reader)
        {
            Person personReaded = new Person();
            personReaded.ID = (int) reader[0];
            personReaded.RodID = (int)reader[1];
            personReaded.Name = (string)reader[2];
            personReaded.Surname = (string)reader[3];
            personReaded.Patronomyc = (string)reader[4];
            personReaded.BornDate = reader.IsDBNull(5) ? null : (DateTime)reader[5];
            personReaded.BornPlaceID = reader.IsDBNull(6) ? null : (int)reader[6];
            personReaded.DeathDate = reader.IsDBNull(7) ? null : (DateTime)reader[7];
            personReaded.DeathPlaceID = reader.IsDBNull(8) ? null : (int)reader[8];
            personReaded.IsMale = (bool)reader[9];
            personReaded.FatherID = reader.IsDBNull(10) ? 0 : (int)reader[10];
            personReaded.MotherID = reader.IsDBNull(11) ? 0 : (int)reader[11];
            return personReaded;
        }
    }
}
