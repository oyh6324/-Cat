using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDress : MonoBehaviour
{
    private enum Dress
    {
        Top, Helmet, Bottom
    }
    [SerializeField] private Dress dress;

    private Animator anim;
    private int index;

    private void Awake()
    {

        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        WearingCheck();
    }
    public void Walking(bool onOff) { anim.SetBool("isWalking", onOff); }
    public void Jumping(bool onOff) { anim.SetBool("isJumping", onOff); }
    public void Dying() { anim.SetTrigger("isDying"); }
    public void Attackted() { anim.SetTrigger("isAttackted"); }
    public void Shoot() { anim.SetTrigger("PlayerShoot"); }
    private void WearingCheck()
    {
        string dressName = "";
        switch (dress)
        {
            case Dress.Top:
                dressName = DemoDataManager.Instance.characterDatasList[0].top;
                break;
            case Dress.Bottom:
                dressName = DemoDataManager.Instance.characterDatasList[0].bottoms;
                break;
            case Dress.Helmet:
                dressName = DemoDataManager.Instance.characterDatasList[0].helmet;
                break;
        }

        for(int i=0; i<DemoDataManager.Instance.allClothesItemList.Count; i++)
        {
            if (dressName == DemoDataManager.Instance.allClothesItemList[i].name)
                index = i;
        }

        if (dressName == "")
            index = -1;

        anim.SetInteger("index", index);
    }
}