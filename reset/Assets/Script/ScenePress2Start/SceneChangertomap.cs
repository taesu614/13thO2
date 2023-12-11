using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangertomap : MonoBehaviour
{
    public void ChangeScene(string Map)
    {
        SceneManager.LoadScene(Map);
    }
    public void PresstoStart()
    {
        SceneManager.LoadScene("LobiScene");
    }
}
