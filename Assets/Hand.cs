using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour {
    List<GameObject> hand = new List<GameObject>();

    int count = 0;

    public int Count
    {
        get
        {
            return count;
        }

        set
        {
            count = value;
        }
    }

    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            GameObject g = this.gameObject.transform.GetChild(i).gameObject;
            hand.Add(g);
            g.SetActive(false);
        }
    }
    public void AddCard(DominionCard d)
    {
        if (Count < GameManager.MAX_HAND_SIZE)
        {
            for (int i = 0; i < GameManager.MAX_HAND_SIZE; i++)
            {
                if (hand[i].GetComponent<SpriteRenderer>().sprite == null)
                {
                    hand[i].SetActive(true);
                    hand[i].GetComponent<Deck>().AddCard(d);
                    Count++;
                    return;
                }
            }
        }
    }

    public void DiscardAll(Player owner)
    {
        foreach (GameObject obj in hand)
        {
            Deck deck = obj.GetComponent<Deck>();
            if (deck.Count > 0)
            {
                DominionCard c = deck.DrawCard();
                owner.DiscardCard(c);
                count--;
            }
        }
    }
}
