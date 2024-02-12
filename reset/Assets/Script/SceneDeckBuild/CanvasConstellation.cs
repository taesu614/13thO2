using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasConstellation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void OpenUI()
    {
        gameObject.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick);  // 클릭시 임시 효과음
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
