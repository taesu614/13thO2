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
    [SerializeField] GameObject RewardExplantionPrefab; //���� ȭ���� ������
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
    public void Setup(Item item)    //Card.cs�� �����ϰ� ������ �ڵ� -���: ī�� ����
    {
        this.item = item;
        carduiimage.sprite = this.item.colorimg;       //UI���� ���Ƿ��� Image������Ʈ�� �����ؾ���
        costuiimage.sprite = this.item.costcolor;       //SO������ SpriteRender�� �̹����� 2�� �ִ½����� ��
        nameTMP.text = this.item.name;
        functionname = this.item.functionname;
        cardtype = this.item.cardtype;
        selectable = this.item.selectable;
        identifier = this.item.GetID();
        reward = this.item.reward;

        if (this.item.color == 'R')  //���� ���� �����ϸ� �۾� �� �ٲ�
        {
            //nameTMP.color = new Color32(255, 88, 88, 255);
            //costTMP.color = new Color32(255, 88, 88, 255);
        }
        else if (this.item.color == 'G')
        {
            //nameTMP.color = new Color32(88, 255, 88, 255);
            //costTMP.color = new Color32(88, 255, 88, 255);
        }
        if (this.item.color == 'B')
        {
            //nameTMP.color = new Color32(88, 88, 255, 255);
            //costTMP.color = new Color32(88, 88, 255, 255);
        }
    }

    public void SetUIRewardExplanation()
    {
        GameObject newReward = Instantiate(RewardExplantionPrefab, rewardcanvas.transform);   //�ش� �ڽ� ��ġ�� ������ ���� 
        UIRewardExplanation cardsetup = newReward.GetComponent<UIRewardExplanation>();
        cardsetup.Setup(item);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);
    }
}
