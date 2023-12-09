using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDeckButton : MonoBehaviour
{
    public Item item;
    [SerializeField] Image coloruiimage;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    SaveData savedata;
    // Start is called before the first frame update
    void Start()
    {
        GameObject save = GameObject.Find("SaveData");
        savedata = save.transform.GetComponent<SaveData>();
    }

    public void Setup(Item item)
    {
        nameTMP.text = item.name;
        costTMP.text = item.cost.ToString();

        if (item.color == 'R')  //여기 숫자 수정하면 글씨 색 바뀜
        {
            nameTMP.color = new Color32(255, 88, 88, 255);
            costTMP.color = new Color32(255, 88, 88, 255);
        }
        else if (item.color == 'G')
        {
            nameTMP.color = new Color32(88, 255, 88, 255);
            costTMP.color = new Color32(88, 255, 88, 255);
        }
        if (item.color == 'B')
        {
            nameTMP.color = new Color32(88, 88, 255, 255);
            costTMP.color = new Color32(88, 88, 255, 255);
        }
    }
}
