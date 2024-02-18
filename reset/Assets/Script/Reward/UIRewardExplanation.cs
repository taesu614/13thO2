using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIRewardExplanation : MonoBehaviour
{
    public Item item;
    public List<Item> items;
    [SerializeField] Image coloruiimage;
    [SerializeField] Image costuiimage;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;  //��꿡 ���� ���̹Ƿ� num = int.Parse(costTMP); �صѰ�
    [SerializeField] TMP_Text acitveTMP;
    public string functionname;
    public string cardtype;
    public bool selectable;
    public int identifier;
    SaveData savedata;
    private void Start()
    {
        GameObject save = GameObject.Find("SaveData");
        savedata = save.transform.GetComponent<SaveData>();
    }
    public void Setup(Item item)    //Card.cs�� �����ϰ� ������ �ڵ� -���: ī�� ����
    {
        this.item = item;
        coloruiimage.sprite = this.item.colorimg;       //UI���� ���Ƿ��� Image������Ʈ�� �����ؾ���
        costuiimage.sprite = this.item.costcolor;       //SO������ SpriteRender�� �̹����� 2�� �ִ½����� ��
        nameTMP.text = this.item.name;
        acitveTMP.text = this.item.active;
        functionname = this.item.functionname;
        cardtype = this.item.cardtype;
        selectable = this.item.selectable;
        identifier = this.item.GetID();

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
    public void InputCard() //���� �ش� ī�带 ����
    {
        savedata.InputCardInDeck(item);
        SceneManager.LoadScene("MapScene");
        //�� ��ȯ ���� ��
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // �ӽ�
    }

    public void CloseExplanation()      //����â ����
    {
        Destroy(GameObject.Find("UIRewardExplanation(Clone)"));
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);  // �ӽ�
    }
}
