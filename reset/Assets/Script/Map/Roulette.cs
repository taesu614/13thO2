using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    //작동방식, 이미지 등이 정해지지 않아 임시용이므로 전체적으로 바꿔야 할 수도 있음
    //1~6 이외의 상황은 고려하지 않음
    //만들어진 룰렛 쓰고 싶었는데 정 중앙에 설 수 없는 치명적인 문제로 다시만듬..
    public GameObject roulletcount;     //움직일 룰렛의 내부
    public GameObject[] roulletresult;  //룰렛의 숫자 오브젝트
    Transform roulletcounttransform;
    public Sprite[] roulletresultsprite;
    int roulletresultfirstindex = 0;
    public int speed = 10;
    int randomnum;
    int clickcount = 0;
    int maxclickcount = 1;  //여기 수정하면 최대 룰렛돌리는 횟수 수정가능함
    bool isclick = false;
    bool isspin = false;    //계속 클릭하면 더 많이 돌아가는 상황 방지용
    public float delayTime = 1f;   //룰렛 굴러가는시간 설정 - 랜덤하게도 가능할듯 
    // Start is called before the first frame update
    void Start()
    {
        roulletcounttransform = roulletcount.GetComponent<Transform>();
        ChangeRoulletResult();
        ResetRandomNum();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        isclick = true;
        RunRoullet();
    }

    void RunRoullet()   //룰렛 코루틴 실행
    {
        if (isclick && !isspin && clickcount < maxclickcount)
        {   //회전하는동안 작동하지 않게 해당 조건문 작성 
            StartCoroutine(RunRoulletCo());
            isclick = false;
            clickcount++;
        }
    }

    void SetRandomNum() //0~5까지의 숫자 설정 - 걍 1~6까지로 할까 싶다가도 게임오브젝트 index도 생각하면 이게 나을거같고...
    {                   //이거는 잘못된 선택이 아니었길 빌 뿐임
        randomnum = Random.Range(0,6);
    }

    void ResetRandomNum()   //무작위 숫자 초기화
    {
        randomnum = -1;
    }

    void RoulletStart() //룰렛 숫자 이동
    {

        roulletcounttransform.position -= new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        if (roulletcounttransform.position.x < -1.15f)
        {
            roulletcounttransform.position = new Vector3(0, 0.75f, 0);
            ChangeRoulletResult();
        }
    }
    //012 123 234 345 450 501
    void ChangeRoulletResult()  //룰렛 스프라이트 변경
    {
        for (int i = 0; i < roulletresult.Length; i++)
        {
            SpriteRenderer spriterenderer = roulletresult[i].GetComponent<SpriteRenderer>();
            int changenum = roulletresultfirstindex + i;
            if (changenum > 5)
                changenum = 0;
            spriterenderer.sprite = roulletresultsprite[changenum];
        }
        roulletresultfirstindex++;
        if (roulletresultfirstindex > 5)    //인덱스 범위 넘어가면 고장나므로 넣음
            roulletresultfirstindex = 0;
    }

    private IEnumerator RunRoulletCo()    //delayTime초간 작동
    {
        isspin = true;
        float elapsedTime = 0f;

        AudioManager.instance.PlaySFX(AudioManager.SFX.roulette); //회전시작 시 효과음 재생

        while (elapsedTime < delayTime)
        {
            elapsedTime += Time.deltaTime;
            RoulletStart();
            yield return null;
        }

        SetRandomNum();
        isclick = false;

        while (roulletresultfirstindex != randomnum)
        {
            RoulletStart();
        }
        CorrectRoullet();
        Debug.Log(randomnum);
        MapManager.Inst.CloseAllTile();
        MapManager.Inst.OpenTile(randomnum);
        ResetRandomNum();
        isspin = false;
    }

    void CorrectRoullet()   //룰렛 보정
    {
        roulletcounttransform.position = new Vector3 (0, 0.75f, 0);
    }
}
