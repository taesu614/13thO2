using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationManager : MonoBehaviour
{
    GameObject[] constellations;
    Queue<GameObject> constellationqueue = new Queue<GameObject>();
    SaveData savedata;
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
    }
    public void LeftMoveConstellation()
    {
        for(int i = 0; i < constellationqueue.Count - 1; i++)
        {
            GameObject starttoend = constellationqueue.Dequeue();
            constellationqueue.Enqueue(starttoend); //앞에걸 뒤로 밀기를 길이만큼 반복
        }
        SetConstellation(constellationqueue);
    }

    public void SendConstellation()
    {
        savedata.SetPlayerConstellation(constellationqueue.Peek().name);
    }
}
