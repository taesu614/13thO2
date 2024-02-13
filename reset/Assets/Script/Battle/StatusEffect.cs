public class StatusEffect  //스택 형식의 효과는 없앤 상태임
{
    //향후 계획 - Entity의 반복작업으로 인한 분량이 많아지는 현상으로 인해 코드를 통합할 방법 생각중임
    //현 시점에서 Set이름()별로 나누어진 메서드 대신
    //SetEffect(Entity target, string name, int turn, int amount = -1)이 되지 않을까 싶음
    bool isBeneFicialEffect;
    bool isPowerUp = false;
    bool isPowerDown = false;
    bool isShield = false;   //쉴드 존재 여부
    bool isfaint = false;    //기절 존재 여부
    bool isSleep = false;    //수면 존재 여부
    bool isDamageEffect = false;    //체력에 변화를 주는지 여부 - 많을 것 같아 따로 분류
    bool isImmuneSleep = false;
    bool canHeal = true;
    int effectamount = 0;    //효과의 양
    int effectturn = 0;    //지속 턴 수
    string effectname;

    public void SetStatusEffect(string name, int turn, int amount = -1)
    {
        effectamount = amount;
        effectturn = turn;
        effectname = name;

        switch(name)    //이로운 효과/해로운 효과 구분용 - 정화 같은곳에서 사용
        {
            case "powerUp":
            case "shield":
            case "immuneSleep":
            case "healTurn":
                isBeneFicialEffect = true;
                break;
            case "powerDown":
            case "faint":
            case "sleep":
            case "poison":
            case "burn":
            case "healBlock":
                isBeneFicialEffect = false;
                break;
        }

        switch(name)    //시간 부족으로 기존의 코드를 최대한 활용하는식으로 함
        {               //효율적으로 굴리기위해 bool로 할 지, 가독성을 위해 string으로 할지는 나중에 고민해볼것
            case "powerUp":
                isPowerUp = true;
                break;
            case "powerDown":
                isPowerDown = true;
                break;
            case "shield":
                isShield = true;
                break;
            case "faint":
                isfaint = true;
                break;
            case "sleep":
                isSleep = true;
                break;
            case "immuneSleep":
                isImmuneSleep = true;
                break;
            case "poison":
                isDamageEffect = true;
                break;
            case "burn":
                canHeal = false;
                break;
            case "healTurn":
                isDamageEffect = true;
                break;
            case "healBlock":
                canHeal = true;
                break;
        }
    }

    #region PowerUp
    public int GetAllAttackUp()
    {
        if (isPowerUp)
        {
            return effectamount;
        }
        return 0;
    }
    #endregion

    #region PowerDown

    public int GetAllAttackDown()
    {
        if (isPowerDown)
        {
            return effectamount;
        }
        return 0;
    }
    #endregion

    #region Shield

    public bool CheckShield()
    {
        return isShield;
    }

    public int GetShield()
    {
        return effectamount;
    }

    public int CalculateShiled(int damage)
    {
        if (effectamount >= damage) //쉴드량이 더 많은 경우
        {
            effectamount -= damage;
            return 0;
        }
        else//쉴드량이 더 적은 경우
        {
            damage -= effectamount;
            effectamount = 0;
            if (damage < 0)
                damage = 0;
            return damage;
        }
    }
    #endregion

    #region Faint
    public bool GetFaint()
    {
        if (isfaint)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Sleep


    public void SetIsSleep(bool onoff)  // issleep 체크
    {
        isSleep = onoff;
    }

    public bool GetSleep()  //사용할 때 canplay를 바로 설정함
    {
        if (isSleep)
        {
            return true;
        }
        return false;
    }

    public bool GetImmuneSleep()
    {
        if (isImmuneSleep)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Poison
    #endregion

    #region Burn
    #endregion

    #region HealBlock
    public void SetHealBlock(int turn)
    {
        effectturn = turn;
        canHeal = false;
        effectname = "healBlock";
    }

    public bool GetHealBlock()
    {
        return canHeal;
    }
    #endregion

    #region HealTurn
    public void SetHealTurn(int turn)
    {
        effectturn = turn;
        effectname = "healTurn";
    }
    #endregion

    #region BaronetsNap

    #endregion
    public void DecreaseEffectTurn()    //턴 감소
    {
        effectturn--;
    }

    public int GetEffectTurn()  //해당 버프의 남은 턴수 가져오기
    {
        return effectturn;
    }

    public (bool, int) CheckDamageEffect()
    {
        switch (effectname)
        {
            case "poison":
                return (true, effectturn);
            case "burn":
                return (true, effectamount);
            case "healTurn":
                if (!canHeal)   //회복 불가라면 0 회복
                    return (true, 0);
                return (true, -effectturn);   //힐이라서 대미지와 반대 
            default:
                return (false, 0);
        }
    }
}