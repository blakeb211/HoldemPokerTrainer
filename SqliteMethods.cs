using System;
using System.Data.SQLite;
namespace PokerConsoleApp
{
    class SqliteMethods
    {


        public static SQLiteConnection CreateConnection(int player_count)
        {
            SQLiteConnection sqlite_conn;
            // Create new database connection using number of players in the datasource name
            string datasource = $"{player_count}-player-database.db";
            sqlite_conn = new SQLiteConnection("Data Source=" + datasource + ";Version=3;New=True;Compress=True;journal mode=Off;Synchronous=Off");
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
            string Createsql = "CREATE TABLE IF NOT EXISTS PlayerHandsTable (FlopUniquePrime LONG, Winflag INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }

        public static int InsertResultItem(Simulation.GameRecord record, SQLiteCommand command)
        {
            command.Parameters.AddWithValue("@flopCardsUniquePrime", "");
            command.Parameters.AddWithValue("@winFlag", "");
        
            command.CommandText = "INSERT INTO PlayerHandsTable "
                            + "(flopCardsUniquePrime, winFlag) "
                            + "VALUES (@flopCardsUniquePrime, @winFlag)";

            command.Parameters["@flopCardsUniquePrime"].Value = 99;
             command.Parameters["@winFlag"].Value = record.winFlag;

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

        internal static void Show_Database_Statistics()
        {
            throw new NotImplementedException();
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

        internal static void DropIndexIfExists(SQLiteConnection sqlite_conn)
        {
            SQLiteCommand sqlite_cmd;
            string dropsql = "DROP INDEX IF EXISTS hole1_idx";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = dropsql;
            sqlite_cmd.ExecuteNonQuery();

        }
    }
}

