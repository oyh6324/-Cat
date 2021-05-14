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
    private int curHp;
    int PlayerStr;
    public GameObject traceTarget;
    bool isTracing;
    private bool isDied;
    // Start is called before the first frame update
    void Start()
    {
        isDied = false;
        isTracing = false;
        anim = GetComponent<Animator>();
        TotalHp = MonsterStat.MonsterTotalHp[index];
        curHp = TotalHp;
        PlayerStr = 30;
        //PlayerStr=DemoDataManager.characterDatasList[0].allstr;
    }

    // Update is called once per frame
    void Update()
    {
        bar.value = Mathf.Lerp(bar.value, (float)curHp / (float)TotalHp, Time.deltaTime * 10);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            if (curHp < PlayerStr)
            {
                curHp = 0;
                StartCoroutine(Died());
            }
            else
            {
                anim.SetInteger("MonsterIndex", index);
                anim.SetTrigger("MonsterAttacked");
                curHp -= PlayerStr;
            }
        }
        if(collision.gameObject.tag=="Player")
        {
            PlayerMove.MonsterIndex = 2;
        }
    }
    IEnumerator Died()
    {
        isDied = true;
        gameObject.layer = 14;
        anim.SetInteger("MonsterIndex", index);
        anim.SetTrigger("MonsterDie");
        yield return new WaitForSeconds(0.5f);
        bar.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
