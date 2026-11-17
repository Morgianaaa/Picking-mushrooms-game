using System.Collections;
using UnityEngine;

public class MedianJudge : MonoBehaviour, INote
{
    private bool isInTrigger = false;      // 是否在判定区
    private bool isFirstPressed = false; // 是否第一次按下D键
    private bool isMissed = false;       // 是否已经判定结束
    private Animator animator;
    private Coroutine medianCoroutine;   // 用于取消协程
    private TapMove tapmove;

    void Start()
    {
        tapmove = GetComponent<TapMove>();
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
        if (collider.CompareTag("Judge"))
        {
            if (!isMissed && IsFirstNote())
            {
                PlayMiss();
            }
            isInTrigger = false;
        }
    }

    void Update()
    {
        if (isMissed) return;
        if (DataTransfer.lastJudgeFrame == Time.frameCount) // 如果这一帧已经判定过，就不再判定了
            return;
        if (Input.GetKeyDown(KeyCode.D) && IsFirstNote())
        {
            DataTransfer.lastJudgeFrame = Time.frameCount;
            if (isInTrigger)
            {
                if (!isFirstPressed)
                {
                    // 第一次按下D键，进入判定状态
                    isFirstPressed = true;
                    //tapmove.PauseMove();  //暂停移动
                    PlayPause();
                    medianCoroutine = StartCoroutine(MedianJudgeRoutine());
                }
                else
                {
                    // 0.5秒内第二次按下D键
                    PlayHit();
                    if (medianCoroutine != null)
                        StopCoroutine(medianCoroutine);
                    //tapmove.ResumeMove();  //恢复移动
                }
            }
            else
            {
                PlayMiss();
            }
        }
    }

    IEnumerator MedianJudgeRoutine()
    {
        float timer = 0.5f;           // 设置判定时间窗口为0.5秒
        float elapsed = 0f;           // 已经经过的时间，初始为0
        while (elapsed < timer)       // 当经过时间还没到0.5秒
        {
            if (isMissed) yield break; // 如果已经判定（Hit或Miss），立即退出协程
            elapsed += Time.deltaTime; // 每帧累加时间（Time.deltaTime是每帧的时间间隔）
            yield return null;         // 挂起（等待下一帧），协程继续
        }
        // 如果0.5秒内没有第二次按D键，且还没判定
        if (!isMissed)
        {
            PlayMiss();                // 播放Miss动画（判定失败）
            //tapmove.ResumeMove();      //恢复移动
        }
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

    // 播放命中动画
    public void PlayHit()
    {
        animator.SetTrigger("HitMedian");
        isMissed = true;
        RemoveFromQueue();
    }

    // 播放漏判动画
    public void PlayMiss()
    {
        animator.SetTrigger("MMiss");
        isMissed = true;
        RemoveFromQueue();
    }

    public void PlayPause()
    {
        animator.SetTrigger("pause");
    }

    // 动画尾部加Animation Event，调用此方法销毁音符
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
