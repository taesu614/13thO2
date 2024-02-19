using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    }

    [SerializeField] GameObject[] Tile; //씬에 적용된 Tile 프리팹을 넣을 것 0이 맨 마지막칸임, 적어도 현 시점에서 노가다 형식으로 진행되므로 이렇게 씀
    [SerializeField] GameObject playermeeplePrefab;   //플레이어 말
    [SerializeField] GameObject message;
    [SerializeField] TMP_Text mytext;
    public Sprite movesprite;
    SpriteRenderer playermeeplerenderer;
    GameObject playermeeple;
    MapTile maptile;
    SaveData savedata;
    string tilecount;  //타일맵 수
    int playermapindex;
    public bool canselect = true;
    public int selectindex;
    public int nextindex;
    float speed = 2f;
    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        if (!AudioManager.instance.CheckBGM("music_Map"))
            AudioManager.instance.PlayBGM(AudioManager.BGM.map);
        AudioManager.instance.ChangeBGMVolume(1);
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
        nextindex = playermapindex + 1;
        //if (playermapindex == 0 && savedata.IsRoulette == false)
        //    return;
        playermeeple = Instantiate(playermeeplePrefab, Tile[playermapindex].transform);

        if (savedata.GetPlayerMapIndex() != 0)
        {

            MapTile a = Tile[0].GetComponent<MapTile>();
            a.SetSprite(3);
            a.SetStageSceneName("EndingProto");
        }
        message.SetActive(false);
        StartCoroutine(OutputMessage());
        playermeeplerenderer = playermeeple.GetComponent<SpriteRenderer>();
    }

    IEnumerator OutputMessage()
    {
        if (savedata.GetMessage() != null)
        {
            message.SetActive(true);
            mytext.text = savedata.GetMessage();
            savedata.SetMessage(null);
            yield return new WaitForSeconds(3f);
            message.SetActive(false);
        }
        yield break;
    }
    private void Update()
    {
        if (canselect)
            return;
        MovePlayerMeeple();
    }
    
    void MovePlayerMeeple()
    {
        if (nextindex >= 20)
            nextindex = 0;
        playermeeplerenderer.sprite = movesprite;
        if (playermeeple.transform.position.x < Tile[nextindex].transform.position.x)
            playermeeple.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            playermeeple.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (Vector3.Distance(playermeeple.transform.position, Tile[nextindex].transform.position) < 0.1f && nextindex < selectindex)
        {
            nextindex++;
            if (nextindex >= 20)
                nextindex = 0;
        }
        playermeeple.transform.position = Vector3.MoveTowards(playermeeple.transform.position, Tile[nextindex].transform.position, speed * Time.deltaTime);
        Debug.Log(speed);
        if (selectindex == 0)
            selectindex = 20;
        if (Vector3.Distance(playermeeple.transform.position, Tile[nextindex].transform.position) < 0.1f && nextindex >= selectindex || Vector3.Distance(playermeeple.transform.position, Tile[0].transform.position) < 0.1f && selectindex == 20)
        {
            if (selectindex == 20)
                selectindex = 0;
            MapTile selecttile = Tile[selectindex].GetComponent<MapTile>();
            selecttile.ChangeScene();
            //이곳에서 씬 변경
        }
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
        for (int i = 1; i < Tile.Length; i++)
        {
            maptile = Tile[i].GetComponent<MapTile>();
            maptile.Setup(savedata.GetMyMap()[i], i);
        }
        if(!savedata.IsRoulette)
        {
            maptile = Tile[0].GetComponent<MapTile>();
            Debug.Log("maptile: " + maptile.isopen);
            maptile.SetSprite(4);  // 투명
        }
    }

    public void OpenTile(int num)  //지정된 숫자만큼 타일 활성화하는 메서드
    {
        Debug.Log(num);
        if (num == 0)
            num = 6;    //스크립트상 0이 6이므로
        for(int i = savedata.GetPlayerMapIndex() + 1; i < savedata.GetPlayerMapIndex() + num + 1; i++)
        {
            if (savedata.IsRoulette == false)  // 계속 1만 나오면 진행이 안돼서 일단 처음에만 첫번째 발판에도 진입 가능하도록
            {
                savedata.IsRoulette = true;
                int tempNum = i - 1;
                Tile[tempNum].GetComponent<MapTile>().TileSet(true);
                Tile[tempNum].GetComponent<MapTile>().isopen = false;
            }
            if (i > Tile.Length - 1)  //개수는 1부터 시작이라 index상으로는 넘어감
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
