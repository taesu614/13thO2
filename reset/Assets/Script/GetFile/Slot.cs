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
    [SerializeField] SpriteRenderer colorimg;
    [SerializeField] SpriteRenderer costcolor;
    [SerializeField] TMP_Text nameTMP;
    public string functionname;
    public string cardtype;
    public bool selectable;
    private void Start()
    {

    }

    public void ResetImage()
    {
        print("ī�� �̹��� ����");
        this.GetComponent<Image>().sprite = null;
    }
    public void Setup(Item item)    //Card.cs�� �����ϰ� ������ �ڵ� -���: ī�� ����
    {
        this.item = item;
        colorimg.sprite = this.item.colorimg;
        costcolor.sprite = this.item.costcolor;
        //character.sprite = this.item.sprite;
        coloruiimage.sprite = this.item.colorimg;       //UI���� ���Ƿ��� Image������Ʈ�� �����ؾ���
        costuiimage.sprite = this.item.costcolor;       //SO������ SpriteRender�� �̹����� 2�� �ִ½����� ��
        //characterui.sprite = this.item.sprite;          //�ʹ� ��Ŵٺ��� ���� ��Ȳ�� �׽�Ʈ ���� ���߿� ü�� ���ƿ��� �׽�Ʈ �غ���
        nameTMP.text = this.item.name;
        //costTMP.text = this.item.cost.ToString();
        //acitveTMP.text = this.item.active;
        functionname = this.item.functionname;
        cardtype = this.item.cardtype;
        selectable = this.item.selectable;
    }
}
