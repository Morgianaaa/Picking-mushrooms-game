using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuaseAni : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public float animationSpeed = 1f;

    private float currentTime = 0f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // 使用不受影响的时间
        currentTime += Time.unscaledDeltaTime * animationSpeed;

        // 使用曲线值来设置属性
        float curveValue = animationCurve.Evaluate(currentTime % 1f);
        transform.position = initialPosition + Vector3.up * curveValue;
    }
}
