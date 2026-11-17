using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INote
{
    bool IsFirstNote();     // 判断是否队首
    void RemoveFromQueue(); // 从队列移除自身
    void PlayHit();         // 播放命中动画
    void PlayMiss();       // 播放小号Miss动画
    void DestroySelf();     // 销毁自身
}
