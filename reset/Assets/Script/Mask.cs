using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public Sprite sheep;
    public Sprite bull;
    public Sprite goat;
    public Sprite sagittarius;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        
    }

    public void ChangeStarMaskImage(string conname)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch (conname)
        {
            case "Sheep":
                spriteRenderer.sprite = sheep;
                break;
            case "Bull":
                spriteRenderer.sprite = bull;
                break;
            case "Goat":
                spriteRenderer.sprite = goat;
                Debug.Log(conname);
                break;
            case "Sagittarius":
                spriteRenderer.sprite = sagittarius;
                break;
            default:
                Debug.Log("NoneMask");
                break;
        }
    }
}
