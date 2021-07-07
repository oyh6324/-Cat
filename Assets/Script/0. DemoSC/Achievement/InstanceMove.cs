using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstanceMove : MonoBehaviour
{
    private Image instance; //해당 오브젝트 이미지
    private bool isMove; //움직임 확인
    private Animator targetAnim; //상단 보상 애니메이터

    public Sprite[] rewardSprite; //0:멸치 1:진주 2:멸치보석함의상 3:진주보석함의상 4:멸치보석함무기 5:진주보석함무기

    public Vector2 pearlPosition; //상단 진주 위치
    public Vector2 anchovyPostion; //상단 멸치 위치

    public Animator pearlAnim; //상단 진주 애니메이터
    public Animator anchovyAnim; //상단 멸치 애니메이터
    private void Start()
    {
        instance = GetComponent<Image>();
        StartCoroutine(waitMove());
    }
    private void Update()
    {
        if (isMove) //오브젝트 움직이는 중
        {
            Vector2 targetPosition=new Vector2(0,0); //오브젝트가 날아갈 위치 초기화

            if (instance.sprite == rewardSprite[0]) //오브젝트가 멸치일 때
            {
                targetPosition = anchovyPostion; //날아갈 위치
                targetAnim = anchovyAnim; //오브젝트를 받을 상단 바 게임 머니 애니메이터
            }
            else if (instance.sprite == rewardSprite[1]) //오브젝트가 진주일 때
            {
                targetPosition = pearlPosition;
                targetAnim = pearlAnim;
            }
            else //오브젝트가 멸치와 진주 외
            {
                Destroy(gameObject); //파괴
            }

            instance.transform.position = Vector2.MoveTowards(instance.transform.position, targetPosition, 12f); //이동
        }
    }
    private void OnDisable() //오브젝트가 비활성화될 때
    {
        Destroy(gameObject);
    }
    IEnumerator waitMove()
    {
        yield return new WaitForSeconds(0.5f); //0.5초 기다리기
        isMove = true; //움직이는 중 체크
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f; //중력 없앰
    }

    private void OnTriggerEnter2D(Collider2D collision) //상단 바와 오브젝트가 닿았을 때
    {
        if (collision.tag == "TopReward") 
        {
            Destroy(gameObject);
            targetAnim.SetBool("isGet",true); //애니메이션 실행
        }
    }
}
