using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void GotoCut3()
    {
        // Ư�� ������ �̵��ϴ� �ڵ�
        SceneManager.LoadScene("CutScene3");
    }
    public void GotoRobi()
    {
        // Ư�� ������ �̵��ϴ� �ڵ�
        SceneManager.LoadScene("LobiScene");
    }

}