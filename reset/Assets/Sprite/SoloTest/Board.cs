using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnMouseDown()
    {
        Debug.Log("12345");
        BoardManager.Inst.EntityMouseDown(this);
    }

    private void OnMouseUp()
    {
        EntityManager.Inst.EntityMouseUp();
    }

    private void OnMouseDrag()
    {
        EntityManager.Inst.EntityMouseDrag();
    }

    public bool Select(int damage)
    {
        return true;
    }
}
