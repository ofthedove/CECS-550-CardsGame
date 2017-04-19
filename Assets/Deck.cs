using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {
    List<DominionCard> collection = new List<DominionCard>();
    public bool revealed = false; //set in Unity inspector

    public List<DominionCard> Collection
    {
        get
        {
            return collection;
        }

        set
        {
            collection = value;
        }
    }

    public int Count
    {
        get
        {
            return collection.Count;
        }
    }

    public void Shuffle()
    {
        int n = Collection.Count;
        while (n > 1)
        {
            n--;
            int k = new System.Random().Next(n + 1);
            DominionCard value = Collection[k];
            Collection[k] = Collection[n];
            Collection[n] = value;
        }
    }

    public void AddCard(DominionCard d)
    {
        if (GetComponent<SpriteRenderer>().sprite == null && tag != "PlayerDeck")
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(d.SpritePath);
        } else if(tag == "DiscardDeck")
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(d.SpritePath);
        } else if(tag == "PlayerDeck")
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Card Images/cardBack");
        }
        Collection.Add(d);
    }

    public DominionCard DrawCard()
    {
        DominionCard drawn = Collection[0];
        Collection.RemoveAt(0);
        if(Collection.Count == 0)
        {
            if (tag != "HandDeck")
            {
                this.gameObject.SetActive(false);
            } else
            {
                this.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        return drawn;
    }

    internal void PlaysetFill(char signifier, int value)
    {
        int size = 0;
        //Set the size of the deck
        switch (signifier) {
            case 'P':
                //Fill with 7 copper and 3 estate & return
                break;
            case 'K':
                size = 10;
                break;
            case 'T':
                switch(value)
                {
                    case 3:
                        size = 30;
                        break;
                    case 2:
                        size = 40;
                        break;
                    case 1:
                        size = 60 - GameManager.players.Count * 7;
                        break;
                }
                value += 89;
                break;
            case 'V':
                if(GameManager.players.Count == 2)
                {
                    size = 8;
                } else
                {
                    size = 12;
                }
                value += 92;
                break;
        }
        for(int i=0; i<size; i++)
        {
            AddCard(GameManager.cardsLoaded.Find(item => item.id == value));
        }
    }

    private void OnMouseEnter()
    {
        if (revealed && Collection.Count > 0)
        {
            if (GameObject.Find("SelectedCardImage").GetComponent<Image>() == null)
            {
                GameObject.Find("SelectedCardImage").AddComponent<Image>();
            }
            GameObject.Find("SelectedCardImage").GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
            GameObject.Find("SelectedCardText").GetComponent<Text>().text = Collection[0].CardName + "\n" + Collection[0].Desc;
        }
    }

    private void OnMouseDown()
    {
        if (Collection.Count > 0)
        {
            if (tag == "GameDeck")
            {
                if (GameManager.activePlayer.Treasure >= Collection[0].Cost && GameManager.activePlayer.Buys > 0)
                {
                    //if this player is about to buy the last Province
                    if(Collection.Count == 1 && Collection[0].id == 95)
                    {
                        //signal to the GameManager that the Province stockpile is empty
                        GameManager.StockpileEmpty(true);
                    }
                    //or if this player is about to buy the last of any kingdom card
                    else if(Collection.Count == 1)
                    {
                        //signal to the GameManager that a stockpile is empty
                        GameManager.StockpileEmpty(false);
                    }
                    GameManager.activePlayer.BuyCard(DrawCard());
                }
            }
            else if (tag == "PlayerDeck")
            {
                if (this.gameObject.transform.parent.GetComponent<Player>().PlayerID 
                    == GameManager.activePlayer.PlayerID)
                {
                    //if(GameManager.activePlayer.PlayerHand.Count < GameManager.MAX_HAND_SIZE)
                    while (GameManager.activePlayer.Draws > 0)
                    {
                        GameManager.activePlayer.PlayerHand.AddCard(DrawCard());
                        GameManager.activePlayer.Draws--;
                    }
                }
            }
            else if (tag == "HandDeck")
            {
                if (GameManager.activePlayer.Actions > 0 || !Collection[0].CardType.Contains("Action"))
                {
                    GameManager.activePlayer.PlayerHand.Count--;
                    //GameObject.Find("Hand").GetComponent<Hand>().Count--;
                    GameManager.activePlayer.PlayCard(DrawCard());
                }
            } else if(tag == "DiscardDeck")
            {
                if (this.gameObject.transform.parent.GetComponent<Player>().PlayerID
                    == GameManager.activePlayer.PlayerID)
                {
                    //only allow player to reshuffle discard into their deck
                    //if their deck is empty
                    if (GameManager.activePlayer.PlayerDeck.Count == 0)
                    {
                        GameManager.activePlayer.Reshuffle();
                    }
                }
            }
        }
    }
}
