using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TurnOffSetting();
    }

    public void TurnOnSetting()
    {
        gameObject.SetActive(true);

    }

    public void TurnOffSetting()
    {
        gameObject.SetActive(false);

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
