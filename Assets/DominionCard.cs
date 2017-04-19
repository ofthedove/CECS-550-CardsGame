using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CARDLIST
{
    CELLAR, CHAPEL, MOAT, CHANCELLOR, VILLAGE, WOODCUTTER,
    WORKSHOP, BUREAUCRAT, FEAST, GARDENS, MILITIA, MONEYLENDER,
    REMODEL, SMITHY, SPY, THIEF, THRONEROOM, COUNCILROOM,
    FESTIVAL, LABORATORY, LIBRARY, MARKET, MINE, WITCH,
    ADVENTURER, COPPER = 90, SILVER = 91, GOLD = 92, ESTATE = 93, DUCHY = 94, 
    PROVINCE = 95
};
[System.Serializable]
public class DominionCard {
    public int id;
    private string cardName;
    private string desc;
    private string cardType;
    private int cost;
    private int actions;
    private int buys;
    private int cards;
    private int tValue;
    private int vValue;
    
    private string spritePath;

    public string CardName
    {
        get
        {
            return cardName;
        }

        set
        {
            cardName = value;
        }
    }
    public string Desc
    {
        get
        {
            return desc;
        }

        set
        {
            desc = value;
        }
    }

    public string SpritePath
    {
        get
        {
            return spritePath;
        }

        set
        {
            spritePath = value;
        }
    }

    public int Cost
    {
        get
        {
            return cost;
        }

        set
        {
            cost = value;
        }
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

    public int Cards
    {
        get
        {
            return cards;
        }

        set
        {
            cards = value;
        }
    }

    public int TValue
    {
        get
        {
            return tValue;
        }

        set
        {
            tValue = value;
        }
    }

    public int VValue
    {
        get
        {
            return vValue;
        }

        set
        {
            vValue = value;
        }
    }

    public string CardType
    {
        get
        {
            return cardType;
        }

        set
        {
            cardType = value;
        }
    }

    public DominionCard(Dictionary<string,string> dict)
    {
        id = int.Parse(dict["ID"]);
        cardName = dict["CardName"];
        desc = dict["Description"];
        cardType = dict["CardType"];
        cost = int.Parse(dict["Cost"]);
        actions = int.Parse(dict["Actions"]);
        buys = int.Parse(dict["Buys"]);
        cards = int.Parse(dict["Cards"]);
        TValue = int.Parse(dict["TreasureValue"]);
        VValue = int.Parse(dict["VictoryValue"]);
        spritePath = dict["SpritePath"];

    }
    public static Sprite GetSprite(int id)
    {
        return Resources.Load("Card Images/" + ((CARDLIST)id).ToString().ToLower(), typeof(Sprite)) as Sprite;
    }
}
