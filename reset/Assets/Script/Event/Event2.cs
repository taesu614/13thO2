using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Event2 : MonoBehaviour
{
    public TMP_Text Title;
    public TMP_Text texts; //�̺�Ʈ ������ �� ��
    public GameObject selections;
    string selectedButton;
    SaveData savedata;

    int playerHp;
    int playerMaxHp;
    int InjectionCount;

    int textFlow = 0; //�ؽ�Ʈ ���� ǥ��

    void Start()
    {
        savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        ChangeText();
        //����Ǿ��ִ� HP �� MaxHP ������
        playerHp = 0;
        playerMaxHp = 0;
        InjectionCount = 0;
        AudioManager.instance.PlayBGM(AudioManager.BGM.riddle);
    }

    void EventClose()
    { //�̺�Ʈ�� �����鼭 �ٲ� ������ ���Ӱ� ���
        savedata.SetPlayerHealth(savedata.GetPlayerHealth() + playerHp);
        savedata.SetPlayerMaxHealth(savedata.GetPlayerMaxHealth() + playerMaxHp);
        SceneManager.LoadScene("MapScene");
    }

    public void Select(string clickname)
    {

        print("��ư Ŭ�� Ȯ�ο�: " + clickname);

        // ������ ��ư�� ���� �ٷ� �Լ� ȣ��
        switch (clickname)
        {
            case "Injection":
                Injection();
                break;
            case "None":
                None();
                break;
        }
        // ������ ��ư ����
        selectedButton = clickname;
    }
    #region
    void Injection()
    {
        int rand = UnityEngine.Random.Range(0, 10);
        if (rand < 2) //20% Ȯ��
        {
            playerHp -= 5;
            savedata.SetMessage("ü���� " + playerHp + " �����Ͽ����ϴ�");
        }
        else if (rand < 5) //30% Ȯ��
        {
            playerHp += 20;
            savedata.SetMessage("ü���� " + playerHp + " ȸ���Ǿ����ϴ�");
        }
        else if (rand < 9) //40% Ȯ��
        {
            playerHp += 10;
            savedata.SetMessage("ü���� " + playerHp + " ȸ���Ǿ����ϴ�");
        }
        else //10% Ȯ��
        {
            Venom();
            savedata.SetMessage("�͵� ����(5��)�� �Ǿ����ϴ�");
        }
        EventClose();
    }

    void Venom() 
    {
        StatusEffect statuseffect = new StatusEffect();
        statuseffect.SetStatusEffect("poison", 5);
        savedata.AddStatusEffect(statuseffect);
    }

    void None()
    {
        print("�׸��д�.");
        EventClose();
        //�ƹ� �� ����
    }
    #endregion//�̺�Ʈ ������ �޼���
    public void ChangeText()
    {
        if (textFlow == event2.Length - 1)
        {
            texts.text = event2[textFlow];
            texts.gameObject.SetActive(false);
            selections.gameObject.SetActive(true);
            //�� ��ȯ
        }
        else
        {

            texts.text = event2[textFlow];
            textFlow++;
        }

    }

    //�̺�Ʈ ��ũ��Ʈ �ε� ���߿� scv ������ �е��� �ٲٴ����� ��������� ���� �� �� ���� ���� �ֽ��ϴ�.
    string[] event2 = {
        "-������ ����-",
        "���ڶ�� ������ ���ڸ� �߰��Ѵ�.\n" + "�� ���ڴ� �Ǿ�ǰ�� ����ִ� ���ڷ� ���δ�.\n\n" +
        "���̷��� �߾�Ÿ���.\n\n" +
        "�� �� ���ؿ� ��︮���� �ʴ� ������ �ֱ�.\n" + "�̰� ���� �����ر��� �����ΰ�..��",
        "���ڶ�� ������ ����ؿ� ſ�� ���� �����ϴٰ� ������.\n\n" +
        "�� �ֻ縦 ����غ��°� ���?\n" +
        "��¼�� �̰� ��ó�� ġ���� �� �ִ� ��ȣ�� ��ȸ������ �𸥴�.",
        "a"
    };


}
