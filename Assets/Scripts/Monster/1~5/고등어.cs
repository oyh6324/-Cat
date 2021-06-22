using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 고등어 : MonoBehaviour
{
    public float speed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            PlayerMove.MonsterIndex = 0;
            lowspeed();
            Invoke("normalSpeed", speed);
        }
    }
    void lowspeed()
    {
        PlayerMove.maxSpeed = 2.5f;
        PlayerMove.isSlow = true;
    }
    void normalSpeed()
    {
        PlayerMove.isSlow = false;
        PlayerMove.maxSpeed = 4;
    }
}
