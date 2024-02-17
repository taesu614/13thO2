using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] Sprite targetPickerimg;

    public bool myTurn;
    public bool isBossOrEmpty;
    public bool selectable;
    public Vector3 originPos;


    void OnTurnStarted(bool myTurn)
    {
        if (isBossOrEmpty)
            return;
    }

    public void OnButtonClick()
    {
        Debug.Log("12345");
        BoardManager.Inst.EntityMouseDown(this);
    }


    private void OnMouseUp()
    {
        
    }

    private void OnMouseDrag()
    {
       
    }

    public bool Select(int damage)
    {
        return true;
    }
}
