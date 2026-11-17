using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TextAsset chart;
    public AudioSource musicPalyer;
    public GameObject small;
    public GameObject median;
    public GameObject big;
    public GameObject pause;

    // 添加暂停状态变量
    public static bool IsPaused { get; private set; } = false;

    // 添加暂停时的音乐时间记录
    private float pauseTime;

    float[] timeStamps;
    int index = 0;
    float myTime = 0;
    string[] type;

    void Awake()
    {
        // 重置所有静态状态
        DataTransfer.noteQueue.Clear();
        DataTransfer.lastJudgeFrame = -1;
        IsPaused = false; // 重置暂停状态

        // 重置实例变量
        index = 0;
        myTime = 0;

        // 确保时间缩放正常
        Time.timeScale = 1f;

        LoadChart();
        musicPalyer.Pause();  //暂停音乐
        StartCoroutine("GameStart");  //协程函数控制音乐播放/暂停
    }

    void Update()
    {
        // 如果游戏暂停，不执行任何更新
        if (IsPaused) return;

        if (index < timeStamps.Length)
        {
            myTime += Time.deltaTime;
            if (myTime >= timeStamps[index])
            {
                // 根据 type[index] 选择预制体
                GameObject prefabToSpawn = null;
                switch (type[index])
                {
                    case "small":
                        prefabToSpawn = small;
                        break;
                    case "median":
                        prefabToSpawn = median;
                        break;
                    case "big":
                        prefabToSpawn = big;
                        break;
                    default:
                        prefabToSpawn = small; // 默认用 Tap
                        break;
                }
                Instantiate(prefabToSpawn, new Vector2(15, -3), Quaternion.identity);
                index++;
            }
        }
    }

    IEnumerator GameStart()  //协程函数
    {
        yield return new WaitForSeconds(3f);  //延迟三秒
        musicPalyer.Play();  //开始播放音乐
    }

    public void TimeJudge()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
    }

    void LoadChart()
    {
        string[] everyLine = chart.text.Split("\n");  //把 chart.text按行切割，每一行形成一个数组元素。
        timeStamps = new float[everyLine.Length];  //存储每个音符出现的时间点
        type = new string[everyLine.Length]; //存储每个音符的种类
        for (int i = 0; i < everyLine.Length; i++)
        {
            string[] everyPart = everyLine[i].Split(',');  //用逗号分割，得到一个数组
            //把分割出来的第一个部分（时间）和第二个部分（音符类型）转换为 float 类型，分别存到对应数组
            timeStamps[i] = Convert.ToSingle(everyPart[0]);
            type[i] = everyPart[1].Trim();
        }
    }

    // 添加暂停方法
    public void PauseGame()
    {
        if (!IsPaused)
        {
            IsPaused = true;
            pauseTime = musicPalyer.time; // 记录当前音乐时间
            musicPalyer.Pause();
            Time.timeScale = 0; // 暂停游戏时间
        }
    }

    // 添加继续方法
    public void ResumeGame()
    {
        if (IsPaused)
        {
            IsPaused = false;
            Time.timeScale = 1; // 恢复游戏时间
            musicPalyer.time = pauseTime; // 恢复音乐时间
            musicPalyer.Play();
        }
    }

    // 添加切换暂停状态的方法（可用于按钮）
    public void TogglePause()
    {
        if (IsPaused)
        {
            pause.SetActive(false);
            ResumeGame();
        }
        else
        {
            pause.SetActive(true);
            PauseGame();
        }
    }
}