using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobiManager : MonoBehaviour
{
    private void Start()
    {
        if (!AudioManager.instance.CheckBGM("main"))
            AudioManager.instance.PlayBGM(AudioManager.BGM.main);
    }
    public void GoToMap()
    {
        SceneManager.LoadScene("MapScene");
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);
    }
    public void GoToDeckBuild()
    {
        SceneManager.LoadScene("DeckBuildScene");
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);
    }

    public void GoSelectMap(string name)
    {
        SceneManager.LoadScene(name);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);
    }
    public void TurnOnSetting()
    {
        gameObject.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // 임시로 효과음 넣음
    }
}
