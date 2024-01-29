using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Inst { get; private set; } //�̱���
    [SerializeField] GameObject RewardCardPrefab;   //������ �� ī�� ������ �ֱ� 
    [SerializeField] ItemSO itemSO;                 //ItemSO : ī�� ������ ����
    [SerializeField] GameObject carndnameuiprefab;   //���� ǥ����  ī�� �̸��� ������ 
    Transform RewardContent;     //ī�� ���� ���� Content�� Transform
    public Transform DeckContent;          //�� ����Ʈ ���� Content�� Transfrom
    List<Item> RewardList = new List<Item>();  //���� �� ����Ʈ
    GameObject deckscrollview;
    SaveData savedata;
    List<Item> decktemp = new List<Item>();
    private void Start()        //�� ��ȯ �ڿ� �ѹ��� ����ϱ⿡ Start���� ������ 
    {
        deckscrollview = GameObject.Find("DeckScrollView");
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        RewardContent = GameObject.Find("RewardContent").GetComponent<Transform>();
        MakeDeck();
        SetRewardList();
        deckscrollview.SetActive(false);
    }
    private void SetCardReward(Item item)    //ī�� ���� ���� - 3�� ¥�� ���
    {
        GameObject newReward = Instantiate(RewardCardPrefab, RewardContent);   //�ش� �ڽ� ��ġ�� ������ ���� 
        UIRewardButton cardsetup = newReward.GetComponent<UIRewardButton>();
        cardsetup.Setup(item);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void OpenDeck()
    {
        deckscrollview.SetActive(true);
    }

    public void CloseDeck()
    {
        deckscrollview.SetActive(false);
    }

    private void MakeDeck()     //�� ������ ���õ� ���
    {
        foreach (Item A in savedata.GetPlayerDeck())
        {
            decktemp.Add(A);
        }
        SortDeck();
        Debug.Log(decktemp.Count);
        foreach (Item A in decktemp)      //UIǥ�� ����
        {
            var newcardname = Instantiate(carndnameuiprefab, DeckContent);  //ī�� �̸� ������ ����
            UIDeckCardName uideckcardname = newcardname.GetComponent<UIDeckCardName>();
            uideckcardname.Setup(A);          //���� UI�� �����ϴ� ���
        }
    }

    private void SortDeck()   //���� ����
    {
        int n = decktemp.Count;
        for (int i = 1; i < n; ++i)
        {
            Item key = decktemp[i];
            int j = i - 1;

            // key���� ū ���ҵ��� ���������� �̵�
            while (j >= 0 && decktemp[j].identifier > key.identifier)
            {
                decktemp[j + 1] = decktemp[j];
                j = j - 1;
            }

            // key�� ������ ��ġ�� ����
            decktemp[j + 1] = key;
        }
    }

    private void SetRewardList()    //���� ����� ���õ� ���
    {
        foreach (Item A in itemSO.items)
        {
            if (A.reward == "AdventureReward")      //�ش� ī���� Reward�� ���� ������ �� ���� ����Ʈ�� ����
                RewardList.Add(A);                  //���� ���������� Reward ���� �� 1,2,3���� �����ؾ��Ҽ���
        }
        for (int i = 0; i < RewardList.Count; i++)  //���� �����ϰ� ���� 
        {
            int rand = Random.Range(i, RewardList.Count);
            Item temp = RewardList[i];
            RewardList[i] = RewardList[rand];
            RewardList[rand] = temp;
        }
        for (int i = 0; i < 3; i++)
        {
            SetCardReward(RewardList[i]);
        }
    }
}