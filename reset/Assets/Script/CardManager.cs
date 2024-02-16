using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Linq;

public class CardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static CardManager Inst { get; private set;} //�̱���


    private void Awake()
    {
        Inst = this; // �̱��� �ν��Ͻ� ����
        cardfuction = GetComponent<CardFunctionManager>(); // CardFunction ������Ʈ ��������
        //cardfuction.SetCardManager(this); // CardFunctionManager Ŭ������ CardManager �ν��Ͻ� ����
        GameObject save = GameObject.Find("SaveData");  //������ �� ���� ���� SaveData�� ���� ������ ������ �ҷ���
        if(save == null)    //�ٷ� ������ �� ����ϴ� �뵵
        {
            itemBuffer = new List<Item>();
            for (int i = 0; i < itemSO.items.Length; i++)
            {
                Item item = itemSO.items[i];
                itemBuffer.Add(item);
            }
        }
        else
        {
            savedata = save.GetComponent<SaveData>();
        }
        cardlist = GameObject.Find("PastCard").GetComponent<CardList>();
    }
    [SerializeField] PlayerAnimator playerAnimation;
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;        //�� ���� 
    [SerializeField] Transform cardSpawnPoint;
    //[SerializeField] Transform otherCardSpqwnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] ECardState eCardState;
    //[SerializeField] Item item;   //�� �־�������
    private List<Item> itemBuffer;
    Card selectCard;    //���õ� ī�� ����
    CardFunctionManager cardfuction;
    public Entity player;
    public CardList cardList; //UI�� ���� ��ũ��Ʈ �������� ����
    public CostManager costManager;
    bool isMyCardDrag;
    bool onMyCardArea;
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag }
    int myPutCount; //��ƼƼ ����
    bool intrusionencore = false;
    bool intrusioncounter = false;
    SaveData savedata;
    CardList cardlist;

    private List<string> intrusionList = new List<string>();

    public List<Item> GetItemBuffer()
    {
        return itemBuffer;
    }

    public List<Card> GetMyCard()
    {
        return myCards;
    }
    public Item PopItem()   //�Ǿ��� ī�� ���� �뵵
    {
        if (itemBuffer.Count == 0)  //���� ����� ���� �� �� ä��� �뵵
            SetupPastBuffer();

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }
    void SetupItemBuffer()  //ī�� ���� �������� �ٲ�� �ϴ� �뵵
    {
        //GameObject SaveDataCard = GameObject.Find("SaveData");
        //ItemSO itemSO = Resources.Load<ItemSO>("ItemSO/ItemSO");
        if (savedata == null || savedata.GetPlayerDeck().Count == 0)    //���� ��������� �� �⺻ 2�徿 ����ϵ��� ����
        {
            itemBuffer = new List<Item>();
            for (int i = 0; i < itemSO.items.Length; i++)
            {
                Item item = itemSO.items[i];
                itemBuffer.Add(item);
            }
        }
        else
        {
            itemBuffer = new List<Item>();
            for(int i = 0; i < savedata.GetPlayerDeck().Count; i++)
               {
                   Item item = savedata.GetPlayerDeck()[i];
                   itemBuffer.Add(item);
               }
        }
          

        for (int i = 0; i < itemBuffer.Count; i++ )
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    void SetupPastBuffer()  //���ſ��� ī�带 �̾ƿ��� �뵵
    {
        itemBuffer = new List<Item>();
        for (int i = 0; i < cardlist.GetPast().Count; i++)
        {
            Item item = cardlist.GetPast()[i];
            itemBuffer.Add(item);
        }
        cardlist.ClearItems();

        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    void ShuffleItemBuffer()  // �� ���� �뵵
    {
        for(int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    void Start()
    {
        SetupItemBuffer();
        TurnManager.OnAddCard += AddCard;
    }

    void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;    
    }

    void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        DetectCardArea();
        SetECardState();
    }
    
    void AddCard(bool isMine)       //ī�� ��ο� ����
    {
        if(cardlist.GetPast().Count <= 0 && itemBuffer.Count <= 0)
        {

        }
        else
        {
            if(isMine)
            {
                var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
                var card = cardObject.GetComponent<Card>();
                card.Setup(PopItem(), isMine);
                if(isMine)
                    myCards.Add(card);
            
                SetOriginOrder(isMine);
                CardAlignment();
                AudioManager.instance.PlaySFX(AudioManager.SFX.draw);
            }
        }
    }

    void SetOriginOrder(bool isMine)
    {
        int count = myCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];
            targetCard.GetComponent<Order>().SetOriginOrder(i);
        }
    }

   void CardAlignment()  // ī�� ����
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 0.3f);  //�� �� ������ ���� �� ī�� ������ �����
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if(objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }

        return results;
    }

    public bool TryPutCard(bool isMine)    //��ƼƼ ����
    {
        Card card = isMine ? selectCard : selectCard;
        var targetCards = isMine ? myCards : myCards;
        if(card != null)    //ī�� �Ҹ��ؼ� �̹� ������µ� ��ĥ�� ����ϴ� �뵵
        {
            targetCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);
        }

        if(isMine)
        {
            selectCard = null;
            myPutCount++;
        }
        CardAlignment();
        return true;
    }

    public void DiscardMyCard() //ī�带 ������ ���ŷ� ��
    {
        CardAlignment();
        if(myCards.Count > 0 && myCards[0] != null)    //Null���۷��� ����
        {
            Card card = myCards[0];
            cardList.AddCard(card.item);
            myCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);
            selectCard = null;
            CardAlignment();
        }
    }

    public void CheckMyCard()       //ī�� 8�� �ʰ� �� ������ ��� 
    {
        int nowcard = myCards.Count;
        if (nowcard > 8)
        {
           for(int i = 0; i < nowcard - 8; i++)
           {
                DiscardMyCard();
           }
        }
    }

    public void UseCard()
    {
        switch (selectCard.cardtype)  // �̰��� ī�� Ÿ�Կ� ���� ȿ���� �ֱ�
        {
            case "Action":
                AudioManager.instance.PlaySFX(AudioManager.SFX.attack);
                break;

            default:
                AudioManager.instance.PlaySFX(AudioManager.SFX.attack);
                break;
        }
        if (cardfuction != null)
        {
            cardList.AddCard(selectCard.item);
            GetSelectCardType(selectCard.cardtype, selectCard.functionname);
            cardfuction.UseSelectCard(selectCard.functionname);
        }
    }

    public void SearchCard(string tag)  // Ư�� �±׸� ���� ī�带 �з� ���ϴ� ���
    {
        if (itemBuffer != null && itemBuffer.Count > 0)
        {
            string[] tempTag = new string[itemBuffer.Count];
            for (int i = 0; i < itemBuffer.Count; i++)
            {
                tempTag[i] = itemBuffer[i].tag;
            }


            int tagIndex = Array.FindLastIndex(tempTag, i => i == tag);
            if (tagIndex != -1)
            {
                Item temp = itemBuffer[0];
                itemBuffer[0] = itemBuffer[tagIndex];
                itemBuffer[tagIndex] = temp;

                AddCard(true);
                ShuffleItemBuffer();
            }

            else
            {
                Debug.Log("��ġ�� �� �ִ� ī�尡 �����ϴ�!");
            }
        }

        else
        {
            Debug.Log("��ġ�� �� �ִ� ī�尡 �����ϴ�!");
        }
    }

    #region MyCard
    public void CardMouseOver(Card card)    //ī�� ���� ���콺�� �÷� ���� ��(��� ���� X) 
    {
        if (eCardState == ECardState.Nothing)
            return;

        selectCard = card;
        SetMessage(card, true);
        EnlargeCard(true, card);
    }

    

    public void CardMouseExit(Card card)    //ī�� ���� ���콺�� �� ��(��� ���� X) 
    {
        Transform message = card.transform;
        Transform messagetransform = message.Find("Message");
        CardMessage cardmessage = messagetransform.GetComponent<CardMessage>();
        SetMessage(card, false);
        EnlargeCard(false, card);
    }

    public void CardMouseDown() //ī�� ��� �� ���콺 ���� ��
    {
        if(selectCard == null)
        {
            return;
        }
        if(player.canplay)
        {
            if (eCardState != ECardState.CanMouseDrag)
            {
                return;
            }
            if (selectCard.cardtype == "Intrusion")   //Ŭ�� �� ���� ���� ����, �ߺ� Ȯ�� �뵵
            {
                if (IsFullList())
                {
                    GameManager.Inst.Notification("������ �ִ� 5���Դϴ�.");
                    return;
                }
                if (IsIntrusionDuplication(selectCard.functionname))
                {
                    GameManager.Inst.Notification("�ߺ��� ������ ������� �ʽ��ϴ�.");
                    return;
                }

            }
            if (costManager.CompareCost(selectCard))    //�ڽ�Ʈ ��
            {
                isMyCardDrag = true;
                if (selectCard.selectable)
                    selectCard.ChangeCardImage(true);
                if (onMyCardArea)   
                {
                    selectCard.ChangeCardImage(false);  //�̹��� ������ �̻������� ���� ������
                    return;
                }

            }
            else if(!costManager.CompareCost(selectCard))
                GameManager.Inst.Notification("�ڽ�Ʈ�� �����մϴ�");
        }
    }

    public void CardMouseUp()   //���콺�� �� �� ī�� ���
    {
        if (selectCard == null)
        {
            return;
        }
        Debug.Log(selectCard.functionname);
        selectCard.ChangeCardImage(false);
        if (!player.canplay)  //ī�� ��� �ൿ �������� üũ (ex: ����)
        {
            GameManager.Inst.Notification("�������� ���¿����� ī�� ����� �Ұ����մϴ�");
            return;
        }
            
        isMyCardDrag = false;

        if (eCardState != ECardState.CanMouseDrag)
            return;
        if (selectCard.cardtype == "Intrusion")//���� ����
        {
            if (IsFullList())
            {
                return;
            }
            if (IsIntrusionDuplication(selectCard.functionname))
            {
                return;
            }
        }

        if (costManager.CompareCost(selectCard))//�ڽ�Ʈ ��
        {
            if (onMyCardArea)
            {
            }
            else
            {
                bool isObjectin = false;
                GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                GameObject[] entity = monsters.Concat(players).ToArray();
                if (selectCard.selectable)   //���� �������� ����
                {
                    foreach (GameObject obj in entity)
                    {
                        if (IsMouseCollidingWithObject(obj))
                        {
                            cardfuction.SetTarget(obj);
                            isObjectin = true;
                            break;
                        }
                    }
                }
                else if (!selectCard.selectable)  //��ü�����̹Ƿ� ������Ʈ�� ���ִ� ����
                {
                    isObjectin = true;
                }
                else   //Ÿ���� �������� �ʰų�, ��ü ���ذ� �ƴ� ���
                {
                    isObjectin = false;
                }
                if (isObjectin) //�������� ī�� ��� ����, false��� ������� �����Ƿ� ī�� ��� �ȵ�
                {
                    playerAnimation.SetPlayerState("attack");
                    CostManager.Inst.SubtractCost(selectCard);
                    CostManager.Inst.ShowCost();
                    UseCard();
                    IntrusionConditionCheck();
                    EntityManager.Inst.FindDieEntity();
                    TryPutCard(true);
                }
            }
        }
        
    }

    void CardDrag() //ī�� �巡�� ���� ��
    {
        SetMessage(selectCard, false);
        if (!onMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
        }
        else if(!onMyCardArea)     //���� �ؾ��ϴ� ���̽��� ���
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            //Debug.Log("select");
        }
        
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    void EnlargeCard(bool isEnlarge, Card card) //ī�� Ȯ��
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -3.5f, -0.1f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 0.45f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void SetECardState()
    {
        if (TurnManager.Inst.isLoading)
            eCardState = ECardState.Nothing;

        else if (!TurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseOver;

        else if (TurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseDrag;     
    }

    #endregion
    //����
    #region Intrusion 
    //���ڸ�
    #region Encore
    public void SetIntrusionEncore()
    {
        intrusionencore = true;
    }
    public bool ConditionIntrusionEncore()  //���ڸ� ���� Ȯ��
    {
        if (selectCard.cardtype == "Action" && EntityManager.Inst.IsDieEntity())
            return true;
        return false;
    }
    public void UseIntrusionEncore()    //���ڸ� �ɷ� �ߵ�
    {
        cardfuction.UseSelectCard(selectCard.functionname);
        intrusionList.Remove("Encore"); //����Ʈ �����
    }
    #endregion
    //�ݰ�
    #region Counter
    public void SetIntrusionCounter()
    {
        intrusioncounter = true;
    }
    public bool ConditionIntrusionCounter()  //�ݰ� ���� Ȯ��
    {
        if (selectCard.cardtype == "Action" && EntityManager.Inst.IsDieEntity())
            return true;
        return false;
    }

    public void UseIntrusionCounter()    //�ݰ� �ɷ� �ߵ�
    {
        cardfuction.UseSelectCard(selectCard.functionname);
        intrusionList.Remove("Counter"); //����Ʈ �����
    }

    #endregion

    public void GetSelectCardType(string type, string functionName)
    {
        if (type == "Intrusion")
        {
            intrusionList.Add(functionName);
        }
    }

    public bool IsIntrusionDuplication(string function)
    {
        foreach(string key in intrusionList)
        {
            if(key == function)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsFullList()    //��ġ�� ���� ī���� ���� ����
    {
        if (intrusionList.Count >= 5)
        {
            return true;
        }
        return false;
    }

    public void IntrusionConditionCheck()   //���� Ȯ�ο�
    {
        if(intrusionencore == true && ConditionIntrusionEncore() == true)   //���ڸ� ���� Ȯ��
        {
            UseIntrusionEncore();
        }
    }
    #endregion

    bool IsMouseCollidingWithObject(GameObject obj)
    {
        // ���ϴ� ���̾� ����ũ ���� (��: "Entity" ���̾ ���)
        int layerMask = LayerMask.GetMask("Entity");

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition, layerMask);

        if (hitCollider != null && hitCollider.gameObject == obj)
        {
            // ���콺�� obj�� �浹�� ���
            return true;
        }

        return false; // �浹���� ���� ���
    }

    void SetMessage(Card card, bool isDrag)
    {
        Transform message = card.transform;
        Transform messagetransform = message.Find("Message");
        CardMessage cardmessage = messagetransform.GetComponent<CardMessage>();
        cardmessage.ShowCardMessage(isDrag);
    }
}
