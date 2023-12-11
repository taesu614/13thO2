using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CostManager : MonoBehaviour
{
    [SerializeField] TMP_Text costTMP;  //계산에 쓰일 것이므로 num = int.Parse(costTMP); 해둘것
    [SerializeField] TMP_Text rcostTMP;
    [SerializeField] TMP_Text gcostTMP;
    [SerializeField] TMP_Text bcostTMP;
    [SerializeField] Image costimg;

    public GameObject rgbprefab;
    public GameObject RBottle;
    public GameObject GBottle;
    public GameObject BBottle;
    public Sprite[] img; 
    public Sprite[] rgbimg;

    List<GameObject> RPrefabList = new List<GameObject>();
    List<GameObject> GPrefabList = new List<GameObject>();
    List<GameObject> BPrefabList = new List<GameObject>();
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
        SetCostSprite(hasmycost);
    }

    public void CostSet()  //코스트 늘려주기
    {
        if(mycost < 5)
            mycost++;

        hasmycost = mycost;
    }

    public void CostSetNewCost(int cost)    //코스트를 원하는 코스트(int cost)로 설정
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
            if (rcost >= 5)
                return;
            SetRGBSprite('R');
            rcost++;
        }
        else if(card.item.color == 'G')
        {
            if (gcost >= 5)
                return;
            SetRGBSprite('G');
            gcost++;
        }
        else if(card.item.color == 'B')
        {
            if (bcost >= 5)
                return;
            SetRGBSprite('B');
            bcost++;
        }
    }
    public void GetMyStarMask(string name)
    {
        Entity playerentityscript = player.GetComponent<Entity>();
        if(playerentityscript.hasmask)
        {
            Debug.Log("You have mask");
        }
        else
        {
            switch (name)
            {
                case "sheep":
                    if (CompareRGB(name, rcost, gcost, bcost))
                    {
                        SpawnMask(name);
                        playerentityscript.MakeAttackUp(3, 9999);
                        playerentityscript.MakeShield(5, 3);
                        playerentityscript.MakeImmuneSleep(3);
                        playerentityscript.hasmask = true;
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
    }
    private bool CompareRGB(string name, int r, int g, int b)   //RGB 코스트 비교용 메서드
    {
        switch(name)    //이름에 따라서 스위치문 발동 - 직관성 향상 및 속도 향상을 위해 if 대신 스위치문 사용
        {
            case "Sheep":
                if (r >= 5 && g >= 2 && b >= 3)
                {
                    rcost = rcost - 5;
                    for(int i = 0; i < 5; i++)
                    {
                        GameObject prefabToRemove = RPrefabList[RPrefabList.Count - 1];
                        RPrefabList.RemoveAt(RPrefabList.Count - 1);
                        Destroy(prefabToRemove);
                    }
                    gcost = gcost - 2;
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject prefabToRemove = GPrefabList[GPrefabList.Count - 1];
                        GPrefabList.RemoveAt(GPrefabList.Count - 1);
                        Destroy(prefabToRemove);
                    }
                    bcost = bcost - 3;
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject prefabToRemove = BPrefabList[BPrefabList.Count - 1];
                        BPrefabList.RemoveAt(BPrefabList.Count - 1);
                        Destroy(prefabToRemove);
                    }
                    ShowCost();
                    return true;
                }
                break;
            case "Bull":
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

    void SetCostSprite(int num)
    {
        switch(num)
        {
            case 0:
                costimg.sprite = img[0];
                break;
            case 1:
                costimg.sprite = img[1];
                break;
            case 2:
                costimg.sprite = img[2];
                break;
            case 3:
                costimg.sprite = img[3];
                break;
            case 4:
                costimg.sprite = img[4];
                break;
            case 5:
                costimg.sprite = img[5];
                break;
            case 6:
                costimg.sprite = img[6];
                break;
            case 7:
                costimg.sprite = img[7];
                break;
            case 8:
                costimg.sprite = img[8];
                break;
            case 9:
                costimg.sprite = img[9];
                break;
            case 10:
                costimg.sprite = img[10];
                break;
        }
    }

    public void SetRGBSprite(char rgb)
    {
        switch(rgb)
        {
            case 'R':
                GameObject rcost = Instantiate(rgbprefab, RBottle.transform);
                Image rspriterenderer = rcost.GetComponent<Image>();
                rspriterenderer.sprite = rgbimg[0];
                RPrefabList.Add(rcost);
                break;
            case 'G':
                GameObject gcost = Instantiate(rgbprefab, GBottle.transform);
                Image gspriterenderer = gcost.GetComponent<Image>();
                gspriterenderer.sprite = rgbimg[1];
                GPrefabList.Add(gcost);
                break;
            case 'B':
                GameObject bcost = Instantiate(rgbprefab, BBottle.transform);
                Image bspriterenderer = bcost.GetComponent<Image>();
                bspriterenderer.sprite = rgbimg[2];
                BPrefabList.Add(bcost);
                break;
        }
    }
}
