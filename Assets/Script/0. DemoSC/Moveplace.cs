using System.Collections;
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
    public void ShopBtClick() //상점 버튼 클릭
    {
        PlayerMove(1, 0); //화면 이동
    }
    public void InventoryBtClick() //인벤토리 버튼 클릭
    {
        PlayerMove(2,0);
    }
    public void WeaponBtClick() //무기고 버튼 클릭
    {
        PlayerMove(3, 0);
    }
    public void AchievementBtClick() //업적 버튼 클릭
    {
        PlayerMove(4, 0);
    }
    public void CollectionBtClick() //도감 버튼 클릭
    {
        PlayerMove(5, 0);
    }
    public void PlayBtClick() //게임 플레이 버튼 클릭
    {

        PlayerMove(6, 0);
    }
    public void CatBtClick() //고양이 클릭
    {
        StatOpen(); //고양이 스탯 창 활성화
    }
    public void statBackBtClick() //스탯 창 받기 버튼 클릭
    {
        //스탯 창 비활성화
        statBackBt.gameObject.SetActive(false);
        statImg.gameObject.SetActive(false);

        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
    }
    void StatOpen() //유저 고양이 스탯 창
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
    void PlayerMove(int inplace, int outplace) //화면 이동
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        thisCanvas[inplace].gameObject.SetActive(true);
        thisCanvas[outplace].gameObject.SetActive(false);
        DemoTopManager.isLobby = false;
    }
}
