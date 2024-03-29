﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform tr;
    public LayerMask isLayer;


    public decimal coolTime;
    private int agi;
    private decimal crip;
    private bool isShooting;
    Animator anim;

    //의상
    public Animator[] dressAnim;
    public Sprite[] bulletSprites;

    //오디오
    private AudioSource audioSource;
    public AudioClip[] weaponClip;
    private int bulletIndex;
    private bool shootbt_click;
    private void Start()
    {
        //coolTime=DemoDataManager.characterDatasList[0].itemspeed-DemoDataManager.characterDatasList[0].allagi*0.005m;
        anim = GetComponent<Animator>();
        coolTime = coolTime = DemoDataManager.Instance.characterDatasList[0].itemspeed - DemoDataManager.Instance.characterDatasList[0].allagi * 0.005m; //탄속
        agi = 15;
        isShooting = false;
        shootbt_click = false;
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable() 
    {
        string weaponName = DemoDataManager.Instance.characterDatasList[0].weapon;

        for(int i=0; i<DemoDataManager.Instance.allWeaponItemList.Count; i++)
        {
            if (weaponName == DemoDataManager.Instance.allWeaponItemList[i].name)
            {
                bullet.GetComponent<SpriteRenderer>().sprite = bulletSprites[i];
                bulletIndex = i;
            }
        }
    }
    private void Update()
    {
        if (shootbt_click == true && isShooting == false&&PlayerMove.isLiving==true)
        {
            StartCoroutine(shoot());
            anim.SetTrigger("PlayerShoot");
            audioSource.PlayOneShot(weaponClip[bulletIndex]);

            for (int i = 0; i < 3; i++)
                dressAnim[i].SetTrigger("PlayerShoot");
            shootbt_click = false;
        }
    }
    IEnumerator shoot() 
    {
        //총알 생성
        isShooting = true;
        GameObject bulletclone = Instantiate(bullet, tr.position, tr.rotation);
        Rigidbody2D rigid = bulletclone.GetComponent<Rigidbody2D>();

        //총알 방향
        if (this.gameObject.transform.localScale.x==-0.5f|| Input.GetAxisRaw("Horizontal") == 1)
        {
            bullet.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            rigid.AddForce(Vector2.right * 6, ForceMode2D.Impulse);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1|| this.gameObject.transform.localScale.x == 0.5f)
        {
            bullet.transform.localScale = new Vector3(-0.7f, 0.7f, 1);
            rigid.AddForce(Vector2.left * 6, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(decimal.ToSingle(coolTime));
        isShooting = false;

        //삭제
        Destroy(bulletclone, 0.8f);    
    }
    public void Shootbt()
    {
        shootbt_click = true;
        Debug.Log("shoot");
    }

}
