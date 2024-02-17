using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutChange : MonoBehaviour
{
    public Sprite[] images; // 이미지 배열
    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;

    // 각 이미지 간의 전환 시간을 설정
    public float[] transitionTimes = {3.0f, 3.0f, 3.0f, 9.0f, 3.0f, 6.0f, 3.0f };

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("SwitchImage", 0.0f); // 초기 이미지 전환 호출
    }

    void SwitchImage()
    {
        if (currentIndex < images.Length)
        {
            spriteRenderer.sprite = images[currentIndex];
            Invoke("SwitchImage", transitionTimes[currentIndex]); // 현재 이미지에 해당하는 전환 시간으로 다음 이미지 전환 예약
            currentIndex++;
        }
        else
        {
            // 이미지를 모두 보여준 후 원하는 동작 수행
            Debug.Log("모든 이미지를 표시했습니다.");
        }
    }
}