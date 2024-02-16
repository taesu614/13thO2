using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;  //랜덤 턴 삭제 시 삭제할 것
using TMPro;
using System.Xml.Schema;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst { get; private set; }
    public CostManager costManager;
    void Awake() => Inst = this;

    private void Start()
    {
        playerentity.SetPastHealth();
    }
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다")] ETurnMode eTurnMode;
    [SerializeField] [Tooltip("카드 배분이 매우 빨라집니다")] bool fastMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

    [Header("Properties")]  //일반적 상황
    public bool isLoading;  //게임 끝나면 isLoading을 true로 하면 카드와 엔티티 클릭 방지
    public bool myTurn;
    public int nowTurn = 0;

    public Entity playerentity;

    enum ETurnMode { Random, My, Other }    //턴 수 정하는 부분 적 선제공격 가능성으로 삭제하지 않음
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted; 
    void GameSetup() //턴 수 정하는 부분 적 선제공격 가능성으로 삭제하지 않음
    {
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);

        switch (eTurnMode)
        {
            case ETurnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(true);
        }
        StartCoroutine(StartTurnCo());

        AudioManager.instance.PlayBGM(AudioManager.BGM.battle);  // BattleScene 시작하면 BGM 재생
    }

    IEnumerator StartTurnCo()
    {
        IsExistEnemy();
        isLoading = true;
        FaceOnOff();
        if (myTurn)
        {
            playerentity.GetAllCC();
            costManager.CostSet();
            costManager.ShowCost();
            GameManager.Inst.Notification("나의 턴");
            nowTurn++;
        }
            

        yield return delay07;
        OnAddCard?.Invoke(myTurn);
        OnAddCard?.Invoke(myTurn);
        yield return delay07;
        isLoading = false;
        OnTurnStarted?.Invoke(myTurn);  //적의 턴 관련 - EntityManager - OnTurnStarted
    }

    public void EndTrun()
    {
        if (myTurn)
        {
            CardManager.Inst.CheckMyCard();
            playerentity.CheckEffect(); //내 턴이 끝날 때 지속피해를 주는 버프 효과 발동 
            playerentity.SetPastHealth();
        }
        else
        {
            GameObject[] entities = GameObject.FindGameObjectsWithTag("Monster");
            foreach(GameObject A in entities)
            {
                Entity monster = A.GetComponent<Entity>();
                monster.CheckEffect();
            }
        }
        myTurn = !myTurn;
        EntityManager.Inst.FindDieEntity();
        //EntityManager.Inst.DamagedReset();
        StartCoroutine(StartTurnCo());
    }

    public int GetNowTurn()      //SerializeField로 인한 보호수준으로 인해 값을 보내는 기능
    {
        return nowTurn;
    }

    public void BuffDebuffCheck()
    {

    }

    public bool IsEndTurn()
    {
        return false;
    }

    void FaceOnOff() //턴에 따라 화면 중앙 상단의 얼굴을 키고 끄고하는 작업 코드가 비효율 적이라 나중에 수정해야함.
    {
        Color off = new Color(128 / 255f, 128 / 255f, 128 / 255f, 255 / 255f);
        Color on = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        if (myTurn)
        {
            GameObject.Find("Stella Face").GetComponent<SpriteRenderer>().color = on;
            GameObject.Find("Enemy Face").GetComponent<SpriteRenderer>().color = off;
        }
        else
        {
            GameObject.Find("Enemy Face").GetComponent<SpriteRenderer>().color = on;
            GameObject.Find("Stella Face").GetComponent<SpriteRenderer>().color = off;
        }
    }

    void Achivement(int playerHp, int enemysHp) // 재미삼아 만들어본 달성도 표시 
    {
        int totalHp = playerHp + enemysHp;
        float achivePercent = playerHp / totalHp;
        Transform stellaPower = GameObject.Find("Stella Power").transform;
        stellaPower.localScale = new Vector3(achivePercent * 5, 0.7f, 1f);
    }

    public void IsExistEnemy() // Scene상의 몬스터를 찾아서 없으면 Scene을 이동하는 스크립트 
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Monster");
        int enemysHp = 0;
        int playerHp = int.Parse(GameObject.Find("MyPlayer").GetComponentInChildren<TMP_Text>().text);
        for (int i = 0; i < enemys.Length; i++)
        {
            enemysHp += int.Parse(enemys[i].GetComponentInChildren<TMP_Text>().text);
        }
        Achivement(playerHp, enemysHp); //미완성
        if (enemys.Length == 0) { print("게임 끝!"); } // 여기에선 print로 했지만 후에 다른 기능 구현으로 바뀔 예정
    }
}
