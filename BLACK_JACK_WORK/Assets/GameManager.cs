using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Object cardPrefab;
    public UnityEngine.UI.Button dealCardBtn;
    public UnityEngine.UI.Button hitCardBtn;
    public UnityEngine.UI.Button standCardBtn;
    public UnityEngine.UI.Button startGameBtn;
    public UnityEngine.UI.Button exitBtn;
    public UnityEngine.UI.Button restartBtn;
    public GameObject dealCardBtn1;
    public GameObject hitCardBtn1;
    public GameObject standCardBtn1;
    public GameObject startGameBtn1;
    public GameObject exitBtn1;
    public GameObject restartBtn1;
    public GameObject cardSpot1;
    public GameObject cardSpot2;
    public GameObject cardSpot3;
    public GameObject cardSpot4;
    public GameObject cardSpot5;
    public GameObject cardSpot6;
    public GameObject DealerText;
    public GameObject PlayerText;

    public GameObject DealerCardCover1;
    public GameObject DealerCardCover2;

    private int dealerTotal = 0;
    private int playerTotal = 0;

    private List<GameObject> cardGOs = new List<GameObject>();
    public Image background;
    public Sprite gamePlayBG;

    [Space]
    public float cardAnimationDelay;


    private bool saveSpace = false;
    public string endMessage;
    public Text endGameText;
    public GameObject endGameUI;
    public GameObject exitBtn2;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        dealCardBtn1.SetActive(false);
        hitCardBtn1.SetActive(false);
        standCardBtn1.SetActive(false);

        cardPrefab = Resources.Load("Card");
        dealCardBtn.onClick.AddListener(() => DealBtnClicked());
        hitCardBtn.onClick.AddListener(() => HitBtnClicked());
        standCardBtn.onClick.AddListener(() => StandBtnClicked());
        startGameBtn.onClick.AddListener(() => StartBtnClicked());

        // hitCardBtn.onClick.AddListener(() => HitBtnClicked());
        // standCardBtn.onClick.AddListener(() => StandBtnClicked());
        // exitBtn2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Clicked());
        // restartBtn.onClick.AddListener(() => Clicked());
    }


    private void StartBtnClicked()
    {
        // dealCardBtn1.SetActive(true);
        PlayerText.SetActive(true);
        DealerText.SetActive(true);
        background.sprite = gamePlayBG;
        startGameBtn1.SetActive(false);
        exitBtn1.SetActive(false);
        exitBtn2.SetActive(true);
        restartBtn1.SetActive(true);
        DealBtnClicked();
        audioSource.enabled = false;
    }

    private void HitBtnClicked()
    {
        if (playerTotal < 21)

        {
            GameObject newCard = (GameObject)Instantiate(cardPrefab, cardSpot5.transform.position, Quaternion.identity);
            newCard.transform.SetParent(cardSpot5.transform);
            cardGOs.Add(newCard);
            CardInit(newCard);

            if (cardGOs.Count > 1)
            {
                float distanceMod = 1f;
                Debug.Log(cardGOs[cardGOs.Count - 2].transform.localPosition.x);
                if (saveSpace) distanceMod = 1f;
                newCard.transform.localPosition = new Vector3(cardGOs[cardGOs.Count - 2].transform.localPosition.x + distanceMod, newCard.transform.localPosition.y, newCard.transform.localPosition.z);
            }
            playerTotal += Deck.CardValue(newCard.GetComponent<CardScript>().cardName);
            UpdatePlayerText();
            if (playerTotal > 21)
            {
                Bust("player");
            }
        }



    }
    private void Bust(string name)
    {
        hitCardBtn.gameObject.SetActive(false);
        standCardBtn.gameObject.SetActive(false);
        DealerCardCover1.SetActive(false);
        DealerCardCover2.SetActive(false);
        UpdateDealerText();
        if (name == "player")
        {
            endMessage = "Dealer wins!";
        }
        else
        {
            endMessage = "Player wins!";
        }
        endGameUI.SetActive(true);
        endGameText.text = endMessage;
    }
    private void StandBtnClicked()
    {
        standCardBtn1.SetActive(false);

        DealerCardCover1.SetActive(false);
        DealerCardCover2.SetActive(false);
        hitCardBtn1.SetActive(false);

        StartCoroutine(DealerTurn());


    }
    public void Restart()
    {
        Deck.deck.Clear();
        Deck.deck = new List<string>();
        Deck.cardName_Sprite = new Dictionary<string, Sprite>();
        dealerTotal = 0;
        playerTotal = 0;
        foreach (GameObject card in cardGOs)
        {
            Destroy(card);
        }
        cardGOs.Clear();
        cardGOs = new List<GameObject>();
        Deck.NewDeck();
        Deck.Shuffle();
        DealBtnClicked();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    private void DealBtnClicked()
    {
        endGameUI.SetActive(false);
        GameObject newCard = (GameObject)Instantiate(cardPrefab, cardSpot3.transform.position, Quaternion.identity);
        newCard.transform.SetParent(cardSpot3.transform);
        cardGOs.Add(newCard);
        CardInit(newCard);


        GameObject newCard1 = (GameObject)Instantiate(cardPrefab, cardSpot4.transform.position, Quaternion.identity);
        newCard1.transform.SetParent(cardSpot4.transform);
        cardGOs.Add(newCard1);
        CardInit(newCard1);

        GameObject newCard2 = (GameObject)Instantiate(cardPrefab, cardSpot1.transform.position, Quaternion.identity);
        newCard2.transform.SetParent(cardSpot1.transform);
        cardGOs.Add(newCard2);
        CardInit(newCard2);

        GameObject newCard3 = (GameObject)Instantiate(cardPrefab, cardSpot2.transform.position, Quaternion.identity);
        newCard3.transform.SetParent(cardSpot2.transform);
        cardGOs.Add(newCard3);
        CardInit(newCard3);

        hitCardBtn1.SetActive(true);
        standCardBtn1.SetActive(true);

        DealerCardCover1.SetActive(true);
        DealerCardCover2.SetActive(true);


        if (cardGOs.Count > 1)
        {
            float distanceMod = 1f;

            if (saveSpace) distanceMod = 1f;
            newCard.transform.position = new Vector3(cardGOs[cardGOs.Count - 2].transform.position.x + distanceMod, newCard.transform.position.y, newCard.transform.position.z);
        }


        if (cardGOs.Count > 1)
        {
            dealCardBtn1.SetActive(false);
        }

        playerTotal += Deck.CardValue(newCard.GetComponent<CardScript>().cardName);
        playerTotal += Deck.CardValue(newCard1.GetComponent<CardScript>().cardName);
        dealerTotal += Deck.CardValue(newCard2.GetComponent<CardScript>().cardName);
        dealerTotal += Deck.CardValue(newCard3.GetComponent<CardScript>().cardName);
        Debug.Log("player total=" + playerTotal);
        Debug.Log("dealer total=" + dealerTotal);
        UpdatePlayerText();
        UpdateDealerText();

        // Update the player's and dealer's totals and display them on the UI
        //playerTotal += Deck.CardValue(newCard.GetComponent<CardScript>().cardName);
        //dealerTotal += Deck.CardValue(newCard1.GetComponent<CardScript>().cardName);
        //dealerTotal += Deck.CardValue(newCard2.GetComponent<CardScript>().cardName);
        //dealerTotal += Deck.CardValue(newCard3.GetComponent<CardScript>().cardName);
        //UpdatePlayerText();
        //UpdateDealerText();
    }

    private void CardInit(GameObject newCard)
    {
        CardScript cScript = newCard.GetComponent<CardScript>();
        cScript.cardName = Deck.DrawCard();
        cScript.cardval = Deck.CardValue(cScript.cardName);
        newCard.GetComponent<SpriteRenderer>().sprite = Deck.cardName_Sprite[cScript.cardName];
    }
    private void UpdatePlayerText()
    {
        PlayerText.GetComponent<TextMeshProUGUI>().text = "Player Total: " + playerTotal;
    }

    // Method to update the player's total on the UI
    //private void UpdatePlayerText()
    //{
    //PlayerText.GetComponent<Text>().text = "Player Total: " + playerTotal;
    //}


    // Method to update the dealer's total on the UI
    private void UpdateDealerText()
    {
        // Display the dealer's total only if both covered cards are revealed
        if (!DealerCardCover1.activeSelf && !DealerCardCover2.activeSelf)
        {
            DealerText.GetComponent<TextMeshProUGUI>().text = "Dealer Total: " + dealerTotal;
        }
        else
        {
            // Display a message indicating that the dealer's total is hidden
            DealerText.GetComponent<TextMeshProUGUI>().text = "Dealer Total: Hidden";
        }

    }

    IEnumerator DealerTurn()
    {
        int i = 0;
        UpdateDealerText();
        while (dealerTotal < 17)
        {
            yield return new WaitForSeconds(cardAnimationDelay);
            GameObject newCard = (GameObject)Instantiate(cardPrefab, cardSpot2.transform.position, Quaternion.identity);
            newCard.transform.SetParent(cardSpot2.transform);
            cardGOs.Add(newCard);
            CardInit(newCard);
            dealerTotal += Deck.CardValue(newCard.GetComponent<CardScript>().cardName);
            newCard.transform.localPosition = new Vector3(cardSpot2.transform.GetChild(i).localPosition.x + 1, newCard.transform.localPosition.y, newCard.transform.localPosition.z);
            i++;
            UpdateDealerText();


        }
        if (dealerTotal > 21)
        {
            yield return new WaitForSeconds(cardAnimationDelay);
            Bust("dealer");
        }
        else
        {
            yield return new WaitForSeconds(cardAnimationDelay);
            if (dealerTotal > playerTotal)
                Bust("player");
            else
                Bust("dealer");
        }
        UpdateDealerText();

    }
}