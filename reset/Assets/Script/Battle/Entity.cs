using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;  //랜덤 사용을 위함

public class Entity : MonoBehaviour //해당 내용을 통해 별자리 생성 계획 그래서 다른 monsterSO를 만듦
{   //향후 크게 정리 할 것, 예를들면 몬스터와 플레이어를 따로 구분짓는다거나, SeriaizeField를 구분한다거나 등
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();  //버프 제거 시 선입 선출 방식으로 예상되어 큐로 설정
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] SpriteRenderer shieldRenderer;
    [SerializeField] SpriteRenderer patternUI;
    [SerializeField] SpriteRenderer effectUI;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text shieldTMP;
    [SerializeField] GameObject shieldIcoObj;
    [SerializeField] GameObject hpline;
    [SerializeField] Transform DamageMarkTransform;
    [SerializeField] Sprite AttackUI;
    [SerializeField] Sprite ShieldUI;
    [SerializeField] Sprite EffectUI;
    [SerializeField] Sprite WhatUI;
    [SerializeField] Sprite poisonIco;
    [SerializeField] Sprite sleepIco;
    [SerializeField] Sprite burnIco;
    [SerializeField] Sprite powerDownIco;
    [SerializeField] Sprite powerUpIco;
    [SerializeField] Sprite shieldIco;
    [SerializeField] Animator animator;
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
    bool isCoroutineRunning = false; //코루틴 한번만 실행되게 하는 용도
    MonsterAnimator monsterAnimator;
    Order order;
    void Start()
    {
        monsterPatterns["BacteriaVenom"] = () => BacteriaVenomPattern();
        monsterPatterns["Hcoronatus"] = () => HcoronatusPattern();
        monsterPatterns["Umumu"] = () => UmumuPattern();
        pattern = Random.Range(0, 10);
        ExecutePattern(monsterfunctionname);    //isfirst를 이용해서 처음에 사용할 패턴을 정하게 해 둠 
        order = GetComponent<Order>();
        Debug.Log(order);
        order.SetOrder(shieldRenderer.sortingOrder);
        SetShieldTMP();
        if (gameObject.name != "MyPlayer")   //플레이어일 때 해당 내용들 실행 금지
        {
            monsterAnimator = transform.Find("Character").GetComponent<MonsterAnimator>();
        }
        StartCoroutine(DisplayEffect());
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
        animator.runtimeAnimatorController = this.monster.stateController;  //좋은 방법이 아니라 생각이 들어 나중에 고민해볼 것
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
        if(shield > 0)
        {
            shieldIcoObj.SetActive(true);
            shieldTMP.text = shield.ToString();
        }
        else
        {
            shieldTMP.text = shield.ToString();
            shieldIcoObj.SetActive(false);
        }
        
    }
    public int GetLiveCount()
    {
        return liveCount;
    }

    public Transform GetDamageMarkTransform()
    {
        return DamageMarkTransform;
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
    #region BacteriaVenom
    private void BacteriaVenomPattern()
    {
        if (isfirst)
        {
            isfirst = false;
        }
        else
        {
            int damage = attack;
            damage += GetAllAttackUpEffect();
            damage -= GetAllAttackDownEffect();
            monsterAnimator.SetMonsterState("attack");
            switch (patternname)
            {
                case "attack":
                    CardFunctionManager.Inst.Attack("player", damage, "normal", this);
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
    #endregion
    #region Hcoronatus  난삼귀가 뭔지 몰라서 난초사마귀로 임시대체

    private void HcoronatusPattern()
    {
        if (isfirst)
        {
            isfirst = false;
        }
        else
        {
            int damage = attack;
            damage += GetAllAttackUpEffect();
            damage -= GetAllAttackDownEffect();
            monsterAnimator.SetMonsterState("attack");
            switch (patternname)
            {
                case "attack":
                    CardFunctionManager.Inst.Attack("player", damage, "normal", this);
                    Invoke("HcoronatusPatternAttackDelay", 0.2f);
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

    void HcoronatusPatternAttackDelay() //Invoke에 들어가게 하려고 만든 것
    {
        int damage = attack;
        damage += GetAllAttackUpEffect();
        damage -= GetAllAttackDownEffect();
        CardFunctionManager.Inst.Attack("player", damage, "normal", this);
    }
    #endregion
    #region Umumu
    private void UmumuPattern()
    {
        if (isfirst)
        {
            isfirst = false;
        }
        else
        {
            int damage = attack;
            damage += GetAllAttackUpEffect();
            damage -= GetAllAttackDownEffect();
            monsterAnimator.SetMonsterState("attack");
            switch (patternname)
            {
                case "attack":
                    CardFunctionManager.Inst.Attack("player", damage, "normal", this);
                    break;
                case "effect":
                    damage = 6;
                    damage += GetAllAttackUpEffect();
                    damage -= GetAllAttackDownEffect();
                    CardFunctionManager.Inst.Attack("player", damage, "normal", this);
                    CardFunctionManager.Inst.Poison("player", 3);
                    CardFunctionManager.Inst.Attack("enemy", health, "normal", this);
                    EntityManager.Inst.FindDieEntity();
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
            case 5:
            case 6:
                patternname = "attack"; //이후 이미지 변경
                patternUI.sprite = AttackUI;
                break;
            case 7:
            case 8:
            case 9:
                patternname = "effect";
                patternUI.sprite = EffectUI;
                break;
        }
        Debug.Log(patternname);
    }
    #endregion
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

    #region MakeEffect  //버프 생성

    public void AddStatusEffect(StatusEffect effect)   //savedata에 저장된 버프값을 가져오는 용도
    {
        myStatusEffect.Add(effect);
    }
    public void SetStatusEffect(string name, int turn, int amount = -1)
    {
        if (name == "sleep")//수면면역 체크
        {
            foreach (StatusEffect A in myStatusEffect)
            {
                if (A.GetImmuneSleep())
                    return;
            }
        }  
        Debug.Log("Effect - " + name);
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetStatusEffect(name, turn, amount);
        myStatusEffect.Add(newEffect);
        switch(name)
        {
            case "sleep":
            case "faint":
                GetAllCC();//군중제어효과를 자신에게 사용할 때 바로 효과 적용하게 하는 용도
                break;
            case "shield":
                SetShieldTMP();
                break;
            case "immuneSleep":
                canplay = true; 
                RemoveEffect("sleep");  //기존의 모든 수면 효과 삭제
                break;
        }
        if(!isCoroutineRunning)
            StartCoroutine(DisplayEffect());
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
    public void GetAllCC()
    {
        int i = 0;
        foreach (StatusEffect obj in myStatusEffect)
        {
            if (obj.GetSleep() || obj.GetFaint())
            {
                i++;
                Debug.Log("You are sleep");
            }
        }
        if (i > 0)
            canplay = false;
        else
            canplay = true;
    }

    public void RemoveEffect(string name)
    {
        for (int i = myStatusEffect.Count - 1; i >= 0; i--)
        {
            if(name == "sleep") //공격받았을때 모든 수면 효과 제거하는 용도
            {
                myStatusEffect.RemoveAt(i);
                Debug.Log(myStatusEffect.Count);
                continue;
            }
            if (name == "beneficial")   //이로운 효과 제거
            {
                if(myStatusEffect[i].GetBenefitEffect())
                    myStatusEffect.RemoveAt(i);
                break;
            }
            else if(name == "harmful")  //해로운 효과 제거
            {
                if (!myStatusEffect[i].GetBenefitEffect())
                    myStatusEffect.RemoveAt(i);
                break;
            }
        }
        GetAllCC(); //버프 변동 후 버프 체크
        if (!isCoroutineRunning)
            StartCoroutine(DisplayEffect());
    }
    public void CheckEffect()
    {
        Debug.Log(myStatusEffect.Count);
        for (int i = myStatusEffect.Count - 1; i >= 0; i--)  //반드시 역순으로 지울 것 
        {                                                    //매커니즘 변경 가능성 있음
            if (myStatusEffect[i].CheckDamageEffect().Item1)  //지속피해 효과 여부
            {
                health -= myStatusEffect[i].CheckDamageEffect().Item2;
                CardFunctionManager.Inst.MakeDamageMark(this, myStatusEffect[i].CheckDamageEffect().Item2, myStatusEffect[i].GetEffectName());
                SetHealthTMP();
            }
            myStatusEffect[i].DecreaseEffectTurn();
            if (myStatusEffect[i].GetEffectTurn() <= 0)
            {
                myStatusEffect.RemoveAt(i);
                SetShieldTMP(); //쉴드 사라졌는지 확인해야함
            }
        }
        if (!isCoroutineRunning)
            StartCoroutine(DisplayEffect());
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
        if (!isCoroutineRunning)
            StartCoroutine(DisplayEffect());
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

    IEnumerator DisplayEffect()    //버프 표시용
    {
        isCoroutineRunning = true; // 코루틴이 실행 중임을 표시
        List<StatusEffect> coroutineList = new List<StatusEffect>();
        foreach (StatusEffect A in myStatusEffect)
        {
            coroutineList.Add(A);
        }
        while (coroutineList.Count > 0)
        {
            coroutineList.Clear();
            foreach (StatusEffect A in myStatusEffect)
                coroutineList.Add(A);
            Debug.Log(coroutineList.Count);
            for(int i = 0; i < coroutineList.Count; i++)
            {
                switch (coroutineList[i].GetEffectName())
                {
                    case "sleep":
                        effectUI.sprite = sleepIco;
                        break;
                    case "poison":
                        effectUI.sprite = poisonIco;
                        break;
                    case "powerUp":
                        effectUI.sprite = powerUpIco;
                        break;
                    case "powerDown":
                        effectUI.sprite = powerDownIco;
                        break;
                    case "burn":
                        effectUI.sprite = burnIco;
                        break;
                    case "shield":
                        effectUI.sprite = shieldIco;
                        break;
                    default:
                        effectUI.sprite = null;
                        break;
                }
                yield return new WaitForSeconds(2f);
            }
        }


        effectUI.sprite = null;
        isCoroutineRunning = false;
        yield break;
    }
}