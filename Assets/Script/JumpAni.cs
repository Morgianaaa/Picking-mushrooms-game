using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAni : MonoBehaviour
{
    public Animator animator;
    public string animationName ;

    public void OnButtonClick()
    {
        // ²¥·Å¶¯»­
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }
}
