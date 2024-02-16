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
                if (animator.GetBool("playerAttack"))   //카드 연속 사용 시 애니메이션 초기화용
                    animator.Play("PlayerAttack", 0, 0f); // 애니메이션을 재시작하는 예시 - 얘는 상태값이 아닌 파일 이름임
                animator.SetBool("playerAttack", true);
                break;
            case "damage":
                if(!animator.GetBool("playerAttack"))    //플레이어가 공격 중인 상태가 아니어야 함
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
