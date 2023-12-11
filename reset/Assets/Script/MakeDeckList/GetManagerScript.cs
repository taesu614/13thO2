using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    DeckUIManager deckuimanager;
    void Start()
    {
        deckuimanager = GameObject.Find("DeckUIManager").GetComponent<DeckUIManager>();
    }

    public void Button()
    {
        deckuimanager.OpenUI();
    }
}
