using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageMark : MonoBehaviour
{
    [SerializeField] TMP_Text damageTMP;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite heal;
    Transform transform;
    public float speed = 0.5f;
    public float time = 1.0f;
    int damage;
    Order order;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        Destroy(gameObject, time);
        order = GetComponent<Order>();
        order.SetOrder(spriteRenderer.sortingOrder);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    public void SetDamage(int damage, string type)   //����� ���� ���
    {
        this.damage = damage;
        damageTMP.text = damage.ToString();
        switch(type)
        {
            case "heal":
                spriteRenderer.sprite = heal;
                damageTMP.color = new Color32(88, 255, 88, 255);
                break;
            case "poison":
                damageTMP.color = new Color(255, 0, 255, 255);
                break;
        }

    }
    //1. 2�ʰ� �ö󰡴� ���
    //2. 2�� �� �����ϴ� ���
    //3. ����� ���� �޾Ƽ� ���ڸ� ǥ���ϴ� ���
}