using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject carduicontent;
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //리스트로 프리팹을 전체 삭제하도록 하는 용도
    private List<Item> mydecklisttemp = new List<Item>();   //덱 리스트 임시 저장소(이곳을 가지고 놀다 최종적으로 생성된 덱을itemBuffer에 넣음)
    List<Item> uicardlisttemp = new List<Item>();
    public GameObject deckui;
    public RectTransform content;
    public GameObject cardnameuiprefab;
    Canvas deckuicanvas;
    CanvasRenderer panelcanvasrenderer;
    UIDeckButton uideckbutton;

    UICardButton uicardbutton;

    bool isopen = false;
    SaveData savedata;
    List<GameObject> instantiatedCards = new List<GameObject>();    //생성된 카드 프리팹의 리스트 - 나중에 얘네만 모아서 삭제하려는 용도

    private void Awake()
    {
    }
    private void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        deckuicanvas = deckui.GetComponent<Canvas>();
        deckui.SetActive(false);
        //savedata.ResetCardList();
        itemSO.InitializeItems();
        foreach (Item A in itemSO.items)
            uicardlisttemp.Add(A);
        foreach (Item A in savedata.GetPlayerDeck())
            mydecklisttemp.Add(A);
    }

    public bool IsUIOpen()
    {
        return isopen;
    }
    public void OpenUI()//카드 매수 설정 후 ItemSO와 연계하여 덱 설정하는 곳
    {
        if (!isopen)    //닫혀 있다면
        {
            deckui.SetActive(true);
            SortItemSOtouicardlisttemp();
            //itemBuffer = new List<Item>();
            for (int i = 0; i < uicardlisttemp.Count; i++)  //ItemSO값을 넣은 uicardlisttemp에서 카드 데이터 불러옴 - 전체 카드 데이터
            {
                Item item = uicardlisttemp[i];
                if (uicardlisttemp[i].haveCard)
                {
                    var cardObject = Instantiate(cardPrefab, carduicontent.transform);
                    instantiatedCards.Add(cardObject);
                    var card = cardObject.GetComponent<UICardButton>();
                    card.Setup(item);
                }
            }
            MakeCardNameUI();
            isopen = true;
            AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);  // 클릭시 임시 효과음
        }
        else    //이미지 제거하는 코드 만들 것
        {
            isopen = false;
            for (int i = 0; i < itemSO.items.Length; i++)
            {
                foreach (var cardObject in instantiatedCards)
                {
                    Destroy(cardObject);
                }
                deckui.SetActive(false);
                instantiatedCards.Clear(); // 리스트 비우기
            }
            AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // 클릭시 임시 효과음
        }
    }


    private void MakeCardNameUI()
    {
        //itemBuffer = new List<Item>();
        Debug.Log(cardnamePrefabslist.Count);
        foreach (var prefab in cardnamePrefabslist) //덱의 리스트를 보여주는 프리팹 제거
        {
            Destroy(prefab);
        }
        cardnamePrefabslist.Clear();
        SortDeck();

        foreach (Item A in mydecklisttemp)      //UI표시 관련
        {
            GameObject content = GameObject.Find("Content");    //Content 자식이 덱임
            var newcardname = Instantiate(cardnameuiprefab, content.transform);  //카드 이름 프리팹 생성
            uideckbutton = newcardname.GetComponent<UIDeckButton>();
            uideckbutton.Setup(A);          //덱의 UI를 생성하는 기능
            cardnamePrefabslist.Add(newcardname);
            RectTransform rectTransform = newcardname.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }

    public void AddCard(Item item)
    {
        mydecklisttemp.Add(item);
        MakeCardNameUI();
        AudioManager.instance.PlaySFX(AudioManager.SFX.draw);  // 임시로 draw 효과음 넣음
    }

    void SortItemSOtouicardlisttemp()
    {
        int n = uicardlisttemp.Count;
        for (int i = 1; i < n; ++i)
        {
            Item key = uicardlisttemp[i];
            int j = i - 1;

            // key보다 큰 원소들을 오른쪽으로 이동
            while (j >= 0 && uicardlisttemp[j].GetID() > key.GetID())
            {
                uicardlisttemp[j + 1] = uicardlisttemp[j];
                j = j - 1;
            }

            // key를 적절한 위치에 삽입
            uicardlisttemp[j + 1] = key;
        }
    }
    private void SortDeck()   //정렬 관련
    {
        int n = mydecklisttemp.Count;
        for (int i = 1; i < n; ++i)
        {
            Item key = mydecklisttemp[i];
            int j = i - 1;

            // key보다 큰 원소들을 오른쪽으로 이동
            while (j >= 0 && mydecklisttemp[j].GetID() > key.GetID())
            {
                mydecklisttemp[j + 1] = mydecklisttemp[j];
                j = j - 1;
            }

            // key를 적절한 위치에 삽입
            mydecklisttemp[j + 1] = key;
        }
    }

    public void RemoveCard(Item item)
    {
        //itemBuffer = new List<Item>();
        foreach (Item A in mydecklisttemp)
        {
            if (item.GetID() == A.GetID())
            {
                mydecklisttemp.Remove(A);
                Debug.Log("RemoveCard");
                Debug.Log(mydecklisttemp.Count);
                break;
            }
        }
        MakeCardNameUI();

        AudioManager.instance.PlaySFX(AudioManager.SFX.draw);  // 임시로 draw 효과음 넣음
    }

    public void SaveDeck()
    {
        savedata.ResetCardList();
        foreach(Item A in mydecklisttemp)
        {
            savedata.InputCardInDeck(A);
        }
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // 클릭시 효과음 임시
    }
}