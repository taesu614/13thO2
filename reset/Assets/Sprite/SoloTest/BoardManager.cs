using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
    }
    [SerializeField] GameObject targetPicker;
    bool CanMouseInput => TurnManager.Inst.myTurn && !TurnManager.Inst.isLoading;
    bool ExistTargetPickEntity => targetPickBoard != null;
    Board targetPickBoard;
    Board selectBoard;

    private void Update()
    {
        ShowTargetPicker(ExistTargetPickEntity);
    }

    void ShowTargetPicker(bool isShow, string Color)
    {
        targetPicker.SetActive(isShow);
        if(ExistTargetPickEntity)
        {
            targetPicker.transform.position = targetPickBoard.transform.position;
        }
    }

    public void EntityMouseDown(Board board)
    {
        if (!CanMouseInput)
            return;
        Debug.Log("1");
        selectBoard = board;
    }

    public void EntityMouseUp()
    {
        if (!CanMouseInput)
            return;

        // selectBoard, targetPickEntity 둘 다 존재하면 공격한다. 바로 null, null로 만든다.
        if (selectBoard && targetPickBoard && selectBoard.selectable)
            Select(selectBoard, targetPickBoard);

        selectBoard = null;
        targetPickBoard = null;
    }

    public void EntityMouseDrag()
    {
        if (!CanMouseInput || selectBoard == null)
        {
            Debug.Log("123");
            return;
        }


        //Other 타겟 엔티티 찾기
        bool existTarget = false;
        foreach (var hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {
            Board board = hit.collider?.GetComponent<Board>();
            if (board != null && selectBoard.selectable) //적꺼 선택 이거 활용하면 내꺼 선택도 가능할듯
            {
                targetPickBoard = board;
                existTarget = true;
                break;
            }
        }
        if (!existTarget)
            targetPickBoard = null;
    }

    void Select(Board selector, Board receiver)
    {
        //_attacker가 _defende 의 위치로 이동하다 원래 위치로 온다, 이때 order가 높다
        selector.selectable = false;
        selector.GetComponent<Order>().SetMostFrontOrder(true);
    }

    void ShowTargetPicker(bool isShow)
    {
        targetPicker.SetActive(isShow);
        if (ExistTargetPickEntity)
            targetPicker.transform.position = targetPickBoard.transform.position;
    }

    public void UseableReset(bool isMine)
    {
        Debug.Log("test");
    }
}
