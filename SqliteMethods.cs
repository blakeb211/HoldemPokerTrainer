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
                using (var tran = conn.BeginTransaction())
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master " +
                        "WHERE type = 'table'" +
                        "ORDER BY 1";
                    object result = cmd.ExecuteScalar();
                    check0 = (Convert.ToInt32(result) == 1326);

                    cmd.Dispose();
                    tran.Commit();
                }
                conn.Dispose();
                return (check0 == true) ? true : false;
            }

            Tuple<int, int> BuildTables(int playerCount)
            {
                //each table has a different set of flops available
                SQLiteConnection conn = CreateConnection(playerCount);
                List<long> Integer2Prime = new List<long> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41,
                43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139,
                149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239 };

                long tableid = 0;
                long flopid = 0;
                int tableCount = 0;
                int rowCount = 0;

                Tuple<int, int> tbs_rows = new Tuple<int, int>(0, 0);

                for (int a = 0; a < 51; a++)
                {
                    for (int b = a + 1; b < 52; b++)
                    {
                        if (b == a) continue;

                        tableid = Integer2Prime[a] * Integer2Prime[b];
                        string tableName = $"Tbl{tableid}";
                        CreateTableIfNotExists(tableName, conn);
                        tableCount++;

                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = conn.BeginTransaction();

                            for (int c = 0; c < 50; c++)
                                for (int d = c + 1; d < 51; d++)
                                    for (int e = d + 1; e < 52; e++)
                                    {
                                        if (a != b && a != c && a != d && a != e && b != c && b != d && b != e && c != d && c != e && d != e)
                                        {
                                            // Zero flop record - flopid 0	0
                                            flopid = Integer2Prime[c] * Integer2Prime[d] * Integer2Prime[e];
                                            ZeroRecord(tableName, flopid, cmd);
                                            rowCount++;
                                        }
                                    }
                            CreateFreshIndex(tableCount, tableName, cmd);
                            cmd.Transaction.Commit();
                        }
                        // create index here
                    }
                }
                conn.Dispose();
                tbs_rows = new Tuple<int, int>(tableCount, rowCount);
                return tbs_rows;
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
                UtilityMethods.GetKeyPress();
            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Console.WriteLine($"Database for {playerCount} is being built now...");
                var countsTuple = BuildTables(playerCount);
                sw.Stop();
                Console.WriteLine($"Tables Created: {countsTuple.Item1} Rows Created: {countsTuple.Item2}");
                Console.WriteLine($"Database {playerCount} was built in {sw.ElapsedMilliseconds / 60_000} minutes.");
                UtilityMethods.GetKeyPress();
            }
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

        internal static void ShowDatabaseStatistics()
        {
            long CountTotalGamesInDatabase()
            {
                // Calc total of wins and losses columns for all tables.
                // This tells us how many games have already been simulated.
                long total = 0;
                var conn = CreateConnection(Program.PlayerCount);
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.Transaction = conn.BeginTransaction();

                // read in table names
                Console.WriteLine("Reading table names from the database...");
                cmd.CommandText = "SELECT * FROM sqlite_master " +
                            "WHERE type = 'table';";
                var drTables = cmd.ExecuteReader();
                List<string> tableNames = new List<string>(19600);
                while (drTables.Read())
                {
                    // column with index 1 holds the string of the table name
                    tableNames.Add(drTables.GetString(1));
                }
                cmd.Transaction.Commit();
                drTables.Close();

                // read each row of each table and add the win and loss
                // column to total
                Console.WriteLine("Read each row of each table and add the wins and losses to the total...");
                foreach (string tblName in tableNames)
                {
                    cmd.Transaction = conn.BeginTransaction();
                    cmd.CommandText = $"SELECT * FROM {tblName} LIMIT 19600;";
                    var drRows = cmd.ExecuteReader();
                    while (drRows.Read())
                    {
                        total += drRows.GetInt64(1);
                        total += drRows.GetInt64(2);
                    }
                    cmd.Transaction.Commit();
                    drRows.Close();
                }

                cmd.Dispose();
                conn.Close();
                return total;
            }

            string totalGamesStr = String.Format("{0:n0}", CountTotalGamesInDatabase());
            Console.WriteLine($"Total Games in {Program.PlayerCount}-player database: {totalGamesStr}");
            UtilityMethods.GetKeyPress();
        }

        internal static float CalculatePreFlopPercentage(long holeId, SQLiteCommand cmd)
        {
            /**********************************************
            * Each table corresponds to a specific set of hole
            * cards.Add up all the wins and losses of in a given
            * table and take the ratio of wins to the total games
            * that the holecards have participated in (wins + losses)
            * to get the pre-flop probability of winning.
            ***********************************************/
            Trace.WriteLine($"{nameof(CalculatePreFlopPercentage)} method running with HoleId = {holeId}");
            long _winTotal = 0;
            long _lossTotal = 0;
            // need to test this command in Sqlite DB Viewer
            cmd.CommandText = $"SELECT W, L FROM Tbl{holeId};";
            using (SQLiteDataReader dr = cmd.ExecuteReader())
            {
                Trace.Assert(dr.HasRows == true);
                while (dr.Read())
                {
                    // note the indices start at 0 because we only
                    // selected the win and loss columns
                    _winTotal += dr.GetInt64(0);
                    _lossTotal += dr.GetInt64(1);
                }
            }
            return (float)((float)_winTotal / (float)(_winTotal + _lossTotal));
        }

        internal static float CalculatePostFlopPercentage(long holeId, long flopId, SQLiteCommand cmd)
        {
            /**********************************************
            * Each table corresponds to a specific set of hole
            * cards. Each FlopId corresponds to a specific
            * set of flop cards. Take the ratio of wins
            * to the total games (wins + losses) that hole & flop combination
            * has participated in to get the post-flop
            * probability of winning.
            ***********************************************/
            long _winTotal = 0;
            long _lossTotal = 0;
            // need to test this command in Sqlite DB Viewer
            cmd.CommandText = $"SELECT W, L FROM Tbl{holeId} WHERE Flop = {flopId};";
            using (SQLiteDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    // note the indices start at 0 because we only
                    // selected the win and loss columns
                    _winTotal += dr.GetInt64(0);
                    _lossTotal += dr.GetInt64(1);
                }
            }
            return (float)((float)_winTotal / (float)(_winTotal + _lossTotal));
        }

    }
}

