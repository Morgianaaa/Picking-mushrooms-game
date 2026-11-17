using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue111 : MonoBehaviour
{
    public TextAsset texts;
    public Text dialogueText;
    string[] dialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadTexts();

        // 直接启动延迟协程，包含暂停游戏逻辑
        StartCoroutine(DelayedStart(2f));
    }

    IEnumerator DelayedStart(float delay)
    {
        // 等待指定的真实时间（不受timeScale影响）
        yield return new WaitForSecondsRealtime(delay);

        // 延迟结束后暂停游戏并激活对话
        Time.timeScale = 0f;
        isDialogueActive = true;
        ShowCurrentLine();
    }

    void LoadTexts()
    {
        string[] everyLine = texts.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        dialogue = new string[everyLine.Length];
        for (int i = 0; i < everyLine.Length; i++)
        {
            dialogue[i] = everyLine[i].Trim();
        }
    }

    public void StartDialogue()
    {
        currentLineIndex = 0;
        isDialogueActive = true;
        ShowCurrentLine();
    }

    void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogue.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    // 显示当前对话
    void ShowCurrentLine()
    {
        if (string.IsNullOrEmpty(dialogue[currentLineIndex]))
        {
            ShowNextLine(); // 跳过空行
            return;
        }

        // 在UI Text上显示对话
        if (dialogueText != null)
        {
            dialogueText.text = dialogue[currentLineIndex];
        }
        else
        {
            Debug.Log(dialogue[currentLineIndex]); // 备用：在控制台显示
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        Time.timeScale = 1f;
        DestroySelf();
    }

    // 鼠标点击物体时触发对话（需要物体有Collider）
    void OnMouseDown()
    {
        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else
        {
            ShowNextLine();
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
