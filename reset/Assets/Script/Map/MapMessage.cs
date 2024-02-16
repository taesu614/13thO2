using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MessageUIOpen(false);
    }

    public void MessageUIOpen(bool open)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(open);
        }
    }
}
