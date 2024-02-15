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
    [SerializeField] GameObject damageMarkPrefab;

    ItemSO itemSO;
    Card card;
    public CostManager costManager;
    public EntityManager entityManager;
    GameManager gameManager;
    CardManager cardManager;
    PlayerManager playerManager;
    CardList cardlist;

    GameObject pastCardList;
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
        cardEffects["Moon"] = Moon;
        cardEffects["Encore"] = Encore;
        cardEffects["TestBuffAttackUp"] = TestBuffAttackUp;
        cardEffects["TestBuffAttackDown"] = TestBuffAttackDown;
        cardEffects["TestAttack"] = TestAttack;
        cardEffects["TestBuffShield"] = TestBuffShield;
        cardEffects["TestFaint"] = TestFaint;
        cardEffects["TestSleep"] = TestSleep;
        cardEffects["TestImmuneSleep"] = TestImmuneSleep;
        cardEffects["TestPoison"] = TestPoison;
        cardEffects["TestBurn"] = TestBurn;
        cardEffects["TestHeal"] = TestHeal;
        cardEffects["TestHealTurn"] = TestHealTurn;

        cardEffects["SharpNib"] = SharpNib;  // 날카로운 펜촉
        cardEffects["Firestick"] = Firestick;  // 불꽃 스틱
        cardEffects["Mousefire"] = Mousefire;  // 쥐불놀이
        cardEffects["CtrlZ"] = CtrlZ; // 되돌리기
        cardEffects["Gradation"] = Gradation; // 그라데이션
        cardEffects["WowIdea"] = WowIdea;   //반짝! 아이디어
        cardEffects["PiggyBank"] = PiggyBank;   //먹보 저금통
        cardEffects["Layer"] = Layer;   //레이어
        cardEffects["Brush"] = Brush;   //브러쉬
        cardEffects["Woodrill"] = Woodrill; //딱다드구릴
        cardEffects["Firefestival"] = Firefestival; // 불꽃 축제

        cardEffects["SleepBaronets"] = SleepBaronets; // 잠자는 바로네츠
        cardEffects["SleepKeeperArthur"] = SleepKeeperArthur; // 꿈지기 아서
        cardEffects["SleepKeeperOz"] = SleepKeeperOz; // 꿈지기 오즈
        cardEffects["SleepKeeperShadow"] = SleepKeeperShadow; // 꿈지기 쉐도우
        cardEffects["SleepKeeperGooddream"] = SleepKeeperGooddream; // 꿈지기 길몽
        cardEffects["SleepKeeperBaddream"] = SleepKeeperBaddream; // 꿈지기 흉몽
        cardEffects["Yawn"] = Yawn; // 하품
    }
    //단일 적 gameobj 가져오는 함수
    //public void GetEnemy(GameObject targetObj)
    //{
    //    target = targetObj;
    //    Debug.Log("확인");
    //}

    // 카드 액션,난입,연출별로 region다시 설정 할 것
    #region CardEffects
    #region TestCard
    private void TestBuffAttackUp() //버프 테스트용 카드
    {
        FindPlayer();   //플레이어 찾기
        player.SetStatusEffect("powerUp", 2, 5); ;    //엔티티에서 버프 리스트 생성
        Debug.Log("버프 생성");
    }

    private void TestBuffAttackDown() //버프 테스트용 카드
    {
        FindPlayer();   //플레이어 찾기
        player.SetStatusEffect("powerDown", 2, 3);   //엔티티에서 버프 리스트 생성
        Debug.Log("버프 생성");
    }

    private void TestBuffShield()
    {
        FindPlayer();
        player.SetStatusEffect("shield", 10, 5);
    }   //쉴드는 시간날때 한번 전체적으로 봐봐야함 버프 -> 지속효과로 바꿔야할수도

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
        Sleep("anything", 10);
    }

    private void TestImmuneSleep()  //플레이어 수면 면역
    {
        ImmuneSleep("anything", 3);
    }

    private void TestPoison()   //독
    {
        Poison("anything", 4);
    }

    private void TestBurn() //화상
    {
        Burn("anything", 3, 6);
    }

    private void TestHeal() //회복
    {
        Heal("anything", 10);
    }

    private void TestHealTurn() //지속회복
    {
        HealTurn("anything", 5);
    }
    #endregion
    private void Moon() //코스트 회복이 쓸 일은 있을거 같아 얘는 남김 
    {
        //달, TheMoon, 코스트를 1 얻습니다
        int cost = int.Parse(costTMP.text);
        cost++;
        costManager.CostSetNewCost(cost);
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

    private void WowIdea()  //반짝 아이디어
    {
        int cost = int.Parse(costTMP.text);
        cost += 2;
        if (cost > 10)
            cost = 10;
        costManager.CostSetNewCost(cost);
    }

    private void PiggyBank()    //돼지저금통
    {
        int mycard = CardManager.Inst.GetMyCard().Count;
        if (mycard > 3)
            mycard = 3; //손패 최대 드로우 개수 설정
        CardManager.Inst.DiscardMyCard();
        CardManager.Inst.DiscardMyCard();
        CardManager.Inst.DiscardMyCard();
        for (int i = 0; i < mycard; i++)   //손패만큼 드로우
        {
            TurnManager.OnAddCard.Invoke(true);
        }
    }
    private void Brush()    //브러쉬
    {
        Attack("anything", 3, "normal");
    }
    private void Layer()    //레이어   - 쉴드 관련이라 쉴드 수정할 때 같이 수정할 것
    {
        FindPlayer();
        player.SetStatusEffect("shield", 5, 2);
    }

    private void Woodrill()     //딱다드구릴 - 쉴드 관련이라 쉴드 수정할 때 같이 수정할 것 
    {
        int shield = target.GetShieldTMP();
        Attack("anything", shield, "normal");
        Attack("anything", 13, "normal");
        Rebound(13, 50, player);
    }
    private void Firefestival()  // 화상 거는 대상이 내가 직접 지정한 대상인지 아님 공격받는 전부인지 몰라서 일단 전체 대상으로 만들었습니다.
    {
        int randNumDmg;
        int randNum;

        // 각 개체에 확률 적용을 따로 하기 위해 만들었습니다  (Success, fail (숫자) == 왼쪽부터 세서 몬스터 위치(0 ~ 2))
        FindPlayer();
        FindAllMonster();

        foreach (Entity A in monsters)
        {
            randNumDmg = UnityEngine.Random.Range(2, 5);
            randNum = UnityEngine.Random.Range(1, 3);

            for (int i = 0; i < randNumDmg; i++)
            {
                target = A;
                Attack("anything", 4, "normal");
            }
            if (randNum == 1)
            {
                A.SetStatusEffect("burn", 2, 6);    //2턴으로 임의 설정함
            }
        }

        randNumDmg = UnityEngine.Random.Range(2, 5);
        randNum = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < randNumDmg; i++)
        {
            Attack("player", 4, "normal");
        }
        if (randNum == 1)
        {
            player.SetStatusEffect("burn", 2, 6);
        }
        ResetTarget();
    }
    private void CtrlZ()
    {
        FindPlayer();
        player.health = player.pastHealth;
        player.SetHealthTMP();
    }

    #region sheep

    private void SleepBaronets()
    {
        cardManager = GetComponent<CardManager>();
        cardManager.SearchCard("꿈지기");
        Debug.Log("꿈지기 카드 찾아오기!!");
    }

    private void SleepKeeperArthur()  // 꿈지기 아서
    {
        FindPastCardList();
        FindPlayer();
        if (cardlist == null)
        {
            Debug.LogError("CardList 컴포넌트를 찾을 수 없습니다.");
            return;
        }
        if (cardlist.CheckPast("잠자는 바로네츠"))
        {
            player.SetStatusEffect("shield", 2, 12);  // 완충 효과 대신 보호막 추가로 5 더 얻는걸로 임시구현
        }
        else
            player.SetStatusEffect("shield", 2, 7);
    }

    private void SleepKeeperOz()  // 꿈지기 오즈
    {
        FindPastCardList();
        Attack("anything", 7, "normal");
        if(cardlist.CheckPast("잠자는 바로네츠"))
        {
            Sleep("anything", 2);
        }
    }

    private void SleepKeeperShadow()  // 꿈지기 쉐도우
    {
        FindPastCardList();
        Attack("anything", 3, "normal");
        Attack("anything", 3, "normal");
        if (cardlist.CheckPast("잠자는 바로네츠"))
        {
            TurnManager.OnAddCard.Invoke(true);
            TurnManager.OnAddCard.Invoke(true);
        }
    }

    private void SleepKeeperGooddream()  // 꿈지기 길몽
    {
        Heal("player", 6);

        int cost = int.Parse(costTMP.text);
        cost += 2;
        costManager.CostSetNewCost(cost);
    }

    private void SleepKeeperBaddream()  // 꿈지기 흉몽
    {
        Attack("anything", 7, "normal");
        int randNum = UnityEngine.Random.Range(1, 11);
        Debug.Log(randNum);
        switch(randNum)
        {
            case <=3 :
                Sleep("all", 2);
                break;
            case > 3:
                break;
            default:

        }
    }

    private void Yawn()
    {
        FindAllMonster();
        foreach (Entity nowmonster in monsters)
        {
            nowmonster.SetStatusEffect("powerDown", 1, 3);
        }
        int randNum = UnityEngine.Random.Range(1, 11);
        Debug.Log(randNum);
        switch (randNum)
        {
            case <= 4:
                Sleep("enemyall", 2);
                break;
            case > 4:
                break;
            default:
        }
                Debug.Log("버프 생성");
    }

    #endregion

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
    {
        FindPlayer();      //기본적인 계산                                              //targetcount(anything: 단일 아무나, enemy:적, player: 플레이어, all:전체, enemyall: 적 전체 
        if (user == "player")    //몬스터에게도 사용되므로 기준이 플레이어야 몬스터냐에 따라 달라질 것
        {
            damage += player.GetAllAttackUpEffect();                        //damage(피해량)모든 공격력 증가 효과 가져와서 적용
            damage -= player.GetAllAttackDownEffect();                      //type (normal: 통상 공격, piercing: 관통 공격(보호막에 관계없이 체력에 직접적으로 공격))
        }
        if (damage < 0) //공격력 감소가 너무 늘어나 음수값이 되면 회복되는 현상 방지
            damage = 0;
        switch (type)
        {
            case "normal":
                switch (targetcount)
                {
                    case "anything":
                        Debug.Log(target);
                        NormalDamage(target, damage);
                        target.SetHealthTMP();
                        target.SetShieldTMP();
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
                        PiercingDamage(target, damage);
                        break;
                    case "player":
                        PiercingDamage(player, damage);
                        break;
                    case "all":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            PiercingDamage(nowmonster, damage);
                        }
                        PiercingDamage(player, damage);
                        break;
                    case "enemyall":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            PiercingDamage(nowmonster, damage);
                        }
                        break;
                }
                break;
        }
    }

    public void MemoryEffect(string cardName)  // 기억 키워드
    {

    }
    public void Rebound(int damage, int percentage, Entity user) //대미지, 반동수치, 사용자
    {
        damage += user.GetAllAttackUpEffect();
        damage -= user.GetAllAttackDownEffect();

        damage = (int)(damage * percentage * 0.01);

        user.health -= damage;      //우선은 고정피해로
        user.SetHealthTMP();
    }

    public void Heal(string targetcount, int heal, Entity me = null)   //회복 - 공격력 증가, 감소 효과와는 상관 없으므로 type 없음
    {                                                                  //적 지정 시 me를 자신을 보내게 할 것 
        switch (targetcount)
        {
            case "anything":
                NormalHeal(target, heal);
                break;
            case "enemy":
                NormalHeal(me, heal);
                break;
            case "player":
                NormalHeal(player, heal);
                break;
            case "all":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    NormalHeal(nowmonster, heal);
                }
                NormalHeal(player, heal);
                break;
            case "enemyall":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    NormalHeal(nowmonster, heal);
                }
                break;
        }
    }

    public void HealTurn(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                target.SetStatusEffect("healTurn", turn);
                break;
            case "enemy":
                break;
            case "player":
                FindPlayer();
                player.SetStatusEffect("healTurn", turn);
                break;
            case "all":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("healTurn", turn);
                }
                break;
            case "enemyall":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("healTurn", turn);
                }
                break;
        }
    }

    public void Faint(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                target.SetStatusEffect("faint", turn);
                break;
            case "enemy":
                break;
            case "player":
                FindPlayer();
                player.SetStatusEffect("faint", turn);
                break;
            case "all":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("faint", turn);
                }
                FindPlayer();
                player.SetStatusEffect("faint", turn);
                break;
            case "enemyall":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("faint", turn);
                }
                break;
        }
    }

    public void Sleep(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                Debug.Log(target);
                target.SetStatusEffect("sleep", turn);
                break;
            case "enemy":
                break;
            case "player":
                FindPlayer();
                player.SetStatusEffect("sleep", turn);
                break;
            case "all":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("sleep", turn);
                }
                FindPlayer();
                player.SetStatusEffect("sleep", turn);
                break;
            case "enemyall":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("sleep", turn);
                }
                break;
        }
    }

    public void CheckSleepAttack(Entity entity)  // sleep상태인 entity가 공격받았는지 체크하는 메소드
    {

    }
    public void ImmuneSleep(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                Debug.Log(target);
                target.SetStatusEffect("immuneSleep", 3);
                break;
            case "enemy":
                break;
            case "player":
                FindPlayer();
                player.SetStatusEffect("immuneSleep", 3);
                break;
            case "all":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("immuneSleep", 3);
                }
                FindPlayer();
                player.SetStatusEffect("immuneSleep", 3);
                break;
            case "enemyall":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("immuneSleep", 3);
                }
                break;
        }
    }

    public void Poison(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                Debug.Log(target);
                target.SetStatusEffect("poison", turn);
                break;
            case "enemy":
                break;
            case "player":
                FindPlayer();
                player.SetStatusEffect("poison", turn);
                break;
            case "all":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("poison", turn);
                }
                FindPlayer();
                player.SetStatusEffect("poison", turn);
                break;
            case "enemyall":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("poison", turn);
                }
                break;
        }
    }

    public void Burn(string targetcount, int turn, int damage)
    {
        switch (targetcount)
        {
            case "anything":
                Debug.Log(target);
                target.SetStatusEffect("burn", turn, damage);
                break;
            case "enemy":
                break;
            case "player":
                FindPlayer();
                player.SetStatusEffect("burn", turn, damage);
                break;
            case "all":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("burn", turn, damage);
                }
                FindPlayer();
                player.SetStatusEffect("burn", turn, damage);
                break;
            case "enemyall":
                FindAllMonster();
                foreach (Entity nowmonster in monsters)
                {
                    nowmonster.SetStatusEffect("burn", turn, damage);
                }
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
        for (int i = 0; i < findmonsters.Length; i++)
        {
            monsters[i] = findmonsters[i].GetComponent<Entity>();
        }
    }

    private void FindPlayer()   //플레이어 태그를 가진 게임 오브젝트 찾아 엔티티 추출
    {
        findplayer = GameObject.FindGameObjectWithTag("Player");
        player = findplayer.GetComponent<Entity>();
    }

    private void FindPastCardList()  // 과거 리스트 참조용
    {
        pastCardList = GameObject.Find("PastCard");
        cardlist = pastCardList.GetComponent<CardList>();
    }

    private void NormalDamage(Entity entity, int damage)
    {
        MakeDamageMark(entity, damage);
        damage = entity.CalculateShiled(damage);
        entity.health -= damage;
        entity.SetHealthTMP();
        if (entity.GetSleep())  // 수면 상태일 때 체력 변동시 
        {
            entity.canplay = true;
            entity.RemoveEffect("sleep");
        }
    }

    private void PiercingDamage(Entity entity, int damage)
    {
        MakeDamageMark(entity, damage);
        entity.health -= damage;
        entity.SetHealthTMP();
        if (entity.GetSleep())
            entity.SetSleep(false);
    }

    private void NormalHeal(Entity target, int healamount)
    {
        if (!target.CheckBlockHeal())
        {
            target.health += healamount;
            if (target.maxhealth < target.health)
                target.health = target.maxhealth;
            target.SetHealthTMP();
        }
    }

    public void MakeDamageMark(Entity entity, int damage)
    {
        GameObject myInstance = Instantiate(damageMarkPrefab, entity.transform); // 부모 지정
        DamageMark damagemark = myInstance.GetComponent<DamageMark>();
        if(damage >= 0)
        {
            damagemark.SetDamage(damage);
        }
        else
        {
            damagemark.SetDamage(-damage);  //회복 이미지로 변경할 것
        }
    }
    #endregion
}