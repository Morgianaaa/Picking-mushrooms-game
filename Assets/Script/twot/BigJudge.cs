using System.Collections;
using UnityEngine;

public class BigJudge : MonoBehaviour, INote
{
    public float holdTime = 1.5f;  // 需要长按的时间（秒）
    private bool isHolding = false;
    private bool isInTrigger = false;      // 是否在判定区
    private bool isMissed = false;         // 是否已经漏判
    private Animator animator;
    private Coroutine holdCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        DataTransfer.noteQueue.Add(this);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Judge"))
        {
            isInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Judge") && IsFirstNote())
        {
            if (!isMissed) PlayMiss();
            StopHoldCoroutine();
        }
        isInTrigger = false;
    }

    void Update()
    {
        if (isMissed) return;
        if (DataTransfer.lastJudgeFrame == Time.frameCount) // 如果这一帧已经判定过，就不再判定了
            return;
        DataTransfer.lastJudgeFrame = Time.frameCount;
        if (isInTrigger)
        {
            // 开始长按
            if (!isMissed && Input.GetKeyDown(KeyCode.D) && !isHolding && IsFirstNote())
            {
                isHolding = true;
                PlayHold();
                holdCoroutine = StartCoroutine(HoldJudgeRoutine());
            }

            // 提前松开
            if (isHolding && Input.GetKeyUp(KeyCode.D))
            {
                PlayMiss();
                StopHoldCoroutine();
            }
        }
        else
        {
            // 判定区外按下D键直接Miss
            if (Input.GetKeyDown(KeyCode.D))
            {
                PlayMiss();
            }
        }
    }

    IEnumerator HoldJudgeRoutine()
    {
        float elapsed = 0f;
        while (elapsed < holdTime)
        {
            if (!Input.GetKey(KeyCode.D) || !isInTrigger || isMissed)
            {
                // 长按期间任何时候松开D键/离开判定区/已经判定都Miss
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        // 成功长按到规定时间
        PlayHit();
    }

    void StopHoldCoroutine()
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        isHolding = false;
    }


    // 判定自己是不是队头
    public bool IsFirstNote()
    {
        return DataTransfer.noteQueue.Count > 0 && object.ReferenceEquals(DataTransfer.noteQueue[0], this);
    }

    // 移除自己
    public void RemoveFromQueue()
    {
        if (IsFirstNote())
            DataTransfer.noteQueue.RemoveAt(0);
    }

    public void PlayHold()
    {
        animator.SetTrigger("Hold");
    }

    // 播放命中动画
    public void PlayHit()
    {
        animator.SetTrigger("HitBig");
        isMissed = true;
        RemoveFromQueue();
    }

    // 播放漏判动画
    public void PlayMiss()
    {
        animator.SetTrigger("BMiss");
        isMissed = true;
        RemoveFromQueue();
    }

    // 动画尾部加Animation Event，调用此方法销毁音符
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
