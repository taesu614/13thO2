using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    // �޽����� ǥ���� �ؽ�Ʈ UI
    public Text messageText;

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnButtonClick()
    {
        messageText.text = "��ư�� Ŭ���Ǿ����ϴ�!";
        Debug.Log("test");
    }
}
