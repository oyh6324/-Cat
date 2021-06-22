using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 배럴아이_빔 : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
    private Vector2 playerLocation;
    private float beamSpeed = 6f;

    public int index = 17;
    private void Awake()
    {
        player = GameObject.Find("Player");
        playerLocation = player.transform.position;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,playerLocation,Time.deltaTime*beamSpeed);

        if ((Vector2)transform.position == playerLocation)
        {
            animator.SetTrigger("Attack");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            animator.SetTrigger("Attack");
            PlayerMove.MonsterIndex = index;
        }
    }
    public void MyDestroy()
    {
        Destroy(gameObject);
    }
}
