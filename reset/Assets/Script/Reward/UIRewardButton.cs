using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRewardButton : MonoBehaviour
{
    public Item item;
    public List<Item> items;
    [SerializeField] Image carduiimage;
    [SerializeField] Image costuiimage;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text acitveTMP;
    [SerializeField] GameObject RewardExplantionPrefab; //보상 화면의 프리팹
    Canvas rewardcanvas;
    public string functionname;
    public string cardtype;
    public bool selectable;
    public int identifier;
    string reward;
    
    private void Start()
    {
        rewardcanvas = GameObject.Find("RewardCanvas").GetComponent<Canvas>();
    }
    public void Setup(Item item)    //Card.cs를 복붙하고 수정한 코드 -기능: 카드 세팅
    {
        this.item = item;
        carduiimage.sprite = this.item.colorimg;       //UI에서 사용되려면 Image컴포넌트를 수정해야함
        costuiimage.sprite = this.item.costcolor;       //SO파일은 SpriteRender라 이미지를 2번 주는식으로 함
        nameTMP.text = this.item.name;
        functionname = this.item.functionname;
        cardtype = this.item.cardtype;
        selectable = this.item.selectable;
        identifier = this.item.identifier;
        reward = this.item.reward;

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

    public void SetUIRewardExplanation()
    {
        GameObject newReward = Instantiate(RewardExplantionPrefab, rewardcanvas.transform);   //해당 자식 위치에 프리팹 생성 
        UIRewardExplanation cardsetup = newReward.GetComponent<UIRewardExplanation>();
        cardsetup.Setup(item);
    }
}
