using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMessage : MonoBehaviour
{
    // Start is called before the first frame update

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowCardMessage(bool isdrag)
    {
        gameObject.SetActive(isdrag);
    }
}
