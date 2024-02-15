using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public ItemSO itemSO;
    List<Item> cardlist = new List<Item>();
    List<Item> deck = new List<Item>();
    List<int> cardcount = new List<int>();
    string constellation = "Sheep"; //�⺻���� sheep����
    // Start is called before the first frame update
    public static SaveData instance;   //�̱������� ����
    private int playermaxhelath = 100;
    private int playerhealth = 100;
    public int playermoney = 10;
    
    //�� ��ġ ����
    string mymap;
    int playermapindex = 0; //�÷��̾� ���� ��ġ
    private void Awake()
    {
        if (instance == null)   //�� ��ȯ �� ������ ���� �뵵
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetCardList() //����� ī�� ����Ʈ �ʱ�ȭ
    {
        Debug.Log("�ʱ�ȭ �Ϸ�");
        cardlist.Clear();
    }

    public void InputCardInDeck(Item name)
    {
        cardlist.Add(name);
        name.haveCard = true;  // �ӽ÷� ī�� �߰��� haveCard true����
        //Debug.Log(cardlist);
    }
    
    /*public void Test()  //ī�� �ż� ���� �� ItemSO�� �����Ͽ� �� �����ϴ� ��
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
    }*/ //�� �־����� �𸣰ھ �ϴ� ����

    public void DefaultDeckSetting()    //�⺻ �� �����ϴ� ��   - �� ������ �� ��� �ǵ帮�� �� ��
    {
    }

    public void SetMyMap(string map)    //���� ����
    {
        mymap = map;
    }

    public void ResetMyMap()
    {
        mymap = null;
        playermapindex = 0;
    }

    public string GetMyMap()      //���� ������
    {
        Debug.Log(mymap);
        return mymap;
    }
    public List<Item> GetPlayerDeck()
    {
        return cardlist;
    }
    public void SetPlayerMoney(int money)   //�÷��̾� �� ���� (�α⵵- �ּ� 0, �ִ� 999)
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

    public void SetPlayerConstellation(string name)     //������ ���ڸ� ����
    {
        constellation = name;
        Debug.Log(constellation);
    }

    public string GetPlayerConstellation()
    {
        return constellation;
    }

    public void SetPlayerMapIndex(int index)    //�÷��̾� ������ ��ġ ����
    {
        playermapindex = index;
    }

    public int GetPlayerMapIndex()
    {
        return playermapindex;
    }
}
