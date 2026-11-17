using UnityEngine;
using UnityEngine.EventSystems;

public class PreciseHoverDetection : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite hoverSprite;
    private Animator animator;

    private SpriteRenderer spriteRenderer;
    private Collider2D objectCollider;
    public bool playOnClick = true;
    public string animationName = "tone";
    public AudioSource odd;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    void Update()
    {
        HandleHoverEffect();
    }

    void HandleHoverEffect()
    {
        if (objectCollider == null) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isOverlapping = objectCollider.OverlapPoint(mousePosition);

        if (isOverlapping)
        {
            // 鼠标在物体上
            if (hoverSprite != null && spriteRenderer.sprite != hoverSprite)
            {
                spriteRenderer.sprite = hoverSprite;
            }
        }
        else
        {
            // 鼠标不在物体上
            if (normalSprite != null && spriteRenderer.sprite != normalSprite)
            {
                spriteRenderer.sprite = normalSprite;
            }
        }
    }

    void OnMouseDown()
    {
        // 当鼠标点击该物体时触发
        odd.Play();
        PlayToneAnimation();
    }

    void PlayToneAnimation()
    {
        if (playOnClick && animator != null)
        {
            // 使用Animator播放动画
            animator.Play(animationName);
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
