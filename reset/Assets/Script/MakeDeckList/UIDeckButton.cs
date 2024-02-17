using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDeckButton : MonoBehaviour
{
    public Item itemname;
    [SerializeField] Image coloruiimage;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] TMP_Text cardsTMP;  // 카드 장수 정하는거
    [SerializeField] Image image;
    [SerializeField] Sprite blue;
    [SerializeField] Sprite red;
    [SerializeField] Sprite green;
    public int identifier;
    public int cards = 0;  // 카드 장수
    string a;
    SaveData savedata;
    DeckUIManager deckuimanager;
    // Start is called before the first frame update
    void Start()
    { 
        GameObject save = GameObject.Find("SaveData");
        savedata = save.transform.GetComponent<SaveData>();
        GameObject deckui = GameObject.Find("DeckUIManager");
        deckuimanager = deckui.GetComponent<DeckUIManager>();
    }

    public void Setup(Item item)
    {
        this.itemname = item;
        nameTMP.text = item.name;
        costTMP.text = item.cost.ToString();
       // cards++;
       // cardsTMP.text = cards.ToString();
        identifier = item.GetID();

        if (item.color == 'R')  //여기 숫자 수정하면 글씨 색 바뀜
        {
            ///nameTMP.color = new Color32(255, 88, 88, 255);
            //costTMP.color = new Color32(255, 88, 88, 255);
            image.sprite = red;
        }
        else if (item.color == 'G')
        {
            //nameTMP.color = new Color32(88, 255, 88, 255);
            //costTMP.color = new Color32(88, 255, 88, 255);
            image.sprite = green;
        }
        if (item.color == 'B')
        {
            //nameTMP.color = new Color32(88, 88, 255, 255);
            //costTMP.color = new Color32(88, 88, 255, 255);
            image.sprite = blue;
        }
    }

    public void AddCards()  // 카드 장수 증가
    {
        cards++;
    }

    public void Remove()
    {
        Debug.Log(itemname.name);
        deckuimanager.RemoveCard(itemname);

    }
}
