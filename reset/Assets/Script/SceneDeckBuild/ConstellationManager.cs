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
        constellations = GameObject.FindGameObjectsWithTag("Constellation");   //�ش� �±��� ������Ʈ'��' ��������
        foreach (GameObject A in constellations)    //���ڸ� ť ����
        {
            constellationqueue.Enqueue(A);
            index++;
        }
        SetConstellation(constellationqueue);
        Message.SetActive(false);
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

    public void RightMoveConstellation()
    {
        GameObject starttoend = constellationqueue.Dequeue();
        constellationqueue.Enqueue(starttoend); //�տ��� �ڷ� ��
        SetConstellation(constellationqueue);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // Ŭ���� ȿ���� �ӽ�
    }
    public void LeftMoveConstellation()
    {
        for(int i = 0; i < constellationqueue.Count - 1; i++)
        {
            GameObject starttoend = constellationqueue.Dequeue();
            constellationqueue.Enqueue(starttoend); //�տ��� �ڷ� �б⸦ ���̸�ŭ �ݺ�
        }
        SetConstellation(constellationqueue);
        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // Ŭ���� ȿ���� �ӽ�
    }

    public void SendConstellation()
    {
        savedata.SetPlayerConstellation(constellationqueue.Peek().name);

        AudioManager.instance.PlaySFX(AudioManager.SFX.openClick); // Ŭ���� ȿ���� �ӽ�
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
                text.text = "������ �Ϸ���� �ʾ� ����� �� �����ϴ�.";
                StartCoroutine(AlertMessage());
                break;
            case "Sagittarius":
                

                text.text = "������ �Ϸ���� �ʾ� ����� �� �����ϴ�.";
                StartCoroutine(AlertMessage());

                break;
            default:
                break;
        }
    }

    IEnumerator AlertMessage()
    {
        Message.SetActive(true);
        // �ڷ�ƾ ���� ����
        yield return new WaitForSeconds(2f);

        Message.SetActive(false);
        yield break;
    }
}
