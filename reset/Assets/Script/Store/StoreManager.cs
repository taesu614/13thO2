using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Inst { get; private set; } //�̱���
    [SerializeField] ItemSO itemSO;
    [SerializeField] TMP_Text moneyTMP;
    List<Item> storecardlist = new List<Item>();
    List<GameObject> storecardprefablist = new List<GameObject>();
    public Transform StoreContent;
    public GameObject StoreCardPrefab;
    bool isReload = false;
    int count = 5;  //�ż� ����
    SaveData savedata;
    // Start is called before the first frame update

    private void Awake()
    {
        Inst = this; // �̱��� �ν��Ͻ� ����
    }
    private void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        SetMoneyTMP();
        LoadStoreCard();
        CardRadom();
        SetStoreCard(count);
        Debug.Log(storecardlist.Count);
    }

    private void LoadStoreCard()    //ItemSO�� Reward�� Store�� ī�� �ֱ�
    {
        foreach (Item A in itemSO.items) 
        {
            if (A.reward == "AdventureReward" || A.reward == "Sheep")
                storecardlist.Add(A);
        }
    }

    private void CardRadom() //storecardlist �����ϰ� ����
    {
        for (int i = 0; i < storecardlist.Count; i++)  
        {
            int rand = Random.Range(i, storecardlist.Count);
            Item temp = storecardlist[i];
            storecardlist[i] = storecardlist[rand];
            storecardlist[rand] = temp;
        }
    }

    private void SetStoreCard(int count)    //����ī�� ������ ����
    {
        int num = 0;
        for (int i = 0; i < storecardlist.Count; i++)
        {
            if (num >= count)
                break;
            GameObject myInstance = Instantiate(StoreCardPrefab, StoreContent);
            StoreCardButton card = myInstance.GetComponent<StoreCardButton>();
            card.Setup(storecardlist[i]);
            storecardprefablist.Add(myInstance);
            num++;
        }
    }

    public void ReLoadStoreCard()   //���� ���� 
    {
        if (isReload)
            return;
        foreach(GameObject A in storecardprefablist)    //���� �ִ� ���� ī�� ������ �ı�
        {
            Destroy(A);
        }
        storecardprefablist.Clear();
        int num = 0;
        if (storecardlist.Count < 6)    //ī�尡 5�� ���ϸ� ���� ������ ���� ����
            return;
        for (int i = count; i < storecardlist.Count; i++)
        {
            if (num >= count)
                break;
            GameObject myInstance = Instantiate(StoreCardPrefab, StoreContent);
            StoreCardButton card = myInstance.GetComponent<StoreCardButton>();
            card.Setup(storecardlist[i]);
            storecardprefablist.Add(myInstance);
            num++;
        }
        isReload = true;
        AudioManager.instance.PlaySFX(AudioManager.SFX.roulette);  // Ŭ���� �ӽ� ȿ����
    }

    public void SetMoneyTMP()
    {
        moneyTMP.text = savedata.playermoney.ToString();
    }

    public void GotoMap()
    {
        SceneManager.LoadScene("MapScene");
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // Ŭ���� �ӽ� ȿ����
    }
}
