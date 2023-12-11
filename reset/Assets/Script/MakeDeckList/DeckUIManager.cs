using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    BinarySearchTree test;
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //리스트로 프리팹을 전체 삭제하도록 하는 용도
    private List<Item> itemBuffer = new List<Item>();
    Queue<Item> testqueue = new Queue<Item>();
    public GameObject deckui;
    public RectTransform content;
    public GameObject cardnameuiprefab;
    Canvas deckuicanvas;
    GameObject cardlistpanel;
    CanvasRenderer panelcanvasrenderer;
    UIDeckButton uideckbutton;
    bool isopen = false;
    SaveData savedata;
    List<GameObject> instantiatedCards = new List<GameObject>();    //생성된 카드 프리팹의 리스트 - 나중에 얘네만 모아서 삭제하려는 용도

    private void Awake()
    {
    }
    private void Start()
    {
        //savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        deckuicanvas = deckui.GetComponent<Canvas>();
        cardlistpanel = GameObject.Find("CardListPanel");
        test = new BinarySearchTree();
        deckui.SetActive(false);
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
            //itemBuffer = new List<Item>();
            for(int i = 0; i<itemSO.items.Length; i++)  //ItemSO에서 카드 데이터 불러옴
            {
                Item item = itemSO.items[i];
                var cardObject = Instantiate(cardPrefab, cardlistpanel.transform);
                Transform newparent = GameObject.Find("CardListContent").GetComponent<Transform>();
                cardObject.transform.SetParent(newparent);
                RectTransform cardRectTransform = cardObject.GetComponent<RectTransform>(); //UI에 있어서 RectTransform사용
                instantiatedCards.Add(cardObject);
                var card = cardObject.GetComponent<UICardButton>();
                card.Setup(item);
            }


            isopen = true;

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
        }
    }


    public void MakeCardNameUI(Item item)
    {
        test.Insert(item);
        //itemBuffer = new List<Item>();
        testqueue.Clear();
        test.InorderTraversal(testqueue);
        Debug.Log(testqueue.Count);
        Debug.Log(test.Search(item));
        SortCard();

    }

    void SortCard()
    {
        Debug.Log(cardnamePrefabslist.Count);
        foreach (var prefab in cardnamePrefabslist)
        {
            Destroy(prefab);
        }
        cardnamePrefabslist.Clear();
        foreach (Item A in testqueue)      //정렬 관련
        {
            GameObject content = GameObject.Find("Content");
            var newcardname = Instantiate(cardnameuiprefab, content.transform);  //카드 이름 프리팹 생성
            uideckbutton = newcardname.GetComponent<UIDeckButton>();
            uideckbutton.Setup(A);
            cardnamePrefabslist.Add(newcardname);
            RectTransform rectTransform = newcardname.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }

    public void RemoveCard(Item item)
    {
        //itemBuffer = new List<Item>();

        test.ResetTree();
        Debug.Log("FinishDelete");
        testqueue.Clear();
        test.InorderTraversal(testqueue);
        Debug.Log(testqueue.Count);
        SortCard();
    }
    public void Test(Item itemname)
    {
        Debug.Log(test.Search(itemname));
    }
}
//아직 제작중임
#region binarySearchTree    
//사용한 사이트 https://gamemakerslab.tistory.com/30
public class BinarySearchTree
{
    public class Node
    {
        public Item item;
        public Node Left;
        public Node Right;

        public Node(Item itemname)
        {
            item = itemname;
            Left = null;
            Right = null;
        }
    }

    public Node Root;

    public BinarySearchTree()
    {
        Root = null;
    }

    public void Insert(Item item)
    {
        Debug.Log(item.name);
        Root = InsertRecursively(Root, item);
    }

    private Node InsertRecursively(Node node, Item itemname)
    {
        if (node == null)
        {
            Debug.Log(itemname.name);
            node = new Node(itemname);
            return node;
        }

        if (itemname.identifier <= node.item.identifier)
        {
            node.Left = InsertRecursively(node.Left, itemname);
        }
        else if (itemname.identifier > node.item.identifier)
        {
            node.Right = InsertRecursively(node.Right, itemname);
        }

        return node;
    }

    public bool Search(Item itemname)
    {
        return SearchRecursively(Root, itemname) != null;
    }

    private Node SearchRecursively(Node node, Item itemname)
    {
        if (node == null || node.item.identifier == itemname.identifier)
        {
            return node;
        }

        if (itemname.identifier < node.item.identifier)
        {
            return SearchRecursively(node.Left, itemname);
        }

        return SearchRecursively(node.Right, itemname);
    }

    // 삭제 연산
    public void Delete(Item itemname)
    {
        Root = DeleteRecursively(Root, itemname);
    }

    private Node DeleteRecursively(Node node, Item itemname)
    {
        Debug.Log(node);
        if(node == null)
        {
            return node;
        }
        else if (itemname.identifier == node.item.identifier)
        {
            Debug.Log(node.item.name);
            node = DeleteNode(node);
            return node;
        }
        else if(itemname.identifier < node.item.identifier)
        {
            Debug.Log(node.item.name);
            node.Left = DeleteRecursively(node.Left, itemname);
            return node;
        }
        else
        {
            Debug.Log(node.item.name);
            node.Right = DeleteRecursively(node.Right, itemname);
            return node;
        }
    }

    Node DeleteNode(Node node)
    {
        if(node.Left == null && node.Right == null)     //못찾은 경우
        {
            return null;
        }
        else if(node.Left == null)  
        {
            return node.Right;  //왼자식만
        }
        else if(node.Right == null)
        {
            return node.Left;   //오른자식만
        }
        else            //두 자식 모두 있음
        {
            (int minItem, Node newRight) = DeleteMinItem(node.Right);
            node.item.identifier = minItem;
            node.Right = newRight;
            return node;
        }
    }

    (int, Node) DeleteMinItem(Node node)
    {
        if (node.Left == null)
            return (node.item.identifier, node.Right);
        else
        {
            (int minItem, Node newLeft) = DeleteMinItem(node.Left);
            node.Left = newLeft;
            return (minItem, node);
        }
    }

    public void ResetTree()
    {
        Root = null;
    }

    // 순회 연산
    public void InorderTraversal(Queue<Item> deck)
    {
        InorderTraversalRecursively(Root, deck);
        Debug.Log("finish line");
    }

    private void InorderTraversalRecursively(Node node, Queue<Item> deck)
    {
        if (node != null)
        {
            InorderTraversalRecursively(node.Left, deck);
            Debug.Log(node.item.name);
            deck.Enqueue(node.item);
            InorderTraversalRecursively(node.Right, deck);
        }
    }
}
#endregion