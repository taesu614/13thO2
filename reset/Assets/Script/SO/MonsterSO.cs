using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster    //몬스터 관련으로 꾸미는곳 몬스터 기능 설정시 등급_몬스터 순서로 작성할 것
{                       //하급 - Low / 상급 - Senior / 엘리트 - Elite / 보스 - Boss
    public string name;
    public string grade; //등급
    public string monsterfunctionname;  //몬스터 기능
    public Sprite sprite;
    public int maxhealth;
    public int health;
    public int attack;
    public int shield;
    public float percent;
    public RuntimeAnimatorController stateController; ///애니메이션 컨트롤러
}

[CreateAssetMenu(fileName = "MonsterSO", menuName = "Scriptable Object/MonsterSO")]   //에셋 메뉴에 추가 가능해짐
public class MonsterSO : ScriptableObject
{
    public Monster[] monsters;
}
