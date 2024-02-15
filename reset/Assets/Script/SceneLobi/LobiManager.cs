using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobiManager : MonoBehaviour
{
    public void GoToMap()
    {
        SceneManager.LoadScene("MapScene");
    }
    public void GoToDeckBuild()
    {
        SceneManager.LoadScene("DeckBuildScene");
    }

    public void GoSelectMap(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void TurnOnSetting()
    {
        gameObject.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // 임시로 효과음 넣음
    }
}
