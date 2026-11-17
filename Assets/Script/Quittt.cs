using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quittt : MonoBehaviour
{
    public float delayTime = 1f; // 延迟时间
    public AudioSource QuitSFX;

    public void QuitWithDelay()
    {
        QuitSFX.Play();
        StartCoroutine(QuitAfterDelay());
    }

    IEnumerator QuitAfterDelay()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delayTime);
        QuitGame();
    }

    // 退出游戏
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
