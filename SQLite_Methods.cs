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
                Console.WriteLine("CreateConnection - Exception Msg: " + ex.ToString());
            }
            return sqlite_conn;
        }

        public static void CreateTableIfNotExists(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS PlayerHandsTable (Hole1 VARCHAR(50), Hole2 VARCHAR(50), Flop VARCHAR(50), Turn VARCHAR(50), River VARCHAR(50), Winflag INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();


        }

        public static int InsertResultItem(string card1, string card2, string flop, string card6, string card7, string win_flag, SQLiteCommand command)
        {
            command.Parameters["@card1"].Value = card1;
            command.Parameters["@card2"].Value = card2;
            command.Parameters["@flop"].Value = flop;
            command.Parameters["@card6"].Value = card6;
            command.Parameters["@card7"].Value = card7;
            command.Parameters["@win_flag"].Value = win_flag;
            return command.ExecuteNonQuery();
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

