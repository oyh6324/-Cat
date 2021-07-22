using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public static float maxSpeed;
    public float JumpPower;
    SpriteRenderer spriterenderer;
    public SpriteRenderer gunsprite;
    public GameObject weapon;
    Animator anim;

    //hp manage
    public static bool isLiving;
    public Slider hpbar;
    public Image Heart1;
    public Image Heart2;
    public Image Heart3;
    public Text Hptx;
    private float maxHp;
    public int HeartCnt;

    MonsterStat monstat;
    public static bool isSlow; //캐릭터의 움직임이 느려졌는지 확인
    public static float curHp; //스킬을 이용한 공격을 받을 때 체력 불러야함
    public static int MonsterIndex; //스칠 때 데미지 
    int damage;

    //의상
    public Animator[] dressAnim;
    public SpriteRenderer[] dressRenderer;
    public Sprite[] weaponSprites;

    //오디오
    private AudioSource audioSource;
    public AudioClip attacktedClip;
    public AudioClip deadClip;
    public AudioClip jumpClip;
    void Start()
    {
        isSlow = false;
        isLiving = true;
        rigid = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        //gunsprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        maxHp = maxHp=DemoDataManager.Instance.characterDatasList[0].hp;
        curHp = maxHp;
        HeartCnt = 3;
        hpbar.value = (float)curHp / (float)maxHp;
        Hptx.text = curHp + "/" + maxHp;
        maxSpeed = 4f;

        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        WearingCheck();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSlow==false)
        {
            maxSpeed = 4;
        }
        Debug.Log(PlayerMove.maxSpeed);
        //체력바 업데이트
        hpbar.value = Mathf.Lerp(hpbar.value, (float)curHp / (float)maxHp, Time.deltaTime * 10);
        //jump ani
        if (Input.GetButtonDown("Jump")&&!anim.GetBool("isJumping")&&isLiving==true)
        {
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            audioSource.PlayOneShot(jumpClip);

            for(int i=0; i<3; i++)
                dressAnim[i].SetBool("isJumping", true);
        }
        //stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        //direction sprite
        if (Input.GetAxisRaw("Horizontal") == 1 && isLiving == true)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 && isLiving == true)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        //animation isWalking
        if (rigid.velocity.normalized.x==0)
        {
            anim.SetBool("isWalking", false);

            for (int i = 0; i < 3; i++)
                dressAnim[i].SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);

            for (int i = 0; i < 3; i++)
                dressAnim[i].SetBool("isWalking", true);
        }
        Hptx.text = curHp + "/" + maxHp;
    }
    private void FixedUpdate()
    {
        //press directionn key
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 raypos = new Vector3(transform.position.x, transform.position.y - 0.5f); //지면에 붙어있는지 확인하는 선
        if(isLiving==true)
        {
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse); //addforce 누를수록 가속
            if (rigid.velocity.x > maxSpeed)
            {
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            }
            else if (rigid.velocity.x < maxSpeed * (-1))
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
        //landing Platform
        if(rigid.velocity.y<0)
        {
            Debug.DrawRay(raypos, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(raypos, Vector3.down, 1, LayerMask.GetMask("tile"));
            if (rayhit.collider != null)
            {
                if (rayhit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);

                    for (int i = 0; i < 3; i++)
                        dressAnim[i].SetBool("isJumping", false);
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) //적, 적스킬, 적공격에 충돌할 경우
    {
        if(collision.gameObject.tag=="Enemy")
        {
            damage = MonsterStat.MonsterStr[MonsterIndex];
            onDamaged(collision.transform.position);
        }
        else if(collision.gameObject.tag=="EnemySkill"||collision.gameObject.tag=="EnemyBullet")
        {
            damage = MonsterStat.MonsterAttackStr[MonsterIndex];
            onDamaged(collision.transform.position);
        }
        if(collision.gameObject.tag=="Fall") //Player fall
        {
            HeartCnt--;
            StartCoroutine(offFall());
        }
    }
    private void OnCollisionStay2D(Collision2D collision) //적 총알과 충돌시 적 총알 삭제
    {
        if(collision.gameObject.tag=="EnemyBullet")
        {
            Destroy(collision.gameObject);
        }
    }
    void onDamaged(Vector2 targetPos) //데미지 입었을 때
    {
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        if (curHp >damage)
        {
            gameObject.layer = 13;
            curHp -= damage;
            spriterenderer.color = new Color(1, 1, 1, 0.4f); //반투명화
            gunsprite.color = new Color(1, 1, 1, 0.4f); //반투명화

            for(int i=0; i<3; i++)
                dressRenderer[i].color= new Color(1, 1, 1, 0.4f); //반투명화

            rigid.velocity = Vector2.zero; //붙은 가속 제거
            rigid.AddForce(new Vector2(dirc * 10f,5f), ForceMode2D.Impulse);
            anim.SetTrigger("isAttacked");
            audioSource.PlayOneShot(attacktedClip);

            for (int i = 0; i < 3; i++)
                dressAnim[i].SetTrigger("isAttacked");

            Invoke("offDameged", 1.2f);
        }
        else
        {
            HeartCnt--;
            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(dirc * 2f, 7f), ForceMode2D.Impulse);
            anim.SetTrigger("isAttacked");

            for (int i = 0; i < 3; i++)
                dressAnim[i].SetTrigger("isDying");
            anim.SetTrigger("isDying");
            audioSource.PlayOneShot(deadClip);

            StartCoroutine(onDied());
        }
    }
    void offDameged() //투명화 상태 돌아옴
    {
        gameObject.layer = 12;
        spriterenderer.color = new Color(1, 1, 1, 1);
        gunsprite.color = new Color(1, 1, 1, 1);

        for (int i = 0; i < 3; i++)
            dressRenderer[i].color = new Color(1, 1, 1, 1);
    }
    IEnumerator onDied() //하트 소모
    {
        weapon.SetActive(false);
        isLiving = false;
        gameObject.layer = 13;
        curHp = 0;

        for (int i=1; i<=5; i++)
        {
            gunsprite.color = new Color(1, 1, 1, 1 - 0.2f * i);
            spriterenderer.color = new Color(1, 1, 1, 1 - 0.2f * i);

            for (int j = 0; j < 3; j++)
                dressRenderer[j].color = new Color(1, 1, 1, 1 - 0.2f * i);

            yield return new WaitForSeconds(0.58f);
        }
        for(int i=0; i<=5; i++)
        {
            if (HeartCnt == 2)
            {
                Heart3.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.18f);
                Heart3.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.18f);        
            }
            else if (HeartCnt == 1)
            {
                Heart2.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.18f);
                Heart2.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.18f);
            }
            else if (HeartCnt == 0)
            {
                Heart1.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.18f);
                Heart1.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.18f);
            }
        }
        StartCoroutine(offDied());
    }
    IEnumerator offDied() //부활할때 투명도, 레이어 조절
    {
        yield return new WaitForSeconds(1.2f);
        curHp = maxHp;
        Vector2 relifePos = new Vector2(transform.position.x, transform.position.y + 0.5f);
        transform.position = relifePos;
        for (int i = 1; i <= 5; i++)
        {
            spriterenderer.color = new Color(1, 1, 1, 0+ 0.2f * i);
            gunsprite.color = new Color(1, 1, 1, 0 +0.2f * i);

            for (int j = 0; j < 3; j++)
                dressRenderer[j].color = new Color(1, 1, 1, 0 + 0.2f * i);

            yield return new WaitForSeconds(0.5f);
        }
        isLiving = true;
        weapon.SetActive(true);
        gameObject.layer = 12;
    }

    IEnumerator offFall() //추락시 다시 부활
    {
        isLiving = false;
        gameObject.layer = 13;
        for (int i = 0; i <= 5; i++)
        {
            if (HeartCnt == 2)
            {
                Heart3.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.18f);
                Heart3.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.18f);
            }
            else if (HeartCnt == 1)
            {
                Heart2.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.18f);
                Heart2.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.18f);
            }
            else if (HeartCnt == 0)
            {
                Heart1.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.18f);
                Heart1.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.18f);
            }
        }
        curHp = maxHp;
        Vector2 relifePos = new Vector2(-11,-5);
        if (HeartCnt > 0)
        {
            transform.position = relifePos;
            isLiving = true;
            for (int i = 1; i <= 5; i++)
            {
                spriterenderer.color = new Color(1, 1, 1, 0 + 0.2f * i);
                yield return new WaitForSeconds(0.5f);
            }
            gameObject.layer = 12;
        }
    }

    private void WearingCheck()
    {
        string topName = DemoDataManager.Instance.characterDatasList[0].top;
        string bottomName = DemoDataManager.Instance.characterDatasList[0].bottoms;
        string helmetName = DemoDataManager.Instance.characterDatasList[0].helmet;
        string gunName = DemoDataManager.Instance.characterDatasList[0].weapon;

        for(int i=0; i<DemoDataManager.Instance.allClothesItemList.Count; i++)
        {
            if(topName == DemoDataManager.Instance.allClothesItemList[i].name)
                dressAnim[0].SetInteger("index", i);

            if (bottomName == DemoDataManager.Instance.allClothesItemList[i].name)
                dressAnim[1].SetInteger("index", i);

            if (helmetName == DemoDataManager.Instance.allClothesItemList[i].name)
                dressAnim[2].SetInteger("index", i);
        }

        if (topName == "")
            dressAnim[0].SetInteger("index", -1);

        if (bottomName == "")
            dressAnim[1].SetInteger("index", -1);

        if (helmetName == "")
            dressAnim[2].SetInteger("index", -1);

        for(int i=0; i<DemoDataManager.Instance.allWeaponItemList.Count; i++)
        {
            if (gunName == DemoDataManager.Instance.allWeaponItemList[i].name)
            {
                weapon.GetComponent<SpriteRenderer>().sprite = weaponSprites[i];
            }
        }
    }
}
