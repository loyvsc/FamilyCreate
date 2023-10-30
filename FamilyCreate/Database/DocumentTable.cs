﻿using FamilyCreate.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class DocumentTable : IDBSet<Document>
    {
        private readonly MySqlConnection _connection;
        public DocumentTable()
        {
            _connection = new MySqlConnection(DatabaseContext.ConnectionString);
        }
        public const string CreateTableQuery
            = "CREATE TABLE Documents (ID INT NOT NULL AUTO_INCREMENT," +
            "TreeID int not null, Text varchar(300), FileStream varchar(1000)," +
            "adddate date,sourceid int not null, PRIMARY KEY(ID));";

        public void Add(Document item) => App.DatabaseContext?.Query("INSERT INTO " +
            $"Documents (TreeID,Text,FileStream,adddate,sourceid) VALUES " +
            $"({item.TreeID},'{item.Text}','{item.FileStream}','{item.AddDate.ToMySQLDateString()}',{item.SourceID});");

        public void Remove(Document item) => RemoveAt(item.ID);

        public void RemoveAt(int id) => App.DatabaseContext?.Query($"DELETE FROM Documents WHERE ID = {id}");

        public Document ElementAt(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM Documents WHERE ID = {id};";
                var reader = command.ExecuteReader();
                Document pers = ReadValue(reader);
                _connection.CloseAsync().Wait();
                return pers;
            }
        }

        public List<Document> Select(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                var list = new List<Document>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(ReadValue(reader));
                }
                _connection.CloseAsync().Wait();
                return list;
            }
        }

        public List<Document> ToList() => Select("SELECT * FROM Documents;");

        public void Update(Document item)
        {
            App.DatabaseContext!.Query($"UPDATE Documents SET TreeID = {item.TreeID}," +
                $"Text='{item.Text}', Filestream = '{item.FileStream}', Adddate = '{item.AddDate.ToMySQLDateString()}'," +
                $"SourceId = {item.SourceID} WHERE ID = {item.ID};");
        }

        private Document ReadValue(MySqlDataReader reader) =>
            new Document(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetInt32(5));
    }
}
