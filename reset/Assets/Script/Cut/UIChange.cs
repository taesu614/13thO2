using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIChange : MonoBehaviour
{
    public Sprite[] images; // �̹��� �迭
    private SpriteRenderer spriteRenderer;
    private int imageIndex = 0;

    public float[] imageTransitionTimes = { 3.0f, 24.0f, 3.0f};

    public string[] texts = { "���� ��~�� ������...��������� ��� ��� ������...\n�׵��� ����ϴ� ���̰� �־����.",
        "",
        "��������� �� ����� �������ر������ �����߾��"};
    private TextMeshProUGUI textMeshPro;
    private int textIndex = 0;

    public float[] textTransitionTimes = { 3.0f, 24.0f, 3.0f };

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        Invoke("SwitchImage", 0.0f); // �ʱ� �̹��� ��ȯ ȣ��
        Invoke("SwitchText", 0.0f); // �ʱ� �ؽ�Ʈ ��ȯ ȣ��
    }

    void SwitchImage()
    {
        if (imageIndex < images.Length)
        {
            spriteRenderer.sprite = images[imageIndex];
            Invoke("SwitchImage", imageTransitionTimes[imageIndex]); // ���� �̹����� �ش��ϴ� ��ȯ �ð����� ���� �̹��� ��ȯ ����
            imageIndex++;
        }
        else
        {
            // �̹����� ��� ������ �� ���ϴ� ���� ����
            Debug.Log("��� �̹����� ǥ���߽��ϴ�.");
        }
    }

    void SwitchText()
    {
        if (textMeshPro != null)
        {
            if (textIndex < texts.Length)
            {
                textMeshPro.text = texts[textIndex];
                Invoke("SwitchText", textTransitionTimes[textIndex]); // ���� �ؽ�Ʈ�� �ش��ϴ� ��ȯ �ð����� ���� �ؽ�Ʈ ��ȯ ����
                textIndex++;
                if(textIndex == 3)
                    AudioManager.instance.PlayBGM(AudioManager.BGM.battle);
            }
            else
            {
                Debug.Log("��� �ؽ�Ʈ�� ǥ���߽��ϴ�.");
            }
        }
        else
        {
            Debug.Log("textMeshPro is not assigned.");
        }
    }
}