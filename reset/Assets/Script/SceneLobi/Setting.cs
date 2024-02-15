using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject settingUI;
    void Start()
    {
        TurnOffSetting();
    }

    public void TurnOnSetting()
    {
        settingUI.SetActive(true);

    }

    public void TurnOffSetting()
    {
        settingUI.SetActive(false);
    }

    private void OnMouseDown()
    {
        TurnOnSetting();
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
