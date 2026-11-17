using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapMove : MonoBehaviour
{
    public float speed = 5f;
    private bool canMove = true;

    void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    // ÔÝÍ£ÒÆ¶¯
    public void PauseMove()
    {
        canMove = false;
    }

    // »Ö¸´ÒÆ¶¯
    public void ResumeMove()
    {
        canMove = true;
    }
}
