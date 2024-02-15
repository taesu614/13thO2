using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;  //���� ����� ����

public class Entity : MonoBehaviour //�ش� ������ ���� ���ڸ� ���� ��ȹ �׷��� �ٸ� monsterSO�� ����
{
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();  //���� ���� �� ���� ���� ������� ����Ǿ� ť�� ����
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] SpriteRenderer shieldRenderer;
    [SerializeField] SpriteRenderer patternUI;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text shieldTMP;
    [SerializeField] GameObject shieldIcoObj;
    [SerializeField] GameObject hpline;
    [SerializeField] Sprite AttackUI;
    [SerializeField] Sprite ShieldUI;
    [SerializeField] Sprite EffectUI;
    [SerializeField] Sprite WhatUI;

    List<StatusEffect> myStatusEffect = new List<StatusEffect>();    //��� ��ã�Ƽ� public ����� 
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
    //�����̻� ����
    public bool debuffPoisonBool;
    public int debuffPosionInt = 0;
    public Vector3 originPos;
    public int liveCount = 0;
    public bool canplay = true;
    public bool hasmask = false;

    private int pattern;
    private string patternname;
    private bool isfirst = true;   //ù�Ͽ� UI������ ���ؼ� ���� ��ġ
    private int addtionpattern = 0;
    Order order;
    void Start()
    {
        monsterPatterns["BacteriaVenom"] = () => BacteriaVenomPattern();
        monsterPatterns["Hcoronatus"] = () => HcoronatusPattern();
        pattern = Random.Range(0, 10);
        ExecutePattern(monsterfunctionname);    //isfirst�� �̿��ؼ� ó���� ����� ������ ���ϰ� �� �� 
        order = GetComponent<Order>();
        Debug.Log(order);
        order.SetOrder(shieldRenderer.sortingOrder);
        SetShieldTMP();
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

    public int GetHealthTMP()      //SerializeField�� ���� ��ȣ�������� ���� ���� ������ ���
    {
        return int.Parse(healthTMP.text);
    }
    public int GetAttackTMP()      //SerializeField�� ���� ��ȣ�������� ���� ���� ������ ���
    {
        return int.Parse(attackTMP.text);
    }
    public int GetShieldTMP()      //SerializeField�� ���� ��ȣ�������� ���� ���� ������ ���
    {
        return int.Parse(shieldTMP.text);
    }

    public void SetHealthTMP()  //ü���� health�� ����
    {
        if (health >= maxhealth)
            health = maxhealth;
        healthTMP.text = health.ToString();
        hpline.transform.localScale = new Vector3(1 - (float)health / maxhealth, 0.65f, 1f);
    }

    public void SetShieldTMP()  //����� �ش� ��ƼƼ�� ���� myStatusEffect�� ���尪�� ���ؼ� ǥ����
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

        pattern = Random.Range(0, 10);   //������ ���� ����
        switch (pattern)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                patternname = "attack"; //���� �̹��� ����
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
    #endregion  Hcoronatus  ����Ͱ� ���� ���� ���ʻ縶�ͷ� �ӽô�ü

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

        pattern = Random.Range(0, 10);   //������ ���� ����
        switch (pattern)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                patternname = "attack"; //���� �̹��� ����
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

    // ���� ���� ���� �޼���
    public void ExecutePattern(string patternName)
    {
        if (monsterPatterns.TryGetValue(patternName, out Action monsterPattern))
        {
            // �ش� ������ ��� ����
            monsterPattern();
        }
        else
        {
            Debug.Log("MonsterPattern not found.");
        }
    }

    #endregion

    #region MakeEffect  //���� ����

    public void SetStatusEffect(string name, int turn, int amount = -1)
    {
        if (name == "sleep")//����鿪 üũ
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
                GetAllCC();//��������ȿ���� �ڽſ��� ����� �� �ٷ� ȿ�� �����ϰ� �ϴ� �뵵
                break;
            case "shield":
                SetShieldTMP();
                break;
            case "immuneSleep":
                canplay = true; 
                RemoveEffect("sleep");  //������ ��� ���� ȿ�� ����
                break;
        }
    }
    #endregion
    public int GetAllAttackUpEffect()   //���ݷ� ���� ȿ�� ��������
    {
        int result = 0;
        foreach (StatusEffect obj in myStatusEffect)
        {
            result += obj.GetAllAttackUp();
        }
        return result;
    }

    public int GetAllAttackDownEffect()   //���ݷ� ���� ȿ�� ��������
    {
        int result = 0;
        foreach (StatusEffect obj in myStatusEffect)
        {
            result += obj.GetAllAttackDown();
        }
        return result;
    }
    public bool GetSleep()  // sleep���� �ƴ���, immunesleep���� �ƴ��� üũ
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
    public void SetSleep(bool onoff)  // StatusEffect Ŭ�������� ��� üũ�Ϸ��� �ϴٰ� �ε����ϰ� ������ ���� ��������ϴ�.
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
            if(name == "sleep") //���ݹ޾����� ��� ���� ȿ�� �����ϴ� �뵵
            {
                myStatusEffect.RemoveAt(i);
                Debug.Log(myStatusEffect.Count);
                continue;
            }
            if (name == "beneficial")   //�̷ο� ȿ�� ����
            {
                myStatusEffect.RemoveAt(i);
                break;
            }
            else if(name == "harmful")  //�طο� ȿ�� ����
            {
                myStatusEffect.RemoveAt(i);
                break;
            }
        }
        GetAllCC(); //���� ���� �� ���� üũ
    }
    public void CheckEffect()
    {
        Debug.Log(myStatusEffect.Count);
        for (int i = myStatusEffect.Count - 1; i >= 0; i--)  //�ݵ�� �������� ���� �� 
        {                                                    //��Ŀ���� ���� ���ɼ� ����
            if (myStatusEffect[i].CheckDamageEffect().Item1)  //�������� ȿ�� ����
            {
                health -= myStatusEffect[i].CheckDamageEffect().Item2;
                CardFunctionManager.Inst.MakeDamageMark(this, myStatusEffect[i].CheckDamageEffect().Item2);
                SetHealthTMP();
            }
            myStatusEffect[i].DecreaseEffectTurn();
            if (myStatusEffect[i].GetEffectTurn() <= 0)
            {
                myStatusEffect.RemoveAt(i);
                SetShieldTMP(); //���� ��������� Ȯ���ؾ���
            }
        }
    }

    public void CheckShield()   //���� 0�̸� ���� �ϴ� ���
    {
        for (int i = myStatusEffect.Count - 1; i >= 0; i--)  //�ݵ�� �������� ���� �� 
        {                                                   //��Ŀ���� ���� ���ɼ� ����
            if (myStatusEffect[i].CheckShield() && myStatusEffect[i].GetShield() <= 0)
            {
                myStatusEffect.RemoveAt(i);
            }
        }
        SetShieldTMP(); //���� ��������� Ȯ���ؾ���
    }

    public int CalculateShiled(int damage)
    {
        foreach (StatusEffect A in myStatusEffect)
        {
            if (A.CheckShield())
            {
                damage = A.CalculateShiled(damage);
                if (damage <= 0)    //����� 0�̸� ���� ���� ���� ���� ����
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