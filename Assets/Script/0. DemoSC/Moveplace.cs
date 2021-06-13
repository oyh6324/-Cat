﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Moveplace : MonoBehaviour
{   
    //0:lobby 1:shop 2:inventory 3:weapon 4:achievement 5: collection 6: stageSelect
    public Canvas[] thisCanvas;
    public Animator catAnim;
    public Image statImg;
    public Text statExTx;
    public Button statBackBt;

    //오디오
    public AudioSource soundEffectAS;
    public AudioSource bgmAS;
    public AudioClip buttonClickClip;
    public AudioClip beachClip;
    void OnEnable()
    {
        bgmAS.clip = beachClip;
        bgmAS.Play();
    }
    void OnDisable()
    {
        statBackBt.gameObject.SetActive(false);
        statImg.gameObject.SetActive(false);
    }
    public void ShopBtClick()
    {
        PlayerMove(1, 0);
    }
    public void InventoryBtClick()
    {
        PlayerMove(2,0);
    }
    public void WeaponBtClick()
    {
        PlayerMove(3, 0);
    }
    public void AchievementBtClick()
    {
        PlayerMove(4, 0);
    }
    public void CollectionBtClick()
    {
        PlayerMove(5, 0);
    }
    public void PlayBtClick()
    {

        PlayerMove(6, 0);
    }
    public void CatBtClick()
    {
        StatOpen();
    }
    public void statBackBtClick()
    {
        statBackBt.gameObject.SetActive(false);
        statImg.gameObject.SetActive(false);

        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
    }
    void StatOpen() //데모 버전
    {
        statImg.gameObject.SetActive(true);
        statBackBt.gameObject.SetActive(true);
        statExTx.text = "레벨: " + DemoDataManager.Instance.characterDatasList[0].level + "\n이름: " + DemoDataManager.Instance.characterDatasList[0].name + "\nHP: " + DemoDataManager.Instance.characterDatasList[0].hp +
    "\nSTR: " + DemoDataManager.Instance.characterDatasList[0].str;
        if (DemoDataManager.Instance.characterDatasList[0].itemstr != 0)
            statExTx.text += " (+" + (DemoDataManager.Instance.characterDatasList[0].itemstr + DemoDataManager.Instance.characterDatasList[0].setstr) + ")";
        statExTx.text += "\nDEF: " + DemoDataManager.Instance.characterDatasList[0].def;
        if (DemoDataManager.Instance.characterDatasList[0].itemdef != 0)
            statExTx.text += " (+" + (DemoDataManager.Instance.characterDatasList[0].itemdef + DemoDataManager.Instance.characterDatasList[0].setdef) + ")";
        statExTx.text += "\nAGI: " + DemoDataManager.Instance.characterDatasList[0].agi;
        if (DemoDataManager.Instance.characterDatasList[0].itemagi != 0)
            statExTx.text += " (+" + (DemoDataManager.Instance.characterDatasList[0].itemagi + DemoDataManager.Instance.characterDatasList[0].setagi) + ")";
        statExTx.text += "\n공격 속도: " + DemoDataManager.Instance.characterDatasList[0].itemspeed + "\n크리티컬 확률: " + DemoDataManager.Instance.characterDatasList[0].crip;
        if (DemoDataManager.Instance.characterDatasList[0].itemcrip != 0)
            statExTx.text += " (+" + DemoDataManager.Instance.characterDatasList[0].itemcrip + "%)";
        if (DemoDataManager.Instance.characterDatasList[0].setname != "")
            statExTx.text += "\n" + DemoDataManager.Instance.characterDatasList[0].setname + " 세트 적용!" + "\nSTR +" + DemoDataManager.Instance.characterDatasList[0].setstr +
                        "\nDEF +" + DemoDataManager.Instance.characterDatasList[0].setdef + "\nAGI +" + DemoDataManager.Instance.characterDatasList[0].setagi + "\n크리티컬 확률 +" +
                         DemoDataManager.Instance.characterDatasList[0].setcrip;
    }

    /*void StatOpen() //정식 버전
    {
        statImg.gameObject.SetActive(true);
        statBackBt.gameObject.SetActive(true);
        statExTx.text = "레벨: " + DataManager.characterDatasList[0].level + "\n이름: " + DataManager.characterDatasList[0].name + "\nHP: " + DataManager.characterDatasList[0].hp +
    "\nSTR: " + DataManager.characterDatasList[0].str;
        if (DataManager.characterDatasList[0].itemstr != 0)
            statExTx.text += " (+" + DataManager.characterDatasList[0].itemstr + ")";
        statExTx.text += "\nDEF: " + DataManager.characterDatasList[0].def;
        if (DataManager.characterDatasList[0].itemdef != 0)
            statExTx.text += " (+" + DataManager.characterDatasList[0].itemdef + ")";
        statExTx.text += "\nAGI: " + DataManager.characterDatasList[0].agi;
        if (DataManager.characterDatasList[0].itemagi != 0)
            statExTx.text += " (+" + DataManager.characterDatasList[0].itemagi + ")";
        statExTx.text += "\n공격 속도: " + DataManager.characterDatasList[0].itemspeed + "\n크리티컬 확률: " + DataManager.characterDatasList[0].crip;
        if (DataManager.characterDatasList[0].itemcrip != 0)
            statExTx.text += " (+" + DataManager.characterDatasList[0].itemcrip + "%)";
        if (DataManager.characterDatasList[0].setname != "")
            statExTx.text += "\n" + DataManager.characterDatasList[0].setname + " 세트 적용!" + "\nSTR +" + DataManager.characterDatasList[0].setstr +
                        "\nDEF +" + DataManager.characterDatasList[0].setdef + "\nAGI +" + DataManager.characterDatasList[0].setagi + "\n크리티컬 확률 +" +
                         DataManager.characterDatasList[0].setcrip;
    }*/
    void PlayerMove(int inplace, int outplace)
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        thisCanvas[inplace].gameObject.SetActive(true);
        thisCanvas[outplace].gameObject.SetActive(false);
        DemoTopManager.isLobby = false;
    }
}
