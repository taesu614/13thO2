using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    BinarySearchTree test = new BinarySearchTree();
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //리스트로 프리팹을 전체 삭제하도록 하는 용도
    private List<Item> itemBuffer;
    public GameObject deckui;
    public RectTransform content;
    public GameObject cardnameuiprefab;
    Canvas deckuicanvas;
    GameObject panel;
    CanvasRenderer panelcanvasrenderer;
    UIDeckButton uideckbutton;
    bool isopen = true;
    List<GameObject> instantiatedCards = new List<GameObject>();    //생성된 카드 프리팹의 리스트 - 나중에 얘네만 모아서 삭제하려는 용도

    private void Start()
    {
        deckuicanvas = deckui.GetComponent<Canvas>();
        panel = GameObject.Find("Panel");
        deckui.SetActive(false);
    }
    public void OpenUI()//카드 매수 설정 후 ItemSO와 연계하여 덱 설정하는 곳
    {
        if (isopen)
        {
            deckui.SetActive(true);
            itemBuffer = new List<Item>();
            for(int i = 0; i<itemSO.items.Length; i++)  //ItemSO에서 카드 데이터 불러옴
            {
                Item item = itemSO.items[i];
                var cardObject = Instantiate(cardPrefab, panel.transform);
                RectTransform cardRectTransform = cardObject.GetComponent<RectTransform>(); //UI에 있어서 RectTransform사용
                instantiatedCards.Add(cardObject);
                var card = cardObject.GetComponent<UICardButton>();
                card.Setup(item);
                //Debug.Log(item.name);
                switch (i%8)    //카드 좌표
                {
                    case 0:
                        cardRectTransform.anchoredPosition = new Vector2(-450f, 250);
                        break;
                    case 1:
                        cardRectTransform.anchoredPosition = new Vector2(-150f, 250);
                        break;
                    case 2:
                        cardRectTransform.anchoredPosition = new Vector2(150f, 250);
                        break;
                    case 3:
                        cardRectTransform.anchoredPosition = new Vector2(450f, 250);
                        break;
                    case 4:
                        cardRectTransform.anchoredPosition = new Vector2(-450f, -250);
                        break;
                    case 5:
                        cardRectTransform.anchoredPosition = new Vector2(-150f, -250);
                        break;
                    case 6:
                        cardRectTransform.anchoredPosition = new Vector2(150f, -250);
                        break;
                    case 7:
                        cardRectTransform.anchoredPosition = new Vector2(450f, -250);
                        break;
                }
                if(i > 8)
                {
                    break;
                }
            }


            isopen = false;

        }
        else    //이미지 제거하는 코드 만들 것
        {
            isopen = true;
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
        itemBuffer = new List<Item>();
        test.InOrderTraversal(itemBuffer);
        Debug.Log(itemBuffer.Count);
        foreach (var prefab in cardnamePrefabslist)
        {
            Destroy(prefab);
        }
        cardnamePrefabslist.Clear();
        foreach (Item A in itemBuffer)
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
}

#region binarySearchTree
//사용한 사이트 https://gamemakerslab.tistory.com/30
public class BinarySearchTree
{
    public class Node
    {
        public Item item;
        public int Value;
        public Node Left;
        public Node Right;

        public Node(Item name)
        {
            item = name;
            Value = name.identifier;
            Left = null;
            Right = null;
        }
    }

    public Node Root;

    public BinarySearchTree()
    {
        Root = null;
    }

    public void Insert(Item name)
    {
        Root = InsertRecursively(Root, name);
    }

    private Node InsertRecursively(Node node, Item name)
    {
        if (node == null)
        {
            node = new Node(name);
            return node;
        }

        if (name.identifier <= node.Value)
        {
            node.Left = InsertRecursively(node.Left, name);
        }
        else if (name.identifier > node.Value)
        {
            node.Right = InsertRecursively(node.Right, name);
        }

        return node;
    }

    public void InOrderTraversal(List<Item> item)
    {
        InOrderTraversalRecursively(Root, item);
    }

    private void InOrderTraversalRecursively(Node node, List<Item> item)
    {
        if (node != null)
        {
            // 왼쪽 서브 트리 순회
            InOrderTraversalRecursively(node.Left, item);

            // 현재 노드 처리
            item.Add(OutputItem(node.item));

            // 오른쪽 서브 트리 순회
            InOrderTraversalRecursively(node.Right, item);
        }
    }

    public Item OutputItem(Item item)
    {
        return item;
    }

    public int CountNodes()
    {
        return CountNodesRecursively(Root);
    }

    private int CountNodesRecursively(Node node)
    {
        if (node == null)
        {
            return 0;
        }

        // 현재 노드를 포함하여 왼쪽 서브 트리와 오른쪽 서브 트리의 노드 개수를 합산
        return 1 + CountNodesRecursively(node.Left) + CountNodesRecursively(node.Right);
    }
}
#endregion