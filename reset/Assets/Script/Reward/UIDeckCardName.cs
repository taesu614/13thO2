using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDeckCardName : MonoBehaviour
{
    // Start is called before the first frame update
    public Item itemname;
    [SerializeField] Image coloruiimage;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] Image image;
    [SerializeField] Sprite blue;
    [SerializeField] Sprite red;
    [SerializeField] Sprite green;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Setup(Item item)
    {
        this.itemname = item;
        nameTMP.text = item.name;
        costTMP.text = item.cost.ToString();

        if (item.color == 'R')  //여기 숫자 수정하면 글씨 색 바뀜
        {
            //nameTMP.color = new Color32(255, 88, 88, 255);
            //costTMP.color = new Color32(255, 88, 88, 255);
            image.sprite = red;
        }
        else if (item.color == 'G')
        {
            //nameTMP.color = new Color32(88, 255, 88, 255);
            //costTMP.color = new Color32(88, 255, 88, 255);
            image.sprite = green;
        }
        if (item.color == 'B')
        {
            //nameTMP.color = new Color32(88, 88, 255, 255);
            //costTMP.color = new Color32(88, 88, 255, 255);
            image.sprite = blue;
        }
    }
}
