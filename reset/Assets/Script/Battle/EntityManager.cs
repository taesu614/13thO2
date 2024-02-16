using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class EntityManager : MonoBehaviour  //���ڸ� �������� ��ü�� ���ɼ� ����
{
    public static EntityManager Inst { get; private set; }
    public delegate void EntityEvent(); //��������Ʈ ����
    public static event EntityEvent EventEntitySpawn;   //���� ������ �˸���
    public static event EntityEvent EventEntityDestroy; //���� �ı��� �˸���
    private void Awake()
    {
        Inst = this; // �̱��� �ν��Ͻ� ����
        //monsterpattern = GetComponent<MonsterPatternManager>(); // monsterpattern ������Ʈ ��������
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

    const int MAX_ENTITY_COUNT = 6; //��ƼƼ �ִ� ���� �� ����
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

    void Start()
    {
        myplayer = GameObject.Find("MyPlayer");
        myplayerentity = myplayer.GetComponent<Entity>();
        SetupMonsterBuffer();
        SetPlayer();
        AddEntity(0);       //���� ��ǥ �� �̹����� ���� �����ؼ��� ������ ��
        AddEntity(2.5f);
        TurnManager.OnTurnStarted += OnTurnStarted;
    }

    void SetPlayer()
    {
        GameObject savedata = GameObject.Find("SaveData");
        if(!savedata)   //���� ���ϰ� �Ϸ��� ���� �� ���� �� ���� ��
        {
            savedata = GameObject.Find("willdelete");
        }
        SaveData playerdata = savedata.GetComponent<SaveData>();    //���� ���� �� ����� ������
        GameObject player = GameObject.Find("MyPlayer");
        Entity playernow = player.GetComponent<Entity>();           //���ӻ� ������ ������, ���� �� ������ ������  

        playernow.health = playerdata.GetPlayerHealth();
        playernow.maxhealth = playerdata.GetPlayerMaxHealth();
        playernow.SetHealthTMP();
    }

    void SetupMonsterBuffer()    //���������� �����Ѵ� �ѵ�  �ϱ� ��� ����Ʈ ���� 4���� Ʋ���� �������� �������� ������ �ؼ� �ϴ� ����
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
        var entityObject = Instantiate(entityPrefab, new Vector3(4.3f + distance, -1.5f, 0), Quaternion.identity);
        var monster = entityObject.GetComponent<Entity>();
        monster.Setup(PopMonster());
        EventEntitySpawn(); //�̺�Ʈ ���� ���� �˸�
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;   
    }

    void OnTurnStarted(bool myTurn) //���� �ൿ ���� ����
    {
        if (!myTurn)
            StartCoroutine(AICo());
    }

    private void Update()
    {
        ShowTargetPicker(ExistTargetPickEntity);
        if(myplayerentity.health<=0)    //�ӽÿ�
        {
            GameObject obj = GameObject.Find("Canvas").transform.Find("GameOverCanvas").gameObject;
            GameObject ani = GameObject.Find("MyPlayer").transform.Find("MyBoss").gameObject;

            Animator a = ani.GetComponent<Animator>();

            obj.SetActive(true);
            Destroy(a);

            AudioManager.instance.ChangeBGMVolume(0);

        }


    }

    IEnumerator AICo()  //�� �� ���۽� AI �ڷ�ƾ ���۵�
    {
        // Monster �±׸� ���� ������Ʈ�� �߿��� ù ��°�� ������
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Monster");   //���������� ��� ���� �ڽ��� ������ ������

        foreach(GameObject entityObject in entities)
        {
            Entity selectEntity = entityObject.GetComponent<Entity>();
            selectEntity.GetAllCC();    //��ƼƼ���Լ� CCȿ�� �ҷ���
            if(selectEntity.canplay == false)
            {
                selectEntity.canplay = true;
                Debug.Log("This Entity can't attack");
                continue;
            }
            // selectEntity�� ������ null�� ���, �⺻ ���� ����
            if (selectEntity != null)
            {
                yield return delay1;
                selectEntity.ExecutePattern(selectEntity.monsterfunctionname);
                //���ݷ���
            }
            else
            {
                // selectEntity�� ������� �� �⺻ ���� ����
                // ����: �����ϰ� ������ ���͸� �����ϰų�, �⺻���� AI ���� ����
                Debug.Log("selectEntity is null. Performing default AI action.");
            }
        }
        FindDieEntity();

        TurnManager.Inst.EndTrun();
    }

    public void FindDieEntity()
    {
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject entityObject in entities)
        {
            Entity selectEntity = entityObject.GetComponent<Entity>();
            if (selectEntity.health < 1)
            {
                selectEntity.isDie = true;
                //AudioManager.instance.PlaySFX(AudioManager.SFX.);  // ���� ���� �� ȿ����
            }

            if (!selectEntity.isDie)
                continue;
            if (selectEntity.isMine)
            {
                myEntities.Remove(selectEntity);
                Destroy(selectEntity.gameObject);
                entities = GameObject.FindGameObjectsWithTag("Monster");
                continue;
            }
            else
            {
                EventEntityDestroy();   //���� �ı� �˸�
                otherEntites.Remove(selectEntity);
                Destroy(selectEntity.gameObject);
                entities = GameObject.FindGameObjectsWithTag("Monster");
                continue;
            }
        }
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

    void EntityAlignment(bool isMine)   //��ƼƼ ���� �̰� Ȱ���ؼ� ���� ���� �� ���� ��ȯ �� �����ؾ��ҵ�
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

    public void InsertMyEmptyEntity(float xPos) //���콺 �ʵ� �巡�� �� ��ƼƼ ����Ʈ ��ġ ������ε�? �Ͻ����� �ϼ��� ���α� ���� �̸� ��ġ�ϴ� �װ�
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

    public bool SpawnEntity(bool isMine, Monster monster, Vector3 spawnPos)
    {
        InsertMyEmptyEntity(0f);
        //if (isMine)
        //{
        //    if (IsFullMyEntities || !ExistMyEmptyEntity)
        //        return false;
        //}
        //else
        //{
        //    if (IsFullMyEntities)
        //        return false;
        //}

        var entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI);
        var entity = entityObject.GetComponent<Entity>();

        if (isMine)
            myEntities[MyEmptyEntityIndex] = entity;
        else
            otherEntites.Insert(Random.Range(0, otherEntites.Count), entity);

        entity.isMine = isMine;
        entity.Setup(monster);
        EntityAlignment(isMine);

        return true;
    }

    public void RemoveMyEmptyEntity()   //InsertMyEmptyEntity�� ���� ���� �뵵�ε�
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

        // selectEntity, targetPickEntity �� �� �����ϸ� �����Ѵ�. �ٷ� null, null�� �����.
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
            

        //Other Ÿ�� ��ƼƼ ã��
        bool existTarget = false;
        foreach (var hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {
            Entity entity = hit.collider?.GetComponent<Entity>();
            if(entity != null && !entity.isMine && selectEntity.attackable) //���� ���� �̰� Ȱ���ϸ� ���� ���õ� �����ҵ�
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
        //_attacker�� _defende �� ��ġ�� �̵��ϴ� ���� ��ġ�� �´�, �̶� order�� ����
        attacker.attackable = false;
        attacker.GetComponent<Order>().SetMostFrontOrder(true);
        Sequence sequence = DOTween.Sequence()
            .Append(attacker.transform.DOMove(defender.transform.position, 0.4f)).SetEase(Ease.InSine)
            .AppendCallback(() =>
            {
                //������ �ְ� �ޱ�  
                attacker.Damaged(defender.attack);
                defender.Damaged(attacker.attack);
            })
            .Append(attacker.transform.DOMove(attacker.originPos, 0.4f)).SetEase(Ease.OutSine)
            .OnComplete(() => { });//����ó��
    }

    void ShowTargetPicker(bool isShow)
    {
        targetPicker.SetActive(isShow);
        if (ExistTargetPickEntity)
            targetPicker.transform.position = targetPickEntity.transform.position;
    }
}
