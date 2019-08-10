using System;
using System.Collections.Generic;
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
        public static void Reorder_Cards_Uniquely(ref List<Card> lst_input)
        {
            int MAX_COUNT = lst_input.Count;
            // reorder list of cards by rank and suit, modifying the input list
            List<Card> lst_output = new List<Card> { };
            foreach (Card.Rank r in Enum.GetValues(typeof(Card.Rank)))
            {
                if (lst_output.Count == MAX_COUNT)
                    break;
                foreach (Card.Suit s in Enum.GetValues(typeof(Card.Suit)))
                {
                    for (int input_cards_index = 0; input_cards_index < lst_input.Count; input_cards_index++)
                    {
                        if (lst_input[input_cards_index].GetRank() == r && lst_input[input_cards_index].GetSuit() == s)
                        {
                            lst_output.Add(lst_input[input_cards_index]);
                        }
                    }
                }
            }
            lst_input = lst_output;
        }
        public static void InsertData(SQLiteConnection conn, Card[] card_objects_to_insert, bool winner_flag)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            string[] card_values = new string[7];
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

