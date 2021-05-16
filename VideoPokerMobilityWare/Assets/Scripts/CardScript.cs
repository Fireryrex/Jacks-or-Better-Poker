using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField] CardScriptable cardInfo;
    [SerializeField] Image cardImage;
    // Start is called before the first frame update
    void Start()
    {
        //temp
        setCardInfo(cardInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //sets the info of the card 
    //called by gamemanager when dealing
    public void setCardInfo(CardScriptable setTo)
    {
        cardInfo = setTo;
        cardImage.sprite = cardInfo.getCardSprite(); 
    }

    public SuitType getCardSuit()
    {
        return cardInfo.getCardSuit();
    }

    public int getCardValue()
    {
        return cardInfo.getCardValue();
    }
}
