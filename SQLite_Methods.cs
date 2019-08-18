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
            string datasource = $"{player_count}-player-database.db";
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
            string Createsql = "CREATE TABLE IF NOT EXISTS PlayerHandsTable (HoleCards VARCHAR(50), Flop VARCHAR(50), Turn VARCHAR(50), River VARCHAR(50), Winflag INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();


        }

        public static int InsertResultItem(string holecards, string flop, string turn, string river, string win_flag, SQLiteCommand command)
        {
            command.Parameters["@holecards"].Value = holecards;
            command.Parameters["@flop"].Value = flop;
            command.Parameters["@turn"].Value = turn;
            command.Parameters["@river"].Value = river;
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

        public static void Create_Fresh_Index_On_HoleCards(SQLiteConnection conn)
        {
            /**************************************************************
             * This method creates an index so that queries on the database go much faster
             * ***********************************************************/
            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();

            string create_sql = "CREATE INDEX IF NOT EXISTS holecards_idx ON PlayerHandsTable(HoleCards)";
            sqlite_cmd.CommandText = create_sql;
            sqlite_cmd.ExecuteNonQuery();

        }
    }
}

