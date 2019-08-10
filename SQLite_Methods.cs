using System;
using System.Data.SQLite;
namespace PokerConsoleApp
{
    class SQLite_Methods
    {

        public static SQLiteConnection CreateConnection(int player_count)
        {
            SQLiteConnection sqlite_conn;
            // Create new database connection using number of players in the datasource name
            string datasource = $"{player_count} cards-database.db";
            sqlite_conn = new SQLiteConnection("Data Source=" + datasource + ";Version=3;New=True;Compress=True;");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        public static void CreateTableIfNotExists(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS SampleTable (Col1 VARCHAR(20), Col2 INT)";
            string Createsql1 = "CREATE TABLE IF NOT EXISTS SampleTable1 (Col1 VARCHAR(20), Col2 INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = Createsql1;
            sqlite_cmd.ExecuteNonQuery();

        }

        public static void InsertData(SQLiteConnection conn, Card[] cards_to_insert, bool winner_flag)
        { 
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test1 Text1 ', 2); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test2 Text2 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable1 (Col1, Col2) VALUES('Test3 Text3 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();

        }

        public static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }
            conn.Close();
        }
    }
}

