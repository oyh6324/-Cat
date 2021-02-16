using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DemoShopPageManager : MonoBehaviour
{
    //0:jewelbox 1:anchovy 2:pearl 3:key
    public Button[] productBt;
    public GameObject[] thisPage;

    private bool[] isPage;

    public Button clothesBt;
    public Button weaponBt;

    //무료이용권 개수 표시
    public Text anchovyboxFreeTx;
    public Text pearlboxFreeTx;
    //무료이용권 사용 여부
    public Text anchovyboxFreePriceTx;
    public Text pearlboxFreePriceTx;
    //보석함 구분
    public Text anchovyBoxTx;
    public Text pearlBoxTx;
    //오디오
    public AudioSource soundEffectAS;
    public AudioSource bgmAS;
    public AudioClip buttonClickClip;
    public AudioClip shopBgmClip;
    //무료이용권 개수 관리
    private int anchovyboxFreeCount_C;
    private int pearlboxFreeCount_C;
    private int anchovyboxFreeCount_W;
    private int pearlboxFreeCount_W;

    private Vector2 clothesbtPosion;
    private Vector2 weaponbtPosion;

    void Awake()
    {
        isPage = new bool[4];
    }
    void OnEnable() //첫 번째 화면 초기화, 상단바에서 바로 들어왔을 때 화면 초기화
    {
        bgmAS.clip = shopBgmClip;
        bgmAS.Play();
        if (PlayerPrefs.HasKey("멸치상점"))
        {
            PlayerPrefs.DeleteKey("멸치상점");
            IsPage(1);
        }
        else if (PlayerPrefs.HasKey("진주상점"))
        {
            PlayerPrefs.DeleteKey("진주상점");
            IsPage(2);
        }
        else if (PlayerPrefs.HasKey("열쇠상점"))
        {
            PlayerPrefs.DeleteKey("열쇠상점");
            IsPage(3);
        }
        else
        {
            IsPage(0);
            PlayerPrefs.SetInt("isClothes", 1);
        }
    }
    void Update()
    {
        //상자 이용권 개수 확인
        anchovyboxFreeCount_C = DemoDataManager.moneyItemList[3].count; //멸치 의상
        pearlboxFreeCount_C = DemoDataManager.moneyItemList[4].count; //진주 의상
        anchovyboxFreeCount_W = DemoDataManager.moneyItemList[5].count; //멸치 무기
        pearlboxFreeCount_W = DemoDataManager.moneyItemList[6].count; //진주 무기
        if (PlayerPrefs.HasKey("isClothes")) //무료이용권세팅
            ClothesBoxFreeSet();
        else
            WeaponBoxFreeSet();

        if (isPage[0]) //페이지 넘기기
        {
            OnPage(0);
            if (PlayerPrefs.HasKey("isClothes")) //의상 보석함 화면으로 돌려놓기
            {
                clothesBt.transform.localPosition = Vector2.MoveTowards(clothesbtPosion, new Vector2(-1235, 0), 15f);
                weaponBt.transform.localPosition = Vector2.MoveTowards(weaponbtPosion, new Vector2(-1158, -350), 15f);
            }
            else
            {
                clothesBt.transform.localPosition = Vector2.MoveTowards(clothesbtPosion, new Vector2(-1158, 0), 15f);
                weaponBt.transform.localPosition = Vector2.MoveTowards(weaponbtPosion, new Vector2(-1235, -350), 15f);
            }
        }
        else if (isPage[1])
            OnPage(1);
        else if (isPage[2])
            OnPage(2);
        else if (isPage[3])
            OnPage(3);
    }
    void OnPage(int outproductbt) //페이지 넘김
    {
        clothesbtPosion = new Vector2(clothesBt.transform.localPosition.x, clothesBt.transform.localPosition.y);
        weaponbtPosion = new Vector2(weaponBt.transform.localPosition.x, weaponBt.transform.localPosition.y);

        if (outproductbt == 0)
        {
            clothesBt.transform.localPosition = Vector2.MoveTowards(clothesbtPosion, new Vector2(-1158, 0), 15f);
            weaponBt.transform.localPosition = Vector2.MoveTowards(weaponbtPosion, new Vector2(-1158, -350), 15f);
        }
        else
        {
            clothesBt.transform.localPosition = Vector2.MoveTowards(clothesbtPosion, new Vector2(-950, 0), 15f);
            weaponBt.transform.localPosition = Vector2.MoveTowards(weaponbtPosion, new Vector2(-950, -350), 15f);
        }

        for (int i = 0; i < 4; ++i)
        {
            Vector2 nowPosition = new Vector2(productBt[i].transform.localPosition.x, productBt[i].transform.localPosition.y);
            Vector2 inPosition = new Vector2(productBt[i].transform.localPosition.x, 250);
            Vector2 outPosition = new Vector2(productBt[i].transform.localPosition.x, 320);

            if (i == outproductbt)
            {
                productBt[i].transform.localPosition = Vector2.MoveTowards(nowPosition, outPosition, 15f);
                thisPage[i].SetActive(true);
            }
            else
            {
                productBt[i].transform.localPosition = Vector2.MoveTowards(nowPosition, inPosition, 15f);
                thisPage[i].SetActive(false);
            }
        }
    }
    void IsPage(int pagenumber) //현재 페이지를 제외한 나머지 페이지 false
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == pagenumber)
                isPage[i] = true;
            else
                isPage[i] = false;
        }
    }
    void ClothesBoxFreeSet() //의상 이용권 세팅
    {
        anchovyBoxTx.text = "1성~3성 확정 의상 보석함";
        pearlBoxTx.text = "3성~5성 확정 의상 보석함";
        anchovyboxFreeTx.text = "무료이용권: " + anchovyboxFreeCount_C;
        pearlboxFreeTx.text = "무료이용권: " + pearlboxFreeCount_C;

        if (anchovyboxFreeCount_C > 0)
            anchovyboxFreePriceTx.text = "무료이용권 사용";
        else
            anchovyboxFreePriceTx.text = "멸치 10000마리";
        if (pearlboxFreeCount_C > 0)
            pearlboxFreePriceTx.text = "무료이용권 사용";
        else
            pearlboxFreePriceTx.text = "진주 100개";
    }
    void WeaponBoxFreeSet() //무기 이용권 세팅
    {
        anchovyBoxTx.text = "1성~3성 확정 무기 보석함";
        pearlBoxTx.text = "3성~5성 확정 무기 보석함";
        anchovyboxFreeTx.text = "무료이용권: " + anchovyboxFreeCount_W;
        pearlboxFreeTx.text = "무료이용권: " + pearlboxFreeCount_W;

        if (anchovyboxFreeCount_W > 0)
            anchovyboxFreePriceTx.text = "무료이용권 사용";
        else
            anchovyboxFreePriceTx.text = "멸치 10000마리";
        if (pearlboxFreeCount_W > 0)
            pearlboxFreePriceTx.text = "무료이용권 사용";
        else
            pearlboxFreePriceTx.text = "진주 100개";
    }
    //버튼
    public void JewelboxBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        IsPage(0);
        PlayerPrefs.SetInt("isClothes", 1);
    }
    public void AnchovyBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        IsPage(1);
    }
    public void PearlBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        IsPage(2);
    }
    public void KeyBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        IsPage(3);
    }
    public void ClothesBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        PlayerPrefs.SetInt("isClothes", 1);
    }
    public void WeaponBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        PlayerPrefs.DeleteKey("isClothes");
    }
}
