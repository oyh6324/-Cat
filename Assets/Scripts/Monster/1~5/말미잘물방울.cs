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
        Invoke("skill", speed);
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void shoot()
    {
        GameObject dropclone = Instantiate(drop, tr.position, tr.rotation);
        Rigidbody2D rigid = dropclone.GetComponent<Rigidbody2D>();
        if (this.gameObject.transform.localScale.x == -0.6f)
        {
            rigid.AddForce(Vector2.right * 6, ForceMode2D.Impulse);
        }
        else if (this.gameObject.transform.localScale.x == 0.6f)
        {
            rigid.AddForce(Vector2.left * 6, ForceMode2D.Impulse);
        }
        Destroy(dropclone, 1.5f);
        anim.SetBool("MonsterSkill", false);
        Invoke("skill", speed);
    }
    void skill() //skill 애니 트리거 설정
    {
        anim.SetBool("MonsterSkill",true);

    }
}
