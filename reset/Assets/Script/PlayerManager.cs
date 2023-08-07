using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool encore = false;


    public PlayerManager Inst { get; private set; }    
    
    private void Start()
    {
        SetInstrusion();
    }

    public void Encore()
    {
    }

    private void Getintrusion()
    {
        encore = false;
    }

    private void SetInstrusion()
    {
        encore = false;
    }

    public void SetInstursionEncore()
    {
        encore = true;
    }

}
