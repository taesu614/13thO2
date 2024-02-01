using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangertowait : MonoBehaviour
{
    public void ChangeScene(string SampleScene)
    {
        SceneManager.LoadScene(SampleScene);
    }
    public void GotoBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }
    public void GotoBattle2()
    {
        SceneManager.LoadScene("BattleScene");
    }
    public void GotoEvent()
    {
        SceneManager.LoadScene("EventProto");
    }

    public void GotoStore()
    {
        SceneManager.LoadScene("StoreScene");
    }

    public void GoToLobi()
    {
        SaveData.instance.ResetMyMap();
        SceneManager.LoadScene("LobiScene");
    }
}