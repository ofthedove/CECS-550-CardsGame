﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private GameObject TreasureCount;

    [SerializeField]
    private GameObject ActionCount;

    [SerializeField]
    private GameObject BuyCount;

    [SerializeField]
    private GameObject PlayerTurn;

    public int[] playset = new int[10];
    public static List<DominionCard> cardsLoaded = new List<DominionCard>();
    public const int MAX_HAND_SIZE = 7;
    public static List<Player> players = new List<Player>();
    public static Player activePlayer;
    public static int activePlayerIndex;
    public static int stockPilesEmpty = 0;

    private TextAsset dbLocation;
    private Dictionary<string, string> cardDictionary = new Dictionary<string, string>();
    private static List<Dictionary<string, string>> playsetDictionary = new List<Dictionary<string, string>>();

    // Use this for initialization
    void Start () {
        List<GameObject> p = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        foreach (GameObject player in p)
        {
            players.Add(player.GetComponent<Player>());
        }
        activePlayerIndex = 1;
        activePlayer = players[activePlayerIndex];
        PlayerTurn.GetComponent<Text>().text = "Player " + activePlayer.PlayerID + "'s Turn";
        dbLocation = (TextAsset)Resources.Load("CardDatabase");
        LoadCardsFromXML();
        InitialSetup();
    }
	
	// Update is called once per frame
	void InitialSetup () {
        List<Deck> set = new List<Deck>();
        set.AddRange(GameObject.Find("Kingdom").GetComponentsInChildren<Deck>());
        set.AddRange(GameObject.Find("Victory").GetComponentsInChildren<Deck>());
        set.AddRange(GameObject.Find("Treasure").GetComponentsInChildren<Deck>());
        foreach (Deck d in set)
        {
            char signifier = char.Parse(d.gameObject.name.Substring(0, 1));
            int value = 0;
            if (signifier == 'K')
            {
                value = playset[int.Parse(d.gameObject.name.Substring(1)) - 1];
            } else
            {
                value = int.Parse(d.gameObject.name.Substring(1));
            }
            d.PlaysetFill(signifier, value);
        }
        /*
        Deck playerDeck = GameObject.Find("PlayerDeck").GetComponent<Deck>();
        for (int i = 0; i < 7; i++) {
            playerDeck.AddCard(GameManager.FindCardByName("Copper"));
        }
        for (int i = 0; i < 3; i++)
        {
            playerDeck.AddCard(GameManager.FindCardByName("Estate"));
        }
        playerDeck.Shuffle();*/

        //add the beginning cards to the players' decks
        foreach(Player player in players)
        {
            //Deck playerDeck = GameObject.Find("PlayerDeck").GetComponent<Deck>();
            for (int j = 0; j < 7; j++)
            {
                player.PlayerDeck.AddCard(FindCardByName("Copper"));
            }
            for (int j = 0; j < 3; j++)
            {
                player.PlayerDeck.AddCard(FindCardByName("Estate"));
            }
            player.PlayerDeck.Shuffle();
        }
    }

    void LoadCardsFromXML()
    {
        //Refactor this to only load the cards in playset
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dbLocation.text);
        XmlNodeList cardList = xmlDocument.GetElementsByTagName("DominionCard");
        foreach (XmlNode cardInfo in cardList)
        {
            XmlNodeList cardContent = cardInfo.ChildNodes;
            cardDictionary = new Dictionary<string, string>(); 
            foreach (XmlNode content in cardContent)
            {
                cardDictionary.Add(content.Name, content.InnerText);
            }
            playsetDictionary.Add(cardDictionary);
        }
        foreach (Dictionary<string,string> d in playsetDictionary)
        {
            cardsLoaded.Add(new DominionCard(d));
        }
    }

    public static DominionCard FindCardByName(string cardName)
    {
        foreach(Dictionary<string,string> d in playsetDictionary)
        {
            if (d["CardName"].Equals(cardName))
            {
                return new DominionCard(d);
            }
        }
        return null;
    }

    public void NextPlayerTurn()
    {
        activePlayer.PlayerHand.DiscardAll(activePlayer);
        activePlayerIndex++;
        activePlayerIndex %= players.Count;
        activePlayer = players[activePlayerIndex];
        activePlayer.Reset();
        UpdateCounts();
    }

    public void UpdateCounts()
    {
        TreasureCount.GetComponent<Text>().text = "Treasure: " + activePlayer.Treasure;
        ActionCount.GetComponent<Text>().text = "Actions: " + activePlayer.Actions;
        BuyCount.GetComponent<Text>().text = "Buys: " + activePlayer.Buys;
        PlayerTurn.GetComponent<Text>().text = "Player " + activePlayer.PlayerID + "'s Turn";
    }

    public static void StockpileEmpty(bool province)
    {
        if(province)
        {
            //game winning code here
        }
        else
        {
            stockPilesEmpty++;
            if(stockPilesEmpty == 3)
            {
                //game winning code here
            }
        }
    }
}
