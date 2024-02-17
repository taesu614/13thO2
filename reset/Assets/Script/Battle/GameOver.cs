using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }


    public void GoToPress2Start()
    {
        SaveData.instance.ResetData();
        SaveData.instance.ResetMyMap();
        SceneManager.LoadScene("Press2StartScene");
    }
}