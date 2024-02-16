using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ġƮ ui ��ŷ ���ӿ���
public class GameManager : MonoBehaviour
{
    Monster monster;
    GameObject[] monsterentity;
    public static GameManager Inst { get; private set; }
    void Awake() => Inst = this;

    private int playerdamage;
    private int enemydamage;

    [SerializeField] NotificatonPanel notificationPanel;
    void Start()
    {
        StartGame();
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))  //���߿� ��ư���� �۵��ϰ� �ٲٱ�
        {
            TurnManager.OnAddCard.Invoke(true);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            TurnManager.Inst.EndTrun();
        }

        if(Input.GetKeyDown(KeyCode.Keypad5))
        {
            CostManager.Inst.CostSet();
            //EntityManager.Inst.SpawnEntity(true, monster, Vector3.zero);
        }
        //if(Input.GetKeyDown(KeyCode.Keypad0))
        //{
        //    SceneManager.LoadScene("SampleScene");
        //}
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            CardManager.Inst.DiscardMyCard();
        }
        if(Input.GetKeyDown(KeyCode.Keypad8))
        {
            Entity player = GameObject.Find("MyPlayer").GetComponent<Entity>();
            player.health = 1;
        }
        if(Input.GetKeyDown(KeyCode.Keypad9))
        {
            monsterentity = GameObject.FindGameObjectsWithTag("Monster");
            Debug.Log(monsterentity.Length);
            foreach (GameObject A in monsterentity)
            {
                Entity entity = A.GetComponent<Entity>();
                entity.health = 0;
                EntityManager.Inst.FindDieEntity();
            }
        }
    }
    
    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }
    /*public void SetPlayerDamage(int damage, GameObject target)
    {
        playerdamage = damage;
        Debug.Log("�� ���� ����");
        GetEnemyEffect();
    }

    private void GetEnemyEffect(GameManager target)
    {

    }
    */
}