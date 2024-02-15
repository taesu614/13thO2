using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    List<Item> past = new List<Item>();    //���� ����Ʈ
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //����Ʈ�� �������� ��ü �����ϵ��� �ϴ� �뵵
    List<Item> tempPast = new List<Item>();
    public CardManager cardManager;
    public CardFunctionManager cardfunctionmanager;
    [SerializeField]
    private Transform slotParent;
    [SerializeField] GameObject cardPrefab;
    public RectTransform content;

#if UNITY_EDITOR
    private void OnValidate()
    {
        
    }
#endif

    void Awake()
    {
        FreshSlot();
    }

    private void Start()
    {
    }

    public void FreshSlot()
    {
        //items = cardManager.GetItemBuffer();
        
        /*for(; i <slots.Length;i++)
        {
            slots[i].item = null;
        }  */
    }
    public void AddCard(Item item)  //���ſ� ������ �̹��� ���� �뵵 + ��� ī�� üũ��
    {
        var cardObject = Instantiate(cardPrefab, content);
        var card = cardObject.GetComponent<Slot>();
        card.Setup(item);
        past.Add(item);

        cardnamePrefabslist.Add(cardObject);
    }
    
    public void GetCardList(Item cards)
    {
        print("ī�� ������ ����"+cardManager.GetItemBuffer().Count);
        for(int i=0; i<cardManager.GetItemBuffer().Count;i++)
        {
            past[i] = cardManager.GetItemBuffer()[i];
        }
        FreshSlot();
    }

    public List<Item> GetPast()
    {
        return past;
    }
    public void ClearItems()
    {
        foreach(var prefab in cardnamePrefabslist)  //��� ������ ����
        {
            Destroy(prefab);
        }
        past.Clear();   //���� ����
        FreshSlot();
    }

    public bool CheckPast(string cardName)  // ���ſ� Ư�� ī�尡 �ִ��� Ȯ���ؼ� bool�� ����
    {

        if (past != null && past.Count > 0)
        {
            string[] tempName = new string[past.Count];
            for (int i = 0; i < past.Count; i++)
            {
                tempName[i] = past[i].name;
            }


            int nameIndex = Array.FindLastIndex(tempName, i => i == cardName);
            if (nameIndex != -1)
                return true;
            else
            {
                Debug.Log("���ſ� " + cardName + "�� �����ϴ�!");
                return false;
            }
        }

        else
        {
            Debug.Log("���� ���� �����ϴ�!");
            return false;
        }
    }


}