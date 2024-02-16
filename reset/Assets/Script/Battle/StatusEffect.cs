public class StatusEffect  //���� ������ ȿ���� ���� ������
{
    //���� ��ȹ - Entity�� �ݺ��۾����� ���� �з��� �������� �������� ���� �ڵ带 ������ ��� ��������
    //�� �������� Set�̸�()���� �������� �޼��� ���
    //SetEffect(Entity target, string name, int turn, int amount = -1)�� ���� ������ ����
    bool isBeneFicialEffect;
    bool isPowerUp = false;
    bool isPowerDown = false;
    bool isShield = false;   //���� ���� ����
    bool isfaint = false;    //���� ���� ����
    bool isSleep = false;    //���� ���� ����
    bool isDamageEffect = false;    //ü�¿� ��ȭ�� �ִ��� ���� - ���� �� ���� ���� �з�
    bool isImmuneSleep = false;
    bool canHeal = true;
    int effectamount = 0;    //ȿ���� ��
    int effectturn = 0;    //���� �� ��
    string effectname;

    public void SetStatusEffect(string name, int turn, int amount = -1)
    {
        effectamount = amount;
        effectturn = turn;
        effectname = name;

        switch(name)    //�̷ο� ȿ��/�طο� ȿ�� ���п� - ��ȭ ���������� ���
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

        switch(name)    //�ð� �������� ������ �ڵ带 �ִ��� Ȱ���ϴ½����� ��
        {               //ȿ�������� ���������� bool�� �� ��, �������� ���� string���� ������ ���߿� ����غ���
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
        if (effectamount >= damage) //���差�� �� ���� ���
        {
            effectamount -= damage;
            return 0;
        }
        else//���差�� �� ���� ���
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


    public void SetIsSleep(bool onoff)  // issleep üũ
    {
        isSleep = onoff;
    }

    public bool GetSleep()  //����� �� canplay�� �ٷ� ������
    {
        if (isSleep)
        {
            return true;
        }
        return false;
    }

    public bool GetImmuneSleep()
    {
        return isImmuneSleep;
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
    public void DecreaseEffectTurn()    //�� ����
    {
        effectturn--;
    }

    public int GetEffectTurn()  //�ش� ������ ���� �ϼ� ��������
    {
        return effectturn;
    }

    public string GetEffectName()
    {
        return effectname;
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
                if (!canHeal)   //ȸ�� �Ұ���� 0 ȸ��
                    return (true, 0);
                return (true, -effectturn);   //���̶� ������� �ݴ� 
            default:
                return (false, 0);
        }
    }
}