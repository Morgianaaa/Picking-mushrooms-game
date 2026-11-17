using UnityEngine;

public class QPut : MonoBehaviour
{
    private Animator animator;
    private bool isHoldingD = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 检测D键按下
        if (Input.GetKeyDown(KeyCode.D))
        {
            isHoldingD = true;
            PlayAnimation();
        }

        // 检测D键释放
        if (Input.GetKeyUp(KeyCode.D))
        {
            isHoldingD = false;
            ReturnToDefault();
        }
    }

    public void PlayAnimation()
    {
        // 直接切换到QQQ状态
        animator.Play("QQQ", 0, 0f);
    }

    private void ReturnToDefault()
    {
        // 返回默认状态
        animator.Play("QOrd", 0, 0f);
    }
}