using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
namespace PokerConsoleApp
{
    class SqliteMethods
    {
        public static SQLiteConnection InitDatabase(int playerCount)
        {
            SQLiteConnection conn = CreateConnection(playerCount);
            Dictionary<Card, long> CardPrimeDict = Card.BuildCardToPrimeDict();

            SQLiteCommand comm = new SQLiteCommand(conn);
            SQLiteTransaction tran = conn.BeginTransaction();
            comm.Transaction = tran;

            List<long> tableNums = Generate2CardUniquePrimes(CardPrimeDict);
            List<long> flopPrimes = Generate3CardUniquePrimes(CardPrimeDict);

            foreach (var num in tableNums)
            {
                string tableStr = $"Tbl{num.ToString()}";
                CreateTableIfNotExists(tableStr, conn);

                foreach (var flopNum in flopPrimes)
                {
                    ZeroRecord(tableStr, flopNum, comm);
                }
            }

            tran.Commit();
            comm.Dispose();
            tran.Dispose();
            return conn;
        }

        public static SQLiteConnection CreateConnection(int playerCount)
        {
            SQLiteConnection conn;
            // Create new database connection using number of players in the datasource name
            string datasource = $"{playerCount}-player-database.db";
            conn = new SQLiteConnection("Data Source=" + datasource + ";Version=3;New=True;Compress=True;journal mode=Off;Synchronous=Off");
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

    }
}

