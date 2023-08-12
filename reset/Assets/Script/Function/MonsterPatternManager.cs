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

       // monsterPatterns["Snail"] = () => SnailPattern();
    }
}
