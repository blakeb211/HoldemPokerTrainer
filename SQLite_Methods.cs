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
            sqlite_conn = new SQLiteConnection("Data Source=" + datasource + ";Version=3;New=True;Compress=True;journal mode=Off;");
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
            string Createsql = "CREATE TABLE IF NOT EXISTS PlayerHandsTable (Hole1 INT, Hole2 INT, Flop1 INT, Flop2 INT, Flop3 INT, Winflag INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();


        }

        public static int InsertResultItem(Card hole1, Card hole2, Card flop1, Card flop2, Card flop3, int win_flag, SQLiteCommand command)
        {
            command.Parameters.AddWithValue("@hole1", "");
            command.Parameters.AddWithValue("@hole2", "");
            command.Parameters.AddWithValue("@flop1", "");
            command.Parameters.AddWithValue("@flop2", "");
            command.Parameters.AddWithValue("@flop3", "");
            command.Parameters.AddWithValue("@win_flag", "");
            command.CommandText = "INSERT INTO PlayerHandsTable "
                            + "(Hole1, Hole2, Flop1, Flop2, Flop3, Winflag) "
                            + "VALUES (@hole1, @hole2, @flop1,@flop2, @flop3, @win_flag)";
            command.Parameters["@hole1"].Value = Program.card_to_int_dict[hole1.ToString()];
            command.Parameters["@hole2"].Value = Program.card_to_int_dict[hole2.ToString()];
            command.Parameters["@flop1"].Value = Program.card_to_int_dict[flop1.ToString()];
            command.Parameters["@flop2"].Value = Program.card_to_int_dict[flop2.ToString()];
            command.Parameters["@flop3"].Value = Program.card_to_int_dict[flop3.ToString()];
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

            string create_sql = "CREATE INDEX IF NOT EXISTS hole1_idx ON PlayerHandsTable(Hole1)";
            sqlite_cmd.CommandText = create_sql;
            sqlite_cmd.ExecuteNonQuery();

        }

        internal static void Drop_Index_On_HoleCards(SQLiteConnection sqlite_conn)
        {
            SQLiteCommand sqlite_cmd;
            string dropsql = "DROP INDEX IF EXISTS hole1_idx";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = dropsql;
            sqlite_cmd.ExecuteNonQuery();

        }
    }
}

