# HoldemPokerTrainer
Train yourself for live Texas Holdem games by seeing the probability of winning change as more cards are dealt.

This program "simulates" thousands of Texas Holdem Hands by shuffling the deck, 
dealing the game, and saving the results to a SQLite database. 

Once the database of saved games is built up, the user can enter "Training Mode" where cards are dealt and the
probability of winning the hand is shown to them.

The probabilities are calculated by running SQLite queries on the database of simulated games.

For example, if you have pocket Jacks and the flop is 2, 4, Queen unsuited, what are your chances
of winning a game with 4 other players? This program teaches you that.


PROJECT STATUS:
August 15 2019. This program is still in development and only parts of it are working.
