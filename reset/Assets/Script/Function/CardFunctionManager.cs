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

    CardManager cardManager;
    PlayerManager playerManager;

    GameObject target;
    bool isRushUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        // 카드 이름과 기능을 매칭하여 Dictionary에 저장
        cardEffects["Counter"] = Counter;
        cardEffects["Vaccination"] = Vaccination;
        cardEffects["StageAccident"] = StageAccident;
        cardEffects["PenOfTruepenny"] = PenOfTruepenny;
        cardEffects["Sortie"] = Sortie;
        cardEffects["Boomerang"] = Boomerang;
        cardEffects["Rush"] = Rush;
        cardEffects["Moon"] = Moon;
        cardEffects["TheHangedMan"] = TheHangedMan;
        //cardEffects["TheDevil"] = () => TheDevil (myscore); // 람다식 사용 예시
        cardEffects["Encore"] = Encore;
    }
    //단일 적 gameobj 가져오는 함수
    public void GetEnemy(GameObject targetObj)
    {
        target = targetObj;
        Debug.Log("확인");
    }

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
    private void PenOfTruepenny() //정직한 자의 펜촉 고정피해 TrueDamage라 설정 따로 함수를 만들어서 고정피해 전용 데미지계산 적용
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
    }
    private void Sortie() //돌격
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
    }
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

    public void SetTarget(GameObject gameobject)
    {
        target = gameobject;
    }
}