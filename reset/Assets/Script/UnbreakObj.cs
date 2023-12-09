using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakObj : MonoBehaviour
{
    private UnbreakObj instance = null;
    private int check = 0;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance == null) 
        { 
            Destroy(gameObject);
        }
    }
}
