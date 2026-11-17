using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Header("UI References")]
    public Button openButton;    // 触发面板显示的按钮
    public GameObject panel;     // 需要控制的面板对象

    [Header("Game Pause Settings")]
    public bool pauseGameWhenOpen = true;  // 打开面板时是否暂停游戏

    void Start()
    {
        // 确保面板初始状态为关闭
        if (panel != null)
        {
            panel.SetActive(false);
        }

        // 绑定按钮点击事件
        if (openButton != null)
        {
            openButton.onClick.AddListener(TogglePanel);
        }
    }

    // 点击按钮时切换面板状态
    public void TogglePanel()
    {
        if (panel == null) return;

        // 切换面板激活状态（开/关）
        bool willActivate = !panel.activeSelf;
        panel.SetActive(willActivate);
    }

    // 关闭面板的公开方法
    public void ClosePanel()
    {
        if (panel != null && panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }
}