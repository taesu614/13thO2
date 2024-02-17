using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void GotoCut3()
    {
        // 특정 씬으로 이동하는 코드
        SceneManager.LoadScene("CutScene3");
    }
    public void GotoRobi()
    {
        // 특정 씬으로 이동하는 코드
        SceneManager.LoadScene("LobiScene");
    }

}