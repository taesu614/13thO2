using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    List<Item> past = new List<Item>();    //과거 리스트


    public CardManager cardManager;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake()
    {
        for (int i = 0; i < past.Count && i < slots.Length; i++)   //처음 시작 시 과거 리스트 비움
        {
            slots[i].item = past[i];
            slots[i].item = null;
        }
        FreshSlot();
    }

    private void Start()
    {
    }

    public void FreshSlot()
    {
        //items = cardManager.GetItemBuffer();
        for(int i = 0; i < past.Count && i< slots.Length;i++) {
            slots[i].item = past[i];
            if (past[i] == null ) {
                slots[i].item = null;
            }
        }
        /*for(; i <slots.Length;i++)
        {
            slots[i].item = null;
        }  */
    }
    public void AddCard(Item _item)
    {
        
        if(past.Count < slots.Length)
        {
            FreshSlot();
            past.Add(_item);
            FreshSlot();
            print("카드추가!!!");
        }
        else
        {
            print("슬롯가득참");
        }
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
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
        past.Clear();
        FreshSlot();
    }
}
