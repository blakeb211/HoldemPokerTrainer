# HoldemPokerTrainer
Train yourself for live Texas Holdem games by seeing the probability of winning change as more cards are dealt.

![what poker training mode looks like](screenshots/screenshot_poker%20training%20mode_3.JPG)

# How it works

This program simulates thousands of Texas Holdem Hands by shuffling the deck, 
dealing the game, and saving the results to a SQLite database. 

Once the database of saved games is built up, the user can enter "Training Mode" where cards are dealt and the
probability of winning the hand is shown to them.

For example, if you have pocket jacks and the flop is 2, 4, queen unsuited, what are your chances
of winning a game with 4 other players? This program teaches you that.

The probabilities are calculated by running *SQL queries* on the database of simulated games.

This approach is called brute force monte carlo.


