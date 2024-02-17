using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTile : MonoBehaviour
{
    //시간은 없는데 지도 이미지가 임시인 점, 적어도 현재로써는 위치가 정해져 있으므로 노가다 형식을 취했음
    //향후 기획이 변경된다면 해당 스크립트는 전체적으로 뜯어 고쳐도 상관 없음
    [SerializeField] Sprite[] Tileimg;  //타일 이미지 변경용
    [SerializeField] Sprite[] Stageimg; //스테이지 이미지 변경용
    [SerializeField] GameObject TileObject;     //타일의 오브젝트
    [SerializeField] GameObject StageObject;    //스테이지의 오브젝트
    SpriteRenderer mysprite;
    bool isopen = false;
    string stagescenename;
    int tileindex;
    private void Start()
    {
        //TileObject.SetActive(false) ;       //룰렛 굴려서 들어가야 활성화가 되므로
    }

    public void Setup(char stage, int index)
    {
        if (index == 0) //빈 타일
            return;
        tileindex = index;
        switch (stage)
            {
            case '0':   //전투 설정
            case '1':
            case '2':
            case '3':
            case '4':
                SetSprite(0);
                stagescenename = "BattleScene";
                break;
            case '5':   //상점 설정
            case '6':
                SetSprite(1);
                stagescenename = "StoreScene";
                break;
            case '7':   //수수께끼 설정
                SetSprite(2);
                stagescenename = "EventProto2";
                break;
        }
    }

    void SetSprite(int i)   //향후 타일(발판) 이미지도 변경될 수 있어서 만듦
    {
        mysprite = StageObject.GetComponent<SpriteRenderer>();
        mysprite.sprite = Stageimg[i];
    }

    public void TileSet(bool isopen)  ///타일맵 활성화 기능
    {
        TileObject.SetActive(isopen);
        this.isopen = isopen;
    }

    public void ChangeScene()
    {
        SaveData.instance.SetPlayerMapIndex(tileindex);
        SceneManager.LoadScene(stagescenename);
    }

    void OnMouseDown()  //해당 콜라이더를 누를때
    {
        if(isopen)
            ChangeScene();
    }
}
