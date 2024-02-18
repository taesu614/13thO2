using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardNamePanel : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 원하는 동작 수행
        gameObject.SetActive(false); // 또는 다른 동작 수행
    }
}
