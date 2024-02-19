using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICardButtonPanel : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update

    [SerializeField] GameObject realPanel;
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���ϴ� ���� ����
        gameObject.SetActive(false); // �Ǵ� �ٸ� ���� ����
        realPanel.SetActive(false);
    }
}
