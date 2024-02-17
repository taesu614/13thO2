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
        "�������� ��� ���̾߱Ⱑ ��վ����ڳ�.����� �����ϼ���?",
        "�¾ƿ�. �츮�� ������ ����� �̾߱��\n�̸��� ������ �����ر��κ��� �����ؿ�!",
        "�������! �������� 13���� ���ڸ��� �ʴ��ҰԿ�.\nũ��..�ű�! ������ �����ϴ� ���� �ڵ��� �˶��� ���ֽð�..",
        "�� �����ϴ� ������ ���ΰ��鿡�� �Ƴ����� �ڼ��� ��Ź�帳�ϴ�.^��^",
        "�׷�.. ����� �̸�!",
        ""
    };

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        Invoke("ShowText", 0.0f); // �ʱ� �ؽ�Ʈ ǥ�� ȣ��
    }

    void ShowText()
    {
        if (currentIndex < texts.Length)
        {
            textMeshPro.text = texts[currentIndex];
            Invoke("ShowText", transitionTimes[currentIndex]); // ���� �ؽ�Ʈ�� �ش��ϴ� ��ȯ �ð����� ���� �ؽ�Ʈ ǥ�� ����
            currentIndex++;
        }
        else
        {
            // �ؽ�Ʈ�� ��� ǥ���� �� ���ϴ� ���� ����
            Debug.Log("��� �ؽ�Ʈ�� ǥ���߽��ϴ�.");
            Invoke("LoadNextScene", 0.0f);
        }
    }

    void LoadNextScene()
    {
        // 3�� �ڿ� ���� ������ ��ȯ
        SceneManager.LoadScene("LobiScene");
    }
}