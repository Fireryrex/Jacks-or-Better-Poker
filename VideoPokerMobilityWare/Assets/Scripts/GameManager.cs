using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    #region Deck Vars
    [SerializeField] CardScriptable[] baseDeck;
    [SerializeField] List<CardScriptable> deckCopy;
    #endregion

    #region Hand Vars
    [SerializeField] GameObject[] playerHand;
    [SerializeField] int numDeals = 0;
    #endregion

    #region Canvas Vars
    [SerializeField] GameObject[] cardPositions;
    [SerializeField] GameObject emptyCard;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject endCanvas;
    [SerializeField] GameObject dealButton;
    [SerializeField] GameObject holdButton;
    [SerializeField] GameObject[] discardButtons;
    [SerializeField] TextMeshProUGUI currentPointsText;
    [SerializeField] TextMeshProUGUI totalPointsText;
    #endregion


    public void Awake()
    {
        gameManagerInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Loads all of the card scriptables from Resources/Deck
        baseDeck = Resources.LoadAll<CardScriptable>("Deck");
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Resets the gane  to its original state, except for the player's total points
    public void ResetGame()
    {
        //activates necessary buttons and disables unecessary ones
        dealButton.SetActive(true);
        foreach(GameObject button in discardButtons)
        {
            button.SetActive(true);
        }
        endCanvas.SetActive(false);
        //creates a new list, copying over the cards from the deck
        deckCopy = new List<CardScriptable>(baseDeck);
        //empties your hand
        for(int i = 0; i < playerHand.Length; i++)
        {
            DiscardCard(i);
        }
        numDeals = 0;
        totalPointsText.text = "You have: " + ScoreManager.scoreManagerInstance.getTotalPoints().ToString() + " points";
    }

    //Deals the number of cards passed to it.
    //This function searches through the player's hand and replaces any empty cards with new ones
    public void DrawCard()
    {
        dealButton.SetActive(false);
        holdButton.SetActive(true);
        for (int i = 0; i < playerHand.Length; i++)
        {
            if (playerHand[i] == null)
            {
                int randomCardNum = Random.Range(0, deckCopy.Count - 1);
                playerHand[i] = Instantiate(emptyCard, cardPositions[i].transform.position, Quaternion.identity, gameCanvas.transform);
                playerHand[i].GetComponent<CardScript>().setCardInfo(deckCopy[randomCardNum]);
                deckCopy.RemoveAt(randomCardNum);
            }
        }
        if (numDeals > 0)
        {
            ScoreManager.scoreManagerInstance.CalculateScore();
        }
        else
        {
            numDeals++;
        }
    }

    //removes card from hand, allowing player to replace it
    public void DiscardCard(int index)
    {
        dealButton.SetActive(true);
        holdButton.SetActive(false);
        Destroy(playerHand[index]);
    }

    //pulls up the end screen, allowing the player to start a new round.
    public void activateEndScreen()
    {
        //gameCanvas.SetActive(false);
        dealButton.SetActive(false);
        holdButton.SetActive(false);
        foreach(GameObject button in discardButtons)
        {
            button.SetActive(false);
        }
        endCanvas.SetActive(true);
        currentPointsText.text = "You got: " + ScoreManager.scoreManagerInstance.getPointsWon().ToString() + " points!";
        totalPointsText.text = "You have: " + ScoreManager.scoreManagerInstance.getTotalPoints().ToString() + " points";
    }

    public GameObject[] getPlayerHand()
    {
        return playerHand;
    }
}
