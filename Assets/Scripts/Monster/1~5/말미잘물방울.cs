using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 말미잘물방울 : MonoBehaviour
{
    public GameObject drop;
    public Transform tr;
    public int index;
    Animator anim;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("shoot", speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CancelInvoke();
            Invoke("shoot", speed);
        }
    }
    // Update is called once per frame
    void shoot()
    {
        GameObject dropclone = Instantiate(drop, tr.position, tr.rotation);
        Rigidbody2D rigid = dropclone.GetComponent<Rigidbody2D>();
        if (this.gameObject.transform.localScale.x == -0.6f)
        {
            //drop.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            rigid.AddForce(Vector2.right * 6, ForceMode2D.Impulse);
        }
        else if (this.gameObject.transform.localScale.x == 0.6f)
        {
            //drop.transform.localScale = new Vector3(-0.8f, 0.8f, 1);
            rigid.AddForce(Vector2.left * 6, ForceMode2D.Impulse);
        }
        Invoke("shoot", speed);
        Destroy(dropclone, 1.2f);
    }
}
