using FamilyCreate.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class TreeTable : IDBSet<Tree>
    {
        private readonly MySqlConnection _connection;
        public TreeTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }
        public string CreateTableQuery { get; set; } = "CREATE TABLE Trees (ID INT NOT NULL AUTO_INCREMENT, NAME VARCHAR(100) NOT NULL, PRIMARY KEY(ID));";

        public void Add(Tree item) => App.DatabaseContext?.Query($"INSERT INTO Trees (NAME) VALUES ('{item.Name}');");

        public void Remove(Tree item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Trees WHERE ID = {id}");

        public Tree ElementAt(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Trees WHERE ID = {id};";
                var reader = command.ExecuteReader();
                Tree pers = ReadValue(reader);
                _connection.CloseAsync().Wait();
                return pers;
            }
        }

        public List<Tree> Select(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                List<Tree> list = new List<Tree>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
                _connection.CloseAsync().Wait();
                return list;
            }
        }

        public List<Tree> ToList() => Select("SELECT * FROM Trees;");

        public int GetIDByItem(Tree item)
        {
            int id = 0;
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("SELECT MAX(ID) FROM Trees;", _connection))
            {
                var reader = command.ExecuteReader();
                reader.Read();
                id = reader.GetInt32(0);
            }
            _connection.CloseAsync().Wait();
            return id;
        }

        public void Update(Tree item) => App.DatabaseContext!.Query($"UPDATE Trees SET Name = '{item.Name}' WHERE ID = {item.ID};");

        private Tree ReadValue(MySqlDataReader reader) =>
            new Tree(reader.GetInt32(0), reader.GetString(1));
    }
}
