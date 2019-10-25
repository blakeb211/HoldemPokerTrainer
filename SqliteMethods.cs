using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace PokerConsoleApp
{
    class SqliteMethods
    {
        public static void InitDatabase(int playerCount)
        {
            Console.WriteLine($"{nameof(InitDatabase)} method -- Generating table names...");

            List<long> tableNums = Generate2CardUniquePrimes(Card.CardUniquePrimeDict);
            SQLiteConnection conn = CreateConnection(playerCount);
            List<long> flopPrimes = Generate3CardUniquePrimes(Card.CardUniquePrimeDict);

            using (SQLiteCommand comm = new SQLiteCommand(conn))
            {

                foreach (var num in tableNums)
                {
                    using (SQLiteTransaction tran = conn.BeginTransaction())
                    {

                        string tableStr = $"Tbl{num.ToString()}";
                        CreateTableIfNotExists(tableStr, conn);

                        comm.Transaction = tran;
                        foreach (var flopNum in flopPrimes)
                        {
                            ZeroRecord(tableStr, flopNum, comm);
                        }
                        comm.Transaction.Commit();
                    };
                }
            };

            conn.Dispose();
        }

        public static SQLiteConnection CreateConnection(int playerCount)
        {
            SQLiteConnection conn;
            // Create new database connection using number of players in the datasource name
            StringBuilder _path = new StringBuilder(Directory.GetCurrentDirectory().ToString());
            _path = _path.Append(@"\Databases");
            string datasource = $"{playerCount}-player-database.db";
            
            conn = new SQLiteConnection("Data Source=" + _path + "\\" + datasource + ";Version=3;New=True;Compress=True;journal mode=Off;Synchronous=Off");
            conn.Flags = SQLiteConnectionFlags.BindUInt32AsInt64;

            // Open the connection:
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateConnection - Exception Msg: " + ex.ToString());
            }
            return conn;
        }

        private static void CreateTableIfNotExists(string tableName, SQLiteConnection conn)
        {

            SQLiteCommand command;
            command = conn.CreateCommand();
            command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} (FlopUniquePrime INT, Wins INT, Losses INT)";
            command.ExecuteNonQuery();
        }

        private static int ZeroRecord(string tableString, long flopPrime, SQLiteCommand command)
        {
            command.CommandText = $"INSERT INTO {tableString} "
                            + "(FlopUniquePrime, Wins, Losses) "
                            + "VALUES (:FlopNum, :WinNum, :LossNum)";

            command.Parameters.Add("tableName", DbType.String).Value = tableString;
            command.Parameters.Add("FlopNum", DbType.Int64).Value = flopPrime;
            command.Parameters.Add("WinNum", DbType.Int64).Value = 0;
            command.Parameters.Add("LossNum", DbType.Int64).Value = 0;

            return command.ExecuteNonQuery();
        }

        public static int InsertResultItem(Simulation.GameRecord record, SQLiteCommand command)
        {
            string tableStr = $"Tbl{record.holeUniquePrime}";
            command.CommandText = $"SELECT * FROM {tableStr} where FlopUniquePrime = {record.flopUniquePrime}";
            SQLiteDataReader dr = command.ExecuteReader();

            if (record.winFlag == 1)
            {
                // read integer in Wins column
                dr.Read();
                long updatedWinCount = dr.GetInt64(1) + 1;

                // update integer in Wins column
                dr.Close();
                command.CommandText = $"UPDATE {tableStr} SET Wins = {updatedWinCount} WHERE FlopUniquePrime = {record.flopUniquePrime}";
            }
            else if (record.winFlag == 0)
            {
                // read integer in Losses column
                dr.Read();
                long updatedLossCount = dr.GetInt64(2) + 1;

                // update integer in Losses column
                dr.Close();
                command.CommandText = $"UPDATE {tableStr} SET Losses = {updatedLossCount} WHERE FlopUniquePrime = {record.flopUniquePrime}";
            }

            return command.ExecuteNonQuery();
        }

        public static List<long> Generate3CardUniquePrimes(Dictionary<Card, long> dict)
        {
            List<long> threeCardPrimes = new List<long>(2652);
            List<Card> deck1 = Board.BuildDeck();
            List<Card> deck2 = Board.BuildDeck();
            List<Card> deck3 = Board.BuildDeck();

            for (int i = 0; i < 50; i++)
            {
                for (int j = i + 1; j < 51; j++)
                {
                    for (int k = j + 1; k < 52; k++)
                    {
                        if (i != j && i != k && j != k)
                        {
                            ulong num = (ulong)dict[deck1[i]] * (ulong)dict[deck2[j]] * (ulong)dict[deck3[k]];
                            threeCardPrimes.Add((long)num);
                        }
                    }
                }
            }
            return threeCardPrimes;
        }

        public static List<long> Generate2CardUniquePrimes(Dictionary<Card, long> dict)
        {
            List<long> twoCardPrimes = new List<long>(2652);
            List<Card> deck1 = Board.BuildDeck();
            List<Card> deck2 = Board.BuildDeck();

            for (int i = 0; i < 51; i++)
            {
                for (int j = i + 1; j < 52; j++)
                {
                    if (i == j) break;
                    long num = (long)dict[deck1[i]] * (long)dict[deck2[j]];
                    twoCardPrimes.Add(num);
                }
            }
            return twoCardPrimes;
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

        internal static void ShowDatabaseStatistics()
        {
            throw new NotImplementedException();
        }

        public static void CreateFreshIndex(SQLiteConnection conn)
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

        public static bool IsDatabaseInitialized(int playerCount)
        {
            var conn = CreateConnection(playerCount);
            bool check1 = false;
            bool check2 = false;
            bool check3 = false;

            using (var tran = conn.BeginTransaction())
            {
                var cmd = conn.CreateCommand(); 
                cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master " +
                    "WHERE type = 'table'" +
                    "ORDER BY 1";
                object result = cmd.ExecuteScalar();
                bool check0 = (Convert.ToInt32(result) == 1326);
                
                if (check0.Equals(false))
                {
                    return false;
                }

                cmd.Dispose();
                var cmd2 = conn.CreateCommand();
                cmd2.CommandText = "SELECT COUNT(*) FROM Tbl10";
                var dr = cmd2.ExecuteReader();
                dr.Read();
                check1 = (dr.GetInt64(0) == 22100);

                cmd2.Dispose();
                var cmd3 = conn.CreateCommand();
                cmd3.CommandText = "SELECT COUNT(*) FROM Tbl55687";
                dr = cmd3.ExecuteReader();
                dr.Read();
                check2 = (dr.GetInt64(0) == 22100);

                cmd3.Dispose();
                var cmd4 = conn.CreateCommand();
                cmd4.CommandText = "SELECT * FROM Tbl55687 WHERE FlopUniquePrime == 12752323";
                dr = cmd4.ExecuteReader();
                dr.Read();
                long _wins = dr.GetInt64(1);
                long _losses = dr.GetInt64(2);
                check3 = (_wins >= 0 && _losses >= 0);

                tran.Commit();
            }
            conn.Dispose();
            return (check1 == true && check2 == true && check3 == true) ? true : false;
        }



        public static void CreateDatabaseDirectoryIfNotExists()
        {
            StringBuilder _path = new StringBuilder(Directory.GetCurrentDirectory().ToString());
            _path = _path.Append(@"\Databases");

            FileInfo[] _files = Directory.CreateDirectory(_path.ToString()).GetFiles();
            _files.OrderByDescending(_files => _files.Name);

            if (_files.Length < 1)
            {
                Console.WriteLine($"Databases will be saved to [{_path}]. Directory is currently empty.");
                return;
            }

            Console.WriteLine($"{_files.Length} files in [{_path}]");
            for (int i = 0; i < _files.Length; i++)
            {
                Console.WriteLine($"\t\t+{_files[i].Name} , {_files[i].Length / 1024 / 1024} megabytes");
            }
        }

    }
}

