using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public ItemSO itemSO;
    List<Item> cardlist = new List<Item>();
    List<Item> deck = new List<Item>();
    List<int> cardcount = new List<int>();
    // Start is called before the first frame update
    private static SaveData instance;   //싱글톤으로 설정
    private int playerhealth = 300;
    private void Awake()
    {
        if (instance == null)   //씬 전환 시 생성을 막는 용도
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InputCardInDeck(Item name)
    {
        cardlist.Add(name);
        //Debug.Log(cardlist);
    }
    
    /*public void Test()  //카드 매수 설정 후 ItemSO와 연계하여 덱 설정하는 곳
    {
        Debug.Log("11111111111111");
        for (int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            cardlist.Add(item);
        }

        foreach (Item A in cardlist)
        {
            Debug.Log(A.name);
        }
    }*/ //왜 넣었는지 모르겠어서 일단 냅둠

    public void DefaultDeckSetting()    //기본 덱 설정하는 곳
    {
    }
    public List<Item> GetPlayerDeck()
    {
        return cardlist;
    }

    public void SetPlayerHealth(int a)
    {
        playerhealth = a;
    }

    public int GetPlayerHealth()
    {
        return playerhealth;
    }
}
