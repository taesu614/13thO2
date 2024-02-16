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
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);
    }

    public void TurnOffSetting()
    {
        settingUI.SetActive(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);
    }

    private void OnMouseDown()
    {
        TurnOnSetting();
    }
    public void ExitGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
