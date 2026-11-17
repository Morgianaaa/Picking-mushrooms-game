using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniStart : MonoBehaviour
{
    private Animator animator;
    public string animationName;

    void Start()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();

        // 播放默认动画
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }
}
