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

    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        playerhp.text = savedata.GetPlayerHealth().ToString();
        hpwhite.transform.localScale = new Vector3(1-savedata.GetPlayerHealthPercent(),1f,1f);
        if(savedata.GetPlayerHealth() <= 0)
        {
            Destroy(GameObject.Find("SaveData"));
            SceneManager.LoadScene("Press2StartScene");
        }
    }

    
}
