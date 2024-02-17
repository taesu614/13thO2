using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Cut2Text : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private int currentIndex = 0;

    public float[] transitionTimes = { 2.5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f };

    public string[] texts = {
        "하하하하 방금 ‘이야기가 재밌어지겠네.’라고 생각하셨죠?",
        "맞아요. 우리가 앞으로 들려줄 이야기는\n미리내 연극의 설정붕괴로부터 시작해요!",
        "어서오세요! 여러분을 13월의 별자리에 초대할게요.\n크흠..거기! 연극을 감상하는 동안 핸드폰 알람은 꺼주시고..",
        "곧 등장하는 무대의 주인공들에게 아낌없는 박수도 부탁드립니다.^ㅇ^",
        "그럼.. 저희는 이만!",
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
            Invoke("LoadNextScene", 0.0f);
        }
    }

    void LoadNextScene()
    {
        // 3초 뒤에 다음 씬으로 전환
        SceneManager.LoadScene("LobiScene");
    }
}