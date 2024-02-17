using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextChange : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private int currentIndex = 0;

    public float[] transitionTimes = {
        3.5f,
        3.5f,
        3.5f,
        3.5f,3.5f,3.5f,
        3.5f,
        3.5f, 3.5f,
        3.5f
    };

    public string[] texts = {
        "",
        "그것은 마법을 사용한 미리내 연극...!\n인형들에게 생명을 주어 그들의 모습을 관찰하는 놀이였죠.",
        "마법의 힘으로 생명을 받은 인형들은\n연극의 흐름에 맞춰 말하고 행동할 수 있었어요.",

        "인형들은 마법사의 의지에 따라 서로를 다치게하고...",
        "때로는...죽이기도 했죠.",
        "그런데... 흐음",

        "인형들의 반란일까요?\n아니면...  마법에 문제라도 생긴걸까요??",

        "어느날 이상한 일이 일어났어요!",
        "다른 연극에서 인형이 넘어오기도 하고,\n서로 다른 무대의 배경이 합쳐지는 이상한 일이 일어나기도 했죠.",
        ""
    };

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        Invoke("ShowText", 0.0f); // 초기 텍스트 표시 호출
    }

    void ShowText()
    {
        if (currentIndex < texts.Length)
        {
            textMeshPro.text = texts[currentIndex];
            Invoke("ShowText", transitionTimes[currentIndex]); // 현재 텍스트에 해당하는 전환 시간으로 다음 텍스트 표시 예약
            currentIndex++;
        }
        else
        {
            // 텍스트를 모두 표시한 후 원하는 동작 수행
            Debug.Log("모든 텍스트를 표시했습니다.");
            Invoke("LoadNextScene",0.5f);
            Debug.Log("씬 전환 호출");
        }
    }

    void LoadNextScene()
    {
        // 3초 뒤에 다음 씬으로 전환
        SceneManager.LoadScene("CutScene2");
    }
}