using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public Sprite sheep;
    public Sprite bull;

    private SpriteRenderer spriteRenderer;

    public void ChangeStarMaskImage(string name)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (name)
        {
            case "sheep":
                spriteRenderer.sprite = sheep;
                break;
            case "bull":
                spriteRenderer.sprite = bull;
                break;
            default:
                Debug.Log("NoneMask");
                break;
        }
    }
}
