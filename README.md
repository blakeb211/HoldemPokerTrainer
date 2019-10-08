# HoldemPokerTrainer
Train yourself for live Texas Holdem games by seeing the probability of winning change as more cards are dealt.
PROJECT STATUS:
Oct 7 2019.		Idea. Can make this faster by using Cactus Kev method of grouping certain types of hands that have same rank instead of recording
				them all in the database. This will reduce (a) number of simulated games necessary to get good statistics and thus (b) 
				total simulation time and (c) time for retrieving statistics from the sqlite database

Sept 7 2019.	Program works. Uses multi-threading and BlockingCollection to simulate games quickly and save them to a SQLite database.
				You need around 300 million games simulated (takes a few hours) to get good statistics for the post flop winning percentage.
				The SQLite queries that calculate the probabilities is slow as hell once you have 10 million or so records. I believe I could 
				fix this by using a separate Table to hold all the hands for each pair of Hole Cards and add a Table Index.

August 22 2019. Program works. It takes a few hours of simulating games to get a big enough database to see decent post-flop statistics

August 15 2019. This is my first C# program. Not currently working.


This program "simulates" thousands of Texas Holdem Hands by shuffling the deck, 
dealing the game, and saving the results to a SQLite database. 

Once the database of saved games is built up, the user can enter "Training Mode" where cards are dealt and the
probability of winning the hand is shown to them.

For example, if you have pocket Jacks and the flop is 2, 4, Queen unsuited, what are your chances
of winning a game with 4 other players? This program teaches you that.

The probabilities are calculated by running SQLite queries on the database of simulated games.


TECHNOLOGIES USED:
Visual Studio Community 2019
Github Extension
Github.com
SQLite
Test Driven Development
MSTest
ConsoleTables nuget package


