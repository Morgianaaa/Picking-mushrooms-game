using UnityEngine;
using System.Collections.Generic;

public class FinalPanelManager : MonoBehaviour
{
    [Header("面板设置")]
    public GameObject[] panels; // 面板ABCDE数组
    public int currentPanelIndex = 0; // 当前显示的面板索引

    [Header("按钮设置")]
    public GameObject buttonX; // 全部正确时激活的按钮
    public GameObject buttonY; // 有错误时激活的按钮

    [Header("游戏状态")]
    public bool isGamePaused = false;

    private List<bool> panelCorrectChoices = new List<bool>(); // 记录每个面板的选择是否正确

    void Start()
    {
        // 初始化所有面板状态
        InitializePanels(true);

        // 确保开始时按钮状态正确
        UpdateButtonStates();
    }

    void Update()
    {
        
    }

    // 初始化所有面板
    private void InitializePanels(bool hideAll = false)
    {
        // 初始化选择记录列表
        panelCorrectChoices = new List<bool>();
        for (int i = 0; i < panels.Length; i++)
        {
            panelCorrectChoices.Add(false);

            // 默认隐藏所有面板
            panels[i].SetActive(false);
        }

        currentPanelIndex = 0;

        // 如果不要求隐藏所有面板，则显示第一个面板
        if (!hideAll && panels.Length > 0)
        {
            panels[0].SetActive(true);
        }
    }

    // 当正确按钮被点击时调用
    public void OnCorrectButtonClicked()
    {
        // 记录正确选择
        if (currentPanelIndex < panelCorrectChoices.Count)
        {
            panelCorrectChoices[currentPanelIndex] = true;
        }

        // 显示下一个面板或结束序列
        ShowNextPanel();
    }

    // 当错误按钮被点击时调用
    public void OnWrongButtonClicked()
    {
        // 记录错误选择
        if (currentPanelIndex < panelCorrectChoices.Count)
        {
            panelCorrectChoices[currentPanelIndex] = false;
        }

        // 显示下一个面板或结束序列
        ShowNextPanel();
    }

    // 显示下一个面板
    private void ShowNextPanel()
    {
        // 隐藏当前面板
        if (currentPanelIndex < panels.Length)
        {
            panels[currentPanelIndex].SetActive(false);
        }

        // 增加索引
        currentPanelIndex++;

        // 检查是否还有更多面板
        if (currentPanelIndex < panels.Length)
        {
            // 显示下一个面板
            panels[currentPanelIndex].SetActive(true);
        }
        else
        {
            // 所有面板已完成，检查结果
            CheckAllPanelsResult();
        }
    }

    // 检查所有面板的结果
    private void CheckAllPanelsResult()
    {
        bool allCorrect = true;

        foreach (bool correct in panelCorrectChoices)
        {
            if (!correct)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            // 所有面板都选择了正确按钮
            buttonX.SetActive(true);
            buttonY.SetActive(false);
        }
        else
        {
            // 至少有一个面板选择了错误按钮
            buttonX.SetActive(false);
            buttonY.SetActive(true);
        }
    }

    // 按钮Y的功能：关闭所有面板并继续游戏
    public void OnButtonYClicked()
    {
        HideAllPanels();
        ResumeGame();
    }

    // 暂停游戏并显示第一个面板
    public void PauseGameAndShowPanels()
    {
        InitializePanels(false); // 重置并显示第一个面板
    }

    // 继续游戏
    public void ResumeGame()
    {
    }

    // 隐藏所有面板
    private void HideAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    // 更新按钮状态
    private void UpdateButtonStates()
    {
        buttonX.SetActive(false);
        buttonY.SetActive(false);
    }

    // 重置所有面板状态
    public void ResetAllPanels()
    {
        InitializePanels(true);
        UpdateButtonStates();
    }
}