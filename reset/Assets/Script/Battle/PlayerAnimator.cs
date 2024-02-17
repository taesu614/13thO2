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
        if (!animator)
            return;
        switch(name)
        {
            case "attack":
                if (animator.GetBool("playerAttack") && !animator.GetBool("playerSheep"))   //ī�� ���� ��� �� �ִϸ��̼� �ʱ�ȭ��
                    animator.Play("PlayerAttack", 0, 0f); // �ִϸ��̼��� ������ϴ� ���� - ��� ���°��� �ƴ� ���� �̸���
                else if (animator.GetBool("playerAttack") && animator.GetBool("playerSheep"))   //ī�� ���� ��� �� �ִϸ��̼� �ʱ�ȭ��
                    animator.Play("PlayerSheepAttack", 0, 0f);
                animator.SetBool("playerAttack", true);
                break;
            case "damage":
                if(!animator.GetBool("playerAttack"))    //�÷��̾ ���� ���� ���°� �ƴϾ�� ��
                {
                    if (animator.GetBool("playerDamage")&&!animator.GetBool("playerSheep"))   
                        animator.Play("PlayerDamage", 0, 0f); 
                    animator.SetBool("playerDamage", true);
                    break;
                }
                break;
        }
    }

    public void SetPlayerConstellaState(string name)
    {
        if (!animator)
            return;
        switch (name)
        {
            case "Sheep":
                animator.SetBool("playerSheep", true);
                break;
            case "Idle":
                animator.SetBool("playerSheep", false);
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
