using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{

    private void Start()
    {
        ExitPast();
    }
    public void ExitPast()
    {
        gameObject.SetActive(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // 임시로 효과음 넣음
    }
    public void OpenPast()
    {
        gameObject.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);  // 임시로 효과음 넣음
    }
}
