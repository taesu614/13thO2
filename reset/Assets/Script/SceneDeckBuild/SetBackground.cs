using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SetBackground : MonoBehaviour
{   //향후 ConstellationManager와 통합할 것
    GameObject deckui;
    DeckUIManager deckuimanager;
    bool isclick = false;
    GameObject background;
    Order order;
    public ConstellationManager constellationManager;
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
        gameObject.SetActive(true); //처음 해당 오브젝트는 활성화
        order.SetOrder(1);
        constellatext = GameObject.Find("ConstellationText").GetComponent<TMP_Text>();
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        savedata.SetPlayerConstellation("Sheep");
        SetSaid();
    }

    private void Update()   //기능이 많지 않아 전투씬과 달리 Update 사용
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

            if (hit.collider != null && hit.collider.CompareTag("Button1")) //중앙 거울
            {
                // ButtonNotInUI 레이어에 적용된 Collider와 충돌하고, Button1 태그를 가진 게임 오브젝트가 클릭되었을 때 실행할 코드
                BackgroundActive(false);
                canvasconstellation.OpenUI();
            }
            if (hit.collider != null && hit.collider.CompareTag("Button2")) //우측 하단 버튼
            {
                // ButtonNotInUI 레이어에 적용된 Collider와 충돌하고, Button2 태그를 가진 게임 오브젝트가 클릭되었을 때 실행할 코드
                deckuimanager.OpenUI();
            }
            if (hit.collider != null && hit.collider.CompareTag("Button3")) //좌측 상단 화살표
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
        switch (name)
        {
            case "Sheep":
                speech = "바로네츠가 악몽으로부터 위협받지 않도록 꿈지기들로 여왕을 수호하세요!“ 흐아아암..나의 안전은 곧 공주의 안식..병사들이여 나를 지켜..라 zZ ”" + System.Environment.NewLine + System.Environment.NewLine + "RGB 코스트를 각각 3,1,2 소모해 별자리의 힘을 얻을 수 있습니다" + System.Environment.NewLine + "힘을 얻는 동안 공격력이 3 증가합니다";
                BackgroundActive(true);
                break;
            case "Goat":
                speech = "유일무이, 언터쳐블, GOAT. 대단합니다, GOAT. 감사합니다, GOAT. 숭배합니다, GOAT. 수고하셨습니다, GOAT." + System.Environment.NewLine + "아 그저... GOAT 당신의 헌신과 열정이 또다시 팀을 승리로 이끌었습니다." + System.Environment.NewLine + "R0G1B2 보호막 30증가";
                break;
            case "Sagittarius":
                speech = "노루...약해요..." + System.Environment.NewLine + "R3G0B0 공격력 7 증가" + System.Environment.NewLine + "약한 노루답게 녹용 성능은 확실합니다";
                break;
            default:
                speech = "죽은 자는 말이 없다.";
                    break;
        }
        constellatext.text = speech;
    }
}
