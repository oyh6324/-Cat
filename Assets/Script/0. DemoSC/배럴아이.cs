using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 배럴아이 : MonoBehaviour
{
    public GameObject player;
    public string shooterName="shooter";
    public GameObject beam;
    public Slider hpBar;
    public int index=17;
    public float speed = 1f;
    
    private float rotateSpeed = 10f;
    private int totalHp;
    private int monsterCurHp;
    private int playerStr = 30;

    private Animator anim;
    private RaycastHit hit;

    private enum State
    {
        Idle, Attacked, Shooting, Traking, Dead, RandomMove, Heading
    }
    private State state;

    private GameObject shooter;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        shooter = transform.Find(shooterName).gameObject;
    }
    private void Start()
    {
        totalHp = MonsterStat.MonsterTotalHp[index];
        monsterCurHp = totalHp;
    }
    private void Update()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, (float)monsterCurHp / (float)totalHp, Time.deltaTime * 10);

        if (state != State.RandomMove)
        {
            LookPlayer();
        }
        if (state == State.Traking)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
        }
    }
    private void Dead()
    {
        Destroy(gameObject);
    }
    private void BarInvisible()
    {
        hpBar.gameObject.SetActive(false);
    }
    IEnumerator Tracking()
    {
        state = State.Traking;

        speed = speed * 1.5f;
        yield return new WaitForSeconds(10f);

        state = State.RandomMove;
    }
    private void Heading()
    {
        state = State.Heading;
        anim.SetBool("isHeading", true);
    }
    IEnumerator Shooting()
    {
        state = State.Shooting;

        int count = 0;
        while (count < 5)
        {
            GameObject beamObj = Instantiate(beam, shooter.transform.position, shooter.transform.rotation, shooter.transform);

            count++;
            yield return new WaitForSeconds(0.3f);
        }
    }
    private void LookPlayer()
    {
        Vector2 playerLocation = player.transform.position;
        Vector2 directon = playerLocation - (Vector2)transform.position;

        float angle = Mathf.Atan2(directon.y, directon.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle-205f, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }
    private void HeadingOut()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Heading();
        }

        if (collision.gameObject.tag == "bullet")
        {
            Destroy(collision.gameObject);
            if (monsterCurHp <= playerStr)
            {
                state=State.Dead;
                monsterCurHp = 0;
                gameObject.layer = 14;
                anim.SetInteger("MonsterIndex", index);
                anim.SetTrigger("MonsterDie");
            }
            else
            {
                anim.SetInteger("MonsterIndex", index);
                anim.SetTrigger("MonsterAttacked");
                monsterCurHp -= playerStr;
            }
        }
    }
}
