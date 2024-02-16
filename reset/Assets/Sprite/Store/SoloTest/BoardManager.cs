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

        // selectBoard, targetPickEntity �� �� �����ϸ� �����Ѵ�. �ٷ� null, null�� �����.
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


        //Other Ÿ�� ��ƼƼ ã��
        bool existTarget = false;
        foreach (var hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {
            Board board = hit.collider?.GetComponent<Board>();
            if (board != null && selectBoard.selectable) //���� ���� �̰� Ȱ���ϸ� ���� ���õ� �����ҵ�
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
        //_attacker�� _defende �� ��ġ�� �̵��ϴ� ���� ��ġ�� �´�, �̶� order�� ����
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
