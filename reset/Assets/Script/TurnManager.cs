using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;  //���� �� ���� �� ������ ��
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
    [SerializeField] [Tooltip("���� �� ��带 ���մϴ�")] ETurnMode eTurnMode;
    [SerializeField] [Tooltip("ī�� ����� �ſ� �������ϴ�")] bool fastMode;
    [SerializeField] [Tooltip("���� ī�� ������ ���մϴ�")] int startCardCount;

    [Header("Properties")]  //�Ϲ��� ��Ȳ
    public bool isLoading;  //���� ������ isLoading�� true�� �ϸ� ī��� ��ƼƼ Ŭ�� ����
    public bool myTurn;
    public int nowTurn = 0;

    public Entity playerentity;

    enum ETurnMode { Random, My, Other }    //�� �� ���ϴ� �κ� �� �������� ���ɼ����� �������� ����
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted; 
    void GameSetup() //�� �� ���ϴ� �κ� �� �������� ���ɼ����� �������� ����
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

        AudioManager.instance.PlayBGM(AudioManager.BGM.battle);  // BattleScene �����ϸ� BGM ���
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
            GameManager.Inst.Notification("���� ��");
            nowTurn++;
        }
            

        yield return delay07;
        OnAddCard?.Invoke(myTurn);
        OnAddCard?.Invoke(myTurn);
        yield return delay07;
        isLoading = false;
        OnTurnStarted?.Invoke(myTurn);  //���� �� ���� - EntityManager - OnTurnStarted
    }

    public void EndTrun()
    {
        if (myTurn)
        {
            CardManager.Inst.CheckMyCard();
            playerentity.CheckEffect(); //�� ���� ���� �� �������ظ� �ִ� ���� ȿ�� �ߵ� 
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

    public int GetNowTurn()      //SerializeField�� ���� ��ȣ�������� ���� ���� ������ ���
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

    void FaceOnOff() //�Ͽ� ���� ȭ�� �߾� ����� ���� Ű�� �����ϴ� �۾� �ڵ尡 ��ȿ�� ���̶� ���߿� �����ؾ���.
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

    void Achivement(int playerHp, int enemysHp) // ��̻�� ���� �޼��� ǥ�� 
    {
        int totalHp = playerHp + enemysHp;
        float achivePercent = playerHp / totalHp;
        Transform stellaPower = GameObject.Find("Stella Power").transform;
        stellaPower.localScale = new Vector3(achivePercent * 5, 0.7f, 1f);
    }

    public void IsExistEnemy() // Scene���� ���͸� ã�Ƽ� ������ Scene�� �̵��ϴ� ��ũ��Ʈ 
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Monster");
        int enemysHp = 0;
        int playerHp = int.Parse(GameObject.Find("MyPlayer").GetComponentInChildren<TMP_Text>().text);
        for (int i = 0; i < enemys.Length; i++)
        {
            enemysHp += int.Parse(enemys[i].GetComponentInChildren<TMP_Text>().text);
        }
        Achivement(playerHp, enemysHp); //�̿ϼ�
        if (enemys.Length == 0) { print("���� ��!"); } // ���⿡�� print�� ������ �Ŀ� �ٸ� ��� �������� �ٲ� ����
    }
}
