using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScene : MonoBehaviour
{
    public string SceneName;
    public float delayTime = 1f; // 延迟时间
    public GameObject targetObject;
    public AudioSource JS;

    // 按钮点击事件
    public void OnButtonClickkk()
    {
        if (targetObject != null)
        {
            Destroy(targetObject);
            Debug.Log("物体已被销毁");
        }
        else
        {
            Debug.LogWarning("目标物体未指定");
        }
    }

    public void LoadSceneWithDelay()
    {
        JS.Play();
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delayTime);

        // 跳转场景
        if (!string.IsNullOrEmpty(SceneName))
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
