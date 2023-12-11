using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICardButton : MonoBehaviour
{
    public Item item;
    public List<Item> items;
    [SerializeField] Image coloruiimage;
    [SerializeField] Image costuiimage;
    [SerializeField] Image characterui;
    [SerializeField] SpriteRenderer colorimg;
    [SerializeField] SpriteRenderer costcolor;
    [SerializeField] SpriteRenderer character;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;  //계산에 쓰일 것이므로 num = int.Parse(costTMP); 해둘것
    [SerializeField] TMP_Text acitveTMP;
    [SerializeField] Sprite cardFront;  //지워야할수도?
    [SerializeField] Sprite cardBack;   //22
    public string functionname;
    public string cardtype;
    public bool selectable;
    public int identifier;
    SaveData savedata;
    public DeckUIManager deckuimanager;
    private void Start()
    {
        GameObject save = GameObject.Find("SaveData");
        savedata = save.transform.GetComponent<SaveData>();
    }
    public void Setup(Item item)    //Card.cs를 복붙하고 수정한 코드 -기능: 카드 세팅
    {
        this.item = item;
        colorimg.sprite = this.item.colorimg;
        costcolor.sprite = this.item.costcolor;
        //character.sprite = this.item.sprite;
        coloruiimage.sprite = this.item.colorimg;       //UI에서 사용되려면 Image컴포넌트를 수정해야함
        costuiimage.sprite = this.item.costcolor;       //SO파일은 SpriteRender라 이미지를 2번 주는식으로 함
        //characterui.sprite = this.item.sprite;          //너무 헤매다보니 빼는 상황을 테스트 못함 나중에 체력 돌아오면 테스트 해볼것
        nameTMP.text = this.item.name;
        //costTMP.text = this.item.cost.ToString();
        //acitveTMP.text = this.item.active;
        functionname = this.item.functionname;
        cardtype = this.item.cardtype;
        selectable = this.item.selectable;
        identifier = this.item.identifier;

        if (this.item.color == 'R')  //여기 숫자 수정하면 글씨 색 바뀜
        {
            nameTMP.color = new Color32(255, 88, 88, 255);
            //costTMP.color = new Color32(255, 88, 88, 255);
        }
        else if (this.item.color == 'G')
        {
            nameTMP.color = new Color32(88, 255, 88, 255);
            //costTMP.color = new Color32(88, 255, 88, 255);
        }
        if (this.item.color == 'B')
        {
            nameTMP.color = new Color32(88, 88, 255, 255);
            //costTMP.color = new Color32(88, 88, 255, 255);
        }
    }
    public void InputCard()
    {
        savedata.InputCardInDeck(item);
        deckuimanager.MakeCardNameUI(item);
        //Debug.Log(item.name);
    }
}
