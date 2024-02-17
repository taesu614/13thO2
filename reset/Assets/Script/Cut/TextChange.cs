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
        "�װ��� ������ ����� �̸��� ����...!\n�����鿡�� ������ �־� �׵��� ����� �����ϴ� ���̿���.",
        "������ ������ ������ ���� ��������\n������ �帧�� ���� ���ϰ� �ൿ�� �� �־����.",

        "�������� �������� ������ ���� ���θ� ��ġ���ϰ�...",
        "���δ�...���̱⵵ ����.",
        "�׷���... ����",

        "�������� �ݶ��ϱ��?\n�ƴϸ�...  ������ ������ ����ɱ��??",

        "����� �̻��� ���� �Ͼ���!",
        "�ٸ� ���ؿ��� ������ �Ѿ���⵵ �ϰ�,\n���� �ٸ� ������ ����� �������� �̻��� ���� �Ͼ�⵵ ����.",
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
            Invoke("LoadNextScene",0.5f);
            Debug.Log("�� ��ȯ ȣ��");
        }
    }

    void LoadNextScene()
    {
        // 3�� �ڿ� ���� ������ ��ȯ
        SceneManager.LoadScene("CutScene2");
    }
}