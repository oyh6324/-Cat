using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class MonsterMove : MonoBehaviour
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
    void Awake()
    {
        isDied = false;
        rigid = GetComponent<Rigidbody2D>();
        traceTarget = GetComponent<GameObject>();
        spriterenderer = GetComponent<SpriteRenderer>();
        Think();
        isTracing = false;
        anim = GetComponent<Animator>();
        TotalHp = MonsterStat.MonsterTotalHp[index];
        MonstercurHp = TotalHp;
        PlayerStr = 30; //캐릭터 공격력 수정
        //PlayerStr=DemoDataManager.characterDatasList[0].allstr;
        MonsterSpeed = 2f;
    }
    private void Update()
    {
        bar.value = Mathf.Lerp(bar.value, (float)MonstercurHp / (float)TotalHp, Time.deltaTime * 10);
        if (isTracing&&isDied==false)
        {
            Vector3 playerPos = traceTarget.transform.position;
            if (playerPos.x < transform.position.x)
            {
                spriterenderer.flipX = false;
            }
            else if (playerPos.x >= transform.position.x)
            {
                spriterenderer.flipX = true;
            }
        }
        if(isDied==true)
        {
            PlayerMove.isSlow = false;
        }
    }

    private void FixedUpdate()
    {
        //몬스터 움직임
        if(isDied==true)
        {
            return;
        }
        else if (isTracing == false&&isDied==false)
        {
            rigid.velocity = new Vector2(nextmove * MonsterSpeed, rigid.velocity.y); //mosterspeed값 직접 입력
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
        {
            PlayerTracing();
        }
        if (nextmove == 1)
        {
            spriterenderer.flipX = true;
        }
        else if(nextmove==-1)
        {
            spriterenderer.flipX = false;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="Player")
        {
            traceTarget = collision.gameObject;
            CancelInvoke();
        }
        else
        {
            return;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTracing = true;
        }
        else
        {
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTracing = false;
            CancelInvoke();
            StartCoroutine("Think");
        }
        else
            return;
    }

    //재귀 함수 몬스터의 일반 움직임 설정
    void Think()
    {
        if (isDied == false)
        {
            nextmove = Random.Range(-1, 2);
            float nextThinkTime = Random.Range(1f, 4f);
            Invoke("Think", nextThinkTime);
        }
    }
    void PlayerTracing() //플레이어 추적 움직임 설정
    {
        Vector3 moveVelocity=Vector3.zero;
        string dist = "";
        if(isTracing&&isDied==false)
        {
            Vector3 playerPos = traceTarget.transform.position;

            if(playerPos.x<transform.position.x)
            {
                dist = "Left";
                spriterenderer.flipX = false;
            }
            else if(playerPos.x > transform.position.x)
            {
                dist = "Right";
                spriterenderer.flipX = true;
            }
        }
        else
        {
            StartCoroutine("Think");
        }
        if(dist=="Left")
        {
            moveVelocity = Vector3.left;
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
        }
        transform.position += moveVelocity * (MonsterSpeed+1f) * Time.deltaTime;
    }
}
