using MySqlConnector;
using System.Collections.Generic;

namespace FamilyCreate.Database
{
    public class DatabaseContext : IDatabaseContext
    {
        public static string CreateConnectionString = "server=localhost;user=root;password=546909023Var;";
        public static string ConnectionString = "server=localhost;user=root;database=familytree;password=546909023Var;";

        public TreeTable TreeTable { get; private set; }
        public PersonsTable PersonsTable { get; private set; }
        public RodsTable RodsTable { get; private set; }
        public EventTable EventTable { get; private set; }
        public EventPersonTable EventActorsTable { get; private set; }
        public PlaceTable PlaceTable { get; private set; }
        public DocumentTable DocumentTable { get; private set; }
        public SourceTable SourceTable { get; private set; }
        public NoteTable NoteTable { get; private set; }

        private readonly MySqlConnection _connection;

        public DatabaseContext()
        {
            _connection = new MySqlConnection(CreateConnectionString);
            TreeTable = new TreeTable();
            PersonsTable = new PersonsTable();
            RodsTable = new RodsTable();
            EventActorsTable = new EventPersonTable();
            EventTable = new EventTable();
            PlaceTable = new PlaceTable();
            DocumentTable = new DocumentTable();
            NoteTable = new NoteTable();
            SourceTable = new SourceTable();
            InitializeDatabase();            
        }

        public void InitializeDatabase()
        {
            if (IsBDCreated)
            {
                _connection.ConnectionString = ConnectionString;
                return;
            }
            Query("CREATE DATABASE familytree;");
            _connection.ConnectionString = ConnectionString;
            CreateTables();
        }

        public bool IsBDCreated
        {
            get
            {
                using (MySqlConnection con = new MySqlConnection(CreateConnectionString))
                {
                    con.Open();
                    MySqlCommand comm = new MySqlCommand("SELECT * FROM information_schema.tables " +
                            " WHERE table_schema = 'familytree' AND TABLE_NAME = 'trees' LIMIT 1", con);
                    MySqlDataReader reader = comm.ExecuteReader();
                    bool res = reader.HasRows;
                    con.Close();
                    return res;
                }
            }
        }

        private void CreateTables()
        {
            Query(TreeTable.CreateTableQuery);
            Query(PersonsTable.CreateTableQuery);
            Query(RodsTable.CreateTableQuery);
            Query(EventActorsTable.CreateTableQuery);
            Query(PlaceTable.CreateTableQuery);
            Query(EventTable.CreateTableQuery);
            Query(DocumentTable.CreateTableQuery);
            Query(SourceTable.CreateTableQuery);
            Query(NoteTable.CreateTableQuery);
            CreateFKS();
        }

        private void CreateFKS() => Query("ALTER TABLE Rods ADD FOREIGN KEY (TreeID) REFERENCES Trees(ID);" +
            "");

        public void Query(string query)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }
    }

    public interface IDatabaseContext
    {
        public void Query(string query);
    }

    public interface IDBSet<ITable>
    {
        public void Add(ITable item);
        public void Remove(ITable item);
        public void RemoveAt(int id);
        public List<ITable> ToList();
        public List<ITable> Select(string query);
        public ITable ElementAt(int index);
        public void Update(ITable item);
    }

    public interface ITable
    {
        public int ID { get; set; }
    }
}