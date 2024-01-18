using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardFunctionManager : MonoBehaviour
{
    private Dictionary<string, Action> cardEffects = new Dictionary<string, Action>();

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

    // Start is called before the first frame update
    void Start()
    {
        // 카드 이름과 기능을 매칭하여 Dictionary에 저장
        cardEffects["Counter"] = Counter;
        cardEffects["Vaccination"] = Vaccination;
        cardEffects["StageAccident"] = StageAccident;
        //cardEffects["PenOfTruepenny"] = PenOfTruepenny;
        //cardEffects["Sortie"] = Sortie;
        cardEffects["Boomerang"] = Boomerang;
        cardEffects["Rush"] = Rush;
        cardEffects["Moon"] = Moon;
        cardEffects["TheHangedMan"] = TheHangedMan;
        //cardEffects["TheDevil"] = () => TheDevil (myscore); // 람다식 사용 예시
        cardEffects["Encore"] = Encore;
        cardEffects["TestBuffAttackUp"] = TestBuffAttackUp;
        cardEffects["TestBuffAttackDown"] = TestBuffAttackDown;
        cardEffects["TestAttack"] = TestAttack;
        cardEffects["TestBuffShield"] = TestBuffShield;
        cardEffects["TestFaint"] = TestFaint;
        cardEffects["TestSleep"] = TestSleep;
        cardEffects["TestImmuneSleep"] = TestImmuneSleep;
        cardEffects["ImsiCard1"] = ImsiCard1;
        cardEffects["ImsiCard2"] = ImsiCard2;
        cardEffects["ImsiCard3"] = ImsiCard3;
    }
    //단일 적 gameobj 가져오는 함수
    //public void GetEnemy(GameObject targetObj)
    //{
    //    target = targetObj;
    //    Debug.Log("확인");
    //}

    // 카드 액션,난입,연출별로 region다시 설정 할 것
    #region CardEffects

    private void Counter() //반격
    {
        CardManager.Inst.SetIntrusionCounter();
    }

    private void Vaccination() //예방 주사
    {
        Debug.Log("예방 주사!");
        TMP_Text playerHealthTMP = GameObject.Find("MyPlayer").GetComponentInChildren<TMP_Text>();
        if (playerHealthTMP != null)
        {
            int currentPlayerHealth = int.Parse(playerHealthTMP.text);
            currentPlayerHealth -= 2;
            playerHealthTMP.text = currentPlayerHealth.ToString();
        }
        else
            Debug.LogWarning("HealthTMP not found in the Player GameObject.");

        //방어력 버프와 면역 함수 사용 따로 스크립트 만들기


    }
    private void StageAccident() //우당탕탕 무대 사고!!
    {
        Debug.Log("우당탕탕 무대 사고!!");
        GameObject monsterObject = GameObject.FindGameObjectWithTag("Monster");
        TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();
        if (healthTMP != null)
        {
            // 현재 HealthTMP의 값을 가져와서 int로 변환
            int currentHealth = int.Parse(healthTMP.text);


            currentHealth = Mathf.RoundToInt(currentHealth / 2); //절반으로 나눈뒤 반올림

            //제압상태 부여 함수 들어갈 곳

            healthTMP.text = currentHealth.ToString();
        }
    }
    /*private void PenOfTruepenny() //정직한 자의 펜촉 고정피해 TrueDamage라 설정 따로 함수를 만들어서 고정피해 전용 데미지계산 적용
    {
        Debug.Log("정직한 자의 펜촉!!");
        GameObject monsterObject = target;
        TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();
        if (healthTMP != null)
        {
            // 현재 HealthTMP의 값을 가져와서 int로 변환
            int currentHealth = int.Parse(healthTMP.text);

            // -12를 하고 값 변경 여기에 고정피해 함수 들어감
            currentHealth -= 12;
            healthTMP.text = currentHealth.ToString();
        }
    }*/
    /*private void Sortie() //돌격
    {
        Debug.Log("돌격!!");
        GameObject monsterObject = target;
        TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();
        if (healthTMP != null)
        {
            // 현재 HealthTMP의 값을 가져와서 int로 변환
            int currentHealth = int.Parse(healthTMP.text);

            // -8를 하고 값 변경
            currentHealth -= 5;
            healthTMP.text = currentHealth.ToString();
        }
        ResetTarget();
    }*/

    private void Rush() // 기세몰이
    {
        Debug.Log("기세몰이!!");
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        GameObject targetMonsterObject = monsterObjects[0];
        foreach (GameObject monsterObject in monsterObjects)
        {
            if (int.Parse(targetMonsterObject.GetComponentInChildren<TMP_Text>().text) >
                int.Parse(monsterObject.GetComponentInChildren<TMP_Text>().text))
            {
                targetMonsterObject = monsterObject;
            }
        }

        TMP_Text healthTMP = targetMonsterObject.GetComponentInChildren<TMP_Text>();
        if (healthTMP != null)
        {
            // 현재 HealthTMP의 값을 가져와서 int로 변환
            int currentHealth = int.Parse(healthTMP.text);

            // -8를 하고 값 변경
            currentHealth -= 8;
            healthTMP.text = currentHealth.ToString();

            if (isRushUsed)
            {
                isRushUsed = false;
            }
            else if (currentHealth < 0)
            {
                if (!isRushUsed)
                    Debug.Log("한번더!");
                //Rush(); 여기서 버그가 걸려서 일단 디버그로만 해놨음
            }
        }
        else
        {
            Debug.LogWarning("HealthTMP not found in the Monster GameObject.");
        }


    }
    private void Boomerang() //부메랑
    {
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");

        Debug.Log("부메랑!!");

        foreach (GameObject monsterObject in monsterObjects)
        {
            // 각 Monster 게임 오브젝트에서 TMP_Text 컴포넌트를 찾습니다.
            TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();

            if (healthTMP != null)
            {
                // 현재 HealthTMP의 값을 가져와서 int로 변환
                int currentHealth = int.Parse(healthTMP.text);

                // -8를 하고 값 변경
                currentHealth -= 8;
                healthTMP.text = currentHealth.ToString();
            }
            else
            {
                Debug.LogWarning("HealthTMP not found in the Monster GameObject.");
            }
        }
        //플레이어 체력 회복 방법은 ^^위와^^ 같음
        TMP_Text playerHealthTMP = GameObject.Find("MyPlayer").GetComponentInChildren<TMP_Text>();
        if (playerHealthTMP != null)
        {
            int currentPlayerHealth = int.Parse(playerHealthTMP.text);
            currentPlayerHealth += 4;
            playerHealthTMP.text = currentPlayerHealth.ToString();
        }
        else
            Debug.LogWarning("HealthTMP not found in the Player GameObject.");

    }
    // 예시로 만든 메서드들
    private void Moon()
    {
        //달, TheMoon, 코스트를 1 얻습니다
        int cost = int.Parse(costTMP.text);
        cost++;
        costManager.CostSetNewCost(cost);
    }

    private void TheHangedMan()
    {
        // Monster 태그를 가진 모든 게임 오브젝트를 찾습니다.
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        
        foreach (GameObject monsterObject in monsterObjects)
        {
            // 각 Monster 게임 오브젝트에서 TMP_Text 컴포넌트를 찾습니다.
            TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();

            if (healthTMP != null)
            {
                // 현재 HealthTMP의 값을 가져와서 int로 변환
                int currentHealth = int.Parse(healthTMP.text);

                // -5를 하고 값 변경
                currentHealth -= 5;
                healthTMP.text = currentHealth.ToString();
            }
            else
            {
                Debug.LogWarning("HealthTMP not found in the Monster GameObject.");
            }
        }
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
        Attack("anything", 5, "1");
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


    private void ImsiCard1()    //임시 카드 1, 피해 7, 카드 2장 뽑음
    {
        Attack("anything", 7, "1");
        TurnManager.OnAddCard.Invoke(true);
        TurnManager.OnAddCard.Invoke(true);
    }

    private void ImsiCard2()
    {
        FindPlayer();
        player.MakeShield(5, 5);
        Attack("anything", 5, "1");
    }

    private void ImsiCard3()
    {
        Attack("all", 5, "1");
        FindPlayer();
        player.MakeShield(10, 5);
    }

    private void Encore()
    {
        CardManager.Inst.SetIntrusionEncore();
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
    public void Attack(string targetcount, int damage, string type)   //대상에게 피해를 n 줍니다
    {                                   //targetcount(anything: 단일 아무나, enemy:적, player: 플레이어, all:전체, enemyall: 적 전체 
                                        //damage(피해량)
        switch (targetcount)            //type 고정피해 등의 여부
        {
            case "anything":
                FindPlayer();
                damage += player.GetAllAttackUpEffect();    //모든 공격력 증가 효과 가져와서 적용
                damage -= player.GetAllAttackDownEffect();
                target.health -= damage;
                target.SetHealthTMP();
                ResetTarget();
        //target.GetComponents<Entity>();
                break;
            case "enemy":
                break;
            case "player":
                break;
            case "all":
                FindPlayer();
                FindAllMonster();
                damage += player.GetAllAttackUpEffect();    //모든 공격력 증가 효과 가져와서 적용
                damage -= player.GetAllAttackDownEffect();
                player.health -= damage;
                player.SetHealthTMP();
                foreach(Entity A in monsters)
                {
                    A.health -= damage;
                    A.SetHealthTMP();
                }
                break;
            case "enemyall":
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
    #endregion
}