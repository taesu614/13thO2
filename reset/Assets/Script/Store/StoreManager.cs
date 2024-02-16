using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Inst { get; private set; } //싱글톤
    [SerializeField] ItemSO itemSO;
    [SerializeField] TMP_Text moneyTMP;
    [SerializeField] GameObject deckscrollview;
    [SerializeField] GameObject carndnameuiprefab;   //덱에 표시할  카드 이름의 프리팹 
    public Transform DeckContent;          //덱 리스트 넣을 Content의 Transfrom
    List<Item> storecardlist = new List<Item>();
    List<GameObject> storecardprefablist = new List<GameObject>();
    public Transform StoreContent;
    public GameObject StoreCardPrefab;
    bool isReload = false;
    int count = 5;  //매수 관련
    SaveData savedata;
    List<Item> decktemp = new List<Item>();
    List<GameObject> deckuiobjectlist = new List<GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {
        Inst = this; // 싱글톤 인스턴스 설정
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

    private void LoadStoreCard()    //ItemSO내 Reward가 Store인 카드 넣기
    {
        foreach (Item A in itemSO.items) 
        {
            if (A.reward == "AdventureReward" || A.reward == "Sheep")
                storecardlist.Add(A);
        }
    }

    private void CardRandom() //storecardlist 랜덤하게 섞기
    {
        for (int i = 0; i < storecardlist.Count; i++)  
        {
            int rand = Random.Range(i, storecardlist.Count);
            Item temp = storecardlist[i];
            storecardlist[i] = storecardlist[rand];
            storecardlist[rand] = temp;
        }
    }

    private void SetStoreCard(int count)    //상점카드 프리팹 생성
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

    public void ReLoadStoreCard()   //상점 리롤 
    {
        if (isReload)
            return;
        foreach(GameObject A in storecardprefablist)    //원래 있던 상점 카드 프리팹 파괴
        {
            Destroy(A);
        }
        storecardprefablist.Clear();
        int num = 0;
        if (storecardlist.Count < 6)    //카드가 5장 이하면 굳이 리롤할 이유 없음
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
        AudioManager.instance.PlaySFX(AudioManager.SFX.roulette);  // 클릭시 임시 효과음
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

    public void MakeDeck()     //덱 생성과 관련된 기능
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
        foreach (Item A in decktemp)      //UI표시 관련
        {
            GameObject newcardname = Instantiate(carndnameuiprefab, DeckContent);  //카드 이름 프리팹 생성
            UIDeckCardName uideckcardname = newcardname.GetComponent<UIDeckCardName>();
            uideckcardname.Setup(A);          //덱의 UI를 생성하는 기능
            Debug.Log(newcardname);
            deckuiobjectlist.Add(newcardname);
        }
    }

    private void SortDeck()   //정렬 관련
    {
        int n = decktemp.Count;
        for (int i = 1; i < n; ++i)
        {
            Item key = decktemp[i];
            int j = i - 1;

            // key보다 큰 원소들을 오른쪽으로 이동
            while (j >= 0 && decktemp[j].GetID() > key.GetID())
            {
                decktemp[j + 1] = decktemp[j];
                j = j - 1;
            }

            // key를 적절한 위치에 삽입
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
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // 클릭시 임시 효과음
    }
}
