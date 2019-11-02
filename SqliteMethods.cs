using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PokerConsoleApp
{
    class SqliteMethods
    {
        internal static void InitDatabaseIfNeeded(int playerCount)
        {
            void CreateDatabaseDirectoryIfNotExists()
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
            bool IsDatabaseInitialized(int playerCount)
            {
                // this function needs improved to detect partial databases

                var conn = CreateConnection(playerCount);
                bool check0 = false;
                bool check1 = false;
                using (var tran = conn.BeginTransaction())
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master " +
                        "WHERE type = 'table'" +
                        "ORDER BY 1";
                    object result = cmd.ExecuteScalar();
                    check0 = (Convert.ToInt32(result) == 1326);

                    cmd.Dispose();
                    var cmd2 = conn.CreateCommand();
                    cmd2.CommandText = "SELECT COUNT(*) FROM Tbl10";
                    var dr = cmd2.ExecuteReader();
                    dr.Read();
                    check1 = (dr.GetInt64(0) == 19600);

                    tran.Commit();
                }
                conn.Dispose();
                return (check0 == true && check1 == true) ? true : false;
            }

            void BuildTables2(int playerCount)
            {
                //each table has a different set of flops available
                Console.WriteLine($"{nameof(BuildTables)} method -- Creating tables and zeroing the Win and Loss counts...");

                List<long> Integer2Prime = new List<long> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41,
                43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101,
                103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167,
                173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239 };

                SQLiteConnection conn = CreateConnection(playerCount);
                int _IndexNameSubscript = 0;

                long tableid = 0;
                long flopid = 0;

                for (int a = 0; a < 51; a++)
                    for (int b = a + 1; b < 52; b++)
                        using (SQLiteCommand comm = new SQLiteCommand(conn))
                        {
                            if (b == a) continue;
                            tableid = Integer2Prime[a] * Integer2Prime[b];
                            string tableStr = $"Tbl{tableid}";
                            CreateTableIfNotExists(tableStr, conn);

                            using (SQLiteTransaction tran = conn.BeginTransaction())
                            {
                                for (int c = 0; c < 50; c++)
                                    for (int d = c + 1; d < 51; d++)
                                        for (int e = d + 1; e < 52; e++)
                                        {
                                            comm.Transaction = tran;
                                            if (a != b && a != c && a != d && a != e && b != c && b != d && b != e && c != d && c != e && d != e)
                                            {
                                                // Zero flop record
                                                flopid = Integer2Prime[c] * Integer2Prime[d] * Integer2Prime[e];
                                                ZeroRecord(tableStr, flopid, comm);
                                            }
                                        }
                                CreateFreshIndex(_IndexNameSubscript, tableStr, comm);
                                _IndexNameSubscript++;
                                comm.Transaction.Commit();
                            };
                        };
                conn.Dispose();
                return;
            }


            void BuildTables(int playerCount)
            {
                Console.WriteLine($"{nameof(BuildTables)} method -- Generating table names...");

                // CAN SIMPLIFY TO BE A SINGLE LOOP -- CURRENTLY CARDS IN tableNums are 
                // duplicated in flopPrimes, resulting in table entries that will never 
                // occur during the simulation
                List<long> tableNums = Generate2CardUniquePrimes(Card.CardUniquePrimeDict);
                SQLiteConnection conn = CreateConnection(playerCount);
                List<long> flopPrimes = Generate3CardUniquePrimes(Card.CardUniquePrimeDict);

                int _IndexNameSubscript = 0;
                // zero out Wins and Losses in each row
                Console.WriteLine($"{nameof(BuildTables)} method -- Creating tables and zeroing the Win and Loss counts...");

                using (SQLiteCommand comm = new SQLiteCommand(conn))
                {
                    foreach (var num in tableNums)
                    {
                        string tableStr = $"Tbl{num.ToString()}";
                        using (SQLiteTransaction tran = conn.BeginTransaction())
                        {

                            CreateTableIfNotExists(tableStr, conn);

                            comm.Transaction = tran;
                            foreach (var flopNum in flopPrimes)
                            {
                                ZeroRecord(tableStr, flopNum, comm);
                            }
                            CreateFreshIndex(_IndexNameSubscript, tableStr, comm);
                            _IndexNameSubscript++;
                            comm.Transaction.Commit();
                        };
                    }
                };

                conn.Dispose();
            }

            void CreateTableIfNotExists(string tableName, SQLiteConnection conn)
            {
                SQLiteCommand command;
                command = conn.CreateCommand();
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} (Flop INT, W INT, L INT)";
                command.ExecuteNonQuery();
            }
            int ZeroRecord(string tableString, long flopPrime, SQLiteCommand cmd)
            {
                cmd.CommandText = $"INSERT INTO {tableString} "
                                + "(Flop, W, L) "
                                + "VALUES (:FlopNum, :WinNum, :LossNum)";

                cmd.Parameters.Add("tableName", DbType.String).Value = tableString;
                cmd.Parameters.Add("FlopNum", DbType.Int64).Value = flopPrime;
                cmd.Parameters.Add("WinNum", DbType.Int64).Value = 0;
                cmd.Parameters.Add("LossNum", DbType.Int64).Value = 0;

                return cmd.ExecuteNonQuery();
            }

            void CreateFreshIndex(int i, string tableString, SQLiteCommand cmd)
            {
                /**************************************************************
                 * This method creates an index so that queries on the database go much faster
                 * ***********************************************************/
                string create_sql = $"CREATE UNIQUE INDEX IF NOT EXISTS \"flop_idx_{i}\" ON \"{tableString}\"(\"Flop\" ASC);";
                cmd.CommandText = create_sql;
                cmd.ExecuteNonQuery();
            }

            CreateDatabaseDirectoryIfNotExists();
            if (IsDatabaseInitialized(playerCount))
            {
                Console.WriteLine($"Database for {playerCount} players is initialized and passed integrity checks.");
            }
            else
            {
                BuildTables2(playerCount);
            }
        }

        internal static void ReindexTables(int playerCount)
        {
            List<long> Integer2Prime = new List<long> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67,
                                                            71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139,
                                                            149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223,
                                                            227, 229, 233, 239 };
            SQLiteConnection conn = CreateConnection(playerCount);
            for (int a = 0; a < 51; a++)
            {
                for (int b = a + 1; b < 52; b++)
                {
                    long tableid = Integer2Prime[a] * Integer2Prime[b];
                    string cmdStr = $"REINDEX Tbl{tableid};";
                    Console.WriteLine($"Reindex Tbl{tableid}");
                    using (SQLiteCommand cmd = new SQLiteCommand(cmdStr, conn))
                    {
                        cmd.CommandText = cmdStr;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            conn.Close();
        }
        internal static int CullEmptyRowsFromDatabase(int playerCount)
        {
            int recordsRemoved = 0;
            List<long> Integer2Prime = new List<long> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67,
                                                            71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139,
                                                            149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223,
                                                            227, 229, 233, 239 };
            SQLiteConnection conn = CreateConnection(playerCount);
            for (int a = 0; a < 51; a++)
            {
                for (int b = a + 1; b < 52; b++)
                {
                    Debug.Assert(a != b);
                    long tableid = Integer2Prime[a] * Integer2Prime[b];
                    string cmdStr = $"SELECT * FROM Tbl{tableid};";
                    using (SQLiteCommand cmd = new SQLiteCommand(cmdStr, conn))
                    {
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                long _win = dr.GetInt64(1);
                                long _loss = dr.GetInt64(2);
                                long _flop = dr.GetInt64(0);

                                if (_win == 0 && _loss == 0)
                                {
                                    Console.WriteLine($"deleting from table {tableid}, row {_flop}");
                                    string delString = $"DELETE FROM Tbl{tableid} WHERE Flop = {_flop};";
                                    SQLiteCommand delCmd = new SQLiteCommand(delString, conn);
                                    delCmd.ExecuteNonQuery();
                                    recordsRemoved++;
                                    delCmd.Dispose();
                                }
                            }
                        }
                    } // end of cmd loop
                }
            }
            conn.Dispose();
            return recordsRemoved;
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
            Console.WriteLine($"Connection created to {datasource}");
            return conn;
        }
        public static int InsertResultItem(Simulation.GameRecord record, SQLiteCommand command)
        {
            string tableStr = $"Tbl{record.holeUniquePrime}";
            command.CommandText = $"SELECT * FROM {tableStr} where Flop = {record.flopUniquePrime}";
            SQLiteDataReader dr = command.ExecuteReader();

            if (record.winFlag == 1)
            {
                // read integer in Wins column
                dr.Read();
                long updatedWinCount = dr.GetInt64(1) + 1;

                // update integer in Wins column
                dr.Close();
                command.CommandText = $"UPDATE {tableStr} SET W = {updatedWinCount} WHERE Flop = {record.flopUniquePrime};";
            }
            else if (record.winFlag == 0)
            {
                // read integer in Losses column
                dr.Read();
                long updatedLossCount = dr.GetInt64(2) + 1;

                // update integer in Losses column
                dr.Close();
                command.CommandText = $"UPDATE {tableStr} SET L = {updatedLossCount} WHERE Flop = {record.flopUniquePrime};";
            }

            return command.ExecuteNonQuery();
        }

        public static List<long> Generate3CardUniquePrimes(Dictionary<Card, long> dict)
        {
            List<long> threeCardPrimes = new List<long>(2652);
            List<Card> deck1 = Board.GetFreshDeck();
            List<Card> deck2 = Board.GetFreshDeck();
            List<Card> deck3 = Board.GetFreshDeck();

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
            List<Card> deck1 = Board.GetFreshDeck();
            List<Card> deck2 = Board.GetFreshDeck();

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



    }
}

