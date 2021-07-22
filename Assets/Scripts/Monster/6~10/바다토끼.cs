using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 바다토끼 : MonoBehaviour
{
    Rigidbody2D rigid;
    private int nextmove;
    GameObject traceTarget;
    bool isTracing;
    private bool isDied;
    SpriteRenderer spriterenderer;
    Animator anim;
    public int index;
    public Slider bar;
    int TotalHp;
    private int MonstercurHp;
    int PlayerStr;
    public float MonsterSpeed;
    public string aniName;
    bool PlaySkill;
    public GameObject drop;
    public Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        float r = Random.Range(4, 5);
        isDied = false;
        rigid = GetComponent<Rigidbody2D>();
        traceTarget = GetComponent<GameObject>();
        spriterenderer = GetComponent<SpriteRenderer>();
        Think();
        isTracing = false;
        anim = GetComponent<Animator>();
        TotalHp = MonsterStat.MonsterTotalHp[index];
        MonstercurHp = TotalHp;
        PlayerStr = 30;
        //PlayerStr=DemoDataManager.characterDatasList[0].allstr;
        MonsterSpeed = 2f;
        PlaySkill = true;
    }

    private void Update()
    {
        bar.value = Mathf.Lerp(bar.value, (float)MonstercurHp / (float)TotalHp, Time.deltaTime * 10);
        if (isTracing && isDied == false)
        {
            Vector3 playerPos = traceTarget.transform.position;
            if (playerPos.x < transform.position.x)
            {
                this.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1);
                drop.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                bar.transform.localScale = new Vector3(0.007f, 0.005f, 1);
            }
            else if (playerPos.x >= transform.position.x)
            {
                this.gameObject.transform.localScale = new Vector3(-0.6f, 0.6f, 1);
                drop.transform.localScale = new Vector3(-0.5f, 0.5f, 0);
                bar.transform.localScale = new Vector3(-0.007f, 0.005f, 1);
            }
        }
    }
    private void FixedUpdate()
    {
        if (isDied == true)
        {
            return;
        }
        else if (isTracing == false && isDied == false)
        {
            rigid.velocity = new Vector2(nextmove * MonsterSpeed, rigid.velocity.y);
            Vector2 frontVec = new Vector2(rigid.position.x + nextmove, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("tile"));
            if (rayhit.collider == null)
            {
                nextmove *= -1;
                rigid.velocity = Vector2.zero;
                CancelInvoke("Think");
                Invoke("Think", 1.5f);
            }
        }
        else
            return;
        if (nextmove == 1)
        {
            this.gameObject.transform.localScale = new Vector3(-0.6f, 0.6f, 1);
            //spriterenderer.flipX = true;
        }
        else if (nextmove == -1)
        {
            this.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            //spriterenderer.flipX = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Destroy(collision.gameObject);
            if (MonstercurHp <= PlayerStr)
            {
                isDied = true;
                MonstercurHp = 0;
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
    private void barinvisible()
    {
        bar.gameObject.SetActive(false);
    }
    private void Died()
    {
        if (isDied == true)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlaySkill = true;
            rigid.velocity = Vector2.zero; //붙은 가속 제거
            isTracing = true;
            traceTarget = collision.gameObject;
            anim.SetBool("MonsterSkill", true);
        }
        else
        {
            return;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
            traceTarget = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlaySkill = false;
            CancelInvoke("Think");
            Invoke("Think", 4f);
            anim.SetBool("MonsterSkill", false);
        }
        else
            return;
    }

    //재귀 함수 몬스터의 일반 움직임 설정
    void Think()
    {
        if (isDied == false)
        {
            isTracing = false;
            nextmove = Random.Range(-1, 2);
            float nextThinkTime = Random.Range(1f, 4f);
            Invoke("Think", nextThinkTime);
        }
    }
    void shoot() //총알 생성
    {
        GameObject dropclone = Instantiate(drop, tr.position, tr.rotation);
        Rigidbody2D rigid = dropclone.GetComponent<Rigidbody2D>();
        if(drop.transform.localScale.x==0.5f)
        {
            rigid.AddForce(Vector2.left * 4, ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(Vector2.right * 4, ForceMode2D.Impulse);
        }
        StartCoroutine(activedrop(dropclone));
        Destroy(dropclone,4f);
    }
    IEnumerator activedrop(GameObject drop) 
    {
        yield return new WaitForSeconds(1f);
        drop.gameObject.SetActive(false);
    }
}
