using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //����Ʈ�� �������� ��ü �����ϵ��� �ϴ� �뵵
    private List<Item> mydecklisttemp = new List<Item>();   //�� ����Ʈ �ӽ� �����(�̰��� ������ ��� ���������� ������ ����itemBuffer�� ����)
    public GameObject deckui;
    public RectTransform content;
    public GameObject cardnameuiprefab;
    Canvas deckuicanvas;
    GameObject cardlistpanel;
    CanvasRenderer panelcanvasrenderer;
    UIDeckButton uideckbutton;
    bool isopen = false;
    SaveData savedata;
    List<GameObject> instantiatedCards = new List<GameObject>();    //������ ī�� �������� ����Ʈ - ���߿� ��׸� ��Ƽ� �����Ϸ��� �뵵

    private void Awake()
    {
    }
    private void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        deckuicanvas = deckui.GetComponent<Canvas>();
        cardlistpanel = GameObject.Find("CardListPanel");
        deckui.SetActive(false);
        savedata.ResetCardList();
        itemSO.InitializeItems();
    }

    public bool IsUIOpen()
    {
        return isopen;
    }
    public void OpenUI()//ī�� �ż� ���� �� ItemSO�� �����Ͽ� �� �����ϴ� ��
    {
        if (!isopen)    //���� �ִٸ�
        {
            deckui.SetActive(true);
            //itemBuffer = new List<Item>();
            for (int i = 0; i < itemSO.items.Length; i++)  //ItemSO���� ī�� ������ �ҷ��� - ��ü ī�� ������
            {
                Item item = itemSO.items[i];
                if (itemSO.items[i].haveCard)
                {
                    var cardObject = Instantiate(cardPrefab, cardlistpanel.transform);
                    Transform newparent = GameObject.Find("CardListContent").GetComponent<Transform>();
                    cardObject.transform.SetParent(newparent);
                    RectTransform cardRectTransform = cardObject.GetComponent<RectTransform>(); //UI�� �־ RectTransform���
                    instantiatedCards.Add(cardObject);
                    var card = cardObject.GetComponent<UICardButton>();
                    card.Setup(item);
                }
            }


            isopen = true;
            AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);  // Ŭ���� �ӽ� ȿ����
        }
        else    //�̹��� �����ϴ� �ڵ� ���� ��
        {
            isopen = false;
            for (int i = 0; i < itemSO.items.Length; i++)
            {
                foreach (var cardObject in instantiatedCards)
                {
                    Destroy(cardObject);
                }
                deckui.SetActive(false);
                instantiatedCards.Clear(); // ����Ʈ ����
            }
            AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // Ŭ���� �ӽ� ȿ����
        }
    }


    private void MakeCardNameUI()
    {
        //itemBuffer = new List<Item>();
        Debug.Log(cardnamePrefabslist.Count);
        foreach (var prefab in cardnamePrefabslist) //���� ����Ʈ�� �����ִ� ������ ����
        {
            Destroy(prefab);
        }
        cardnamePrefabslist.Clear();
        SortDeck();
        foreach (Item A in mydecklisttemp)      //UIǥ�� ����
        {
            GameObject content = GameObject.Find("Content");    //Content �ڽ��� ����
            var newcardname = Instantiate(cardnameuiprefab, content.transform);  //ī�� �̸� ������ ����
            uideckbutton = newcardname.GetComponent<UIDeckButton>();
            uideckbutton.Setup(A);          //���� UI�� �����ϴ� ���
            cardnamePrefabslist.Add(newcardname);
            RectTransform rectTransform = newcardname.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }

    public void AddCard(Item item)
    {
        mydecklisttemp.Add(item);
        MakeCardNameUI();
        AudioManager.instance.PlaySFX(AudioManager.SFX.draw);  // �ӽ÷� draw ȿ���� ����
    }

    private void SortDeck()   //���� ����
    {
        int n = mydecklisttemp.Count;
        for (int i = 1; i < n; ++i)
        {
            Item key = mydecklisttemp[i];
            int j = i - 1;

            // key���� ū ���ҵ��� ���������� �̵�
            while (j >= 0 && mydecklisttemp[j].GetID() > key.GetID())
            {
                mydecklisttemp[j + 1] = mydecklisttemp[j];
                j = j - 1;
            }

            // key�� ������ ��ġ�� ����
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

        AudioManager.instance.PlaySFX(AudioManager.SFX.draw);  // �ӽ÷� draw ȿ���� ����
    }

    public void SaveDeck()
    {
        foreach(Item A in mydecklisttemp)
        {
            savedata.InputCardInDeck(A);
        }
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // Ŭ���� ȿ���� �ӽ�
    }
}