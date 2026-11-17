using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : MonoBehaviour
{
    public GameObject button;

    //初始隐藏按钮与对话框
    void Start()
    {
        if (button != null)
            button.SetActive(false);
    }
    //玩家触碰时显示按钮
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            button.gameObject.SetActive(true);
        }
    }
    //玩家离开时隐藏按钮
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            button.gameObject.SetActive(false);
        }
    }

    /*public void StartPanelSequence()
    {
        FinalPanelManager manager = FindObjectOfType<FinalPanelManager>();
        manager.PauseGameAndShowPanels();
    }*/
}
