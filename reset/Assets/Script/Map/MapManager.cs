using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] GameObject playermeeple;   //�÷��̾� ��
    MapTile maptile;
    SaveData savedata;
    string tilecount;  //Ÿ�ϸ� ��
    int playermapindex;

    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        if (!AudioManager.instance.CheckBGM("music_Map"))
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
        //if (playermapindex == 0 && savedata.IsRoulette == false)
        //    return;
        Instantiate(playermeeple, Tile[playermapindex].transform);

        if (savedata.GetPlayerMapIndex() != 0)
        {

            MapTile a = Tile[0].GetComponent<MapTile>();
            a.SetSprite(3);
            a.SetStageSceneName("EndingProto");
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
