using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobiButton : MonoBehaviour
{
    public string mapname;
    LobiManager lobimanager;
    void Start()
    {
        lobimanager = GameObject.Find("LobiManager").GetComponent<LobiManager>();
    }

    private void OnMouseDown()
    {
        lobimanager.GoSelectMap(mapname);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
