using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SuitType { Spades, Clubs, Hearts, Diamonds};

[CreateAssetMenu]
public class CardScriptable : ScriptableObject
{
    [SerializeField] SuitType cardSuit;
    [SerializeField] Sprite cardSprite;
    [SerializeField] int cardValue;

    public SuitType getCardSuit()
    {
        return cardSuit;
    }

    public Sprite getCardSprite()
    {
        return cardSprite;
    }

    public int getCardValue()
    {
        return cardValue;
    }
}
