using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour   //������ ����
{
    //�̺�Ʈ ��� ��Ű��ó ���� �ٽ� �����ϰ� ����� ������ ��
    GameObject[] monster;
    private int MonsterCount = 0;
    private int PlayerCount = 0;    //�ӽ�
    //Enable() : Start()�� ����� �뵵�� Ȱ��ȭ �� ������ ȣ�� ��

    private void Start()
    {
        
    }
    void OnEnable()
    {
        // ���⿡ �ʱ�ȭ �ڵ峪 �ٸ� �۾��� �߰��� �� �ֽ��ϴ�.
    }

    //Onenable()�� ¦�� 
    void OnDisable()    
    {
    }

    void CheckMonster() //���� �� Ȯ�ο�
    {
        MonsterCount++;
    }

    void DestroyMonster()
    {
       
    }
}
