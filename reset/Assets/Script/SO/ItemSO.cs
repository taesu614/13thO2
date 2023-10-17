using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item   //카드 관련으로 꾸미는 곳
{
    public string name;
    public string functionname;
    public int cost;
    public Sprite colorimg;
    public Sprite costcolor;
    public string cardtype;
    public char color;   //R:R G:G B:B
    public Sprite sprite;
    public string active;
    public float percent;
    public bool selectable;
}

    [CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]   //에셋 메뉴에 추가 가능해짐
public class ItemSO : ScriptableObject
{
    public Item[] items;
}

