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
    public GameObject droppos;

    //오디오
    private AudioSource audioSource;
    public AudioClip attacktedClip;

    // Start is called before the first frame update
    void Start()
    {
        isDied = false;
        isTracing = false;
        traceTarget = GetComponent<GameObject>();
        spriterenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        TotalHp = MonsterStat.MonsterTotalHp[index];
        MonstercurHp = TotalHp;
        PlayerStr = DemoDataManager.Instance.characterDatasList[0].allstr;
        //PlayerStr=DemoDataManager.characterDatasList[0].allstr;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.value = Mathf.Lerp(bar.value, (float)MonstercurHp / (float)TotalHp, Time.deltaTime * 10);
        if (isDied == false && isTracing == true) 
        {
            Vector3 playerPos = traceTarget.transform.position;
            if (playerPos.x < transform.position.x)
            {
                this.gameObject.transform.localScale= new Vector3(0.6f, 0.6f, 1);
                bar.transform.localScale = new Vector3(0.007f, 0.005f, 1);
            }
            else if (playerPos.x >= transform.position.x)
            {
                this.gameObject.transform.localScale = new Vector3(-0.6f, 0.6f, 1);
                bar.transform.localScale = new Vector3(-0.007f, 0.005f, 1);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            audioSource.PlayOneShot(attacktedClip);
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
            isTracing = true;
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
