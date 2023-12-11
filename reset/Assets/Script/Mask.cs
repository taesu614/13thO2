using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public Sprite Sheep;
    public Sprite Bull;

    private SpriteRenderer spriteRenderer;

    public void ChangeStarMaskImage(string name)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (name)
        {
            case "Sheep":
                spriteRenderer.sprite = Sheep;
                break;
            case "Bull":
                spriteRenderer.sprite = Bull;
                break;
            default:
                Debug.Log("NoneMask");
                break;
        }
    }
}
