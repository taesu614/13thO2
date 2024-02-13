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
    [SerializeField] SpriteRenderer patternUI;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text shieldTMP;
    [SerializeField] GameObject hpline;
    [SerializeField] Sprite AttackUI;
    [SerializeField] Sprite ShieldUI;
    [SerializeField] Sprite EffectUI;
    [SerializeField] Sprite WhatUI;

    List<StatusEffect> myStatusEffect = new List<StatusEffect>();    //방법 못찾아서 public 사용함 
    public Monster monster;
    public int attack;
    public int maxhealth = 40;
    public int health = 40;
    public int pastHealth;
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
    public bool canplay = true;
    public bool hasmask = false;

    private int pattern;
    private string patternname;
    private bool isfirst = true;   //첫턴에 UI설정을 위해서 만든 위치
    private int addtionpattern = 0;

    void Start()
    {
        monsterPatterns["Snail"] = () => SnailPattern();
        monsterPatterns["Hcoronatus"] = () => HcoronatusPattern();
        pattern = Random.Range(0, 10);
        ExecutePattern(monsterfunctionname);    //isfirst를 이용해서 처음에 사용할 패턴을 정하게 해 둠 
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
        }

    }
    public void Setup(Monster monster)
    {
        this.monster = monster;
        maxhealth = monster.maxhealth;
        health = monster.health;
        attack = int.Parse(attackTMP.text);
        shield = monster.shield;
        monsterfunctionname = this.monster.monsterfunctionname;

        this.monster = monster;
        charater.sprite = this.monster.sprite;
        healthTMP.text = this.monster.health.ToString();
        shieldTMP.text = this.monster.shield.ToString();
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
        if (health >= maxhealth)
            health = maxhealth;
        healthTMP.text = health.ToString();
        hpline.transform.localScale = new Vector3(1 - (float)health / maxhealth, 0.65f, 1f);
    }

    public void SetShieldTMP()  //쉴드는 해당 엔티티가 가진 myStatusEffect의 쉴드값을 더해서 표기함
    {
        shield = 0;
        foreach(StatusEffect A in myStatusEffect)
        {
            if (A.CheckShield())
                shield += A.GetShield();
        }
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

    public void SetPastHealth()
    {
        pastHealth = health;
    }

    #region MonsterPattern
    #region Snail
    private void SnailPattern()
    {
        if (isfirst)
        {
            isfirst = false;
        }
        else
        {
            int damage = 8;
            damage += GetAllAttackUpEffect();
            damage -= GetAllAttackDownEffect();
            switch (patternname)
            {
                case "attack":
                    CardFunctionManager.Inst.Attack("player", damage, "normal", "monster");
                    break;
                case "effect":
                    CardFunctionManager.Inst.Poison("player", 4);
                    break;
                case "shield":
                    SetStatusEffect("shield", 2, 4);
                    break;
                default:
                    break;
            }
        }

        pattern = Random.Range(0, 10);   //마지막 패턴 설정
        switch (pattern)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                patternname = "attack"; //이후 이미지 변경
                patternUI.sprite = AttackUI;
                break;
            case 5:
            case 6:
                patternname = "effect";
                patternUI.sprite = EffectUI;
                break;
            case 7:
            case 8:
            case 9:
                patternname = "shield";
                patternUI.sprite = ShieldUI;
                break;
        }
    }
    #endregion  Hcoronatus  난삼귀가 뭔지 몰라서 난초사마귀로 임시대체

    private void HcoronatusPattern()
    {
        if (isfirst)
        {
            isfirst = false;
        }
        else
        {
            int damage = 5;
            damage += GetAllAttackUpEffect();
            damage -= GetAllAttackDownEffect();
            switch (patternname)
            {
                case "attack":
                    CardFunctionManager.Inst.Attack("player", damage, "normal", "monster");
                    CardFunctionManager.Inst.Attack("player", damage, "normal", "monster");
                    break;
                case "effect":
                    SetStatusEffect("powerUp", 2, 2);
                    break;
                case "shield":
                    SetStatusEffect("shield", 2, 10);
                    Debug.Log("This monster make Shield");
                    break;
                default:
                    break;
            }
        }

        pattern = Random.Range(0, 10);   //마지막 패턴 설정
        switch (pattern)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                patternname = "attack"; //이후 이미지 변경
                patternUI.sprite = AttackUI;
                break;
            case 5:
            case 6:
                patternname = "effect";
                patternUI.sprite = EffectUI;
                break;
            case 7:
            case 8:
            case 9:
                patternname = "shield";
                patternUI.sprite = ShieldUI;
                break;
        }
        Debug.Log(patternname);
    }
    #endregion

    #region Utils

    // 몬스터 패턴 실행 메서드
    public void ExecutePattern(string patternName)
    {
        if (monsterPatterns.TryGetValue(patternName, out Action monsterPattern))
        {
            // 해당 몬스터의 기능 실행
            monsterPattern();
        }
        else
        {
            Debug.Log("MonsterPattern not found.");
        }
    }

    #endregion

    #region MakeEffect  //작동방식 거의 동일함

    public void SetStatusEffect(string name, int turn, int amount = -1)
    {
        Debug.Log("Effect - " + name);
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetStatusEffect(name, turn, amount);
        myStatusEffect.Add(newEffect);
        if(name == "sleep" || name == "faint")
        {
            GetAllCC(); //군중제어효과를 자신에게 사용할 때 바로 효과 적용하게 하는 용도
        }
        if (name == "shield")
            SetShieldTMP();

    }
    #endregion
    public int GetAllAttackUpEffect()   //공격력 증가 효과 가져오기
    {
        int result = 0;
        foreach (StatusEffect obj in myStatusEffect)
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
    public bool GetSleep()  // sleep인지 아닌지, immunesleep인지 아닌지 체크
    {
        foreach (StatusEffect obj in myStatusEffect)
        {
            if (obj.GetSleep())
            {
                Debug.Log("I sleep");
                return true;
            }
        }

        return false;
    }
    public void SetSleep(bool onoff)  // StatusEffect 클래스에서 모두 체크하려고 하다가 부득이하게 참조를 위해 만들었습니다.
    {
        foreach (StatusEffect obj in myStatusEffect)
        {
            obj.SetIsSleep(onoff);
        }
    }
    public void GetAllCC()
    {
        foreach (StatusEffect obj in myStatusEffect)
        {
            if (obj.GetSleep() || obj.GetFaint())
            {
                Debug.Log("You are sleep");
                canplay = false;
                break;
            }
            else
            {
                canplay = true;
                break;
            }
        }
    }
    public void RemoveEffect(string name)
    {
        for(int i = myStatusEffect.Count - 1; i >= 0; i--)
        {
            if(name == "sleep") //공격받았을때 모든 수면 효과 제거하는 용도
            {
                myStatusEffect.RemoveAt(i);
                Debug.Log(myStatusEffect.Count);
                continue;
            }
            if (name == "beneficial")
            {
                myStatusEffect.RemoveAt(i);
                break;
            }
            else if(name == "harmful")
            {
                myStatusEffect.RemoveAt(i);
                break;
            }
        }
    }
    public void CheckEffect()
    {
        Debug.Log(myStatusEffect.Count);
        for (int i = myStatusEffect.Count - 1; i >= 0; i--)  //반드시 역순으로 지울 것 
        {                                                    //매커니즘 변경 가능성 있음
            if (myStatusEffect[i].CheckDamageEffect().Item1)  //지속피해 효과 여부
            {
                health -= myStatusEffect[i].CheckDamageEffect().Item2;
                CardFunctionManager.Inst.MakeDamageMark(this, myStatusEffect[i].CheckDamageEffect().Item2);
                SetHealthTMP();
            }
            myStatusEffect[i].DecreaseEffectTurn();
            if (myStatusEffect[i].GetEffectTurn() <= 0)
            {
                myStatusEffect.RemoveAt(i);
                SetShieldTMP(); //쉴드 사라졌는지 확인해야함
            }
        }
    }

    public void CheckShield()   //쉴드 0이면 삭제 하는 기능
    {
        for (int i = myStatusEffect.Count - 1; i >= 0; i--)  //반드시 역순으로 지울 것 
        {                                                   //매커니즘 변경 가능성 있음
            if (myStatusEffect[i].CheckShield() && myStatusEffect[i].GetShield() <= 0)
            {
                myStatusEffect.RemoveAt(i);
            }
        }
        SetShieldTMP(); //쉴드 사라졌는지 확인해야함
    }

    public int CalculateShiled(int damage)
    {
        foreach (StatusEffect A in myStatusEffect)
        {
            if (A.CheckShield())
            {
                damage = A.CalculateShiled(damage);
                if (damage <= 0)    //대미지 0이면 굳이 루프 돌릴 이유 없음
                    break;
            }
        }
        CheckShield();
        return damage;
    }

    public bool CheckBlockHeal()
    {
        foreach (StatusEffect A in myStatusEffect)
        {
            if (A.GetHealBlock())
                return true;
        }
        return false;
    }
}