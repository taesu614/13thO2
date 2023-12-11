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
        constellations = GameObject.FindGameObjectsWithTag("Constellation");   //�ش� �±��� ������Ʈ'��' ��������
        foreach (GameObject A in constellations)    //���ڸ� ť ����
        {
            constellationqueue.Enqueue(A);
            index++;
        }
        SetConstellation(constellationqueue);
    }

    // Update is called once per frame
    void SetConstellation(Queue<GameObject> queue) //���ڸ� ��ġ ����
    {                                              
        int count = 0;
        foreach (GameObject A in queue)
        {
            switch(count)
            {
                case 0: //���� �տ� ����
                    A.transform.position = new Vector3(0f, 0.4f, 0);
                    A.transform.localScale = new Vector3(1f, 1f, 0);
                    break;
                case 1: //������
                    A.transform.localScale = new Vector3(0.4f, 0.4f, 0);
                    A.transform.position = new Vector3(6f, 0.4f, 0);
                    break;
                case 2: //����
                    A.transform.localScale = new Vector3(0.4f, 0.4f, 0);
                    A.transform.position = new Vector3(-6f, 0.4f, 0);
                    break;
            }
            if (count > 3)
                break;
            count++;
        }
    }

    public void LeftMoveConstellation()
    {
        GameObject starttoend = constellationqueue.Dequeue();
        constellationqueue.Enqueue(starttoend); //�տ��� �ڷ� ��
        SetConstellation(constellationqueue);
    }
    public void RightMoveConstellation()
    {
        for(int i = 0; i < constellationqueue.Count - 1; i++)
        {
            GameObject starttoend = constellationqueue.Dequeue();
            constellationqueue.Enqueue(starttoend); //�տ��� �ڷ� �б⸦ ���̸�ŭ �ݺ�
        }
        SetConstellation(constellationqueue);
    }

    public void SendConstellation()
    {
        savedata.SetPlayerConstellation(constellationqueue.Peek().name);
    }
}