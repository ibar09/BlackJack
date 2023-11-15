using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardScript : MonoBehaviour
{
    public SpriteRenderer sR;
    public string cardName;
    public int cardval;

    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        //sR.sprite = Resources.LoadAll<Sprite>("DeckOfCards")[Random.Range(0, Resources.LoadAll<Sprite>("DeckOfCards").Length)];

    }


   

    // Update is called once per frame
    void Update()
    {
      
    }
}