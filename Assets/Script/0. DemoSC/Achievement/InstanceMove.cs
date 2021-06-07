using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstanceMove : MonoBehaviour
{
    private Image instance;
    private bool isMove;
    private Animator targetAnim;

    public Sprite[] rewardSprite; //0:멸치 1:진주 2:멸치보석함의상 3:진주보석함의상 4:멸치보석함무기 5:진주보석함무기

    public Vector2 pearlPosition; //상단 진주 위치
    public Vector2 anchovyPostion; //상단 멸치 위치

    public Animator pearlAnim;
    public Animator anchovyAnim;
    private void Start()
    {
        instance = GetComponent<Image>();
        StartCoroutine(waitMove());
    }
    private void Update()
    {
        if (isMove)
        {
            Vector2 targetPosition=new Vector2(0,0);
            if (instance.sprite == rewardSprite[0])
            {
                targetPosition = anchovyPostion;
                targetAnim = anchovyAnim;
            }
            else if (instance.sprite == rewardSprite[1])
            {
                targetPosition = pearlPosition;
                targetAnim = pearlAnim;
            }
            else
            {
                Destroy(gameObject);
            }

            instance.transform.position = Vector2.MoveTowards(instance.transform.position, targetPosition, 12f); //이동
        }
    }
    private void OnDisable()
    {
        Destroy(gameObject);
    }
    IEnumerator waitMove()
    {
        yield return new WaitForSeconds(0.5f);
        isMove = true;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TopReward")
        {
            Destroy(gameObject);
            targetAnim.SetBool("isGet",true);
        }
    }
}
