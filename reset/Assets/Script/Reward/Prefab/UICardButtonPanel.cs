using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICardButtonPanel : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���ϴ� ���� ����
        gameObject.SetActive(false); // �Ǵ� �ٸ� ���� ����
    }
}
