using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardFunctionManager : MonoBehaviour
{
    private Dictionary<string, Action> cardEffects = new Dictionary<string, Action>();
    public static CardFunctionManager Inst { get; private set; }
    //����Ǵ� ������ ����
    [SerializeField] TMP_Text costTMP;  //��꿡 ���� ���̹Ƿ� num = int.Parse(costTMP); �صѰ�
    [SerializeField] TMP_Text rcostTMP;
    [SerializeField] TMP_Text gcostTMP;
    [SerializeField] TMP_Text bcostTMP;
    [SerializeField] GameObject cardPrefab;  //�� �ڽ�Ʈ X ī�忡 ���� �ڽ�Ʈ O
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
        // ī�� �̸��� ����� ��Ī�Ͽ� Dictionary�� ����
        //cardEffects["TheDevil"] = () => TheDevil (myscore); // ���ٽ� ��� ����
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

        cardEffects["SharpNib"] = SharpNib;  // ��ī�ο� ����
        cardEffects["Firestick"] = Firestick;  // �Ҳ� ��ƽ
        cardEffects["Mousefire"] = Mousefire;  // ��ҳ���
        cardEffects["CtrlZ"] = CtrlZ; // �ǵ�����
        cardEffects["Gradation"] = Gradation; // �׶��̼�
        cardEffects["WowIdea"] = WowIdea;   //��¦! ���̵��
        cardEffects["PiggyBank"] = PiggyBank;   //�Ժ� ������
        cardEffects["Layer"] = Layer;   //���̾�
        cardEffects["Brush"] = Brush;   //�귯��
        cardEffects["Woodrill"] = Woodrill; //���ٵ屸��
        cardEffects["Firefestival"] = Firefestival; // �Ҳ� ����

        cardEffects["SleepBaronets"] = SleepBaronets; // ���ڴ� �ٷγ���
        cardEffects["SleepKeeperArthur"] = SleepKeeperArthur; // ������ �Ƽ�
        cardEffects["SleepKeeperOz"] = SleepKeeperOz; // ������ ����
        cardEffects["SleepKeeperShadow"] = SleepKeeperShadow; // ������ ������
        cardEffects["SleepKeeperGooddream"] = SleepKeeperGooddream; // ������ ���
        cardEffects["SleepKeeperBaddream"] = SleepKeeperBaddream; // ������ ���
        cardEffects["Yawn"] = Yawn; // ��ǰ
    }
    //���� �� gameobj �������� �Լ�
    //public void GetEnemy(GameObject targetObj)
    //{
    //    target = targetObj;
    //    Debug.Log("Ȯ��");
    //}

    // ī�� �׼�,����,���⺰�� region�ٽ� ���� �� ��
    #region CardEffects
    #region TestCard
    private void TestBuffAttackUp() //���� �׽�Ʈ�� ī��
    {
        FindPlayer();   //�÷��̾� ã��
        player.SetStatusEffect("powerUp", 2, 5); ;    //��ƼƼ���� ���� ����Ʈ ����
        Debug.Log("���� ����");
    }

    private void TestBuffAttackDown() //���� �׽�Ʈ�� ī��
    {
        FindPlayer();   //�÷��̾� ã��
        player.SetStatusEffect("powerDown", 2, 3);   //��ƼƼ���� ���� ����Ʈ ����
        Debug.Log("���� ����");
    }

    private void TestBuffShield()
    {
        FindPlayer();
        player.SetStatusEffect("shield", 10, 5);
    }   //����� �ð����� �ѹ� ��ü������ �������� ���� -> ����ȿ���� �ٲ���Ҽ���

    private void TestAttack()   //������ ��󿡰� ���ظ� 5 �ݴϴ�
    {
        Attack("anything", 5, "normal");
    }

    private void TestFaint()    //�÷��̾� ���� 
    {
        Faint("anything", 3);
    }

    private void TestSleep()    //�÷��̾� ���� 
    {
        Sleep("anything", 10);
    }

    private void TestImmuneSleep()  //�÷��̾� ���� �鿪
    {
        ImmuneSleep("anything", 3);
    }

    private void TestPoison()   //��
    {
        Poison("anything", 4);
    }

    private void TestBurn() //ȭ��
    {
        Burn("anything", 3, 6);
    }

    private void TestHeal() //ȸ��
    {
        Heal("anything", 10);
    }

    private void TestHealTurn() //����ȸ��
    {
        HealTurn("anything", 5);
    }
    #endregion
    private void Moon() //�ڽ�Ʈ ȸ���� �� ���� ������ ���� ��� ���� 
    {
        //��, TheMoon, �ڽ�Ʈ�� 1 ����ϴ�
        int cost = int.Parse(costTMP.text);
        cost++;
        costManager.CostSetNewCost(cost);
    }

    private void ImsiCard1()    //ī�� ��ο쿡 ���� ������ �־� �ӽ÷� ����
    {
        Attack("anything", 7, "normal");
        TurnManager.OnAddCard.Invoke(true);
        TurnManager.OnAddCard.Invoke(true);
    }

    private void Encore()       //���� �����̶� �Ȱǵ帮�°� ���� �ǴܵǾ� ����
    {
        CardManager.Inst.SetIntrusionEncore();
    }

    private void SharpNib()  // ��ī�ο� ����
    {
        Attack("anything", 7, "piercing");
        ResetTarget();  //��� ���� ��� ���� �� Ÿ���� �����ؾ� ��� �������ص� ī�尡 ���Ǵ� ��Ȳ ������
    }


    private void Firestick()  // �Ҳ� ����
    {
        int randNum = UnityEngine.Random.Range(1, 3);

        for (int i = 0; i < randNum; i++)
        {
            Attack("anything", 2, "normal");
        }
        ResetTarget();  //��� ���� ��� ���� �� Ÿ���� �����ؾ� ��� �������ص� ī�尡 ���Ǵ� ��Ȳ ������
    }

    private void Mousefire()  // ��� ����
    {
        int randNum = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < randNum; i++)
        {
            Attack("all", 3, "normal");
        }
        ResetTarget();  //��� ���� ��� ���� �� Ÿ���� �����ؾ� ��� �������ص� ī�尡 ���Ǵ� ��Ȳ ������
    }

    private void Gradation()    //�׶��̼�
    {
        costManager.AddRGBCost('R');
        costManager.AddRGBCost('G');
        costManager.AddRGBCost('B');
    }

    private void WowIdea()  //��¦ ���̵��
    {
        int cost = int.Parse(costTMP.text);
        cost += 2;
        if (cost > 10)
            cost = 10;
        costManager.CostSetNewCost(cost);
    }

    private void PiggyBank()    //����������
    {
        int mycard = CardManager.Inst.GetMyCard().Count;
        if (mycard > 3)
            mycard = 3; //���� �ִ� ��ο� ���� ����
        CardManager.Inst.DiscardMyCard();
        CardManager.Inst.DiscardMyCard();
        CardManager.Inst.DiscardMyCard();
        for (int i = 0; i < mycard; i++)   //���и�ŭ ��ο�
        {
            TurnManager.OnAddCard.Invoke(true);
        }
    }
    private void Brush()    //�귯��
    {
        Attack("anything", 3, "normal");
    }
    private void Layer()    //���̾�   - ���� �����̶� ���� ������ �� ���� ������ ��
    {
        FindPlayer();
        player.SetStatusEffect("shield", 5, 2);
    }

    private void Woodrill()     //���ٵ屸�� - ���� �����̶� ���� ������ �� ���� ������ �� 
    {
        int shield = target.GetShieldTMP();
        Attack("anything", shield, "normal");
        Attack("anything", 13, "normal");
        Rebound(13, 50, player);
    }
    private void Firefestival()  // ȭ�� �Ŵ� ����� ���� ���� ������ ������� �ƴ� ���ݹ޴� �������� ���� �ϴ� ��ü ������� ��������ϴ�.
    {
        int randNumDmg;
        int randNum;

        // �� ��ü�� Ȯ�� ������ ���� �ϱ� ���� ��������ϴ�  (Success, fail (����) == ���ʺ��� ���� ���� ��ġ(0 ~ 2))
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
                A.SetStatusEffect("burn", 2, 6);    //2������ ���� ������
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
        cardManager.SearchCard("������");
        Debug.Log("������ ī�� ã�ƿ���!!");
    }

    private void SleepKeeperArthur()  // ������ �Ƽ�
    {
        FindPastCardList();
        FindPlayer();
        if (cardlist == null)
        {
            Debug.LogError("CardList ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }
        if (cardlist.CheckPast("���ڴ� �ٷγ���"))
        {
            player.SetStatusEffect("shield", 2, 12);  // ���� ȿ�� ��� ��ȣ�� �߰��� 5 �� ��°ɷ� �ӽñ���
        }
        else
            player.SetStatusEffect("shield", 2, 7);
    }

    private void SleepKeeperOz()  // ������ ����
    {
        FindPastCardList();
        Attack("anything", 7, "normal");
        if(cardlist.CheckPast("���ڴ� �ٷγ���"))
        {
            Sleep("anything", 2);
        }
    }

    private void SleepKeeperShadow()  // ������ ������
    {
        FindPastCardList();
        Attack("anything", 3, "normal");
        Attack("anything", 3, "normal");
        if (cardlist.CheckPast("���ڴ� �ٷγ���"))
        {
            TurnManager.OnAddCard.Invoke(true);
            TurnManager.OnAddCard.Invoke(true);
        }
    }

    private void SleepKeeperGooddream()  // ������ ���
    {
        Heal("player", 6);

        int cost = int.Parse(costTMP.text);
        cost += 2;
        costManager.CostSetNewCost(cost);
    }

    private void SleepKeeperBaddream()  // ������ ���
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
                Debug.Log("���� ����");
    }

    #endregion

    #endregion

    #region IntrusionEffects
    private void IntrusionEncore(string functionName)
    {
        UseSelectCard(functionName);
    }

    #endregion


    // ī�� ���� �޼���
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

    //�޼��� ��ƿ��Ƽ
    #region method
    //������ �ƴ� ��� �޼����� �ѱ� �Ű�������
    //Ÿ��, ��ġ, ���ӽð�(Ȥ�� Ƚ��), ��Ÿ ����� ����
    public void Attack(string targetcount, int damage, string type, string user = "player")   //��󿡰� ���ظ� n �ݴϴ�
    {
        FindPlayer();      //�⺻���� ���                                              //targetcount(anything: ���� �ƹ���, enemy:��, player: �÷��̾�, all:��ü, enemyall: �� ��ü 
        if (user == "player")    //���Ϳ��Ե� ���ǹǷ� ������ �÷��̾�� ���ͳĿ� ���� �޶��� ��
        {
            damage += player.GetAllAttackUpEffect();                        //damage(���ط�)��� ���ݷ� ���� ȿ�� �����ͼ� ����
            damage -= player.GetAllAttackDownEffect();                      //type (normal: ��� ����, piercing: ���� ����(��ȣ���� ������� ü�¿� ���������� ����))
        }
        if (damage < 0) //���ݷ� ���Ұ� �ʹ� �þ �������� �Ǹ� ȸ���Ǵ� ���� ����
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

    public void MemoryEffect(string cardName)  // ��� Ű����
    {

    }
    public void Rebound(int damage, int percentage, Entity user) //�����, �ݵ���ġ, �����
    {
        damage += user.GetAllAttackUpEffect();
        damage -= user.GetAllAttackDownEffect();

        damage = (int)(damage * percentage * 0.01);

        user.health -= damage;      //�켱�� �������ط�
        user.SetHealthTMP();
    }

    public void Heal(string targetcount, int heal, Entity me = null)   //ȸ�� - ���ݷ� ����, ���� ȿ���ʹ� ��� �����Ƿ� type ����
    {                                                                  //�� ���� �� me�� �ڽ��� ������ �� �� 
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

    public void CheckSleepAttack(Entity entity)  // sleep������ entity�� ���ݹ޾Ҵ��� üũ�ϴ� �޼ҵ�
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
    public void SetTarget(GameObject gameobject)    //ī��Ŵ����� ���� ã�� ���� ������Ʈ�� ��ƼƼ ����
    {
        findtarget = gameobject;
        target = findtarget.GetComponent<Entity>();
    }

    private void ResetTarget()
    {
        target = null;
    }

    private void FindAllMonster()   //Monster �±׸� ���� ��� ���� ������Ʈ ã�� ��ƼƼ ����
    {
        findmonsters = GameObject.FindGameObjectsWithTag("Monster");
        monsters = new Entity[findmonsters.Length];
        for (int i = 0; i < findmonsters.Length; i++)
        {
            monsters[i] = findmonsters[i].GetComponent<Entity>();
        }
    }

    private void FindPlayer()   //�÷��̾� �±׸� ���� ���� ������Ʈ ã�� ��ƼƼ ����
    {
        findplayer = GameObject.FindGameObjectWithTag("Player");
        player = findplayer.GetComponent<Entity>();
    }

    private void FindPastCardList()  // ���� ����Ʈ ������
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
        if (entity.GetSleep())  // ���� ������ �� ü�� ������ 
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
        GameObject myInstance = Instantiate(damageMarkPrefab, entity.transform); // �θ� ����
        DamageMark damagemark = myInstance.GetComponent<DamageMark>();
        if(damage >= 0)
        {
            damagemark.SetDamage(damage);
        }
        else
        {
            damagemark.SetDamage(-damage);  //ȸ�� �̹����� ������ ��
        }
    }
    #endregion
}