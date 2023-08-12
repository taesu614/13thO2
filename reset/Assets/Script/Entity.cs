using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Entity : MonoBehaviour //해당 내용을 통해 별자리 생성 계획 그래서 다른 monsterSO를 만듦
{
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;

    public Monster monster;
    public CardManager cardmanager;
    public int attack;
    public int health = 40;
    public string monsterfunctionname;
    public bool isMine;
    public bool myTurn;
    public bool isDie;
    public bool isBossOrEmpty;
    public bool attackable;
    //상태이상 관련
    public bool debuffPoisonBool;
    public int debuffPosionInt = 0;
    public Vector3 originPos;
    public int liveCount = 0;
    public int poisonCount = 0;

    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;
        monsterPatterns["Snail"] = () => SnailPattern();
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;   
    }

    void OnTurnStarted(bool myTurn)
    {
        if (isBossOrEmpty)
            return;

        if (isMine == myTurn)
        {
            liveCount++;
            BuffDown(1);
        }
            
    }

    public void BuffDown(int count)  //버프 지속시간을 깎는 효과
    {
        poisonCount -= count;
    }
    public void Setup(Monster monster)
    {
        this.monster = monster;
        health = int.Parse(healthTMP.text); //아마 똑같은 monsterSO를 만들어서 몬스터를 관리할듯
        attack = int.Parse(attackTMP.text);
        monsterfunctionname = this.monster.monsterfunctionname;

        this.monster = monster;
        charater.sprite = this.monster.sprite;
        healthTMP.text = this.monster.health.ToString();
        attackTMP.text = this.monster.attack.ToString();
    }

    private void OnMouseDown()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseDown(this);
    }

    private void OnMouseUp()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseUp();
    }

    private void OnMouseDrag()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseDrag();
    }

    public bool Damaged(int damage)
    {
        health -= damage;
        healthTMP.text = health.ToString();
        

        if(health <= 0)
        {
            isDie = true;
            return true;
        }
        return false;
    }

    public int GetHealthTMP()      //SerializeField로 인한 보호수준으로 인해 값을 보내는 기능
    {
        return int.Parse(healthTMP.text);
    }
    public int GetAttackTMP()      //SerializeField로 인한 보호수준으로 인해 값을 보내는 기능
    {
        return int.Parse(attackTMP.text);
    }
    public int GetLiveCount()
    {
        return liveCount;
    }

    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
            transform.DOMove(pos, dotweenTime);
        else
            transform.position = pos;
    }

    #region Buff

    public void DebuffPosion()
    {
        if(poisonCount > 0)    //보여지는 효과도 추가해야할려나...
        {
            Debug.Log("test");
            health--;
            healthTMP.text = health.ToString();
            return;
        }
    }

    #endregion


    #region MonsterPattern

    private void SnailPattern()
    {
        // Player 태그를 가진 모든 게임 오브젝트를 찾습니다.
        Entity playerEntity = GameObject.Find("MyPlayer").GetComponent<Entity>();
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            // 각 Player 게임 오브젝트에서 TMP_Text 컴포넌트를 찾습니다.
            TMP_Text playerhealthTMP = playerObject.GetComponentInChildren<TMP_Text>();
            if (playerhealthTMP != null)
            {
                // 현재 HealthTMP의 값을 가져와서 int로 변환
                int currentHealth = int.Parse(playerhealthTMP.text);

                if (health >= 5 && liveCount > 3)
                {
                    int random = UnityEngine.Random.Range(0, 10);
                    if (random < 5)
                    {
                        playerEntity.health -= 4;
                        currentHealth = playerEntity.health;
                    }
                    else
                    {
                        playerEntity.health -= 8;
                        currentHealth = playerEntity.health;
                    }
                }
                else if (health >= 5)
                {
                    // -5를 하고 값 변경
                    playerEntity.health -= 5;
                    currentHealth = playerEntity.health;
                }
                else if (health < 5)
                {
                    Debug.Log("맹독");
                    playerEntity.poisonCount = 1;
                }

                playerhealthTMP.text = currentHealth.ToString();

            }
            else
            {
                Debug.LogWarning("HealthTMP not found in the Player GameObject.");
            }
        }
    }

    // 몬스터 패턴 실행 메서드
    public void ExecutePattern(string patternName)
    {
        if (monsterPatterns.TryGetValue(patternName, out Action monsterPattern))
        {
            // 해당 카드의 기능 실행
            monsterPattern();
        }
        else
        {
            Debug.Log("MonsterPattern not found.");
        }
    }
    #endregion MonsterPattern
}
