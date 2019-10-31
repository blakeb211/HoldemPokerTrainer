using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerConsoleApp;

namespace PokerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PlayerCountAndDeckTests()
        {
            Board b = new Board(4);
            Assert.IsTrue(b.Players.Count == 4);
            b = new Board(2);
            Assert.IsTrue(b.Players.Count == 2);
            b.Deck = Board.GetFreshDeck();
            Assert.IsTrue(b.Deck.Count == 52);
        }

        [TestMethod]
        public void OnePair1()
        {
            Hand h1 = new Hand("Ac-Ad-2c-6c-7c");
            Hand h2 = new Hand("2c-2d-3c-5c-8d");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(-1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void OnePair2()
        {
            Hand h1 = new Hand("2c-2d-7c-6c-4c");
            Hand h2 = new Hand("2c-2d-8c-5c-3d");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void OnePair3()
        {
            Hand h1 = new Hand("2c-2d-7c-5c-9c");
            Hand h2 = new Hand("2c-2d-7c-5c-3d");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(-1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void OnePair4()
        {
            Hand h1 = new Hand("Jc-Jd-7c-6c-4c");
            Hand h2 = new Hand("Jc-Jh-8c-5c-3d");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void OnePair5()
        {
            Hand h1 = new Hand("Jc-Jd-8c-5c-3d");
            Hand h2 = new Hand("Jc-Jd-8c-3c-4c");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(-1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void OnePair6()
        {
            Hand h1 = new Hand("Jc-Jd-8c-5c-3d");
            Hand h2 = new Hand("Jc-Jd-8c-3c-4c");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(-1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void OnePair7()
        {
            Hand h1 = new Hand("9c-9d-8c-5c-3d");
            Hand h2 = new Hand("Jc-Jd-8c-3c-4c");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void OnePair8()
        {
            Hand h1 = new Hand("9c-9d-8c-5c-3d");
            Hand h2 = new Hand("Jc-Jd-8c-3c-4c");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void TwoPair1()
        {
            Hand h1 = new Hand("3c-3d-4h-4c-5h");
            Hand h2 = new Hand("3c-3d-5s-5c-6h");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void TwoPair2()
        {
            Hand h1 = new Hand("8c-8h-4h-4c-5h");
            Hand h2 = new Hand("6c-6d-5s-5c-2h");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(-1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void TwoPair3()
        {
            Hand h1 = new Hand("8c-8h-4h-4c-5h");
            Hand h2 = new Hand("8c-8h-4h-4c-5h");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(0, h1.CompareTo(h2));
        }
        [TestMethod]
        public void TwoPair4()
        {
            Hand h1 = new Hand("Jc-Jh-4h-4d-7h");
            Hand h2 = new Hand("Jc-Jd-4h-4c-5h");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(-1, h1.CompareTo(h2));
        }
        [TestMethod]
        public void TwoPair5()
        {
            Hand h1 = new Hand("Jc-Jh-4h-4d-2h");
            Hand h2 = new Hand("Jc-Jd-4h-4c-3h");
            h1.AssignRankAndName();
            h2.AssignRankAndName();
            Assert.AreEqual(1, h1.CompareTo(h2));
        }

    }


}
