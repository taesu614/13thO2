using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardFunctionManager : MonoBehaviour
{
    private Dictionary<string, Action> cardEffects = new Dictionary<string, Action>();
    public static CardFunctionManager Inst { get; private set; }
    //예상되는 변수들 모음
    [SerializeField] TMP_Text costTMP;  //계산에 쓰일 것이므로 num = int.Parse(costTMP); 해둘것
    [SerializeField] TMP_Text rcostTMP;
    [SerializeField] TMP_Text gcostTMP;
    [SerializeField] TMP_Text bcostTMP;
    [SerializeField] GameObject cardPrefab;  //내 코스트 X 카드에 써진 코스트 O

    ItemSO itemSO;
    Card card;
    public CostManager costManager;
    public EntityManager entityManager;
    GameManager gameManager;
    CardManager cardManager;
    PlayerManager playerManager;

    GameObject findtarget;
    Entity target;
    GameObject[] findmonsters;
    GameObject findplayer;
    Entity player;
    Entity[] monsters;
    Entity targetentity;
    bool isRushUsed = false;

    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // 카드 이름과 기능을 매칭하여 Dictionary에 저장
        //cardEffects["TheDevil"] = () => TheDevil (myscore); // 람다식 사용 예시
        cardEffects["Encore"] = Encore;
        cardEffects["TestBuffAttackUp"] = TestBuffAttackUp;
        cardEffects["TestBuffAttackDown"] = TestBuffAttackDown;
        cardEffects["TestAttack"] = TestAttack;
        cardEffects["TestBuffShield"] = TestBuffShield;
        cardEffects["TestFaint"] = TestFaint;
        cardEffects["TestSleep"] = TestSleep;
        cardEffects["TestImmuneSleep"] = TestImmuneSleep;

        cardEffects["SharpNib"] = SharpNib;  // 날카로운 펜촉
        cardEffects["Firestick"] = Firestick;  // 불꽃 스틱
        cardEffects["Mousefire"] = Mousefire;  // 쥐불놀이
        cardEffects["CtrlZ"] = CtrlZ; // 되돌리기
        cardEffects["Gradation"] = Gradation; // 그라데이션
    }
    //단일 적 gameobj 가져오는 함수
    //public void GetEnemy(GameObject targetObj)
    //{
    //    target = targetObj;
    //    Debug.Log("확인");
    //}

    // 카드 액션,난입,연출별로 region다시 설정 할 것
    #region CardEffects

    private void Moon() //코스트 회복이 쓸 일은 있을거 같아 얘는 남김 
    {
        //달, TheMoon, 코스트를 1 얻습니다
        int cost = int.Parse(costTMP.text);
        cost++;
        costManager.CostSetNewCost(cost);
    }

    private void TestBuffAttackUp() //버프 테스트용 카드
    {
        FindPlayer();   //플레이어 찾기
        player.MakeAttackUp(5, 2);    //엔티티에서 버프 리스트 생성
        Debug.Log("버프 생성");
    }

    private void TestBuffAttackDown() //버프 테스트용 카드
    {
        FindPlayer();   //플레이어 찾기
        player.MakeAttackDown(3, 2);    //엔티티에서 버프 리스트 생성
        Debug.Log("버프 생성");
    }

    private void TestBuffShield()   
    {
        FindPlayer();
        player.MakeShield(10, 5);
    }

    private void TestAttack()   //선택한 대상에게 피해를 5 줍니다
    {
        Attack("anything", 5, "normal");
    }

    private void TestFaint()    //플레이어 수면 
    {
        Faint("anything", 3);
    }

    private void TestSleep()    //플레이어 수면 
    {
        Sleep("anything", 3);
    }

    private void TestImmuneSleep()  //플레이어 수면 면역
    {
        ImmuneSleep("anything", 3);
    }


    private void ImsiCard1()    //카드 드로우에 대한 내용이 있어 임시로 놔둠
    {
        Attack("anything", 7, "normal");
        TurnManager.OnAddCard.Invoke(true);
        TurnManager.OnAddCard.Invoke(true);
    }

    private void Encore()       //난입 관련이라 안건드리는게 낫다 판단되어 남김
    {
        CardManager.Inst.SetIntrusionEncore();
    }

    private void SharpNib()  // 날카로운 펜촉
    {
        Attack("anything", 7, "piercing");
        ResetTarget();  //모든 공격 기능 종료 후 타겟을 리셋해야 대상 지정안해도 카드가 사용되는 상황 방지됨
    }


    private void Firestick()  // 불꽃 막대
    {
        int randNum = UnityEngine.Random.Range(1, 3);

        for (int i = 0; i < randNum; i++)
        {
            Attack("anything", 2, "normal");
        }
        ResetTarget();  //모든 공격 기능 종료 후 타겟을 리셋해야 대상 지정안해도 카드가 사용되는 상황 방지됨
    }

    private void Mousefire()  // 쥐불 놀이
    {
        int randNum = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < randNum; i++)
        {
            Attack("all", 3, "normal");
        }
        ResetTarget();  //모든 공격 기능 종료 후 타겟을 리셋해야 대상 지정안해도 카드가 사용되는 상황 방지됨
    }

    private void Gradation()    //그라데이션
    {

        costManager.AddRGBCost('R');
        costManager.AddRGBCost('G');
        costManager.AddRGBCost('B');
    }

    private void CtrlZ()
    {
        // 적당히 봐보니까 entity에서 몬스터패턴쪽에서 데미지 계산할 때 고려해서 과거 체력 보내봐야될듯
    }
    #endregion

    #region IntrusionEffects
    private void IntrusionEncore(string functionName)
    {
        UseSelectCard(functionName);
    }

    #endregion


    // 카드 실행 메서드
    public void UseSelectCard(string functionName)
    {
        if (cardEffects.TryGetValue(functionName, out Action cardEffect))
        {
            cardEffect();
        }
        else
        {
            Debug.Log("Card not found.");
        }
    }

    //메서드 유틸리티
    #region method
    //버프가 아닌 모든 메서드의 넘길 매개변수는
    //타겟, 수치, 지속시간(혹은 횟수), 기타 내용들 순서
    public void Attack(string targetcount, int damage, string type, string user = "player")   //대상에게 피해를 n 줍니다
    {                                                                  //모든 공격력 증가 효과 가져와서 적용
        FindPlayer();      //기본적인 계산                                             //targetcount(anything: 단일 아무나, enemy:적, player: 플레이어, all:전체, enemyall: 적 전체 
        if(user == "player")    //몬스터에게도 사용되므로 기준이 플레이어야 몬스터냐에 따라 달라질 것
        {
            damage += player.GetAllAttackUpEffect();                        //damage(피해량)
            damage -= player.GetAllAttackDownEffect();                      //type (normal: 통상 공격, piercing: 관통 공격(보호막에 관계없이 체력에 직접적으로 공격))
        }
        switch (type)
        {
            case "normal":
                switch (targetcount)
                {
                    case "anything":
                        NormalDamage(target, damage);
                        target.SetHealthTMP();
                        target.SetShieldTMP();
                        break;
                    case "enemy":
                        break;
                    case "player":
                        NormalDamage(player, damage);
                        break;
                    case "all":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            NormalDamage(nowmonster, damage);
                        }
                        NormalDamage(player, damage);
                        break;
                    case "enemyall":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            NormalDamage(nowmonster, damage);
                        }
                        break;
                }
                break;
            case "piercing":
                switch (targetcount)
                {
                    case "anything":
                        target.health -= damage;
                        target.SetHealthTMP();
                        //target.GetComponents<Entity>();
                        break;
                    case "enemy":
                        break;
                    case "player":
                        player.health -= damage;
                        player.SetHealthTMP();
                        break;
                    case "all":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            nowmonster.health -= damage;
                            nowmonster.SetHealthTMP();
                        }
                        player.health -= damage;
                        player.SetHealthTMP();
                        break;
                    case "enemyall":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            nowmonster.health -= damage;
                            nowmonster.SetHealthTMP();
                        }
                        break;
                }
                break;
        }
    }

    public void Faint(string targetcount, int turn)
    {
        switch (targetcount)            
        {
            case "anything":
                target.MakeFaint(turn);
                break;
            case "enemy":
                break;
            case "player":
                break;
            case "all":
                break;
            case "enemyall":
                break;
        }
    }

    public void Sleep(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                target.MakeSleep(turn);
                break;
            case "enemy":
                break;
            case "player":
                break;
            case "all":
                break;
            case "enemyall":
                break;
        }
    }

    public void ImmuneSleep(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                Debug.Log(target);
                target.MakeImmuneSleep(turn);
                break;
            case "enemy":
                break;
            case "player":
                FindPlayer();
                player.MakeImmuneSleep(turn);
                break;
            case "all":
                break;
            case "enemyall":
                break;
        }
    }
    #endregion

    #region methodUtils
    public void SetTarget(GameObject gameobject)    //카드매니저를 통해 찾은 게임 오브젝트의 엔티티 추출
    {
        findtarget = gameobject;
        target = findtarget.GetComponent<Entity>();
    }

    private void ResetTarget()
    {
        target = null;
    }

    private void FindAllMonster()   //Monster 태그를 가진 모든 게임 오브젝트 찾아 엔티티 추출
    {
        findmonsters = GameObject.FindGameObjectsWithTag("Monster");
        monsters = new Entity[findmonsters.Length];
        for(int i = 0; i < findmonsters.Length; i++)
        {
            monsters[i] = findmonsters[i].GetComponent<Entity>();
        }
    }

    private void FindPlayer()   //플레이어 태그를 가진 게임 오브젝트 찾아 엔티티 추출
    {
        findplayer = GameObject.FindGameObjectWithTag("Player");
        player = findplayer.GetComponent<Entity>();
    }

    private void NormalDamage(Entity entity, int damage)
    {
        if (entity.shield >= damage)
        {
            entity.shield -= damage;
            entity.SetShieldTMP();
        }
        else
        {
            damage = damage - entity.shield;
            entity.health -= damage;
            entity.shield = 0;
            entity.SetHealthTMP();
            entity.SetShieldTMP();
        }
    }
    #endregion
}