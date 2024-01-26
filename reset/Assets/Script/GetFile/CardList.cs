using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    List<Item> past = new List<Item>();    //과거 리스트
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //리스트로 프리팹을 전체 삭제하도록 하는 용도

    public CardManager cardManager;
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
    public void AddCard(Item item)  //과거에 프리팹 이미지 생성 용도
    {
        var cardObject = Instantiate(cardPrefab, content);
        var card = cardObject.GetComponent<Slot>();
        card.Setup(item);
        past.Add(item);

        cardnamePrefabslist.Add(cardObject);
    }
    
    public void GetCardList(Item cards)
    {
        print("카드 버퍼의 갯수"+cardManager.GetItemBuffer().Count);
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
        foreach(var prefab in cardnamePrefabslist)  //모든 프리팹 삭제
        {
            Destroy(prefab);
        }
        past.Clear();   //과거 비우기
        FreshSlot();
    }
}
