using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [SerializeField]
    private GameManager gameManager;

    public int id; //set in Inspector

    int actions = 1;
    int buys = 1;
    int treasure = 0;
    int draws = 5;
    Hand hand;
    Deck deck;
    Deck discard;

    //renamed to Awake to ensure this runs before GameManager.Start()
    private void Awake()
    {
        hand = this.gameObject.transform.GetChild(0).GetComponent<Hand>();
        deck = this.gameObject.transform.GetChild(1).GetComponent<Deck>();
        discard = this.gameObject.transform.GetChild(2).GetComponent<Deck>();
    }

    public int Actions
    {
        get
        {
            return actions;
        }

        set
        {
            actions = value;
        }
    }

    public int Buys
    {
        get
        {
            return buys;
        }

        set
        {
            buys = value;
        }
    }

    public int Treasure
    {
        get
        {
            return treasure;
        }

        set
        {
            treasure = value;
        }
    }

    public int Draws
    {
        get
        {
            return draws;
        }
        set
        {
            draws = value;
        }
    }

    public int PlayerID
    {
        get
        {
            return id;
        }
    }

    public Deck PlayerDeck
    {
        get
        {
            return deck;
        }
    }

    public Hand PlayerHand
    {
        get
        {
            return hand;
        }
    }

    public void BuyCard(DominionCard c)
    {
        Treasure -= c.Cost;
        Buys--;
        discard.AddCard(c);
        /*
        GameObject.Find("TreasureCount").GetComponent<Text>().text = "Treasure: " + GameManager.activePlayer.Treasure;
        GameObject.Find("BuyCount").GetComponent<Text>().text = "Buys: " + GameManager.activePlayer.Buys;*/
        gameManager.UpdateCounts();
    }

    public void PlayCard(DominionCard c)
    {
        if (c.CardType.Contains("Action"))
        {
            Actions--;
        }
        Actions += c.Actions;
        Buys += c.Buys;
        Treasure += c.TValue;
        Draws += c.Cards;
        /*
        GameObject.Find("TreasureCount").GetComponent<Text>().text = "Treasure: " + GameManager.activePlayer.Treasure;
        GameObject.Find("ActionCount").GetComponent<Text>().text = "Actions: " + GameManager.activePlayer.Actions;
        GameObject.Find("BuyCount").GetComponent<Text>().text = "Buys: " + GameManager.activePlayer.Buys;*/
        gameManager.UpdateCounts();
        discard.AddCard(c);
    }

    public void Reshuffle()
    {
        deck.Collection.AddRange(discard.Collection);
        discard.Collection = new List<DominionCard>();

        if (deck.gameObject.activeSelf == false)
        {
            deck.gameObject.SetActive(true);
        }
        if (discard.gameObject.GetComponent<SpriteRenderer>().sprite != null)
        {
            discard.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
        deck.Shuffle();
    }

    public void Reset()
    {
        buys = 1;
        actions = 1;
        treasure = 0;
        draws = 5;
    }
}
