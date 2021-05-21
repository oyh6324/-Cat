using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDestroy : MonoBehaviour
{
    Animator anim;
    GameObject drop;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        drop = this.gameObject;
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMove.MonsterIndex = index;
            anim.SetTrigger("boom");
        }
        if (collision.gameObject.tag == "Collider")
        {
            anim.SetTrigger("boom");
        }
    }
    void OnDestroy()
    {
        Destroy(drop);
    }
}
