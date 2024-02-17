using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    //작동 방식은 플레이어 애니메이터와 거의 동일
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
                    animator.Play("attack", 0, 0f); // 애니메이션을 재시작하는 예시 - 얘는 상태값이 아닌 파일 이름임
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
