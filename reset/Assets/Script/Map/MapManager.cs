using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    //���� �������� ���� �̹�����, ��� ��ĵ�, ������Ʈ ��ġ��ĵ� ��� �ӽ÷� ������ �κи� �־� �밡�ٷ� ����
    //���� Ȯ���Ǹ� GameObject[] Tile �κ��� �ڵ����� Ž���ϵ��� ���� �� 
    //�Ƹ� ���������� �����ϰ� ���� ������ �����ǹǷ� ������ ������Ʈ�� ���� �� List�� �����Ѵٸ� �ڵ����� �� ��
    //����ĭ�� ���ؼ��� ������� ����

    // ��� �̱������� �����ϰ� �Ǵ°� ������ �̷��� �ǳ�?
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

    [SerializeField] GameObject[] Tile; //���� ����� Tile �������� ���� �� 0�� �� ������ĭ��, ��� �� �������� �밡�� �������� ����ǹǷ� �̷��� ��
    [SerializeField] GameObject playermeeplePrefab;   //�÷��̾� ��
    [SerializeField] GameObject message;
    [SerializeField] TMP_Text mytext;
    public Sprite movesprite;
    SpriteRenderer playermeeplerenderer;
    GameObject playermeeple;
    MapTile maptile;
    SaveData savedata;
    string tilecount;  //Ÿ�ϸ� ��
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
            //�̰����� �� ����
        }
    }

    void MapRandomSet()     //�� �����ϰ� ��ġ�ϴ� �뵵
    {
        for (int i = 0; i < Tile.Length; i++)    
        {
            tilecount += Random.Range(0, 8);   //���� ���� : ����:����:�������� = 5:2:1 ����, Ȯ�� �����ؼ��� MapeTile���� �����Ұ�
        }                                   //���� �� ������ ������ ���� ���ӵǴ� ���� ������ �� ����� ���ɼ� ���Ƽ�
        savedata.SetMyMap(tilecount);
        Debug.Log(savedata.GetMyMap());
    }

    void TileSet()  //Ÿ���� ��ġ�ϴ� �ڵ�
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
            maptile.SetSprite(4);  // ����
        }
    }

    public void OpenTile(int num)  //������ ���ڸ�ŭ Ÿ�� Ȱ��ȭ�ϴ� �޼���
    {
        Debug.Log(num);
        if (num == 0)
            num = 6;    //��ũ��Ʈ�� 0�� 6�̹Ƿ�
        for(int i = savedata.GetPlayerMapIndex() + 1; i < savedata.GetPlayerMapIndex() + num + 1; i++)
        {
            if (savedata.IsRoulette == false)  // ��� 1�� ������ ������ �ȵż� �ϴ� ó������ ù��° ���ǿ��� ���� �����ϵ���
            {
                savedata.IsRoulette = true;
                int tempNum = i - 1;
                Tile[tempNum].GetComponent<MapTile>().TileSet(true);
                Tile[tempNum].GetComponent<MapTile>().isopen = false;
            }
            if (i > Tile.Length - 1)  //������ 1���� �����̶� index�����δ� �Ѿ
            {
                i = 0;
                Tile[i].GetComponent<MapTile>().TileSet(true);
                break;  //�ش� ��ġ���� ������ ����Ұ�
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
