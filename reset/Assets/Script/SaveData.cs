using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public ItemSO itemSO;
    List<Item> cardlist = new List<Item>();
    List<Item> deck = new List<Item>();
    List<int> cardcount = new List<int>();
    string constellation = "Sheep"; //기본값을 sheep으로
    // Start is called before the first frame update
    public static SaveData instance;   //싱글톤으로 설정
    private int playermaxhelath = 100;
    private int playerhealth = 100;
    public int playermoney = 10;
    
    //맵 위치 관련
    string mymap;
    int playermapindex = 0; //플레이어 지도 위치
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

    public void ResetCardList() //저장된 카드 리스트 초기화
    {
        Debug.Log("초기화 완료");
        cardlist.Clear();
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

    public void DefaultDeckSetting()    //기본 덱 설정하는 곳   - 뭐 설정한 적 없어서 건드리지 말 것
    {
    }

    public void SetMyMap(string map)    //지도 설정
    {
        mymap = map;
    }

    public string GetMyMap()      //지도 보내기
    {
        return mymap;
    }
    public List<Item> GetPlayerDeck()
    {
        return cardlist;
    }
    public void SetPlayerMoney(int money)   //플레이어 돈 설정 (인기도- 최소 0, 최대 999)
    {
        playermoney = money;

        if (money <= 0)
        {
            playermoney = 0;
        }
        else if (money >= 999)
        {
            playermoney = 999;
        }
    }
    public int GetPlayerMoney()
    {
        return playermoney;
    }

    public void SetPlayerHealth(int a)
    {
        playerhealth = a;
        if (a > playermaxhelath)
            playerhealth = playermaxhelath;
    }

    public void SetPlayerMaxHealth(int a)
    {
        playermaxhelath = a;
    }
    
    public int GetPlayerMaxHealth()
    {
        return playermaxhelath;
    }

    public int GetPlayerHealth()
    {
        return playerhealth;
    }

    public float GetPlayerHealthPercent()
    {
        return (float)playerhealth / playermaxhelath;
    }

    public void SetPlayerConstellation(string name)     //설정한 별자리 저장
    {
        constellation = name;
        Debug.Log(constellation);
    }

    public string GetPlayerConstellation()
    {
        return constellation;
    }

    public void SetPlayerMapIndex(int index)    //플레이어 지도상 위치 설정
    {
        playermapindex = index;
    }

    public int GetPlayerMapIndex()
    {
        return playermapindex;
    }
}
