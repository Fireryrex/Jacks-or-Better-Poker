using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManagerInstance;

    #region Hand Values
    [SerializeField] int royalFlush = 800;
    [SerializeField] int straightFlush = 50;
    [SerializeField] int fourKind = 25;
    [SerializeField] int fullHouse = 9;
    [SerializeField] int flush = 6;
    [SerializeField] int straight = 4;
    [SerializeField] int threeKind = 3;
    [SerializeField] int twoPair = 2;
    [SerializeField] int jacksBetter = 1;
    [SerializeField] int other = 0;
    #endregion

    #region Card Vars
    [SerializeField] List<int> playerHandValues;
    [SerializeField] int[] numDuplicates;
    #endregion

    #region Score Vars
    [SerializeField] int currentPoints = 0;
    [SerializeField] int totalPoints = 0;
    #endregion

    public void Awake()
    {
        scoreManagerInstance = this;
    }

    public void CalculateScore()
    {
        //generates a list of just the card values.
        sortHand();
        if (isFlush())
        {
            //calculate to see which specific flush it is
            if (isBroadwayStraight())
            {
                currentPoints = royalFlush;
            }
            else if (isStraight())
            {
                currentPoints = straightFlush;
            }
            else
            {
                currentPoints = flush;
            }
        }
        else
        {
            if (isFourKind())
            {
                currentPoints = fourKind;
            }
            else if (isFullHouse())
            {
                currentPoints = fullHouse;
            }
            else if (isStraight())
            {
                currentPoints = straight;
            }
            else if (isThreeKind())
            {
                currentPoints = threeKind;
            }
            else if (isTwoPair())
            {
                currentPoints = twoPair;
            }
            else if (isJacksBetter())
            {
                currentPoints = jacksBetter;
            }
            else
            {
                currentPoints = other;
            }
        }
        totalPoints += currentPoints;
        GameManager.gameManagerInstance.activateEndScreen();
    }

    //sorts the hand for easier score calculation 
    //Additionally updates numDuplicates
    public void sortHand()
    {
        //fills numDuplicates with 0s
        //This array should only need to be size 4 as the way I calculate duplicates only makes 4 comparisons
        numDuplicates = new int[GameManager.gameManagerInstance.getPlayerHand().Length - 1]; ;
        //empties out playerHandValues List
        playerHandValues = new List<int>();
        for (int i = 0; i < GameManager.gameManagerInstance.getPlayerHand().Length; i++)
        {
            playerHandValues.Add(GameManager.gameManagerInstance.getPlayerHand()[i].GetComponent<CardScript>().getCardValue());
        }
        playerHandValues.Sort();

        //updates array to store inf on how many of a card is in a hand
        int duplicateIndex = 0;
        for (int i = 1; i < playerHandValues.Count; i++)
        {
            if (playerHandValues[i] == playerHandValues[i - 1])
            {
                numDuplicates[duplicateIndex]++;
            }
            else
            {
                duplicateIndex++;
            }
        }
    }

    //checks to see if the hand is flush. It compares the value to the suit of the first card as suits only matter in flushes.
    public bool isFlush()
    {
        for (int i = 0; i < GameManager.gameManagerInstance.getPlayerHand().Length; i++)
        {
            if (GameManager.gameManagerInstance.getPlayerHand()[i].GetComponent<CardScript>().getCardSuit() != GameManager.gameManagerInstance.getPlayerHand()[0].GetComponent<CardScript>().getCardSuit())
            {
                return false;
            }
        }
        return true;
    }

    //calculates if the hand is four of a kind
    public bool isFourKind()
    {
        foreach (int num in numDuplicates)
        {
            //stored num will be num of duplicates - 1 as I'm sortHand checks for duplicates in pairs
            if (num == 3)
            {
                return true;
            }
        }
        return false;
    }

    //calculates if the hand has a three of a kind and a two of a kind
    public bool isFullHouse()
    {
        bool threeOf = false;
        bool twoOf = false;
        foreach (int num in numDuplicates)
        {
            //stored num will be num of duplicates - 1 as I'm checking in pairs
            if (num == 2)
            {
                threeOf = true;
            }
            else if (num == 1)
            {
                twoOf = true;
            }
        }
        if (twoOf && threeOf)
        {
            return true;
        }
        return false;
    }

    //checks to see if the hand has 10, J, Q, K, and A
    //function is only necessary for royal flush as the normal isStraight function checks for this already.
    public bool isBroadwayStraight()
    {
        if (playerHandValues[0] == 1 && playerHandValues[1] == 10 && playerHandValues[2] == 11 && playerHandValues[3] == 12 && playerHandValues[4] == 13)
        {
            return true;
        }
        return false;
    }

    //checks if cards are all increasing by one. The only exception is if it is a broadway straight (Ace and royals)
    public bool isStraight()
    {
        for (int i = 1; i < playerHandValues.Count; i++)
        {
            //if values aren't incrementing by one the entire way the hand is either a broadway straight or not a straight at all
            if (playerHandValues[i] - playerHandValues[i - 1] != 1)
            {
                //check to see if the two values are an ace and a 10, thus making it a broadway straight
                if (playerHandValues[i - 1] != 1 || playerHandValues[i] != 10)
                {
                    return false;
                }
            }
        }
        return true;
    }

    //checks if the hand has three of a kind. Is just basically the function for full house but only checking for the 3 of a kind part
    //Full house is always checked first, so we don't have to worry about missing a full house.
    public bool isThreeKind()
    {
        foreach (int num in numDuplicates)
        {
            //stored num will be num of duplicates - 1 as I'm checking in pairs
            if (num == 2)
            {
                return true;
            }
        }
        return false;
    }

    //checks if hand has two pairs of equal value cards. 
    //As this is called after other duplicate checking functions we don't have to worry about missing higher tier hands
    public bool isTwoPair()
    {
        int numPairs = 0;
        foreach (int num in numDuplicates)
        {
            //stored num will be num of duplicates - 1 as I'm checking in pairs
            if (num == 1)
            {
                numPairs++;
            }
        }
        if (numPairs == 2)
        {
            return true;
        }
        return false;
    }

    //checks to see if hand has at least a pair of jacks or better.
    public bool isJacksBetter()
    {
        int numJacks = 0;
        int numQueens = 0;
        int numKings = 0;
        for (int i = 0; i < playerHandValues.Count; i++)
        {
            if (playerHandValues[i] == 13)
            {
                numKings++;
            }
            else if (playerHandValues[i] == 12)
            {
                numQueens++;
            }
            else if (playerHandValues[i] == 11)
            {
                numJacks++;
            }
        }
        if (numJacks == 2 || numQueens == 2 || numKings == 2)
        {
            return true;
        }
        return false;
    }

    public int getTotalPoints()
    {
        return totalPoints;
    }

    public int getPointsWon()
    {
        return currentPoints;
    }
}
