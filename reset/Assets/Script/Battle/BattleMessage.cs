using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MessageUIOpen(false);
    }

    public void MessageUIOpen(bool open)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(open);
        }
    }

    public void GoLobi()
    {
        SaveData.instance.ResetData();
        SaveData.instance.ResetMyMap();
        SceneManager.LoadScene("LobiScene");
    }
}
