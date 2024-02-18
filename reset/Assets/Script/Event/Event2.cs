using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Event2 : MonoBehaviour
{
    public TMP_Text Title;
    public TMP_Text texts; //이벤트 내용이 들어갈 곳
    public GameObject selections;
    string selectedButton;
    SaveData savedata;

    int playerHp;
    int playerMaxHp;
    int InjectionCount;

    int textFlow = 0; //텍스트 순서 표시

    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        ChangeText();
        //저장되어있는 HP 와 MaxHP 가져옴
        playerHp = 0;
        playerMaxHp = 0;
        InjectionCount = 0;
        AudioManager.instance.PlayBGM(AudioManager.BGM.riddle);
    }

    void EventClose()
    { //이벤트를 끝내면서 바뀐 정보를 새롭게 등록
        savedata.SetPlayerHealth(savedata.GetPlayerHealth() + playerHp);
        savedata.SetPlayerMaxHealth(savedata.GetPlayerMaxHealth() + playerMaxHp);
        SceneManager.LoadScene("MapScene");
    }

    public void Select(string clickname)
    {

        print("버튼 클릭 확인용: " + clickname);

        // 선택한 버튼에 따라 바로 함수 호출
        switch (clickname)
        {
            case "Injection":
                Injection();
                break;
            case "None":
                None();
                break;
        }
        // 선택한 버튼 저장
        selectedButton = clickname;
    }
    #region
    void Injection()
    {
        int rand = UnityEngine.Random.Range(0, 10);
        if (rand < 2) //20% 확률
        {
            playerHp -= 5;
            savedata.SetMessage("체력이 " + playerHp + " 감소하였습니다");
        }
        else if (rand < 5) //30% 확률
        {
            playerHp += 20;
            savedata.SetMessage("체력이 " + playerHp + " 회복되었습니다");
        }
        else if (rand < 9) //40% 확률
        {
            playerHp += 10;
            savedata.SetMessage("체력이 " + playerHp + " 회복되었습니다");
        }
        else //10% 확률
        {
            Venom();
            savedata.SetMessage("맹독 상태(5턴)가 되었습니다");
        }
        EventClose();
    }

    void Venom() 
    {
        StatusEffect statuseffect = new StatusEffect();
        statuseffect.SetStatusEffect("poison", 5);
        savedata.AddStatusEffect(statuseffect);
    }

    void None()
    {
        print("그만둔다.");
        EventClose();
        //아무 일 없음
    }
    #endregion//이벤트 선택지 메서드
    public void ChangeText()
    {
        if (textFlow == event2.Length - 1)
        {
            texts.text = event2[textFlow];
            texts.gameObject.SetActive(false);
            selections.gameObject.SetActive(true);
            //씬 변환
        }
        else
        {

            texts.text = event2[textFlow];
            textFlow++;
        }

    }

    //이벤트 스크립트 인데 나중에 scv 파일을 읽도록 바꾸는편이 장기적으로 봤을 때 더 편할 수도 있습니다.
    string[] event2 = {
        "-버려진 상자-",
        "스텔라는 버려진 상자를 발견한다.\n" + "이 상자는 의약품이 들어있는 상자로 보인다.\n\n" +
        "케이론이 중얼거린다.\n\n" +
        "“ 이 연극에 어울리지도 않는 물건이 있군.\n" + "이것 또한 설정붕괴의 영향인가..”",
        "스텔라는 모험을 계속해온 탓에 몸이 뻐근하다고 느낀다.\n\n" +
        "이 주사를 사용해보는건 어떨까?\n" +
        "어쩌면 이건 상처를 치료할 수 있는 절호의 기회일지도 모른다.",
        "a"
    };


}
