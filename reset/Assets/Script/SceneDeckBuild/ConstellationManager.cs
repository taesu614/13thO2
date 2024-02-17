using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConstellationManager : MonoBehaviour
{
    GameObject[] constellations;
    Queue<GameObject> constellationqueue = new Queue<GameObject>();
    SaveData savedata;
    public CanvasConstellation canvasconstellation;
    public SetBackground setbg; 
    [SerializeField] GameObject Message;
    // Start is called before the first frame update
    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        int index = 0;
        constellations = GameObject.FindGameObjectsWithTag("Constellation");   //해당 태그의 오브젝트'들' 가져오기
        foreach (GameObject A in constellations)    //별자리 큐 생성
        {
            constellationqueue.Enqueue(A);
            index++;
        }
        SetConstellation(constellationqueue);
        Message.SetActive(false);
    }

    // Update is called once per frame
    void SetConstellation(Queue<GameObject> queue) //별자리 위치 설정
    {                                              
        int count = 0;
        foreach (GameObject A in queue)
        {
            switch(count)
            {
                case 0: //가장 앞에 있음
                    A.transform.position = new Vector3(0f, 0.4f, 0);
                    A.transform.localScale = new Vector3(1f, 1f, 0);
                    break;
                case 1: //오른쪽
                    A.transform.localScale = new Vector3(0.4f, 0.4f, 0);
                    A.transform.position = new Vector3(6f, 0.4f, 0);
                    break;
                case 2: //왼쪽
                    A.transform.localScale = new Vector3(0.4f, 0.4f, 0);
                    A.transform.position = new Vector3(-6f, 0.4f, 0);
                    break;
            }
            if (count > 3)
                break;
            count++;
        }
    }

    public void RightMoveConstellation()
    {
        GameObject starttoend = constellationqueue.Dequeue();
        constellationqueue.Enqueue(starttoend); //앞에걸 뒤로 밈
        SetConstellation(constellationqueue);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // 클릭시 효과음 임시
    }
    public void LeftMoveConstellation()
    {
        for(int i = 0; i < constellationqueue.Count - 1; i++)
        {
            GameObject starttoend = constellationqueue.Dequeue();
            constellationqueue.Enqueue(starttoend); //앞에걸 뒤로 밀기를 길이만큼 반복
        }
        SetConstellation(constellationqueue);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // 클릭시 효과음 임시
    }

    public void SendConstellation()
    {
        savedata.SetPlayerConstellation(constellationqueue.Peek().name);

        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // 클릭시 효과음 임시
    }
    
    public void OpenMirror()
    {
        TMP_Text text = Message.GetComponent<TMP_Text>();
        switch (constellationqueue.Peek().name)
        {
            case "Sheep":
                setbg.BackgroundActive(true);
                canvasconstellation.CloseUI();
                SendConstellation();
                break;
            case "Goat":
                text.text = "개발이 완료되지 않아 사용할 수 없습니다.";
                StartCoroutine(AlertMessage());
                break;
            case "Sagittarius":
                

                text.text = "개발이 완료되지 않아 사용할 수 없습니다.";
                StartCoroutine(AlertMessage());

                break;
            default:
                break;
        }
    }

    IEnumerator AlertMessage()
    {
        Message.SetActive(true);
        // 코루틴 실행 내용
        yield return new WaitForSeconds(2f);

        Message.SetActive(false);
        yield break;
    }
}
