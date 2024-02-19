using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public Item item;
    public List<Item> items;
    [SerializeField] Image coloruiimage;
    [SerializeField] Image costuiimage;
    [SerializeField] TMP_Text nameTMP;
    public string functionname;
    public string cardtype;
    public bool selectable;
    private void Start()
    {
    }

    public void ResetImage()
    {
        print("카드 이미지 삭제");
        this.GetComponent<Image>().sprite = null;
    }
    public void Setup(Item item)    //Card.cs를 복붙하고 수정한 코드 -기능: 카드 세팅
    {
        this.item = item;
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
    }
}
