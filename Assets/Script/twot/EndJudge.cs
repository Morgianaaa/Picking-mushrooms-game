using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndJudge : MonoBehaviour
{
    public GameObject objectA; // 需要激活的物体A
    public GameObject objectB;
    public float MusicTime = 48f;

    void Awake()
    {
        // 确保物体A一开始是禁用的
        if (objectA != null && objectA.activeSelf)
        {
            objectA.SetActive(false);
        }

        StartCoroutine(DelayedStartChecking());
    }

    void Update()
    {
        if (Time.timeScale == 0 && objectA.activeSelf)
        {
            Time.timeScale = 1f;
        }
    }

    IEnumerator DelayedStartChecking()
    {
        yield return new WaitForSeconds(MusicTime);
        objectA.SetActive(true);
        Destroy(objectB);
    }
}