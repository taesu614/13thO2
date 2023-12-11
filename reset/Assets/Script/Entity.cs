using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;  //랜덤 사용을 위함

public class Entity : MonoBehaviour //해당 내용을 통해 별자리 생성 계획 그래서 다른 monsterSO를 만듦
{
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();  //버프 제거 시 선입 선출 방식으로 예상되어 큐로 설정
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text shieldTMP;
    [SerializeField] GameObject hpline;

    Queue<StatusEffect> myStatusEffect = new Queue<StatusEffect>();
    public Monster monster;
    public CardManager cardmanager;
    public int attack;
    public int maxhealth = 40;
    public int health = 40;
    float hppercent;
    public int shield = 0;
    public string monsterfunctionname;
    public bool isMine;
    public bool myTurn;
    public bool isDie;
    public bool isDamaged;
    public bool isBossOrEmpty;
    public bool attackable;
    //상태이상 관련
    public bool debuffPoisonBool;
    public int debuffPosionInt = 0;
    public Vector3 originPos;
    public int liveCount = 0;
    public int poisonCount = 0;
    public bool canplay = true;
    public bool issleep = false;
    public bool hasmask = false;

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
        maxhealth = monster.maxhealth;
        health = int.Parse(healthTMP.text); //아마 똑같은 monsterSO를 만들어서 몬스터를 관리할듯
        attack = int.Parse(attackTMP.text);
        shield = int.Parse(shieldTMP.text);
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
        healthTMP.text = health.ToString();

        if (health <= 0)
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
    public int GetShieldTMP()      //SerializeField로 인한 보호수준으로 인해 값을 보내는 기능
    {
        return int.Parse(shieldTMP.text);
    }

    public void SetHealthTMP()  //체력을 health로 설정
    {
        healthTMP.text = health.ToString();
        hpline.transform.localScale = new Vector3(1 - (float)health/maxhealth, 0.65f, 1f);
    }

