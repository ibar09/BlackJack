using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private Object cardPrefab;
    public UnityEngine.UI.Button dealCardBtn;
    public UnityEngine.UI.Button hitCardBtn;
    public UnityEngine.UI.Button standCardBtn;
    public UnityEngine.UI.Button startGameBtn;
    public UnityEngine.UI.Button exitBtn;
    public UnityEngine.UI.Button restartBtn;
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

    public int dealerAmount, playerAmount;
    public int betAmount;
    public TextMeshProUGUI dealerAmountUI, playerAmountUI;
    public TMP_InputField betInput;
    public GameObject betButton;
    private bool firstTime = true;
    private GameObject dealerCard;
    private Vector3 cardCoverInitialScale;


    // Start is called before the first frame update
    void Start()
    {
        // dealCardBtn1.SetActive(false);
        hitCardBtn1.SetActive(false);
        standCardBtn1.SetActive(false);

        cardPrefab = Resources.Load("Card");
        // dealCardBtn.onClick.AddListener(() => DealBtnClicked());
        hitCardBtn.onClick.AddListener(() => HitBtnClicked());
        standCardBtn.onClick.AddListener(() => StandBtnClicked());
        startGameBtn.onClick.AddListener(() => StartBtnClicked());

        cardCoverInitialScale = DealerCardCover1.transform.localScale;
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
        betButton.SetActive(true);
        restartBtn1.SetActive(true);
        betInput.gameObject.SetActive(true);
        audioSource.enabled = false;
        dealerAmountUI.transform.parent.gameObject.SetActive(true);
        playerAmountUI.transform.parent.gameObject.SetActive(true);
        DealBtnClicked();
    }

    private void HitBtnClicked()
    {
        if (playerTotal < 21)

        {
            GameObject newCard = (GameObject)Instantiate(cardPrefab, cardSpot5.transform.position, Quaternion.identity);
            newCard.transform.SetParent(cardSpot5.transform);
            cardGOs.Add(newCard);
            SoundManager.Instance.Play("card");
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
    public void Bet()
    {

        if (betInput.text != "" && betInput.text != null)
        {
            int newBetAmount = int.Parse(betInput.text);
            if (newBetAmount > playerAmount)
                betAmount = playerAmount;
            else
                betAmount = newBetAmount;
        }
        else
            betAmount = 100;
        betInput.gameObject.SetActive(false);


        if (!firstTime)
        {
            foreach (GameObject card in cardGOs)
            {
                Destroy(card);
            }
            cardGOs.Clear();
            cardGOs = new List<GameObject>();
            DealBtnClicked();
        }
        betButton.SetActive(false);
        hitCardBtn1.SetActive(true);
        standCardBtn1.SetActive(true);
        firstTime = false;

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
            playerAmount -= betAmount;
            playerAmountUI.text = playerAmount.ToString();
            dealerAmount += betAmount;
            dealerAmountUI.text = dealerAmount.ToString();
            endMessage = "Dealer wins!";
        }
        else
        {
            playerAmount += betAmount;
            playerAmountUI.text = playerAmount.ToString();
            if (betAmount > dealerAmount)
                dealerAmount = 0;
            else
                dealerAmount -= betAmount;
            dealerAmountUI.text = dealerAmount.ToString();
            endMessage = "Player wins!";
        }
        endGameUI.SetActive(true);
        endGameText.text = endMessage;
        if (dealerAmount != 0 && playerAmount != 0)
        {
            betButton.SetActive(true);
            betInput.gameObject.SetActive(true);
        }
    }
    private void StandBtnClicked()
    {

        standCardBtn1.SetActive(false);

        DealerCardCover1.transform.DOScaleX(0f, 0.3f).OnComplete(() => { dealerCard.transform.DOScaleX(1f, 0.3f); });

        // DealerCardCover1.SetActive(false);
        DealerCardCover2.SetActive(false);
        SoundManager.Instance.Play("card flip");
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
        firstTime = true;
        playerAmount = 2000;
        playerAmountUI.text = playerAmount.ToString();
        dealerAmount = 2000;
        dealerAmountUI.text = dealerAmount.ToString();
        foreach (GameObject card in cardGOs)
        {
            Destroy(card);
        }
        cardGOs.Clear();
        cardGOs = new List<GameObject>();
        Deck.NewDeck();
        Deck.Shuffle();
        hitCardBtn1.SetActive(false);
        standCardBtn1.SetActive(false);
        betButton.SetActive(true);
        betInput.gameObject.SetActive(true);
        DealBtnClicked();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    private void DealBtnClicked()
    {
        dealerTotal = 0;
        playerTotal = 0;
        endGameUI.SetActive(false);
        GameObject newCard = (GameObject)Instantiate(cardPrefab, cardSpot3.transform.position, Quaternion.identity);
        newCard.transform.SetParent(cardSpot3.transform);
        cardGOs.Add(newCard);
        CardInit(newCard);
        DealerCardCover1.transform.localScale = cardCoverInitialScale;


        GameObject newCard1 = (GameObject)Instantiate(cardPrefab, cardSpot4.transform.position, Quaternion.identity);
        newCard1.transform.SetParent(cardSpot4.transform);
        cardGOs.Add(newCard1);
        CardInit(newCard1);

        GameObject newCard2 = (GameObject)Instantiate(cardPrefab, cardSpot1.transform.position, Quaternion.identity);
        newCard2.transform.SetParent(cardSpot1.transform);
        newCard2.transform.localScale = new Vector3(0, 1, 1);
        cardGOs.Add(newCard2);
        CardInit(newCard2);
        dealerCard = newCard2;

        GameObject newCard3 = (GameObject)Instantiate(cardPrefab, cardSpot2.transform.position, Quaternion.identity);
        newCard3.transform.SetParent(cardSpot2.transform);
        cardGOs.Add(newCard3);
        CardInit(newCard3);



        DealerCardCover1.SetActive(true);
        DealerCardCover2.SetActive(true);


        if (cardGOs.Count > 1)
        {
            float distanceMod = 1f;

            if (saveSpace) distanceMod = 1f;
            newCard.transform.position = new Vector3(cardGOs[cardGOs.Count - 2].transform.position.x + distanceMod, newCard.transform.position.y, newCard.transform.position.z);
        }




        playerTotal += Deck.CardValue(newCard.GetComponent<CardScript>().cardName);
        playerTotal += Deck.CardValue(newCard1.GetComponent<CardScript>().cardName);
        dealerTotal += Deck.CardValue(newCard2.GetComponent<CardScript>().cardName);
        dealerTotal += Deck.CardValue(newCard3.GetComponent<CardScript>().cardName);
        Debug.Log("player total=" + playerTotal);
        Debug.Log("dealer total=" + dealerTotal);
        UpdatePlayerText();
        UpdateDealerText();

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
        if (DealerCardCover1.transform.localScale.x == 0)
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
            SoundManager.Instance.Play("card");
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