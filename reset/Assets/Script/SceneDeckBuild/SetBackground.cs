using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SetBackground : MonoBehaviour
{
    GameObject deckui;
    DeckUIManager deckuimanager;
    bool isclick = false;
    GameObject background;
    Order order;
    public CanvasConstellation canvasconstellation;
    TMP_Text constellatext;
    SaveData savedata;
    // Start is called before the first frame update
    void Start()
    {
        deckui = GameObject.Find("DeckUIManager");
        deckuimanager = deckui.GetComponent<DeckUIManager>();
        background = GameObject.Find("Background");
        order = background.GetComponent<Order>();
        gameObject.SetActive(true); //ó�� �ش� ������Ʈ�� Ȱ��ȭ
        order.SetOrder(1);
        constellatext = GameObject.Find("ConstellationText").GetComponent<TMP_Text>();
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        SetSaid();
    }

    private void Update()   //����� ���� �ʾ� �������� �޸� Update ���
    {
        if (!isclick && Input.GetMouseButtonDown(0) && !deckuimanager.IsUIOpen())
        {
            isclick = true;
        }
        else if (isclick && Input.GetMouseButtonUp(0) && !deckuimanager.IsUIOpen())
        {
            isclick = false;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("ButtonNotInUI"));

            if (hit.collider != null && hit.collider.CompareTag("Button1")) //�߾� �ſ�
            {
                // ButtonNotInUI ���̾ ����� Collider�� �浹�ϰ�, Button1 �±׸� ���� ���� ������Ʈ�� Ŭ���Ǿ��� �� ������ �ڵ�
                BackgroundActive(false);
                canvasconstellation.OpenUI();
            }
            if (hit.collider != null && hit.collider.CompareTag("Button2")) //���� �ϴ� ��ư
            {
                // ButtonNotInUI ���̾ ����� Collider�� �浹�ϰ�, Button2 �±׸� ���� ���� ������Ʈ�� Ŭ���Ǿ��� �� ������ �ڵ�
                deckuimanager.OpenUI();
            }
            if (hit.collider != null && hit.collider.CompareTag("Button3")) //���� ��� ȭ��ǥ
            {
                SceneManager.LoadScene("LobiScene");
            }
        }
    }

    // Update is called once per frame
    public void BackgroundActive(bool isopen)
    {
        gameObject.SetActive(isopen);
    }

    public void SetSaid()
    {
        string name = savedata.GetPlayerConstellation();
        string speech;
        switch(name)
        {
            case "Sheep":
                speech = "���� �Ѹ��� ���� �θ��� ���� ������...Zzz..";
                break;
            case "Goat":
                speech = "���Ϲ���, �����ĺ�, GOAT. ����մϴ�, GOAT. �����մϴ�, GOAT. �����մϴ�, GOAT. �����ϼ̽��ϴ�, GOAT. �� ����... GOAT ����� ��Ű� ������ �Ǵٽ� ���� �¸��� �̲������ϴ�.";
                break;
            case "Sagittarius":
                speech = "ĸƾ�� ������ �ȵſ�";
                break;
            default:
                speech = "���� �ڴ� ���� ����.";
                    break;
        }
        constellatext.text = speech;
    }
}