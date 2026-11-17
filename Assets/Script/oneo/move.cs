using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class move : MonoBehaviour
{
    private Rigidbody2D rb;  //刚体
    private SpriteRenderer sprite;  //图像
    private Animator anim;  //动画
    private float dirX = 0f;  //水平值

    [SerializeField] private float moveSpeed = 3f;  //移动速度

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  //刚体组件
        sprite = GetComponent<SpriteRenderer>();  //图像组件
        anim = GetComponent<Animator>();  //动画组件
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");  //检测水平输入
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);  //水平速度
        UpdateAnimationState();  //动画控制
    }

    private void UpdateAnimationState()
    {
        if (dirX > 0f)
        {
            anim.SetBool("isMoving", true);
            sprite.flipX = false;  //图像不翻转
        }

        else if (dirX < 0f)
        {
            anim.SetBool("isMoving", true);
            sprite.flipX = true;  //图像翻转
        }
        else anim.SetBool("isMoving", false);
    }
}
