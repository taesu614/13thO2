using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    Animator animator;
    public float size = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPlayerState(string name)
    {
        switch(name)
        {
            case "attack":
                if (animator.GetBool("playerAttack"))   //ī�� ���� ��� �� �ִϸ��̼� �ʱ�ȭ��
                    animator.Play("PlayerAttack", 0, 0f); // �ִϸ��̼��� ������ϴ� ���� - ��� ���°��� �ƴ� ���� �̸���
                animator.SetBool("playerAttack", true);
                break;
            case "damage":
                if(!animator.GetBool("playerAttack"))    //�÷��̾ ���� ���� ���°� �ƴϾ�� ��
                {
                    if (animator.GetBool("playerDamage"))   
                        animator.Play("PlayerDamage", 0, 0f); 
                    animator.SetBool("playerDamage", true);
                    break;
                }
                break;
        }
    }

    public void AnimationFinish(string name)
    {
        switch(name)
        {
            case "attack":
                animator.SetBool("playerAttack", false);
                break;
            case "damage":
                animator.SetBool("playerDamage", false);
                break;
        }

    }
}
