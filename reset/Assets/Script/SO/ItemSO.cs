using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item   //ī�� �������� �ٹ̴� ��
{
    public string name;
    public string functionname;
    public int cost;
    public Sprite colorimg;
    public Sprite costcolor;
    public string cardtype;
    public char color;   //R:R G:G B:B
    public int rarity;   //ī�� ���
    //public Sprite sprite;
    public string reward;   //���� ��ġ ���� - ���躸��, ���� ��
    public int price;
    public string active;
    public float percent;
    public bool selectable;
    public bool memoryCard;  // ��� ȿ���� ���� ī��� true�� �ƴϸ� false��
    public bool haveCard;  // �ӽ÷� ī�带 ȹ�� �ߴ��� ����  -> ���� ī��� ó������ true��
    int identifier;  //�׽�Ʈ�� �ĺ���
    public string tag; // Ư�� Ű����� ��ġ�� �� ����Ϸ��� �ӽ÷� ��������ϴ�.
    private static int calletc = 0;

    Item()
    {
        calletc = 0;
    }
    /*
    �⺻ ���ڸ� 1 - int�� 00000000�� �ۼ� �ȵ�
    �ڽ�Ʈ 00 
    �� 0                 //RGB 0 1 2 ����
    �׼� ���� ���� 0      //�׼� 0 ���� 1 ���� 2
    ��Ÿ �ĺ��� ī�� �ѹ� 0000   //������ ���Ѱ� SO���� ������ �ӽ÷� ����

    �׽�Ʈ������ ����� �κ��̰� ���� �Ϻκ��� �ڵ����� �ĺ��� �����ϵ��� �� ��

    ex 100000000	0�ڽ�Ʈ R �׼� ī�� 0000
    ���� ��ȹ������ char�� ������ �Ϻκ� ����� �����ϴµ� �ϳ�
    ī�� ������ �켱����, ���� �� ������ �ű������ �����ļ� int�� ������
    */
    public void SetID()
    {
        string coststr = "0";
        switch (cost)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
                coststr += cost.ToString();
                break;
            default:
                coststr = cost.ToString();
                break;
        }
        char idcolor = '0';
        switch (color)
        {
            case 'R':
                idcolor = '0';
                break;
            case 'G':
                idcolor = '1';
                break;
            case 'B':
                idcolor = '2';
                break;
        }

        char cardtype = '0';
        switch (this.cardtype)
        {
            case "Action":
                cardtype = '0';
                break;
            case "Production":
                cardtype = '1';
                break;
            case "Intrusion":
                cardtype = '2';
                break;
        }

        string etc = "";
        if (calletc >= 0 && calletc < 10)
            etc = "000";
        else if (calletc >= 10 && calletc < 100)
            etc = "00";
        else if (calletc >= 100 && calletc < 1000)
            etc = "0";

        etc += calletc.ToString();

        string result = "1";
        result += cost.ToString();
        result += idcolor.ToString();
        result += cardtype.ToString();
        result += etc;
        calletc++;

        identifier = int.Parse(result);
    }

    public int GetID()
    {
        return identifier;
    }
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]   //���� �޴��� �߰� ��������
public class ItemSO : ScriptableObject
{
    public Item[] items;

    public void InitializeItems()
    {
        foreach (var item in items)
        {
            item.SetID();
            Debug.Log(item.GetID());
        }
    }
}

