using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour  //별자리 전용으로 교체될 가능성 높음
{
    public static EntityManager Inst { get; private set; }
    private void Awake()
    {
        Inst = this; // 싱글톤 인스턴스 설정
        //monsterpattern = GetComponent<MonsterPatternManager>(); // monsterpattern 컴포넌트 가져오기
    }
    [SerializeField] MonsterSO monsterSO;
    [SerializeField] GameObject entityPrefab;
    [SerializeField] List<Entity> myEntities;
    [SerializeField] List<Entity> otherEntites;
    [SerializeField] GameObject targetPicker;
    [SerializeField] Entity myEmptyEntity;
    [SerializeField] Entity myBossEntity;
    [SerializeField] Entity otherBossEntity;
    [SerializeField] GameObject EntityPrefab;
    [SerializeField] GameObject PlayerWin;

    const int MAX_ENTITY_COUNT = 6; //엔티티 최대 개수 및 정렬
    public bool IsFullMyEntities => myEntities.Count >= MAX_ENTITY_COUNT && !ExistMyEmptyEntity;
    bool IsFullOtherEntities => otherEntites.Count >= MAX_ENTITY_COUNT;
    bool ExistTargetPickEntity => targetPickEntity != null;
    bool ExistMyEmptyEntity => myEntities.Exists(x => x == myEmptyEntity);
    int MyEmptyEntityIndex => myEntities.FindIndex(x => x == myEmptyEntity);
    bool CanMouseInput => TurnManager.Inst.myTurn && !TurnManager.Inst.isLoading;

    Entity selectEntity;
    Entity targetPickEntity;
    GameObject myplayer;
    Entity myplayerentity;
    WaitForSeconds delay1 = new WaitForSeconds(1);
    List<Monster> monsterBuffer;
    List<GameObject> nowMonsterList = new List<GameObject>();

    void Start()
    {
        myplayer = GameObject.Find("MyPlayer");
        myplayerentity = myplayer.GetComponent<Entity>();
        SetupMonsterBuffer();
        SetPlayer();
        AddEntity(0);       //향후 좌표 외 이미지와 패턴 관련해서도 수정할 것
        AddEntity(2.5f);
        PlayerWin.SetActive(false);
        TurnManager.OnTurnStarted += OnTurnStarted;
    }

    void SetPlayer()
    {
        GameObject savedata = GameObject.Find("SaveData");
        if(!savedata)   //실행 편하게 하려고 만든 곳 제출 전 지울 것
        {
            savedata = GameObject.Find("willdelete");
        }
        SaveData playerdata = savedata.GetComponent<SaveData>();    //전투 시작 전 저장된 데이터
        GameObject player = GameObject.Find("MyPlayer");
        Entity playernow = player.GetComponent<Entity>();           //게임상에 보여질 데이터, 전투 후 설정될 데이터  

        playernow.health = playerdata.GetPlayerHealth();
        playernow.maxhealth = playerdata.GetPlayerMaxHealth();
        playernow.SetHealthTMP();
    }

    void SetupMonsterBuffer()    //스테이지를 구성한다 한들  하급 상급 엘리트 보스 4가지 틀에서 랜덤으로 정해지지 않을까 해서 일단 만들어봄
    {
        monsterBuffer = new List<Monster>();
        for (int i = 0; i < monsterSO.monsters.Length; i++)
        {
            Monster monster = monsterSO.monsters[i];
            for (int j = 0; j < monster.percent; j++)
                monsterBuffer.Add(monster);
        }

        for (int i = 0; i < monsterBuffer.Count; i++)
        {
            int rand = Random.Range(i, monsterBuffer.Count);
            Monster temp = monsterBuffer[i];
            monsterBuffer[i] = monsterBuffer[rand];
            monsterBuffer[rand] = temp;
        }
    }

    public Monster PopMonster()
    {
        if (monsterBuffer.Count == 0)
            SetupMonsterBuffer();

        Monster monster = monsterBuffer[0];
        monsterBuffer.RemoveAt(0);
        return monster;
    }

    void AddEntity(float distance)
    {
        GameObject entityObject = Instantiate(entityPrefab, new Vector3(4.3f + distance, -1.5f, 0), Quaternion.identity);
        var monster = entityObject.GetComponent<Entity>();
        monster.Setup(PopMonster());
        nowMonsterList.Add(entityObject);
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;   
    }

    void OnTurnStarted(bool myTurn) //적의 행동 관련 내용
    {
        if (!myTurn)
            StartCoroutine(AICo());
    }

    private void Update()
    {
        ShowTargetPicker(ExistTargetPickEntity);
        if(myplayerentity.health<=0)    //임시용
        {
            GameObject obj = GameObject.Find("Canvas").transform.Find("GameOverCanvas").gameObject;
            GameObject ani = GameObject.Find("MyPlayer").transform.Find("MyBoss").gameObject;

            Animator a = ani.GetComponent<Animator>();

            obj.SetActive(true);
            Destroy(a);

            AudioManager.instance.ChangeBGMVolume(0);
        }
    }

    IEnumerator AICo()  //적 턴 시작시 AI 코루틴 시작됨
    {
        // Monster 태그를 가진 오브젝트들 중에서 첫 번째를 가져옴
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Monster");   //여러마리일 경우 각각 자신의 패턴을 진행함

        foreach(GameObject entityObject in entities)
        {
            Entity selectEntity = entityObject.GetComponent<Entity>();
            selectEntity.GetAllCC();    //엔티티에게서 CC효과 불러옴
            if(selectEntity.canplay == false)
            {
                selectEntity.canplay = true;
                Debug.Log("This Entity can't attack");
                continue;
            }
            // selectEntity가 여전히 null인 경우, 기본 동작 수행
            if (selectEntity != null)
            {
                yield return delay1;
                selectEntity.ExecutePattern(selectEntity.monsterfunctionname);
                //공격로직
            }
            else
            {
                // selectEntity가 비어있을 때 기본 동작 수행
                // 예시: 랜덤하게 적절한 몬스터를 선택하거나, 기본적인 AI 동작 수행
                Debug.Log("selectEntity is null. Performing default AI action.");
            }
        }
        FindDieEntity();

        TurnManager.Inst.EndTrun();
    }

    public void FindDieEntity()
    {
        //GameObject[] entities = GameObject.FindGameObjectsWithTag("Monster");
        for(int i = nowMonsterList.Count-1; i >= 0; i--) 
        {
            Entity selectEntity = nowMonsterList[i].GetComponent<Entity>();
            if (selectEntity.health < 1)
            {
                selectEntity.isDie = true;
                //AudioManager.instance.PlaySFX(AudioManager.SFX.);  // 몬스터 죽을 때 효과음
            }

            if (!selectEntity.isDie)
                continue;
            Destroy(nowMonsterList[i]);
            nowMonsterList.Remove(nowMonsterList[i]);

            //entities = GameObject.FindGameObjectsWithTag("Monster");
            continue;
        }
        if (nowMonsterList.Count < 1)
            StartCoroutine(WaitForSecondsExample());
    }

    IEnumerator WaitForSecondsExample() //이벤트 기반 아키텍처 사용 미숙으로 인해 이쪽으로 옮김
    {
        PlayerWin.SetActive(true);
        yield return new WaitForSeconds(3.0f);

        //게임 종료 후 데이터 저장 - 늘어날 코드 분량 생각해서 정리할것
        GameObject savedata = GameObject.Find("SaveData");
        SaveData playerdata = savedata.GetComponent<SaveData>();
        GameObject player = GameObject.Find("MyPlayer");
        Entity playernow = player.GetComponent<Entity>();

        int moneyNow = playerdata.GetPlayerMoney();
        int plusmoney = moneyNow + 10;  // 일단 임시로 몬스터 죽이면 고정으로 10원 추가하는걸로 했습니다.

        playerdata.SetPlayerHealth(playernow.health);
        playerdata.SetPlayerMoney(plusmoney);
        SceneManager.LoadScene("RewardScene");
    }

    public bool IsDieEntity()
    {
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject entityObject in entities)
        {
            Entity selectEntity = entityObject.GetComponent<Entity>();
            if (selectEntity.health < 1)
            {
                return true;
            }
        }
        return false;
    }

    void EntityAlignment(bool isMine)   //엔티티 정렬 이걸 활용해서 게임 시작 후 몬스터 소환 시 정렬해야할듯
    {
        float targetY = isMine ? -1f : 4.15f;
        var targetEntities = isMine ? myEntities : otherEntites;

        for(int i = 0; i < targetEntities.Count; i++)
        {
            float targetX = (targetEntities.Count - 1) * -3.4f + i * 6.8f;

            var targetEntity = targetEntities[i];
            targetEntity.originPos = new Vector3(targetX, targetY, 0);
            targetEntity.MoveTransform(targetEntity.originPos, true, 0.5f);
            targetEntity.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    public void InsertMyEmptyEntity(float xPos) //마우스 필드 드래그 시 엔티티 리스트 위치 변경용인듯? 하스에서 하수인 놔두기 전에 미리 배치하는 그거
    {
        if (IsFullMyEntities)
        {
            return;
        }
        if (!ExistMyEmptyEntity)
            myEntities.Add(myEmptyEntity);

        Vector3 emptyentityPos = myEmptyEntity.transform.position;
        emptyentityPos.x = xPos;
        myEmptyEntity.transform.position = emptyentityPos;

        int _emptyEntityIndex = MyEmptyEntityIndex;
        myEntities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));
        if (MyEmptyEntityIndex != _emptyEntityIndex)
            EntityAlignment(true);
    }

    public void RemoveMyEmptyEntity()   //InsertMyEmptyEntity랑 같이 쓰는 용도인듯
    {
        if (!ExistMyEmptyEntity)
            return;

        myEntities.RemoveAt(MyEmptyEntityIndex);
        EntityAlignment(true);
    }

    public void EntityMouseDown(Entity entity)
    {
        if (!CanMouseInput)
            return;

        selectEntity = entity;
    }    
    
    public void EntityMouseUp()
    {
        if (!CanMouseInput)
            return;

        // selectEntity, targetPickEntity 둘 다 존재하면 공격한다. 바로 null, null로 만든다.
        if (selectEntity && targetPickEntity && selectEntity.attackable)
            Attack(selectEntity, targetPickEntity);

        selectEntity = null;
        targetPickEntity = null;
    }    
    
    public void EntityMouseDrag()
    {
        if (!CanMouseInput || selectEntity == null)
        {
            return;
        }
            

        //Other 타겟 엔티티 찾기
        bool existTarget = false;
        foreach (var hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {
            Entity entity = hit.collider?.GetComponent<Entity>();
            if(entity != null && !entity.isMine && selectEntity.attackable) //적꺼 선택 이거 활용하면 내꺼 선택도 가능할듯
            {
                targetPickEntity = entity;
                existTarget = true;
                break;
            }
        }
        if (!existTarget)
            targetPickEntity = null;
    }

    void Attack(Entity attacker, Entity defender)
    {
        //_attacker가 _defende 의 위치로 이동하다 원래 위치로 온다, 이때 order가 높다
        attacker.attackable = false;
        attacker.GetComponent<Order>().SetMostFrontOrder(true);
        Sequence sequence = DOTween.Sequence()
            .Append(attacker.transform.DOMove(defender.transform.position, 0.4f)).SetEase(Ease.InSine)
            .AppendCallback(() =>
            {
                //데미지 주고 받기  
                attacker.Damaged(defender.attack);
                defender.Damaged(attacker.attack);
            })
            .Append(attacker.transform.DOMove(attacker.originPos, 0.4f)).SetEase(Ease.OutSine)
            .OnComplete(() => { });//죽음처리
    }

    void ShowTargetPicker(bool isShow)
    {
        targetPicker.SetActive(isShow);
        if (ExistTargetPickEntity)
            targetPicker.transform.position = targetPickEntity.transform.position;
    }
}
