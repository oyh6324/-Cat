using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 말미잘 : MonoBehaviour
{
    Animator anim;
    public int index;
    public Slider bar;
    int TotalHp;
    int PlayerStr;
    bool isTracing;
    private bool isDied;
    private int MonstercurHp;
    public float MonsterSpeed;
    SpriteRenderer spriterenderer;
    GameObject traceTarget;
    // Start is called before the first frame update
    void Start()
    {
        isDied = false;
        traceTarget = GetComponent<GameObject>();
        spriterenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        TotalHp = MonsterStat.MonsterTotalHp[index];
        MonstercurHp = TotalHp;
        PlayerStr = 30;
        //PlayerStr=DemoDataManager.characterDatasList[0].allstr;
    }

    // Update is called once per frame
    void Update()
    {
        bar.value = Mathf.Lerp(bar.value, (float)MonstercurHp / (float)TotalHp, Time.deltaTime * 10);
        if (isDied == false)
        {
            Vector3 playerPos = traceTarget.transform.position;
            if (playerPos.x < transform.position.x)
            {
                transform.localScale= new Vector3(0.6f, 0.6f, 1);
                //spriterenderer.flipX = false;
            }
            else if (playerPos.x >= transform.position.x)
            {
                transform.localScale = new Vector3(-0.6f, 0.6f, 1);
                //spriterenderer.flipX = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            if (MonstercurHp <= PlayerStr)
            {
                MonstercurHp = 0;
                isDied = true;
                gameObject.layer = 14;
                anim.SetInteger("MonsterIndex", index);
                anim.SetTrigger("MonsterDie");
            }
            else
            {
                anim.SetInteger("MonsterIndex", index);
                anim.SetTrigger("MonsterAttacked");
                MonstercurHp -= PlayerStr;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            PlayerMove.MonsterIndex = index;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            traceTarget = collision.gameObject;
            CancelInvoke();
        }
        else
        {
            return;
        }
    }
    private void barinvisible()
    {
        bar.gameObject.SetActive(false);
    }
    private void Died()
    {
        if (isDied == true)
        {
            Destroy(this.gameObject);
        }
    }
}
