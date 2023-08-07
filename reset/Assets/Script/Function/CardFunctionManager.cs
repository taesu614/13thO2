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

    // Start is called before the first frame update
    void Start()
    {
        // 카드 이름과 기능을 매칭하여 Dictionary에 저장
        cardEffects["Moon"] = Moon;
        cardEffects["TheHangedMan"] = TheHangedMan;
        //cardEffects["TheDevil"] = () => TheDevil (myscore); // 람다식 사용 예시
        cardEffects["Encore"] = Encore;
    }

    #region CardEffects
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
}