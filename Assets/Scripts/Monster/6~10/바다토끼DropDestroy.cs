using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 바다토끼DropDestroy : MonoBehaviour
{
    Animator anim;
    GameObject drop;
    public int index;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        drop = this.gameObject;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMove.MonsterIndex = index;
            anim.SetTrigger("boom");
            lowspeed();
            Invoke("normalSpeed", speed);
        }
        if (collision.gameObject.tag == "Collider"&&collision.gameObject.tag=="Tile")
        {
            anim.SetTrigger("boom");
        }
    }
    void OnDestroy()
    {
        this.gameObject.SetActive(false);
        Destroy(drop,4f);
    }
    void lowspeed()
    {
        PlayerMove.maxSpeed = 2.5f;
        PlayerMove.isSlow = true;
    }
    void normalSpeed()
    {
        Debug.Log("실행");
        PlayerMove.isSlow = false;
        PlayerMove.maxSpeed = 4f;
    }
}
