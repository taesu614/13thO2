using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreCardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    [SerializeField] Image CardImage;
    [SerializeField] Image CostImage;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text activeTMP;
    [SerializeField] TMP_Text priceTMP;
    [SerializeField] GameObject textpanel;
    [SerializeField] GameObject buttonUI;
    int price;
    SaveData savedata;
    // Start is called before the first frame update
    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        textpanel.SetActive(false);
        buttonUI.SetActive(false);
    }

    public void Setup(Item item)   //SO파일을 통해 카드 상태 설정
    {
        this.item = item;
        CardImage.sprite = this.item.colorimg;
        CostImage.sprite = this.item.costcolor;
        activeTMP.text = this.item.active;
        nameTMP.text = this.item.name;
        priceTMP.text = this.item.price.ToString();
        price = this.item.price;

        if (this.item.color == 'R')  //여기 숫자 수정하면 글씨 색 바뀜
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

    public void OnPointerEnter(PointerEventData eventData)  //마우스 오버될 때 할 일 처리
    {
        textpanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)// 마우스가 UI를 떠날 때 할 일 처리
    {
        textpanel.SetActive(false);
    }

    public void OpenButtonUI()
    {
        buttonUI.SetActive(true);
    }
    public void CloseButtonUI()
    {
        buttonUI.SetActive(false);
    }

    public void BuyCard()   //카드 구매
    {
        if(price <= savedata.playermoney)
        {
            savedata.playermoney = savedata.playermoney - price;
            savedata.InputCardInDeck(item);
            Destroy(gameObject);
            StoreManager.Inst.SetMoneyTMP();
            AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);  // 클릭시 임시 효과음
        }
    }
}
