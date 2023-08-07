using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class MonsterPatternManager : MonoBehaviour
{
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();

    [SerializeField] TMP_Text playerhealthTMP;
    MonsterSO monsterSO;
    Entity entity;
    EntityManager entityManager;
    int monsterhealthTMP;
    int monsterattackTMP;
    int monsterlivecount;

    // Start is called before the first frame update
    void Start()
    {
        // 카드 이름과 기능을 매칭하여 Dictionary에 저장
        //cardEffects["Moon"] = AddCost;
        //cardEffects["TheHangedMan"] = BoostMyBooster;
        //cardEffects["TheDevil"] = () => Combo(myscore); // 람다식 사용 예시

        monsterPatterns["Snail"] = () => SnailPattern();
    }

    private void SnailPattern()
    {
        // Player 태그를 가진 모든 게임 오브젝트를 찾습니다.
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            // 각 Player 게임 오브젝트에서 TMP_Text 컴포넌트를 찾습니다.
            TMP_Text playerhealthTMP = playerObject.GetComponentInChildren<TMP_Text>();
            if (playerhealthTMP != null)
            {
                // 현재 HealthTMP의 값을 가져와서 int로 변환
                int currentHealth = int.Parse(playerhealthTMP.text);

                if (monsterhealthTMP >= 5 && monsterlivecount > 3)
                {
                    int random = Random.Range(0, 10);
                    if (random < 5)
                    {
                        currentHealth -= 4;
                    }
                    else
                    {
                        currentHealth -= 8;
                    }
                }
                else if(monsterhealthTMP >= 5)
                {
                    // -5를 하고 값 변경
                    currentHealth -= 5;
                }
                else if(monsterhealthTMP < 5)
                {
                    Debug.Log("맹독");
                }

                playerhealthTMP.text = currentHealth.ToString();

            }
            else
            {
                Debug.LogWarning("HealthTMP not found in the Player GameObject.");
            }
        }
    }

    // 카드 실행 메서드
    public void ExecutePattern(string patternName)
    {
        if (monsterPatterns.TryGetValue(patternName, out Action monsterPattern))
        {
            // 해당 카드의 기능 실행
            monsterPattern();
        }
        else
        {
            Debug.Log("MonsterPattern not found.");
        }
    }

    public void GetThisValue(Entity selectEntity)   //선택된 엔티티에서 값을 가져오는 기능
    {
        monsterhealthTMP = selectEntity.GetHealthTMP();
        monsterattackTMP = selectEntity.GetAttackTMP();
        monsterlivecount = selectEntity.GetLiveCount();
    }
}
