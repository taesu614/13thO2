using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTile : MonoBehaviour
{
    //�ð��� ���µ� ���� �̹����� �ӽ��� ��, ��� ����ν�� ��ġ�� ������ �����Ƿ� �밡�� ������ ������
    //���� ��ȹ�� ����ȴٸ� �ش� ��ũ��Ʈ�� ��ü������ ��� ���ĵ� ��� ����
    [SerializeField] Sprite[] Tileimg;  //Ÿ�� �̹��� �����
    [SerializeField] Sprite[] Stageimg; //�������� �̹��� �����
    [SerializeField] GameObject TileObject;     //Ÿ���� ������Ʈ
    [SerializeField] GameObject StageObject;    //���������� ������Ʈ
    SpriteRenderer mysprite;
    bool isopen = false;
    string stagescenename;
    int tileindex;
    private void Start()
    {
        //TileObject.SetActive(false) ;       //�귿 ������ ���� Ȱ��ȭ�� �ǹǷ�
    }

    public void Setup(char stage, int index)
    {
        if (index == 0) //�� Ÿ��
            return;
        tileindex = index;
        switch (stage)
            {
            case '0':   //���� ����
            case '1':
            case '2':
            case '3':
            case '4':
                SetSprite(0);
                stagescenename = "BattleScene";
                break;
            case '5':   //���� ����
            case '6':
                SetSprite(1);
                stagescenename = "StoreScene";
                break;
            case '7':   //�������� ����
                SetSprite(2);
                stagescenename = "EventProto2";
                break;
        }
    }

    void SetSprite(int i)   //���� Ÿ��(����) �̹����� ����� �� �־ ����
    {
        mysprite = StageObject.GetComponent<SpriteRenderer>();
        mysprite.sprite = Stageimg[i];
    }

    public void TileSet(bool isopen)  ///Ÿ�ϸ� Ȱ��ȭ ���
    {
        TileObject.SetActive(isopen);
        this.isopen = isopen;
    }

    public void ChangeScene()
    {
        SaveData.instance.SetPlayerMapIndex(tileindex);
        SceneManager.LoadScene(stagescenename);
    }

    void OnMouseDown()  //�ش� �ݶ��̴��� ������
    {
        if(isopen)
            ChangeScene();
    }
}
