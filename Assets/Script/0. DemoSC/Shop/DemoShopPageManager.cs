using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DemoShopPageManager : MonoBehaviour
{
    //0:jewelbox 1:anchovy 2:pearl 3:key
    public Button[] productBt; //상품 카테고리 버튼
    public GameObject[] thisPage; //페이지 이미지

    private bool[] isPage;

    //보석함 종류
    public Button clothesBt; //의상
    public Button weaponBt; //무기

    //보석함 무료이용권 개수
    public Text anchovyboxFreeTx;
    public Text pearlboxFreeTx;
    //보석함 가격표
    public Text anchovyboxFreePriceTx;
    public Text pearlboxFreePriceTx;
    //보석함 구분
    public Text anchovyBoxTx;
    public Text pearlBoxTx;
    //무료이용권 이미지
    public Image anchvoyTicket;
    public Image pearlTicket;
    public Sprite anchovy_clo;
    public Sprite pearl_clo;
    public Sprite anchovy_wea;
    public Sprite pearl_wea;
    //오디오
    public AudioSource soundEffectAS;
    public AudioSource bgmAS;
    public AudioClip buttonClickClip;
    public AudioClip shopBgmClip;
    //무료이용권 개수 관리
    private int anchovyboxFreeCount_C; //멸치 보석함 의상
    private int pearlboxFreeCount_C; //진주 보석함 의상
    private int anchovyboxFreeCount_W; //멸치 보석함 무기
    private int pearlboxFreeCount_W; //진주 보석함 무기

    //보석함 종류 카테고리 위치
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
        if (PlayerPrefs.HasKey("멸치상점")) //멸치 구입 화면
        {
            PlayerPrefs.DeleteKey("멸치상점");
            IsPage(1);
        }
        else if (PlayerPrefs.HasKey("진주상점")) //진주 구입 화면
        {
            PlayerPrefs.DeleteKey("진주상점");
            IsPage(2);
        }
        else if (PlayerPrefs.HasKey("열쇠상점")) //열쇠 구입 화면
        {
            PlayerPrefs.DeleteKey("열쇠상점");
            IsPage(3);
        }
        else //그 외, 보석함 구입 화면
        {
            IsPage(0);
            PlayerPrefs.SetInt("isClothes", 1);
        }
    }
    void Update()
    {
        //상자 이용권 개수 확인
        anchovyboxFreeCount_C = DemoDataManager.Instance.moneyItemList[3].count; //멸치 의상
        pearlboxFreeCount_C = DemoDataManager.Instance.moneyItemList[4].count; //진주 의상
        anchovyboxFreeCount_W = DemoDataManager.Instance.moneyItemList[5].count; //멸치 무기
        pearlboxFreeCount_W = DemoDataManager.Instance.moneyItemList[6].count; //진주 무기

        if (PlayerPrefs.HasKey("isClothes")) //무료이용권세팅
            ClothesBoxFreeSet();
        else
            WeaponBoxFreeSet();

        if (isPage[0]) //페이지 넘기기 //보석함
        {
            OnPage(0);
            if (PlayerPrefs.HasKey("isClothes")) //보석함 의상 화면으로 돌려놓기
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
        else if (isPage[1]) //멸치 
            OnPage(1);
        else if (isPage[2]) //진주
            OnPage(2);
        else if (isPage[3]) //열쇠
            OnPage(3);
    }
    void OnPage(int outproductbt) //원하는 페이지 활성화
    {
        clothesbtPosion = new Vector2(clothesBt.transform.localPosition.x, clothesBt.transform.localPosition.y);
        weaponbtPosion = new Vector2(weaponBt.transform.localPosition.x, weaponBt.transform.localPosition.y);

        if (outproductbt == 0) //보석함 페이지
        {
            //보석함 카테고리 보여주기
            clothesBt.transform.localPosition = Vector2.MoveTowards(clothesbtPosion, new Vector2(-1158, 0), 15f);
            weaponBt.transform.localPosition = Vector2.MoveTowards(weaponbtPosion, new Vector2(-1158, -350), 15f);
        }
        else //보석함 페이지가 아닐 때
        {
            //보석함 카테고리 숨기기
            clothesBt.transform.localPosition = Vector2.MoveTowards(clothesbtPosion, new Vector2(-950, 0), 15f);
            weaponBt.transform.localPosition = Vector2.MoveTowards(weaponbtPosion, new Vector2(-950, -350), 15f);
        }

        for (int i = 0; i < 4; ++i) //상점 카테고리 이동
        {
            Vector2 nowPosition = new Vector2(productBt[i].transform.localPosition.x, productBt[i].transform.localPosition.y); //카테고리의 현재 위치
            Vector2 inPosition = new Vector2(productBt[i].transform.localPosition.x, 250); //카테고리가 들어갈 위치
            Vector2 outPosition = new Vector2(productBt[i].transform.localPosition.x, 320); //카테고리가 나갈 위치

            if (i == outproductbt) //해당 페이지의 카테고리라면
            {
                productBt[i].transform.localPosition = Vector2.MoveTowards(nowPosition, outPosition, 15f); //카테고리는 밖으로 돌출
                thisPage[i].SetActive(true); //원하는 페이지 활성화
            }
            else //보이면 안되는 페이지의 카테고리라면
            {
                productBt[i].transform.localPosition = Vector2.MoveTowards(nowPosition, inPosition, 15f); //카테고리는 안으로 
                thisPage[i].SetActive(false); //페이지 비활성화
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
        anchvoyTicket.sprite = anchovy_clo;
        pearlTicket.sprite = pearl_clo;

        if (anchovyboxFreeCount_C > 0) //멸치 의상 보석함 무료이용권이 있다면
            anchovyboxFreePriceTx.text = "무료이용권 사용";
        else 
            anchovyboxFreePriceTx.text = "멸치 10000마리";
        if (pearlboxFreeCount_C > 0) //진주 의상 보석함 무료이용권이 있다면
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
        anchvoyTicket.sprite = anchovy_wea;
        pearlTicket.sprite = pearl_wea;

        if (anchovyboxFreeCount_W > 0) //멸치 무기 보석함 무료이용권이 있다면
            anchovyboxFreePriceTx.text = "무료이용권 사용";
        else
            anchovyboxFreePriceTx.text = "멸치 10000마리";
        if (pearlboxFreeCount_W > 0) //진주 무기 보석함 무료이용권이 있다면
            pearlboxFreePriceTx.text = "무료이용권 사용";
        else
            pearlboxFreePriceTx.text = "진주 100개";
    }
    //상점 카테고리 버튼
    public void JewelboxBtClick() //보석함
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        IsPage(0);
        PlayerPrefs.SetInt("isClothes", 1);
    }
    public void AnchovyBtClick() //멸치
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        IsPage(1);
    }
    public void PearlBtClick() //진주
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        IsPage(2);
    }
    public void KeyBtClick() //열쇠
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        IsPage(3);
    }
    public void ClothesBtClick() //보석함 의상
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        PlayerPrefs.SetInt("isClothes", 1);
    }
    public void WeaponBtClick() //보석함 무기
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        PlayerPrefs.DeleteKey("isClothes");
    }
}
