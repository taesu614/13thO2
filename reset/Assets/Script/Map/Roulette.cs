using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    //�۵����, �̹��� ���� �������� �ʾ� �ӽÿ��̹Ƿ� ��ü������ �ٲ�� �� ���� ����
    //1~6 �̿��� ��Ȳ�� ������� ����
    //������� �귿 ���� �;��µ� �� �߾ӿ� �� �� ���� ġ������ ������ �ٽø���..
    public GameObject roulletcount;     //������ �귿�� ����
    public GameObject[] roulletresult;  //�귿�� ���� ������Ʈ
    Transform roulletcounttransform;
    public Sprite[] roulletresultsprite;
    int roulletresultfirstindex = 0;
    public int speed = 10;
    int randomnum;
    int clickcount = 0;
    int maxclickcount = 1;  //���� �����ϸ� �ִ� �귿������ Ƚ�� ����������
    bool isclick = false;
    bool isspin = false;    //��� Ŭ���ϸ� �� ���� ���ư��� ��Ȳ ������
    public float delayTime = 1f;   //�귿 �������½ð� ���� - �����ϰԵ� �����ҵ� 
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

    void RunRoullet()   //�귿 �ڷ�ƾ ����
    {
        if (isclick && !isspin && clickcount < maxclickcount)
        {   //ȸ���ϴµ��� �۵����� �ʰ� �ش� ���ǹ� �ۼ� 
            StartCoroutine(RunRoulletCo());
            isclick = false;
            clickcount++;
        }
    }

    void SetRandomNum() //0~5������ ���� ���� - �� 1~6������ �ұ� �ʹٰ��� ���ӿ�����Ʈ index�� �����ϸ� �̰� �����Ű���...
    {                   //�̰Ŵ� �߸��� ������ �ƴϾ��� �� ����
        randomnum = Random.Range(0,6);
    }

    void ResetRandomNum()   //������ ���� �ʱ�ȭ
    {
        randomnum = -1;
    }

    void RoulletStart() //�귿 ���� �̵�
    {

        roulletcounttransform.position -= new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        if (roulletcounttransform.position.x < -1.15f)
        {
            roulletcounttransform.position = new Vector3(0, 0.75f, 0);
            ChangeRoulletResult();
        }
    }
    //012 123 234 345 450 501
    void ChangeRoulletResult()  //�귿 ��������Ʈ ����
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
        if (roulletresultfirstindex > 5)    //�ε��� ���� �Ѿ�� ���峪�Ƿ� ����
            roulletresultfirstindex = 0;
    }

    private IEnumerator RunRoulletCo()    //delayTime�ʰ� �۵�
    {
        isspin = true;
        float elapsedTime = 0f;

        AudioManager.instance.PlaySFX(AudioManager.SFX.roulette); //ȸ������ �� ȿ���� ���

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

    void CorrectRoullet()   //�귿 ����
    {
        roulletcounttransform.position = new Vector3 (0, 0.75f, 0);
    }
}
