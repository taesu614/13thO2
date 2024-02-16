using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour   //옵저버 역할
{
    [SerializeField] GameObject PlayerWin;
    private int MonsterCount = 0;
    private int PlayerCount = 0;    //임시
    //Enable() : Start()와 비슷한 용도나 활성화 될 때마다 호출 됨

    private void Start()
    {
        PlayerWin.SetActive(false);
    }
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
    }

    void DestroyMonster()
    {
        MonsterCount--;
        if (MonsterCount <= 0)
        {
            PlayerWin.SetActive(true);
            StartCoroutine(WaitForSecondsExample());
        }
    }

    IEnumerator WaitForSecondsExample()
    {
        yield return new WaitForSeconds(3.0f);

        //게임 종료 후 데이터 저장 - 늘어날 코드 분량 생각해서 정리할것
        GameObject savedata = GameObject.Find("SaveData");
        SaveData playerdata = savedata.GetComponent<SaveData>();
        GameObject player = GameObject.Find("MyPlayer");
        Entity playernow = player.GetComponent<Entity>();

        int moneyNow = playerdata.GetPlayerMoney();
        int plusmoney = moneyNow + 10;  // 일단 임시로 몬스터 죽이면 고정으로 10원 추가하는걸로 했습니다.

        playerdata.SetPlayerHealth(playernow.health);
        playerdata.SetPlayerMoney(plusmoney);
        SceneManager.LoadScene("RewardScene");
    }
}
