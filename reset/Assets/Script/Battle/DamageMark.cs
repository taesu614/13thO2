using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageMark : MonoBehaviour
{
    [SerializeField] TMP_Text damageTMP;
    [SerializeField] SpriteRenderer spriteRenderer;
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

    public void SetDamage(int damage)   //����� ���� ���
    {
        this.damage = damage;
        damageTMP.text = damage.ToString();
    }
    //1. 2�ʰ� �ö󰡴� ���
    //2. 2�� �� �����ϴ� ���
    //3. ����� ���� �޾Ƽ� ���ڸ� ǥ���ϴ� ���
}