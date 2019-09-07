using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerConsoleApp;
using System.Collections.Generic;

namespace PokerConsoleAppTests
{
    [TestClass]
    public class Find_Best_Hand_Tests
    {
        [TestMethod]
        public void Test1()
        {
            // TEST TWO HANDS
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            c1 = new Card(Card.Suit.Heart, Card.Rank.EIGHT);
            c2 = new Card(Card.Suit.Heart, Card.Rank.SEVEN);
            c3 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.FOUR);

            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            h1.Sort();
            h2.Sort();
            List<int> result = Hand.FindBestHand(new List<Hand> { h1, h2 });
            // second hand is best so result should be 2
            Assert.AreEqual(1, result[0]);
        }
        [TestMethod]
        public void Test2()
        {
            // TEST THREE HANDS

            // first hand is flush with Jack High
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // second hand is one pair with ace high
            c1 = new Card(Card.Suit.Heart, Card.Rank.EIGHT);
            c2 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            c4 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            c5 = new Card(Card.Suit.Heart, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // third hand is four of a kind, 9s with a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h3 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            h3.EvaluateHandtype();
            h1.Sort();
            h2.Sort();
            h3.Sort();
            List<int> result = Hand.FindBestHand(new List<Hand> { h1, h2, h3 });
            // third hand is best so result should be 3
            Assert.AreEqual(2, result[0]);
        }

        [TestMethod]
        public void Test3()
        {
            // TEST FOUR HANDS
            // first hand is flush with Jack High
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // second hand is one pair with ace high
            c1 = new Card(Card.Suit.Heart, Card.Rank.EIGHT);
            c2 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            c4 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            c5 = new Card(Card.Suit.Heart, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // third hand is four of a kind, 9s with a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h3 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fourth hand is straight flush low ace
            c1 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            c3 = new Card(Card.Suit.Heart, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            c5 = new Card(Card.Suit.Heart, Card.Rank.ACE);
            Hand h4 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            h3.EvaluateHandtype();
            h4.EvaluateHandtype();
            h1.Sort();
            h2.Sort();
            h3.Sort();
            h4.Sort();
            // pass hands into the method in a random order
            List<int> result = Hand.FindBestHand(new List<Hand> { h1, h4, h2, h3 });
            // Hand4 is best so result should be 2
            Assert.AreEqual(1, result[0]);
        }
        [TestMethod]
        public void Test4()
        {
            // TEST FIVE HANDS, with two hands that tie and two that are same HandType

            // first hand is flush with Jack High
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // second hand is flush with Queen High
            c1 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            c2 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            c3 = new Card(Card.Suit.Heart, Card.Rank.TEN);
            c4 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c5 = new Card(Card.Suit.Heart, Card.Rank.EIGHT);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // third hand is four of a kind, 9s with a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h3 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fourth hand is four of a kind, 9s with a four kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Hand h4 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fifth hand is a three of a kind, threes with a five and a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.THREE);
            c2 = new Card(Card.Suit.Club, Card.Rank.THREE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h5 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            h3.EvaluateHandtype();
            h4.EvaluateHandtype();
            h5.EvaluateHandtype();
            h1.Sort();
            h2.Sort();
            h3.Sort();
            h4.Sort();
            h5.Sort();
            // hand3 is passed in twice so those will tie, hand4 is best hand
            List<int> result = Hand.FindBestHand(new List<Hand> { h1, h4, h2, h3, h3, h5 });
            // Hand4 is best so result should be 2
            Assert.AreEqual(1, result[0]);
        }
        [TestMethod]
        public void Test5()
        {
            // TEST FIVE HANDS, with the winning hands being two hands that tie so have to check for returning either one as the right answer

            // first hand is flush with Jack High
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // second hand is flush with Straight Flush Queen High
            c1 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            c2 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c3 = new Card(Card.Suit.Heart, Card.Rank.TEN);
            c4 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.EIGHT);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // third hand is four of a kind, 9s with a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h3 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fourth hand is four of a kind, 9s with a four kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Hand h4 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fifth hand is a three of a kind, threes with a five and a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.THREE);
            c2 = new Card(Card.Suit.Club, Card.Rank.THREE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h5 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            h3.EvaluateHandtype();
            h4.EvaluateHandtype();
            h5.EvaluateHandtype();
            h1.Sort();
            h2.Sort();
            h3.Sort();
            h4.Sort();
            h5.Sort();
            // hand2 is straight flush with queen high so it wins
            List<int> result = Hand.FindBestHand(new List<Hand> { h2, h1, h4, h2, h3, h3, h5 });
            // Hand2 is best and it is passed in twice so result should be 1 or 4
            Assert.IsTrue(result[0] == 0 || result[0] == 3);
        }

    }
    [TestClass]
    public class EvaluateTests
    {
        [TestMethod]
        public void TestEvaluate_StraightFlush()
        {
            // TEST STRAIGHT FLUSH
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.StraightFlush);
        }
        [TestMethod]
        public void TestEvaluate_Flush()
        {
            // TEST FLUSH
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.QUEEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.Flush);
        }
        [TestMethod]
        public void TestEvaluate_StraightFlush_LowAce()
        {
            // TEST STRAIGHT FLUSH WITH A LOW ACE
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.ACE);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.StraightFlush);
        }
        [TestMethod]
        public void TestEvaluate_FourOfAKind()
        {
            // TEST FOUR OF A KIND
            Card c1 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Heart, Card.Rank.SEVEN);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.FourOfAKind);
        }
        [TestMethod]
        public void TestEvaluate_FullHouse()
        {
            // TEST FULL HOUSE
            Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c3 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.FullHouse);
        }
        [TestMethod]
        public void TestEvaluate_Straight()
        {
            // TEST STRAIGHT with a LOW ACE
            Card c1 = new Card(Card.Suit.Club, Card.Rank.ACE);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Card c3 = new Card(Card.Suit.Heart, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.Straight);
        }
        [TestMethod]
        public void TestEvaluate_ThreeOfAKind()
        {
            // TEST STRAIGHT with a LOW ACE
            Card c1 = new Card(Card.Suit.Club, Card.Rank.ACE);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Card c3 = new Card(Card.Suit.Heart, Card.Rank.ACE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.ThreeOfAKind);
        }
        [TestMethod]
        public void TestEvaluate_TwoPair()
        {
            // TEST STRAIGHT with a LOW ACE
            Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c3 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.QUEEN);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.TwoPair);
        }
        [TestMethod]
        public void TestEvaluate_OnePair()
        {
            // TEST STRAIGHT with a LOW ACE
            Card c1 = new Card(Card.Suit.Club, Card.Rank.NINE);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.SEVEN);
            Card c3 = new Card(Card.Suit.Heart, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.OnePair);
        }
        [TestMethod]
        public void TestEvaluate_HighCard()
        {
            // TEST STRAIGHT with a LOW ACE
            Card c1 = new Card(Card.Suit.Club, Card.Rank.ACE);
            Card c2 = new Card(Card.Suit.Club, Card.Rank.TWO);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.EvaluateHandtype();
            Hand.HandType test_ht = h.GetHandType();
            Assert.AreEqual(test_ht, Hand.HandType.HighCard);
        }

    }

    [TestClass]
    public class DoesThisHandBeatThatHandTests
    {
        // tests in this class are all written so that the first hand beats the second hand
        [TestMethod]
        public void DifferentHandtypes1()
        {
            // Compare Flush to Straight
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.QUEEN);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            c2 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void DifferentHandtypes2()
        {
            // Compare Straight to Two Pair
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void DifferentHandTypes3()
        {
            // Compare ThreeOfAKind to Two Pair
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            Card c5 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void StraightFlush_Test1()
        {
            // Compare Two Straight Flushes, first one is higher
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.KING);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.QUEEN);
            Card c3 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c4 = new Card(Card.Suit.Spade, Card.Rank.TEN);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.QUEEN);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.TEN);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.EIGHT);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void StraightFlush_Test2()
        {
            // Compare Two Straight Flushes, first one higher and second one is Ace Low Straight
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            Card c3 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c4 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void StraightFlush_Test3()
        {
            // Compare Two Straight Flushes, both are Ace Low Straights so they tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c3 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
        [TestMethod]
        public void StraightFlush_Test4()
        {
            // Compare Two Straight Flushes that tie but neither is Ace low straight
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.TEN);
            Card c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            Card c4 = new Card(Card.Suit.Spade, Card.Rank.EIGHT);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.SEVEN);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.TEN);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.EIGHT);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
        [TestMethod]
        public void Flush_Test2()
        {
            // Compare Two Flushes, differing at First spot
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.KING);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void Flush_Test3()
        {
            // Compare Two Flushes, differing at fourth spot
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.TEN);
            Card c3 = new Card(Card.Suit.Spade, Card.Rank.EIGHT);
            Card c4 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.TEN);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.EIGHT);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            c5 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void Flush_Test4()
        {
            // Compare Two Flushes that tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.TEN);
            Card c3 = new Card(Card.Suit.Spade, Card.Rank.EIGHT);
            Card c4 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c2 = new Card(Card.Suit.Heart, Card.Rank.TEN);
            c3 = new Card(Card.Suit.Heart, Card.Rank.EIGHT);
            c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
        [TestMethod]
        public void Straight_Test1()
        {
            // Compare Two Straights, differing in high card
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.KING);
            Card c2 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c4 = new Card(Card.Suit.Club, Card.Rank.TEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Spade, Card.Rank.QUEEN);
            c2 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.TEN);
            c4 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.EIGHT);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void Straight_Test2()
        {
            // Compare Two Straights, One with a low ace
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c4 = new Card(Card.Suit.Club, Card.Rank.THREE);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Club, Card.Rank.TWO);
            c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void Straight_Test3()
        {
            // Compare Two Straights that tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.KING);
            Card c2 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c4 = new Card(Card.Suit.Club, Card.Rank.TEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Club, Card.Rank.KING);
            c2 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            c4 = new Card(Card.Suit.Club, Card.Rank.TEN);
            c5 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
        [TestMethod]
        public void FourOfAKind_Test1()
        {

            // Compare Two Four of A Kind
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            c2 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            c4 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);

        }
        [TestMethod]
        public void FourOfAKind_Test2()
        {
            // Compare Two Four of A Kind with a different kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void FourOfAKind_Test3()
        {

            // Compare Two Four of A Kind that tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);

        }
        [TestMethod]
        public void Fullhouse_Test1()
        {
            // Compare Two Full Houses with different triples
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c3 = new Card(Card.Suit.Club, Card.Rank.FIVE);
            c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void Fullhouse_Test2()
        {
            // Compare Two Full Houses with different doubles
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Heart, Card.Rank.THREE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void Fullhouse_Test3()
        {
            // Compare two Full Houses that tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
        [TestMethod]
        public void ThreeOfAKind_Test1()
        {
            // Compare Three of a kind differing in the triple
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.EIGHT);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.EIGHT);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.EIGHT);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SEVEN);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            c4 = new Card(Card.Suit.Club, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void ThreeOfAKind_Test2()
        {
            // Compare Three of a Kind differing in the second kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            c5 = new Card(Card.Suit.Spade, Card.Rank.EIGHT);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        public void ThreeOfAKind_Test3()
        {
            // Compare Three of a Kind differing in the first kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Club, Card.Rank.THREE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void ThreeOfAKind_Test4()
        {
            // Compare Three of a kind where they tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Club, Card.Rank.JACK);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
        [TestMethod]
        public void TwoPair_Test1()
        {
            // Compare two of a kind differing in high pair
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.QUEEN);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.JACK);
            c4 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }

        [TestMethod]
        public void TwoPair_Test2()
        {
            // Compare two of a kind differing in the low pair
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.NINE);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c3 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }

        [TestMethod]
        public void TwoPair_Test3()
        {
            // Compare two of a kind differing in the kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.KING);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.KING);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.KING);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.KING);
            c5 = new Card(Card.Suit.Spade, Card.Rank.TEN);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void TwoPair_Test4()
        {
            // Compare two of a kind that tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.KING);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.KING);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.KING);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.KING);
            c5 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
        [TestMethod]
        public void OnePair_Test1()
        {
            Program.Build_Pair_Rank_Dict();
            // Compare one pair with a higher pair
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c3 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            c4 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void OnePair_Test2()
        {
            // Compare matching pairs with a different 1st kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Club, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void OnePair_Test3()
        {
            // Compare matching pairs with a different 2nd kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.TWO);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.TWO);
            c4 = new Card(Card.Suit.Club, Card.Rank.THREE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.ACE);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void OnePair_Test4()
        {
            // Compare matching pairs with a different 3rd kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.NINE);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.TEN);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.QUEEN);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Club, Card.Rank.TEN);
            c5 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }

        [TestMethod]
        public void OnePair_Test5()
        {
            // Compare matching pairs that tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.NINE);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.TEN);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.QUEEN);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Club, Card.Rank.TEN);
            c5 = new Card(Card.Suit.Spade, Card.Rank.QUEEN);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
        [TestMethod]
        public void HighCard_Test1()
        {
            // Compare with a different high card
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.TEN);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Club, Card.Rank.TWO);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void HighCard_Test2()
        {
            // Compare with a different 2nd kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Club, Card.Rank.SIX);
            c4 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void HighCard_Test3()
        {
            // Compare with a different 1st kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.EIGHT);
            c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            c4 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void HighCard_Test4()
        {
            // Compare with a different 3rd kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            c4 = new Card(Card.Suit.Club, Card.Rank.THREE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void HighCard_Test5()
        {
            // Compare with a different 4th kicker
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            c4 = new Card(Card.Suit.Club, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.TWO);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, 1);
        }
        [TestMethod]
        public void HighCard_Test6()
        {
            // Compare High Card that is a tie
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.FIVE);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            c4 = new Card(Card.Suit.Club, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.CompareTo(h1, h2);
            Assert.AreEqual(ret_val, -1);
        }
    }
}