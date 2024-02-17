using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIChange : MonoBehaviour
{
    public Sprite[] images; // 이미지 배열
    private SpriteRenderer spriteRenderer;
    private int imageIndex = 0;

    public float[] imageTransitionTimes = { 3.0f, 24.0f, 3.0f};

    public string[] texts = { "옛날 아~주 옛날에...마법사들이 살던 어느 마을에...\n그들이 즐겨하는 놀이가 있었어요.",
        "",
        "마법사들은 이 모습을 ‘설정붕괴’라고 정의했어요"};
    private TextMeshProUGUI textMeshPro;
    private int textIndex = 0;

    public float[] textTransitionTimes = { 3.0f, 24.0f, 3.0f };

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        Invoke("SwitchImage", 0.0f); // 초기 이미지 전환 호출
        Invoke("SwitchText", 0.0f); // 초기 텍스트 전환 호출
    }

    void SwitchImage()
    {
        if (imageIndex < images.Length)
        {
            spriteRenderer.sprite = images[imageIndex];
            Invoke("SwitchImage", imageTransitionTimes[imageIndex]); // 현재 이미지에 해당하는 전환 시간으로 다음 이미지 전환 예약
            imageIndex++;
        }
        else
        {
            // 이미지를 모두 보여준 후 원하는 동작 수행
            Debug.Log("모든 이미지를 표시했습니다.");
        }
    }

    void SwitchText()
    {
        if (textMeshPro != null)
        {
            if (textIndex < texts.Length)
            {
                textMeshPro.text = texts[textIndex];
                Invoke("SwitchText", textTransitionTimes[textIndex]); // 현재 텍스트에 해당하는 전환 시간으로 다음 텍스트 전환 예약
                textIndex++;
                if(textIndex == 3)
                    AudioManager.instance.PlayBGM(AudioManager.BGM.battle);
            }
            else
            {
                Debug.Log("모든 텍스트를 표시했습니다.");
            }
        }
        else
        {
            Debug.Log("textMeshPro is not assigned.");
        }
    }
}