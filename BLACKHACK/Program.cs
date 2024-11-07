using System;
using System.Collections.Generic;

namespace Blackjack
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public class Card
    {
        public Suit Suit { get; set; }
        public int Value { get; set; }

        public Card(Suit suit, int value)
        {
            Suit = suit;
            Value = value;
        }

        public override string ToString()
        {
            string[] faceValues = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            return $"{faceValues[Value - 1]} of {Suit}";
        }
    }

    public class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                for (int value = 1; value <= 13; value++)
                {
                    cards.Add(new Card(suit, value));
                }
            }
        }

        public void Shuffle()
        {
            Random random = new Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card Draw()
        {
            if (cards.Count == 0) return null;
            Card drawnCard = cards[0];
            cards.RemoveAt(0);
            return drawnCard;
        }
    }

    public class Game
    {
        private Deck deck;
        private List<Card> playerHand;
        private List<Card> dealerHand;

        public Game()
        {
            deck = new Deck();
            deck.Shuffle();
            playerHand = new List<Card>();
            dealerHand = new List<Card>();

            playerHand.Add(deck.Draw());
            playerHand.Add(deck.Draw());
            dealerHand.Add(deck.Draw());
            dealerHand.Add(deck.Draw());
        }

        public void Play()
        {
            Console.WriteLine("ブラックジャックで勝負しましょう！");
            DisplayHands(false);

            // プレイヤーのターン
            while (true)
            {
                Console.WriteLine("あなたのターンです。 (H)it or (S)tand?");
                string action = Console.ReadLine().ToUpper();

                if (action == "H")
                {
                    playerHand.Add(deck.Draw());
                    DisplayHands(false);

                    if (CalculateHandValue(playerHand) > 21)
                    {
                        Console.WriteLine("あなたはバーストしました。ディーラーの勝利です。");
                        return;
                    }
                }
                else if (action == "S")
                {
                    break;
                }
            }

            // ディーラーのターン
            Console.WriteLine("ディーラーのターンです。");
            DisplayHands(true);
            while (CalculateHandValue(dealerHand) < 17)
            {
                dealerHand.Add(deck.Draw());
                DisplayHands(true);
            }

            // 勝敗の判定
            int playerValue = CalculateHandValue(playerHand);
            int dealerValue = CalculateHandValue(dealerHand);

            if (dealerValue > 21)
            {
                Console.WriteLine("ディーラーはバーストしました。あなたの勝利です。");
            }
            else if (playerValue > dealerValue)
            {
                Console.WriteLine("あなたの勝利です。");
            }
            else if (playerValue < dealerValue)
            {
                Console.WriteLine("あなたの負けです。");
            }
            else
            {
                Console.WriteLine("引き分けです。");
            }
        }

        public int CalculateHandValue(List<Card> hand)
        {
            int value = 0;
            int aceCount = 0;

            foreach (var card in hand)
            {
                if (card.Value == 1)
                {
                    aceCount++;
                    value += 11;
                }
                else if (card.Value > 10)
                {
                    value += 10;
                }
                else
                {
                    value += card.Value;
                }
            }

            // エースの場合、合計値が21以下に
            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }
            return value;
        }

        public void DisplayHands(bool showDealerHand)
        {
            Console.WriteLine("\nあなたの手札:");
            foreach (var card in playerHand)
            {
                Console.WriteLine(card);
            }
            Console.WriteLine($"Total Value: {CalculateHandValue(playerHand)}\n");

            Console.WriteLine("ディーラーの手札:");
            if (showDealerHand)
            {
                foreach (var card in dealerHand)
                {
                    Console.WriteLine(card);
                }
                Console.WriteLine($"Total Value: {CalculateHandValue(dealerHand)}\n");
            }
            else
            {
                Console.WriteLine(dealerHand[0]);

                Console.WriteLine("隠された手札");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Play();
        }
    }
}
