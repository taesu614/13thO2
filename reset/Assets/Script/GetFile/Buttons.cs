using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{

    private void Start()
    {
        ExitPast();
    }
    public void ExitPast()
    {
        gameObject.SetActive(false);
    }
    public void OpenPast()
    {
        gameObject.SetActive(true);
    }
}
