using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MushDia : MonoBehaviour
{
    public TextAsset[] textFiles;
    public Text dialogueText;
    public GameObject targetObject;
    public bool waitForObjectDestroy = false;

    private string[] dialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private int currentTextFileIndex = 0;
    private bool objectDestroyed = false;
    private bool canProcessMouseClick = true;

    // 静态实例，用于管理全局对话状态
    private static MushDia currentActiveDialogue = null;

    void Start()
    {
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false);
        }

        if (textFiles != null && textFiles.Length > 0)
        {
            LoadTexts(currentTextFileIndex);
        }

        if (waitForObjectDestroy && targetObject != null)
        {
            StartCoroutine(WaitForObjectDestroy());
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canProcessMouseClick)
        {
            // 检测鼠标点击的物体
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                MushDia clickedDialogue = hit.transform.GetComponent<MushDia>();
                if (clickedDialogue != null && clickedDialogue != this)
                {
                    // 如果点击了其他有对话脚本的物体，中断当前对话
                    if (currentActiveDialogue != null && currentActiveDialogue != clickedDialogue)
                    {
                        currentActiveDialogue.InterruptDialogue();
                    }
                    clickedDialogue.StartDialogue();
                    return;
                }
            }

            // 如果没有点击其他对话物体，正常处理
            if (!isDialogueActive && (!waitForObjectDestroy || objectDestroyed))
            {
                StartDialogue();
            }
            else if (isDialogueActive)
            {
                ShowNextLine();
            }
        }
    }

    // 中断当前对话
    public void InterruptDialogue()
    {
        if (isDialogueActive)
        {
            EndDialogue();
            Debug.Log("对话被中断");
        }
    }

    IEnumerator WaitForObjectDestroy()
    {
        while (targetObject != null)
        {
            yield return null;
        }

        objectDestroyed = true;
        Debug.Log("目标物体已销毁，可以开始对话");
        StartDialogue();
    }

    void LoadTexts(int fileIndex)
    {
        if (textFiles == null || fileIndex >= textFiles.Length || textFiles[fileIndex] == null)
        {
            Debug.LogError("文本文件未分配或索引越界!");
            dialogue = new string[0];
            return;
        }

        string[] everyLine = textFiles[fileIndex].text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        List<string> validLines = new List<string>();

        for (int i = 0; i < everyLine.Length; i++)
        {
            string trimmedLine = everyLine[i].Trim();
            if (!string.IsNullOrEmpty(trimmedLine))
            {
                validLines.Add(trimmedLine);
            }
        }

        dialogue = validLines.ToArray();
    }

    public void StartDialogue(int fileIndex = 0)
    {
        // 如果已经有活跃对话，先中断它
        if (currentActiveDialogue != null && currentActiveDialogue != this)
        {
            currentActiveDialogue.InterruptDialogue();
        }

        if (waitForObjectDestroy && !objectDestroyed)
        {
            Debug.Log("等待目标物体销毁中...");
            return;
        }

        if (textFiles == null || textFiles.Length == 0)
        {
            Debug.LogWarning("没有可用的文本文件!");
            return;
        }

        if (fileIndex >= textFiles.Length)
        {
            Debug.LogWarning("文本文件索引越界!");
            return;
        }

        LoadTexts(fileIndex);
        currentTextFileIndex = fileIndex;

        if (dialogue.Length == 0)
        {
            Debug.LogWarning("对话内容为空");
            return;
        }

        currentLineIndex = 0;
        isDialogueActive = true;
        canProcessMouseClick = true;

        // 设置当前活跃对话
        currentActiveDialogue = this;

        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
        }

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

    void ShowCurrentLine()
    {
        if (currentLineIndex >= dialogue.Length || string.IsNullOrEmpty(dialogue[currentLineIndex]))
        {
            ShowNextLine();
            return;
        }

        if (dialogueText != null)
        {
            dialogueText.text = dialogue[currentLineIndex];
        }
        else
        {
            Debug.Log(dialogue[currentLineIndex]);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        canProcessMouseClick = false;

        // 如果当前对话是活跃对话，清除引用
        if (currentActiveDialogue == this)
        {
            currentActiveDialogue = null;
        }

        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false);
        }
    }

    // 外部方法：重新启用鼠标点击处理
    public void EnableMouseClickProcessing()
    {
        canProcessMouseClick = true;
    }

    // 外部方法：禁用鼠标点击处理
    public void DisableMouseClickProcessing()
    {
        canProcessMouseClick = false;
    }

    public void SetObjectDestroyed(bool destroyed)
    {
        objectDestroyed = destroyed;
    }

    public void StartWatchingObject(GameObject objToWatch)
    {
        targetObject = objToWatch;
        waitForObjectDestroy = true;
        objectDestroyed = false;
        StartCoroutine(WaitForObjectDestroy());
    }

    // 获取当前是否有活跃对话
    public static bool IsAnyDialogueActive()
    {
        return currentActiveDialogue != null;
    }

    // 获取当前活跃的对话
    public static MushDia GetActiveDialogue()
    {
        return currentActiveDialogue;
    }
}