using UnityEngine;

public class NoteJudge : MonoBehaviour,INote
{
    private bool isInTrigger = false;      // 是否在判定区
    private bool isMissed = false;         // 是否已经漏判
    private Animator animator;

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
        if (collider.CompareTag("Judge"))
        {
            // 离开判定区，如果没按D键，触发Miss动画
            if (!isMissed && IsFirstNote())
            {
                PlayMiss();
            }
            isInTrigger = false;
        }
    }

    void Update()
    {
        if (DataTransfer.lastJudgeFrame == Time.frameCount) // 如果这一帧已经判定过，就不再判定了
            return;
        if (!isMissed && Input.GetKeyDown(KeyCode.D) && IsFirstNote())
        {
            DataTransfer.lastJudgeFrame = Time.frameCount;
            if (isInTrigger)
            {
                PlayHit();
            }
            else
            {
                PlayMiss();
            }
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
        animator.SetTrigger("Hit");
        isMissed = true;
        RemoveFromQueue();
    }

    // 播放漏判动画
    public void PlayMiss()
    {
        animator.SetTrigger("SMiss");
        isMissed = true;
        RemoveFromQueue();
    }

    // 动画尾部加Animation Event，调用此方法销毁音符
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}