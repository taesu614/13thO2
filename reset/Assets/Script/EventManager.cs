using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour   //옵저버 역할
{
    //이벤트 기반 아키텍처 관련 다시 공부하고 제대로 수정할 것
    GameObject[] monster;
    private int MonsterCount = 0;
    private int PlayerCount = 0;    //임시
    //Enable() : Start()와 비슷한 용도나 활성화 될 때마다 호출 됨

    private void Start()
    {
        
    }
    void OnEnable()
    {
        // 여기에 초기화 코드나 다른 작업을 추가할 수 있습니다.
    }

    //Onenable()과 짝꿍 
    void OnDisable()    
    {
    }

    void CheckMonster() //몬스터 수 확인용
    {
        MonsterCount++;
    }

    void DestroyMonster()
    {
       
    }
}
