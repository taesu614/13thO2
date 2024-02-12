using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Inst { get; private set; } //싱글톤
    [SerializeField] GameObject RewardCardPrefab;   //보상이 될 카드 프리팹 넣기 
    [SerializeField] ItemSO itemSO;                 //ItemSO : 카드 데이터 관련
    [SerializeField] GameObject carndnameuiprefab;   //덱에 표시할  카드 이름의 프리팹 
    Transform RewardContent;     //카드 보상 넣을 Content의 Transform
    public Transform DeckContent;          //덱 리스트 넣을 Content의 Transfrom
    List<Item> RewardList = new List<Item>();  //보상에 들어갈 리스트
    GameObject deckscrollview;
    SaveData savedata;
    List<Item> decktemp = new List<Item>();
    private void Start()        //씬 전환 뒤에 한번만 사용하기에 Start에서 관리함 
    {
        deckscrollview = GameObject.Find("DeckScrollView");
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        RewardContent = GameObject.Find("RewardContent").GetComponent<Transform>();
        MakeDeck();
        SetRewardList();
        deckscrollview.SetActive(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.success); // 씬 전환 하면서 성공 효과음
        itemSO.InitializeItems();
    }
    private void SetCardReward(Item item)    //카드 보상 설정 - 3개 짜리 얘기
    {
        GameObject newReward = Instantiate(RewardCardPrefab, RewardContent);   //해당 자식 위치에 프리팹 생성 
        UIRewardButton cardsetup = newReward.GetComponent<UIRewardButton>();
        cardsetup.Setup(item);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("MapScene");
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);
    }

    public void OpenDeck()
    {
        deckscrollview.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);
    }

    public void CloseDeck()
    {
        deckscrollview.SetActive(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);
    }

    private void MakeDeck()     //덱 생성과 관련된 기능
    {
        foreach (Item A in savedata.GetPlayerDeck())
        {
            decktemp.Add(A);
        }
        SortDeck();
        Debug.Log(decktemp.Count);
        foreach (Item A in decktemp)      //UI표시 관련
        {
            var newcardname = Instantiate(carndnameuiprefab, DeckContent);  //카드 이름 프리팹 생성
            UIDeckCardName uideckcardname = newcardname.GetComponent<UIDeckCardName>();
            uideckcardname.Setup(A);          //덱의 UI를 생성하는 기능
        }
    }

    private void SortDeck()   //정렬 관련
    {
        int n = decktemp.Count;
        for (int i = 1; i < n; ++i)
        {
            Item key = decktemp[i];
            int j = i - 1;

            // key보다 큰 원소들을 오른쪽으로 이동
            while (j >= 0 && decktemp[j].GetID() > key.GetID())
            {
                decktemp[j + 1] = decktemp[j];
                j = j - 1;
            }

            // key를 적절한 위치에 삽입
            decktemp[j + 1] = key;
        }
    }

    private void SetRewardList()    //보상 띄우기와 관련된 기능
    {
        foreach (Item A in itemSO.items)
        {
            if (A.reward == "AdventureReward")      //해당 카드의 Reward가 모험 보상일 시 보상 리스트에 넣음
                RewardList.Add(A);                  //향후 스테이지별 Reward 설정 시 1,2,3으로 변경해야할수도
        }
        for (int i = 0; i < RewardList.Count; i++)  //보상 랜덤하게 섞기 
        {
            int rand = Random.Range(i, RewardList.Count);
            Item temp = RewardList[i];
            RewardList[i] = RewardList[rand];
            RewardList[rand] = temp;
        }
        for (int i = 0; i < 3; i++)
        {
            SetCardReward(RewardList[i]);
        }
    }
}
