using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutChange : MonoBehaviour
{
    public Sprite[] images; // �̹��� �迭
    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;

    // �� �̹��� ���� ��ȯ �ð��� ����
    public float[] transitionTimes = {3.0f, 3.0f, 3.0f, 9.0f, 3.0f, 6.0f, 3.0f };

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("SwitchImage", 0.0f); // �ʱ� �̹��� ��ȯ ȣ��
    }

    void SwitchImage()
    {
        if (currentIndex < images.Length)
        {
            spriteRenderer.sprite = images[currentIndex];
            Invoke("SwitchImage", transitionTimes[currentIndex]); // ���� �̹����� �ش��ϴ� ��ȯ �ð����� ���� �̹��� ��ȯ ����
            currentIndex++;
        }
        else
        {
            // �̹����� ��� ������ �� ���ϴ� ���� ����
            Debug.Log("��� �̹����� ǥ���߽��ϴ�.");
        }
    }
}