using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // 계속 싱글톤으로 선언하게 되는거 같은데 이래도 되나?
    private static MapManager Inst = null;
    void Awake()
    {
        if (null == Inst)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    [SerializeField] GameObject[] Tile; //씬에 적용된 Tile 프리팹을 넣을 것, 적어도 현 시점에서 노가다 형식으로 진행되므로 이렇게 씀
    MapTile maptile;
    string tilecount;  //타일맵 수
    void Start()
    {
        for(int i = 0; i < Tile.Length; i++)    //맵 랜덤하게 배치하는 용도
        {
            tilecount+=Random.Range(0, 8);   //랜덤 비율 : 전투:상점:수수께기 = 5:2:1 비율, 확률 관련해서는 MapeTile에서 조정할것
        }                                   //굳이 이 과정을 넣은건 향후 연속되는 과정 수정할 때 사용할 가능성 높아서
        for (int i = 0; i < Tile.Length; i++)
        {
            maptile = Tile[i].GetComponent<MapTile>();
            maptile.Setup(tilecount[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
