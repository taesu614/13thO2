using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UICardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public List<Item> items;
    [SerializeField] Image coloruiimage;
    [SerializeField] Image costuiimage;
    [SerializeField] SpriteRenderer colorimg;
    [SerializeField] SpriteRenderer costcolor;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;  //계산에 쓰일 것이므로 num = int.Parse(costTMP); 해둘것
    [SerializeField] TMP_Text acitveTMP;
    [SerializeField] GameObject panel;
    public string functionname;
    public string cardtype;
    public bool selectable;
    public int identifier;
    SaveData savedata;
    DeckUIManager deckuimanager;
    private void Start()
    {
        GameObject save = GameObject.Find("SaveData");
        savedata = save.transform.GetComponent<SaveData>();
        deckuimanager = GameObject.Find("DeckUIManager").GetComponent<DeckUIManager>();
        panel.SetActive(false);
    }
    public void Setup(Item item)    //Card.cs를 복붙하고 수정한 코드 -기능: 카드 세팅
    {
        this.item = item;
        colorimg.sprite = this.item.colorimg;
        costcolor.sprite = this.item.costcolor;
        coloruiimage.sprite = this.item.colorimg;       //UI에서 사용되려면 Image컴포넌트를 수정해야함
        costuiimage.sprite = this.item.costcolor;       //SO파일은 SpriteRender라 이미지를 2번 주는식으로 함
        nameTMP.text = this.item.name;
        acitveTMP.text = this.item.active;
        functionname = this.item.functionname;
        cardtype = this.item.cardtype;
        selectable = this.item.selectable;
        identifier = this.item.GetID();
    }
    public void InputCard()
    {
        //savedata.InputCardInDeck(item);
        deckuimanager.AddCard(item);
        //deckuimanager.MakeCardNameUI(item);
        //Debug.Log(item.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
