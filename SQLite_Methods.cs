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
            string datasource = $"{player_count}-cards-database.db";
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
            string Createsql = "CREATE TABLE IF NOT EXISTS PlayerHandsTable (Col1 VARCHAR(50), Col2 VARCHAR(50), Col3 VARCHAR(50), Col4 VARCHAR(50), Col5 VARCHAR(50), Col6 VARCHAR(50), Col7 VARCHAR(50), Col8 INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
      

        }

        public static void InsertData(SQLiteConnection conn, Card[] card_objects_to_insert, bool winner_flag)
        { 
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            string [] card_values = new string[7];
            for (int i = 0; i < 7; i++)
                card_values[i] = card_objects_to_insert[i].ToString();
            string command_string = $"INSERT INTO PlayerHandsTable (Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8) VALUES( '{card_values[0]}', '{card_values[1]}', '{card_values[2]}', '{card_values[3]}', '{card_values[4]}', '{card_values[5]}', '{card_values[6]}', '{winner_flag}');";
            sqlite_cmd.CommandText = command_string;
            sqlite_cmd.ExecuteNonQuery();
           
        }

        public static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM PlayerHandsTable";

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

