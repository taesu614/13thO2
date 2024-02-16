using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //제작 시점에서 지도 이미지도, 사용 방식도, 오브젝트 배치방식도 모두 임시로 정해진 부분만 있어 노가다로 때움
    //지도 확정되면 GameObject[] Tile 부분을 자동으로 탐색하도록 만들 것 
    //아마 프리팹으로 생성하게 만들 것으로 추정되므로 프리팹 오브젝트를 생성 후 List로 관리한다면 자동으로 될 듯
    //고정칸에 대해서는 고려하지 않음

    // 계속 싱글톤으로 선언하게 되는거 같은데 이래도 되나?
    public static MapManager Inst = null;
    void Awake()
    {
        if (null == Inst)
        {
            Inst = this;
        }
        else
            Destroy(gameObject);
        if(!AudioManager.instance.CheckBGM("main"))
            AudioManager.instance.PlayBGM(AudioManager.BGM.main);
    }

    [SerializeField] GameObject[] Tile; //씬에 적용된 Tile 프리팹을 넣을 것 0이 맨 마지막칸임, 적어도 현 시점에서 노가다 형식으로 진행되므로 이렇게 씀
    [SerializeField] GameObject playermeeple;   //플레이어 말
    MapTile maptile;
    SaveData savedata;
    string tilecount;  //타일맵 수
    int playermapindex;
    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        if (!AudioManager.instance.CheckBGM("map"))
            AudioManager.instance.PlayBGM(AudioManager.BGM.map);
        if (savedata.GetMyMap() != null)
        {
            TileSet();
        }
        else
        {
            MapRandomSet();
            TileSet();
        }
        playermapindex = savedata.GetPlayerMapIndex();
        if (playermapindex == 0)
            return;
        Instantiate(playermeeple, Tile[playermapindex].transform);
    }

    void MapRandomSet()     //맵 랜덤하게 배치하는 용도
    {
        for (int i = 0; i < Tile.Length; i++)    
        {
            tilecount += Random.Range(0, 8);   //랜덤 비율 : 전투:상점:수수께기 = 5:2:1 비율, 확률 관련해서는 MapeTile에서 조정할것
        }                                   //굳이 이 과정을 넣은건 향후 연속되는 과정 수정할 때 사용할 가능성 높아서
        savedata.SetMyMap(tilecount);
        Debug.Log(savedata.GetMyMap());
    }

    void TileSet()  //타일을 배치하는 코드
    {
        for (int i = 0; i < Tile.Length; i++)
        {
            maptile = Tile[i].GetComponent<MapTile>();
            maptile.Setup(savedata.GetMyMap()[i], i);
        }
    }

    public void OpenTile(int num)  //지정된 숫자만큼 타일 활성화하는 메서드
    {
        Debug.Log(num);
        if (num == 0)
            num = 6;    //스크립트상 0이 6이므로
        for(int i = savedata.GetPlayerMapIndex()+1; i < savedata.GetPlayerMapIndex() + num + 1; i++)
        {
            if (i > Tile.Length-1)  //개수는 1부터 시작이라 index상으로는 넘어감
            {
                i = 0;
                Tile[i].GetComponent<MapTile>().TileSet(true);
                break;  //해당 위치에서 보스전 출력할것
            }
            Tile[i].GetComponent<MapTile>().TileSet(true);
        }
    }

    public void CloseAllTile()
    {
        foreach(GameObject A in Tile)
        {
            A.GetComponent<MapTile>().TileSet(false);
        }
    }
}
