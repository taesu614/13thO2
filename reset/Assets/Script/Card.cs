using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer colorimg;
    [SerializeField] SpriteRenderer costcolor;
    [SerializeField] SpriteRenderer character;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text acitveTMP;
    [SerializeField] Sprite cardFront;  //지워야할수도?
    [SerializeField] Sprite cardBack;   //22
    [SerializeField] Sprite targetSprite;
    Sprite originalImage;   //원본 이미지 
    public Item item;
    bool isFront;   //delete
    public PRS originPRS;
    public string functionname;
    public string cardtype;
    public bool selectable;
    public bool isdrag = false;
    public void Setup(Item item, bool isMine)   //SO파일을 통해 카드 상태 설정
    {
        this.item = item;
        colorimg.sprite = this.item.colorimg;
        costcolor.sprite = this.item.costcolor;
        //character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        acitveTMP.text = this.item.active;
        functionname = this.item.functionname;
        cardtype = this.item.cardtype;
        selectable = this.item.selectable;
        originalImage = colorimg.sprite;
    }

    void OnMouseOver()
    {
        CardManager.Inst.CardMouseOver(this);
    }

    void OnMouseExit()
    {
        CardManager.Inst.CardMouseExit(this);    
    }
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if(useDotween)  //도트윈 써서 부드럽게 움직이게하기
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    public void ChangeCardImage(bool targetMarkOn)   //카드 이미지 변경
    {
        if(targetMarkOn)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            colorimg.sprite = targetSprite;
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            Transform message = transform;
            Transform messagetransform = message.Find("Message");
            CardMessage cardmessage = messagetransform.GetComponent<CardMessage>();
            cardmessage.ShowCardMessage(false);
            colorimg.sprite = originalImage;
        }


    }

    void OnMouseDown()
    {
        CardManager.Inst.CardMouseDown();
    }

    void OnMouseUp()
    {
        CardManager.Inst.CardMouseUp();
    }
}
