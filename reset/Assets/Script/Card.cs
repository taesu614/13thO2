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

        if(this.item.color == 'R')  //여기 숫자 수정하면 글씨 색 바뀜
        {
            nameTMP.color = new Color32(255, 88, 88, 255);
        }
        else if (this.item.color == 'G')
        {
            nameTMP.color = new Color32(88, 255, 88, 255);
        }
        if (this.item.color == 'B')
        {
            nameTMP.color = new Color32(88, 88, 255, 255);
        }
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