    public void SetShieldTMP()
    {
        shieldTMP.text = shield.ToString();
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
            TMP_Text playershieldTMP = playerObject.GetComponentInChildren<TMP_Text>();
            if (playerhealthTMP != null && playershieldTMP != null)
            {
                // 현재 HealthTMP의 값을 가져와서 int로 변환
                int currentHealth = int.Parse(playerhealthTMP.text);
                int currentShield = int.Parse(playershieldTMP.text);
                if (health >= 5 && liveCount > 3)   //체력이 5 이상, 턴수 3 초과
                {
                    int random = UnityEngine.Random.Range(0, 10);
                    if (random < 5)
                    {
                        if(playerEntity.shield > 0)
                        {
                            playerEntity.shield -= 4;
                            currentShield = playerEntity.shield;
                            playerEntity.SetShieldTMP();
                        }
                        else
                        {
                            playerEntity.health -= 4;
                            currentHealth = playerEntity.health;
                        }

                    }
                    else
                    {
                        playerEntity.health -= 8;
                        currentHealth = playerEntity.health;
                    }
                }
                else if (health >= 5)   //체력 5 이상 조건
                {
                    if (playerEntity.shield > 0)
                    {
                        Debug.Log("쉴드 깎는중");
                        playerEntity.shield -= 5;
                        playerEntity.SetShieldTMP();
                    }
                    else
                    {
                        playerEntity.health -= 5;
                        currentHealth = playerEntity.health;
                    }
                }
                else if (health < 5)
                {
                    Debug.Log("맹독");
                    playerEntity.poisonCount = 1;
                }

                playerhealthTMP.text = currentHealth.ToString();
                playerEntity.SetHealthTMP();

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

    #region MakeEffect
    public void MakeAttackUp(int damage, int count)
    {
        Debug.Log("Effect - Attack Up");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetPowerUp(damage, count);
        myStatusEffect.Enqueue(newEffect);
    }

    public void MakeAttackDown(int damage, int count)
    {
        Debug.Log("Effect - Attack Down");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetPowerDown(damage, count);
        myStatusEffect.Enqueue(newEffect);
    }

    public void MakeShield(int amount, int turn)
    {
        Debug.Log("Effect - Shield");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetShield(amount, turn);
        myStatusEffect.Enqueue(newEffect);
        shield += amount;
        SetShieldTMP();
    }

    public void MakeFaint(int turn) //기절 생성
    {
        Debug.Log("Effect - Faint");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetFaint(turn);
        myStatusEffect.Enqueue(newEffect);
    }

    public void MakeSleep(int turn) //수면 생성
    {
        Debug.Log("Effect - Sleep");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetSleep(turn);
        myStatusEffect.Enqueue(newEffect);
        issleep = true;
    }

    public void MakeImmuneSleep(int turn)   //수면 면역 생성
    {
        Debug.Log("Effect - Immnue Sleep");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetImmuneSleep(turn);
        myStatusEffect.Enqueue(newEffect);
    }
    #endregion
    public int GetAllAttackUpEffect()   //공격력 증가 효과 가져오기
    {
        int result = 0;
        foreach(StatusEffect obj in myStatusEffect)
        {
            result += obj.GetAllAttackUp();
        }
        return result;
    }

    public int GetAllAttackDownEffect()   //공격력 감소 효과 가져오기
    {
        int result = 0;
        foreach (StatusEffect obj in myStatusEffect)
        {
            result += obj.GetAllAttackDown();
        }
        return result;
    }

    public void GetAllCC()
    {
        foreach (StatusEffect obj in myStatusEffect)
        {
            if(obj.GetFaint())
            {
                Debug.Log("You are faint");
                canplay = false;
            }
        }
    }

    public bool GetSleep()  //임시용
    {
        int sleep = Random.Range(0, 10);    //0~9의 난수
        foreach (StatusEffect obj in myStatusEffect)
        {
            if (obj.GetImmuneSleep())
            {
                Debug.Log("I can't sleep");
                sleep = 100;
                break;
            }
        }
        Debug.Log(sleep);
        if (sleep < 7)   //0,1,2,3,4,5,6 = 70% = 실패
        {
            Debug.Log("I sleep");
            return true;
        }
        Debug.Log("I don't sleep");
        return false;
    }
}

class StatusEffect
{
    public bool ispowerUp = false;
    public bool ispowerDown = false;
    public bool isshield = false;   //쉴드 존재 여부
    public bool isfaint = false;    //기절 존재 여부
    public bool issleep = false;    //수면 존재 여부
    public bool isimmunesleep = false;
    public int effectamount;    //효과의 양
    public int effectcount;   //횟수
    public int effectturn;    //지속 턴 수
    #region PowerUp
    public void SetPowerUp(int amount, int count)
    {
        effectamount = amount;
        effectcount = count;
        ispowerUp = true;
    }

    public int GetAllAttackUp()
    {
        if (ispowerUp)
        {
            return effectamount;
        }
        return 0;
    }
    #endregion

    #region PowerDown
    public void SetPowerDown(int amount, int count)
    {
        effectamount = amount;
        effectcount = count;
        ispowerDown = true;
    }

    public int GetAllAttackDown()
    {
        if (ispowerDown)
        {
            return effectamount;
        }
        return 0;
    }
    #endregion

    #region Shield
    public void SetShield(int amount, int turn)
    {
        effectamount = amount;
        effectturn = turn;
        isshield = true;
    }
    #endregion

    #region Faint
    public void SetFaint(int turn)  //수면 생성
    {
        effectturn = turn;
        isfaint = true;
    }

    public bool GetFaint()  //해당 위치에서 수면 면역 체크
    {
        if(isfaint)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Sleep
    public void SetSleep(int turn)
    {
        effectturn = turn;
    }

    /*public bool GetSleep()  //사용할 때 canplay를 바로 설정함
    {
        int sleep = Random.Range(0, 10);    //0~9의 난수
        Debug.Log(sleep);
        if(sleep < 7)   //0,1,2,3,4,5,6 = 70% = 실패
        {
            return false;
        }
        return true;
    }*/

    public void SetImmuneSleep(int turn)
    {
        effectturn = turn;
        isimmunesleep = true;
    }

    public bool GetImmuneSleep()    
    {
        if (isimmunesleep)
        {
            Debug.Log(isimmunesleep);
            return true;
        }
        return false;
    }
    #endregion
}
