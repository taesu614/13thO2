using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour  //���ڸ� �������� ��ü�� ���ɼ� ����
{
    public static EntityManager Inst { get; private set; }
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
    [SerializeField] GameObject PlayerWin;

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
    List<GameObject> nowMonsterList = new List<GameObject>();

    void Start()
    {
        myplayer = GameObject.Find("MyPlayer");
        myplayerentity = myplayer.GetComponent<Entity>();
        SetupMonsterBuffer();
        SetPlayer();
        AddEntity(0);       //���� ��ǥ �� �̹����� ���� �����ؼ��� ������ ��
        AddEntity(2.5f);
        PlayerWin.SetActive(false);
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
        GameObject entityObject = Instantiate(entityPrefab, new Vector3(4.3f + distance, -1.5f, 0), Quaternion.identity);
        var monster = entityObject.GetComponent<Entity>();
        monster.Setup(PopMonster());
        nowMonsterList.Add(entityObject);
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
        //GameObject[] entities = GameObject.FindGameObjectsWithTag("Monster");
        for(int i = nowMonsterList.Count-1; i >= 0; i--) 
        {
            Entity selectEntity = nowMonsterList[i].GetComponent<Entity>();
            if (selectEntity.health < 1)
            {
                selectEntity.isDie = true;
                //AudioManager.instance.PlaySFX(AudioManager.SFX.);  // ���� ���� �� ȿ����
            }

            if (!selectEntity.isDie)
                continue;
            Destroy(nowMonsterList[i]);
            nowMonsterList.Remove(nowMonsterList[i]);

            //entities = GameObject.FindGameObjectsWithTag("Monster");
            continue;
        }
        if (nowMonsterList.Count < 1)
            StartCoroutine(FinishBattle());
    }

    IEnumerator FinishBattle() //�̺�Ʈ ��� ��Ű��ó ��� �̼����� ���� �������� �ű�
    {
        int getmoney = 10;
        AudioManager.instance.ChangeBGMVolume(-80);
        AudioManager.instance.PlaySFX(AudioManager.SFX.success);
        PlayerWin.SetActive(true);
        yield return new WaitForSeconds(3.0f);

        //���� ���� �� ������ ���� - �þ �ڵ� �з� �����ؼ� �����Ұ�
        GameObject savedata = GameObject.Find("SaveData");
        SaveData playerdata = savedata.GetComponent<SaveData>();
        GameObject player = GameObject.Find("MyPlayer");
        Entity playernow = player.GetComponent<Entity>();

        int moneyNow = playerdata.GetPlayerMoney();
        int plusmoney = moneyNow + getmoney;  // �ϴ� �ӽ÷� ���� ���̸� �������� 10�� �߰��ϴ°ɷ� �߽��ϴ�.
        playerdata.SetPlayerGetMoney(getmoney);
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
}
