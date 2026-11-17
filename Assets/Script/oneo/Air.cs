using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Air : MonoBehaviour
{
    public Text dialogueText;
    public string[] dialogueArray; // 直接用数组存储对话内容
    public GameObject triggerObjectA; // 物体A，碰撞后触发对话
    public string triggerTag = "air"; // 或者使用标签检测
    public bool useTag = false; // 是否使用标签检测
    public bool allowRepeatedTriggers = true; // 是否允许重复触发
    public float triggerCooldown = 1.0f; // 触发冷却时间

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool hasTriggered = false; // 防止重复触发
    private bool isInCooldown = false; // 冷却状态

    void Start()
    {
        // 开始时隐藏对话框
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 如果对话已激活，检测鼠标点击继续对话
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    // 碰撞检测 - 碰到物体A时触发
    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckTrigger(collision.gameObject);
    }

    // 触发器检测
    void OnTriggerEnter2D(Collider2D other)
    {
        CheckTrigger(other.gameObject);
    }

    // 检查触发条件
    void CheckTrigger(GameObject otherObject)
    {
        // 如果正在冷却中，不触发
        if (isInCooldown) return;

        // 如果对话正在进行中，不重复触发
        if (isDialogueActive) return;

        // 如果不允许重复触发且已经触发过，不触发
        if (!allowRepeatedTriggers && hasTriggered) return;

        bool shouldTrigger = false;

        if (useTag)
        {
            // 使用标签检测
            shouldTrigger = otherObject.CompareTag(triggerTag);
        }
        else
        {
            // 使用特定物体检测
            shouldTrigger = (triggerObjectA != null && otherObject == triggerObjectA);
        }

        if (shouldTrigger)
        {
            hasTriggered = true;
            StartDialogue();

            // 如果需要冷却，启动冷却协程
            if (triggerCooldown > 0)
            {
                StartCoroutine(TriggerCooldown());
            }
        }
    }

    // 冷却协程
    IEnumerator TriggerCooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(triggerCooldown);
        isInCooldown = false;
    }

    public void StartDialogue()
    {
        if (dialogueArray == null || dialogueArray.Length == 0)
        {
            Debug.LogWarning("对话内容为空!");
            return;
        }

        currentLineIndex = 0;
        isDialogueActive = true;

        // 显示对话框
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
        }

        ShowCurrentLine();
    }

    void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueArray.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowCurrentLine()
    {
        if (currentLineIndex >= dialogueArray.Length || string.IsNullOrEmpty(dialogueArray[currentLineIndex]))
        {
            ShowNextLine();
            return;
        }

        if (dialogueText != null)
        {
            dialogueText.text = dialogueArray[currentLineIndex];
        }
        else
        {
            Debug.Log(dialogueArray[currentLineIndex]);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;

        // 隐藏对话框
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false);
        }
    }

    // 外部方法：重置触发状态，允许再次触发
    public void ResetTrigger()
    {
        hasTriggered = false;
        isInCooldown = false;
    }

    // 外部方法：设置是否允许重复触发
    public void SetAllowRepeatedTriggers(bool allow)
    {
        allowRepeatedTriggers = allow;
    }

    // 外部方法：设置冷却时间
    public void SetTriggerCooldown(float cooldown)
    {
        triggerCooldown = cooldown;
    }

    // 外部方法：动态设置对话内容
    public void SetDialogueContent(string[] newDialogue)
    {
        dialogueArray = newDialogue;
    }

    // 外部方法：添加单条对话
    public void AddDialogueLine(string line)
    {
        if (dialogueArray == null)
        {
            dialogueArray = new string[1] { line };
        }
        else
        {
            string[] newArray = new string[dialogueArray.Length + 1];
            dialogueArray.CopyTo(newArray, 0);
            newArray[dialogueArray.Length] = line;
            dialogueArray = newArray;
        }
    }

    // 外部方法：强制触发对话
    public void ForceTriggerDialogue()
    {
        if (!isDialogueActive)
        {
            StartDialogue();
        }
    }

    // 外部方法：检查是否正在对话中
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}