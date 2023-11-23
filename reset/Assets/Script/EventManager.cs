using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour   //옵저버 역할
{
    private int MonsterCount = 0;
    private int PlayerCount = 0;    //임시
    //Enable() : Start()와 비슷한 용도나 활성화 될 때마다 호출 됨
    void OnEnable()
    {
        EntityManager.EventEntitySpawn += CheckMonster;
        EntityManager.EventEntityDestroy += DestroyMonster;
        // 여기에 초기화 코드나 다른 작업을 추가할 수 있습니다.
    }

    //Onenable()과 짝꿍 
    void OnDisable()    
    {
        EntityManager.EventEntitySpawn -= CheckMonster;
        EntityManager.EventEntityDestroy -= DestroyMonster;
    }

    void CheckMonster() //몬스터 수 확인용
    {
        MonsterCount++;
        Debug.Log("몬스터 생성");
        Debug.Log(MonsterCount);
    }

    void DestroyMonster()
    {
        MonsterCount--;
        Debug.Log("몬스터 파괴");
        if (MonsterCount  <= 0)
        {
            Debug.Log("승리");
        }
    }
}
