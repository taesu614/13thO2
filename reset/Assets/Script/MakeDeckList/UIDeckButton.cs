using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDeckButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item itemname;
    [SerializeField] Image coloruiimage;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] TMP_Text cardsTMP;  // ī�� ��� ���ϴ°�
    [SerializeField] TMP_Text activeTMP;
    [SerializeField] Image image;
    [SerializeField] Sprite blue;
    [SerializeField] Sprite red;
    [SerializeField] Sprite green;
    [SerializeField] GameObject panel;
    public int identifier;
    public int cards = 0;  // ī�� ���
    string a;
    SaveData savedata;
    DeckUIManager deckuimanager;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        GameObject save = GameObject.Find("SaveData");
        savedata = save.transform.GetComponent<SaveData>();
        GameObject deckui = GameObject.Find("DeckUIManager");
        deckuimanager = deckui.GetComponent<DeckUIManager>();
    }

    public void Setup(Item item)
    {
        this.itemname = item;
        nameTMP.text = item.name;
        costTMP.text = item.cost.ToString();
        activeTMP.text = item.active;
        // cards++;
        // cardsTMP.text = cards.ToString();
        identifier = item.GetID();

        if (item.color == 'R')  //���� ���� �����ϸ� �۾� �� �ٲ�
        {
            ///nameTMP.color = new Color32(255, 88, 88, 255);
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

    public void AddCards()  // ī�� ��� ����
    {
        cards++;
    }

    public void Remove()
    {
        Debug.Log(itemname.name);
        deckuimanager.RemoveCard(itemname);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("12313123");
        panel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
