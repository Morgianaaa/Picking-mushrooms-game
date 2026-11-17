using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class illHand : MonoBehaviour
{
    public GameObject pokedexPanel;
    public Animator pokedexAnimator;
    public Text text;
    private bool isActive = false;

    public void OnTogglePokedex()
    {
        if (!isActive)
        {
            pokedexPanel.SetActive(true);         // 激活物体
            text.gameObject.SetActive(true);
            pokedexAnimator.Play("illHand");         // 播放打开动画
            isActive = true;
        }
        else
        {
            pokedexAnimator.Play("illHandc");         // 播放关闭动画
            text.gameObject.SetActive(false);
            isActive = false;
        }
    }

    // 在关闭动画的最后一帧添加Animation Event，调用此方法
    public void DeactivatePanel()
    {
        pokedexPanel.SetActive(false);            // 隐藏Panel
    }
}
