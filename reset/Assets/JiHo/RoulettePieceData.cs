using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoulettePieceData
{
    public Image icon;
    public string description;

    [Range(1, 100)]
    public int chance = 100;

    [HideInInspector]
    public int index;

    [HideInInspector]
    public int weight;
}
