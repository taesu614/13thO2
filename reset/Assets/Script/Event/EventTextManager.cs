using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventTextManager : MonoBehaviour
{
    public TMP_Text Title;
    public TMP_Text texts; //이벤트 내용이 들어갈 곳
    public TMP_Text selectionExplainText; //선택지 설명 들어갈 곳
    public GameObject selections;
    string selectedButton;
    SaveData savedata;

    int playerHp;
    int playerMaxHp;

    int textFlow = 0; //텍스트 순서 표시

    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        ChangeText();
        //저장되어있는 HP 와 MaxHP 가져옴
        playerHp = 0;
        playerMaxHp = 0;
        AudioManager.instance.PlayBGM(AudioManager.BGM.riddle);
    }

    void EventClose()
    { //이벤트를 끝내면서 바뀐 정보를 새롭게 등록
        savedata.SetPlayerHealth(savedata.GetPlayerHealth() + playerHp);
        savedata.SetPlayerMaxHealth(savedata.GetPlayerMaxHealth() + playerMaxHp);
        SceneManager.LoadScene("MapScene");
    }

    public void Select()
    { //버튼 선택
        //현재 눌렀던 버튼의 name을 가져옴
        string clickObjName = EventSystem.current.currentSelectedGameObject.gameObject.name;

        print("버튼 클릭 확인용: " + clickObjName);

        if (selectedButton == clickObjName)//확정적인 선택 이미 선택했던 버튼을 한 번더 눌렀을 경우 실행 
        {
            //지금 선택 하면 바로 적용되고 Map으로 넘어가는데 나중에 
            //결과 UI 창에 띄우고 넘어 갈 수 있도록 변경해야함
            switch (selectedButton)
            {
                case "Blue":
                    Blue();//메서드 실행
                    break;
                case "Red":
                    Red();//메서드 실행
                    break;
                case "None":
                    None();//메서드 실행
                    break;
            }
            return;
        }
        switch (clickObjName) //선택한 항목에 따른 설명 보여주기
        {
            case "Blue":
                selectionExplainText.text = selectionsExplains[0];
                AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // 임시
                break;
            case "Red":
                selectionExplainText.text = selectionsExplains[1];
                AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // 임시
                break;
            case "None":
                selectionExplainText.text = selectionsExplains[2];
                AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // 임시
                break;
        }
        selectedButton = clickObjName; //선택한 버튼 저장

    }
    #region
    //이벤트 버튼 설명 string 
    string[] selectionsExplains =
    {
        "체력 8 회복,\n 최대 체력은 넘지않는다.",
        "50% 확률로 최대 체력 10증가, \n또는 50%확률로 현재 체력 10감소",
        "수상한 음식은 주워먹는게 아니랬어! 학자의 말을 믿자.\n\n변화가 없다."
    };
    void Blue()
    {
        //체력 8회복 최대체력 안 넘게
        playerHp += 8;
        print("체력을 회복했다! 현재체력: " + playerHp.ToString());
        EventClose();

    }
    void Red()
    {
        //50/50으로 최대체력 10증가or 현재체력 10감소, 게임 오버 가능
        //게임 오버 기능 안 넣어서 나중에 추가해야함 
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
        {
            playerMaxHp += 10;
            print("최대체력 증가! 최대 체력:" + playerMaxHp.ToString());
        }
        else
        {
            playerHp -= 10;
            print("체력 감소 현재체력:" + playerHp.ToString());
        }
        EventClose();

    }
    void None()
    {
        print("아무일도 일어나지 않았다.");
        EventClose();
        //아무 일 없음
    }
    #endregion//이벤트 선택지 메서드
    public void ChangeText()
    {
        if (textFlow == event1.Length - 1)
        {
            texts.text = event1[textFlow];
            texts.gameObject.SetActive(false);
            selections.gameObject.SetActive(true);
            //씬 변환
        }
        else
        {

            texts.text = event1[textFlow];
            textFlow++;
        }

    }

    //이벤트 스크립트 인데 나중에 scv 파일을 읽도록 바꾸는편이 장기적으로 봤을 때 더 편할 수도 있습니다.
    string[] event1 = {
        "-수상한 나무-",
        "스텔라가 위그로브의 나뭇가지 위를 걷고 있던 중\n" +
            "그녀 앞에는 마치 굳어버린 리브르오의 형상을 한 나무가 나타났다.\n\n" +
            "겁에 질린듯한 표정. 무언가를 피하려는 몸짓...\n\n 오래되고 낡은 이 나무는" +
            "과거 위그로브에 있었던 \n전쟁의 참혹함을 보여주는 듯하다.",
        "지나가던 리브르오 학자가 스텔라에게 말을 건넨다.\n\n",
            "“그건 오랜 옛날에 적들과 전쟁을 할 때 생긴 나무라고 하네요.\n" +
            "마지막까지 조국을 위해 자리를 지키다가 결국…\n\n" +
            "흑흑.. 위그로브의 일부가 된 우리의 자랑스런 선조님이십니다”",
        "“ 가지에 있는 열매는 드시지 않는게 좋을 거에요.. 먹다가 배탈나서 세상을 뜬 친구도 있거든요.”",
        "이 말을 마치고 학자는 자리를 벗어난다.",
        "이 나무에는 커다란 열매가 열려있다.\n\n" +
            "하나는 색이 푸른 열매고 또 다른 하나는 붉은 열매다.\n" +
            "스텔라는 마침 목이 말라 이 열매를 먹을까 고민하던 참이다.",
        "a"
    };


}
