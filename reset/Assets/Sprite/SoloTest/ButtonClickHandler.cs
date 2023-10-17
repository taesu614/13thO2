using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    // 메시지를 표시할 텍스트 UI
    public Text messageText;

    // 버튼 클릭 시 호출되는 메서드
    public void OnButtonClick()
    {
        messageText.text = "버튼이 클릭되었습니다!";
        Debug.Log("test");
    }
}
