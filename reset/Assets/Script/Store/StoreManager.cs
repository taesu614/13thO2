using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Inst { get; private set; } //싱글톤
    [SerializeField] ItemSO itemSO;
    [SerializeField] TMP_Text moneyTMP;
    List<Item> storecardlist = new List<Item>();
    List<GameObject> storecardprefablist = new List<GameObject>();
    public Transform StoreContent;
    public GameObject StoreCardPrefab;
    bool isReload = false;
    int count = 5;  //매수 관련
    SaveData savedata;
    // Start is called before the first frame update

    private void Awake()
    {
        Inst = this; // 싱글톤 인스턴스 설정
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

    private void LoadStoreCard()    //ItemSO내 Reward가 Store인 카드 넣기
    {
        foreach (Item A in itemSO.items) 
        {
            if (A.reward == "AdventureReward" || A.reward == "Sheep")
                storecardlist.Add(A);
        }
    }

    private void CardRadom() //storecardlist 랜덤하게 섞기
    {
        for (int i = 0; i < storecardlist.Count; i++)  
        {
            int rand = Random.Range(i, storecardlist.Count);
            Item temp = storecardlist[i];
            storecardlist[i] = storecardlist[rand];
            storecardlist[rand] = temp;
        }
    }

    private void SetStoreCard(int count)    //상점카드 프리팹 생성
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

    public void ReLoadStoreCard()   //상점 리롤 
    {
        if (isReload)
            return;
        foreach(GameObject A in storecardprefablist)    //원래 있던 상점 카드 프리팹 파괴
        {
            Destroy(A);
        }
        storecardprefablist.Clear();
        int num = 0;
        if (storecardlist.Count < 6)    //카드가 5장 이하면 굳이 리롤할 이유 없음
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
        AudioManager.instance.PlaySFX(AudioManager.SFX.roulette);  // 클릭시 임시 효과음
    }

    public void SetMoneyTMP()
    {
        moneyTMP.text = savedata.playermoney.ToString();
    }

    public void GotoMap()
    {
        SceneManager.LoadScene("MapScene");
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // 클릭시 임시 효과음
    }
}
