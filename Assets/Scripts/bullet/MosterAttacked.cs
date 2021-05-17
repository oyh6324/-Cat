using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MosterAttacked : MonoBehaviour
{
    Rigidbody2D rigid;
    public LayerMask isLayer;
    Vector2 frontVec;
    RaycastHit2D rayhit;

    //cri 확률
    private decimal crip;
    GameObject bullet;
    public static bool criDamege;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        crip = 0.05m;//crip=DemoDataManager.characterDatasList[0].allcrip;  
        bullet = this.gameObject;
    }
    private void Update()
    {
        if (this.gameObject.transform.localScale.x == 0.5f)
        {
            rayhit = Physics2D.Raycast(transform.position, transform.right, 0.5f, isLayer);
            Debug.DrawRay(transform.position, transform.right, new Color(0, 1, 0));
        }
        else if(this.gameObject.transform.localScale.x == -0.5f)
        {
            rayhit = Physics2D.Raycast(transform.position, -transform.right, 0.5f, isLayer);
            Debug.DrawRay(transform.position, -transform.right, new Color(0, 1, 0));
        }
        if (rayhit.collider != null)
        {
            Destroy(bullet);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            int r;
            if (crip == 0.5m)
            {
                r = Random.Range(0, 201);
                if (r == 5)
                {
                    criDamege = true;
                }
            }
            else
            {
                r = Random.Range(0, 101);
                {
                    if (r <= crip)
                    {
                        criDamege = true;
                    }
                }
            }
            Debug.Log("아야!");
            Destroy(bullet);
            //OnDestroy();
        }
        if(collision.gameObject.tag=="Collider"||collision.gameObject.tag=="EmemySkill")
        {
            Destroy(bullet);
        }
    }
    private void OnDestroy()
    {
        Destroy(bullet);
    }
}
