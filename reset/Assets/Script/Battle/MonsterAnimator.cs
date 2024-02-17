using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    //�۵� ����� �÷��̾� �ִϸ����Ϳ� ���� ����
    [SerializeField] Animator animator;
    public float size = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetMonsterState(string name)
    {
        if (!animator)
            return;
        switch (name)
        {
            case "attack":
                if (animator.GetBool("attack"))   
                    animator.Play("attack", 0, 0f); // �ִϸ��̼��� ������ϴ� ���� - ��� ���°��� �ƴ� ���� �̸���
                animator.SetBool("attack", true);
                break;
            case "damage":
                break;
        }
    }

    public void AnimationFinish(string name)
    {
        switch (name)
        {
            case "attack":
                animator.SetBool("attack", false);
                break;
        }

    }
}
