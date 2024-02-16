using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Inst { get; private set; } //�̱���
    [SerializeField] ItemSO itemSO;
    [SerializeField] TMP_Text moneyTMP;
    [SerializeField] GameObject deckscrollview;
    [SerializeField] GameObject carndnameuiprefab;   //���� ǥ����  ī�� �̸��� ������ 
    public Transform DeckContent;          //�� ����Ʈ ���� Content�� Transfrom
    List<Item> storecardlist = new List<Item>();
    List<GameObject> storecardprefablist = new List<GameObject>();
    public Transform StoreContent;
    public GameObject StoreCardPrefab;
    bool isReload = false;
    int count = 5;  //�ż� ����
    SaveData savedata;
    List<Item> decktemp = new List<Item>();
    List<GameObject> deckuiobjectlist = new List<GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {
        Inst = this; // �̱��� �ν��Ͻ� ����
    }
    private void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        MakeDeck();
        SetMoneyTMP();
        LoadStoreCard();
        CardRandom();
        SetStoreCard(count);
        Debug.Log(storecardlist.Count);
        deckscrollview.SetActive(false);
        itemSO.InitializeItems();
    }

    private void LoadStoreCard()    //ItemSO�� Reward�� Store�� ī�� �ֱ�
    {
        foreach (Item A in itemSO.items) 
        {
            if (A.reward == "AdventureReward" || A.reward == "Sheep")
                storecardlist.Add(A);
        }
    }

    private void CardRandom() //storecardlist �����ϰ� ����
    {
        for (int i = 0; i < storecardlist.Count; i++)  
        {
            int rand = Random.Range(i, storecardlist.Count);
            Item temp = storecardlist[i];
            storecardlist[i] = storecardlist[rand];
            storecardlist[rand] = temp;
        }
    }

    private void SetStoreCard(int count)    //����ī�� ������ ����
    {
        int num = 0;
        for (int i = 0; i < storecardlist.Count; i++)
        {
            if (num >= count)
                break;
            GameObject myInstance = Instantiate(StoreCardPrefab, StoreContent);
            StoreCardButton card = myInstance.GetComponent<StoreCardButton>();
            card.Setup(storecardlist[i]);
            storecardprefablist.Add(myInstance);
            num++;
        }
    }

    public void ReLoadStoreCard()   //���� ���� 
    {
        if (isReload)
            return;
        foreach(GameObject A in storecardprefablist)    //���� �ִ� ���� ī�� ������ �ı�
        {
            Destroy(A);
        }
        storecardprefablist.Clear();
        int num = 0;
        if (storecardlist.Count < 6)    //ī�尡 5�� ���ϸ� ���� ������ ���� ����
            return;
        for (int i = count; i < storecardlist.Count; i++)
        {
            if (num >= count)
                break;
            GameObject myInstance = Instantiate(StoreCardPrefab, StoreContent);
            StoreCardButton card = myInstance.GetComponent<StoreCardButton>();
            card.Setup(storecardlist[i]);
            storecardprefablist.Add(myInstance);
            num++;
        }
        isReload = true;
        AudioManager.instance.PlaySFX(AudioManager.SFX.roulette);  // Ŭ���� �ӽ� ȿ����
    }

    public void OpenDeck()
    {
        deckscrollview.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);
    }

    public void CloseDeck()
    {
        deckscrollview.SetActive(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);
    }

    public void MakeDeck()     //�� ������ ���õ� ���
    {
        if(deckuiobjectlist.Count > 0)
        {
            foreach (GameObject A in deckuiobjectlist)
                Destroy(A);
            deckuiobjectlist.Clear();
            decktemp.Clear();
        }
        foreach (Item A in savedata.GetPlayerDeck())
        {
            decktemp.Add(A);
        }
        SortDeck();
        Debug.Log(decktemp.Count);
        foreach (Item A in decktemp)      //UIǥ�� ����
        {
            GameObject newcardname = Instantiate(carndnameuiprefab, DeckContent);  //ī�� �̸� ������ ����
            UIDeckCardName uideckcardname = newcardname.GetComponent<UIDeckCardName>();
            uideckcardname.Setup(A);          //���� UI�� �����ϴ� ���
            Debug.Log(newcardname);
            deckuiobjectlist.Add(newcardname);
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
            while (j >= 0 && decktemp[j].GetID() > key.GetID())
            {
                decktemp[j + 1] = decktemp[j];
                j = j - 1;
            }

            // key�� ������ ��ġ�� ����
            decktemp[j + 1] = key;
        }
    }

    public void SetMoneyTMP()
    {
        moneyTMP.text = savedata.playermoney.ToString();
    }

    public void GotoMap()
    {
        SceneManager.LoadScene("MapScene");
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // Ŭ���� �ӽ� ȿ����
    }
}
