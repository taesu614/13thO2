using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    public List<Item> items;


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
        FreshSlot();
    }

    private void Start()
    {
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
            slots[i].item = null;
        }
    }

    public void FreshSlot()
    {
        //items = cardManager.GetItemBuffer();
        int i = 0;
        Debug.Log(items);
        for(;i <items.Count && i< slots.Length;i++) {
            slots[i].item = items[i];
            if (items[i] == null ) {
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
        
        if(items.Count < slots.Length)
        {
            FreshSlot();
            items.Add(_item);
            FreshSlot();
            print("Ä«µåÃß°¡!!!");
        }
        else
        {
            print("½½·Ô°¡µæÂü");
        }
    }
    
    public void GetCardList(Item cards)
    {
        print("Ä«µå ¹öÆÛÀÇ °¹¼ö"+cardManager.GetItemBuffer().Count);
        for(int i=0; i<cardManager.GetItemBuffer().Count;i++)
        {
            items[i] = cardManager.GetItemBuffer()[i];
        }
        FreshSlot();
    }

    public void ClearItems()
    {
        items.Clear();
        FreshSlot();
    }
}
