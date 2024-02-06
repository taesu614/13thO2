public class StatusEffect  //스택 형식의 효과는 없앤 상태임
{
    //향후 계획 - Entity의 반복작업으로 인한 분량이 많아지는 현상으로 인해 코드를 통합할 방법 생각중임
    //현 시점에서 Set이름()별로 나누어진 메서드 대신
    //SetEffect(Entity target, string name, int turn, int amount = -1)이 되지 않을까 싶음
    bool ispowerUp = false;
    bool ispowerDown = false;
    bool isshield = false;   //쉴드 존재 여부
    bool isfaint = false;    //기절 존재 여부
    bool issleep = false;    //수면 존재 여부
    bool isdamageeffect = false;    //피해를 주는지 여부
    bool isimmunesleep = false;
    bool canheal = true;
    int effectamount = 0;    //효과의 양
    int effectturn = 0;    //지속 턴 수
    string effectname;
    #region PowerUp
    public void SetPowerUp(int amount, int turn)
    {
        effectamount = amount;
        effectturn = turn;
        ispowerUp = true;
        effectname = "powerup";
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
    public void SetPowerDown(int amount, int turn)
    {
        effectamount = amount;
        effectturn = turn;
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
        if (isfaint)
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
            return true;
        }
        return false;
    }
    #endregion

    #region Poison
    public void SetPoison(int turn)
    {
        effectturn = turn;
        isdamageeffect = true;
    }
    #endregion

    #region Burn
    public void SetBurn(int damage, int turn)
    {
        effectturn = turn;
        effectamount = damage;
        canheal = false;    //이게 있어야 회복 불가능 추가 가능함
        effectname = "burn";
    }
    #endregion

    #region HealBlock
    public void SetHealBlock(int turn)
    {
        effectturn = turn;
        canheal = false;
    }

    public bool GetHealBlock()
    {
        return canheal;
    }
    #endregion

    #region HealTurn
    public void SetHealTurn(int turn)
    {
        effectturn = turn;
        effectname = "healturn";
    }
    #endregion
    public void DecreaseEffectTurn()
    {
        effectturn--;
    }

    public int GetEffectTurn()
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
            case "healturn":
                if (!canheal)   //회복 불가라면 0 회복
                    return (true, 0);
                return (true, -effectturn);   //힐이라서 대미지와 반대 
            default:
                return (false, 0);
        }
    }
}