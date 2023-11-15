using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Deck : MonoBehaviour

//ace = 1/11
//2 = 2
//3 = 3
//4 = 4
//5 = 5
//6 = 6
//7 = 7
//8 = 8
//9 = 9
//10 = 10
//jack. queen, king = 10

{
    public static List<string> deck = new List<string>(); // A deck that takes the form of a list of card names (must be static for a static Shuffle function)
    public static Dictionary<string, Sprite> cardName_Sprite = new Dictionary<string, Sprite>();    //Card name keys to card sprite values

    public static System.Random rng = new System.Random();  //Initializing System randomness

    void Start()
    {
        NewDeck();  //Create a new deck
        Shuffle();  //Shuffle the new deck
    }

    public static void NewDeck()
    {
        string[] suits = { "Hearts", "Spades", "Diamonds", "Clubs" };   //Each of the four suits in a deck of cards

        foreach (string suit in suits)   // For each suit, we need to make the same 13 cards
        {
            for (int i = 1; i <= 13; i++)    //There are 13 cards per suit, ranging 1-10, then J, Q, K
            {
                string num; //For easy concatenation, we'll make the numbers into strings (also great for Aces and Face cards)
                if (i == 1)  // An i value of 1 means the card is an Ace, so we set num to be "Ace"
                {
                    num = "Ace";
                }
                else if (i == 11)
                {
                    num = "Jack";
                }
                else if (i == 12)
                {
                    num = "Queen";
                }
                else if (i == 13)
                {
                    num = "King";
                }
                else
                {
                    num = i.ToString(); //All of the other cards just use their regular numbers
                }
                num = num + " of " + suit;  //We give all card names the structure 'value of suit,' eg. 'Queen of Hearts'
                deck.Add(num);  //We add that full card name to the deck
            }
        }

        foreach (string card in deck)    //Now that the deck is complete, we need to assign a sprite to each card
        {
            int indexOfCard = deck.IndexOf(card); //The index of a specific card within the deck
            Sprite spriteAtIndex = Resources.LoadAll<Sprite>("DeckOfCards")[indexOfCard]; //The sprite at the index given by indexOfCard

            cardName_Sprite[card] = spriteAtIndex;   //Assign the spriteAtIndex as the value pair for the relevant card name
        }

    }

    public static void Shuffle()    //Shuffles the deck of cards (Note that this is static, meaning only static variables can be used in it)
    {
        //Consider this as working with two cards/values and two indexes
        //One random value at a random index, and one known card at a known value
        //We want to place the known value into the random index
        //The random value should then be placed into the known index

        for (int i = deck.Count - 1; i > 0; i--)    //For each of the cards in the deck, we'll do the following
        {
            int j = rng.Next(i + 1);    //Get a random value up to a value we haven't used as an index to be shuffled yet
            string randCard = deck[j]; //Pick a random card and save its value

            deck[j] = deck[i];    //change the card at the random index to a known value
            deck[i] = randCard;    //change the card at the known index to the random value

        }

    }

    public static string DrawCard()
    {
        string dealtCard = deck[0];
        Debug.Log(dealtCard);
        deck.RemoveAt(0);
        return dealtCard;
    }

    public static int CardValue(string cardName)
    {
        Debug.Log(cardName);
        if (int.TryParse(cardName.Substring(0, 2), out int val))
        {
            return val;
        }
        else if (cardName.Contains("Ace"))
        {
            return 11;
        }
        else if (cardName.Contains("Jack") || cardName.Contains("Queen") || cardName.Contains("King"))
        {
            return 10;
        }
        else
        {
            return 10;
        }
    }
}