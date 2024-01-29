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
    public int rarity;   //카드 등급
    //public Sprite sprite;
    public string reward;   //보상 위치 여부 - 모험보상, 상점 등
    public int price;
    public string active;
    public float percent;
    public bool selectable;
    public int identifier;  //테스트용 식별자
    private static int calletc = 0;
    /*
    기본 앞자리 1 - int라 00000000이 작성 안됨
    코스트 00 
    색 0                 //RGB 0 1 2 순서
    액션 연출 난입 0      //액션 0 연출 1 난입 2
    기타 식별용 카드 넘버 0000   //순서에 대한건 SO파일 순서로 임시로 나눔

    테스트용으로 만드는 부분이고 향후 일부분은 자동으로 식별이 가능하도록 할 것

    ex 100000000	0코스트 R 액션 카드 0000
    실제 기획에서는 char로 나눠서 일부분 나누어서 구분하는듯 하나
    카드 정렬이 우선인점, 아직 내 수준이 거기까지는 못미쳐서 int로 설정함
    */

}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]   //에셋 메뉴에 추가 가능해짐
public class ItemSO : ScriptableObject
{
    public Item[] items;
}

