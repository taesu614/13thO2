using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostManager : MonoBehaviour
{
    [SerializeField] TMP_Text costTMP;  //계산에 쓰일 것이므로 num = int.Parse(costTMP); 해둘것
    [SerializeField] TMP_Text rcostTMP;
    [SerializeField] TMP_Text gcostTMP;
    [SerializeField] TMP_Text bcostTMP;
    public static CostManager Inst { get; private set; }
    public GameObject playermaskprafab;
    public Transform playerposition;
    public GameObject player;

    void Awake() => Inst = this;
    private int mycost = 0;
    private int hasmycost;
    private bool can;
    int rcost = 0;
    int gcost = 0;
    int bcost = 0;

    public void ShowCost()
    {
        costTMP.text = hasmycost.ToString();
        rcostTMP.text = rcost.ToString();
        gcostTMP.text = gcost.ToString();
        bcostTMP.text = bcost.ToString();
    }

    public void CostSet()  //코스트 늘려주기
    {
        if(mycost < 5)
            mycost++;

        hasmycost = mycost;
    }

    public void CostSetNewCost(int cost)
    {
        hasmycost = cost;
        ShowCost();
    }

    public bool CompareCost(Card card)  //코스트 비교
    {
        if (hasmycost < card.item.cost)
        {
            can = false;
        }
        else
        {
            can = true;
        }

        return can;
    }

    public void SubtractCost(Card card)
    {
        hasmycost -= card.item.cost;
        if(card.item.color == 'R')
        {
            rcost++;
        }
        else if(card.item.color == 'G')
        {
            gcost++;
        }
        else if(card.item.color == 'B')
        {
            bcost++;
        }
    }
    public void GetMyStarMask(string name)
    {
        Entity playerentityscript = player.GetComponent<Entity>();
        switch (name)
        {
            case "sheep":
                if(CompareRGB(name, rcost, gcost, bcost))
                {
                    SpawnMask(name);
                    playerentityscript.MakeAttackUp(3, 9999);
                    playerentityscript.MakeShield(5, 3);
                }
                break;
            case "bull":
                if (CompareRGB(name, rcost, gcost, bcost))
                {
                    SpawnMask(name);
                }
                break;
            default:
                Debug.Log("fail");
                break;
        }
    }
    private bool CompareRGB(string name, int r, int g, int b)   //RGB 코스트 비교용 메서드
    {
        switch(name)    //이름에 따라서 스위치문 발동 - 직관성 향상 및 속도 향상을 위해 if 대신 스위치문 사용
        {
            case "sheep":
                if (r >= 5 && g >= 2 && b >= 3)
                {
                    return true;
                }
                break;
            case "bull":
                return true;
        }
        return false;
    }

    private void SpawnMask(string name)
    {
        Vector3 spawnposition = new Vector3(playerposition.position.x + 0.25f, playerposition.position.y + 0.25f, playerposition.position.z);    //플레이어 위치를 기준 0.25f 0.25f에 생성하기 위함
        GameObject mask = Instantiate(playermaskprafab, spawnposition, Quaternion.identity);    //프리팹 생성 기본 기능
        Mask mymask = playermaskprafab.GetComponent<Mask>();    //프리팹에서 Mask스크립트를 가져와서 
        mymask.ChangeStarMaskImage(name);                       //이미지를 변경하기 위함
        mask.transform.SetParent(playerposition);
    }
}
