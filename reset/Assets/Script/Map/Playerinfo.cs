using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Playerinfo : MonoBehaviour
{
    SaveData savedata;
    [SerializeField] Image hpwhite;
    [SerializeField] TMP_Text playerhp;
    [SerializeField] TMP_Text playerMoney;

    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        playerhp.text = savedata.GetPlayerHealth().ToString();
        playerMoney.text = savedata.GetPlayerMoney().ToString();
        hpwhite.transform.localScale = new Vector3(1 - savedata.GetPlayerHealthPercent(), 1f, 1f);
        if (savedata.GetPlayerHealth() <= 0)
        {
            Destroy(GameObject.Find("SaveData"));
            AudioManager.instance.DestroyAudioManager();
            SceneManager.LoadScene("Press2StartScene");
        }
    }

    public void SetPlayerInfo() //플레이어 정보 설정
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        playerhp.text = savedata.GetPlayerHealth().ToString();
        playerMoney.text = savedata.GetPlayerMoney().ToString();
        hpwhite.transform.localScale = new Vector3(1 - savedata.GetPlayerHealthPercent(), 1f, 1f);

        if (savedata.GetPlayerHealth() <= 0)
        {
            SceneManager.LoadScene("Press2StartScene");
        }
    }
}