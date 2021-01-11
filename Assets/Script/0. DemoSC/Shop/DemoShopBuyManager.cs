using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoShopBuyManager : MonoBehaviour
{
    //메시지 창
    public Canvas messageCanvas;
    public Text messageTx;
    public Image messageImg;
    public Image boxWatingImg;
    public Image boxonImg;
    public GameObject yesnoBts;
    public Button okBt;
    //오디오
    public AudioSource soundEffectAS;
    public AudioClip buttonClickClip;
    public AudioClip paperClickClip;

    //무료권 사용여부
    private bool isFreeUse;
    private bool[] isFreeBox; //0:멸치의상 1:진주의상 2:멸치무기 3:진주무기

    //구매 상품 목록
    //0:멸치 보석함 1:진주 보석함 2:멸치1000 3:멸치2000 4:멸치3000 5:진주50 6:진주100 7:진주200 8:열쇠1 9:열쇠3 10:열쇠
    List<BuyItemdData> buyitemdataList;
    private bool[] productNumber;

    void Start()
    {
        productNumber = new bool[11];
        isFreeBox = new bool[4];

        //구매 상품 목록 초기화
        buyitemdataList = new List<BuyItemdData>();
        buyitemdataList.Add(new BuyItemdData("멸치 보석함을 ", "멸치", 10000, "마리", "", 1));
        buyitemdataList.Add(new BuyItemdData("진주 보석함을 ", "진주", 100, "개", "", 1));
        buyitemdataList.Add(new BuyItemdData("멸치 1000마리를 ", "진주", 30, "개", "멸치", 1000));
        buyitemdataList.Add(new BuyItemdData("멸치 2000마리를  ", "진주", 50, "개", "멸치", 2000));
        buyitemdataList.Add(new BuyItemdData("멸치 3000마리를", "진주", 80, "개", "멸치", 3000));
        buyitemdataList.Add(new BuyItemdData("진주 50개를 ", "", 1000, "원으", "진주", 50));
        buyitemdataList.Add(new BuyItemdData("진주 100개를 ", "", 1500, "원으", "진주", 1000));
        buyitemdataList.Add(new BuyItemdData("진주 200개를 ", "", 2500, "원으", "진주", 200));
        buyitemdataList.Add(new BuyItemdData("열쇠 1개를 ", "진주", 10, "개", "열쇠", 1));
        buyitemdataList.Add(new BuyItemdData("열쇠 3개를 ", "진주", 25, "개", "열쇠", 3));
        buyitemdataList.Add(new BuyItemdData("열쇠 5개를 ", "진주", 40, "개", "열쇠", 5));
    }
    //버튼 연결
    public void AnchovyboxBtClick()
    {
        if (PlayerPrefs.HasKey("isClothes") && DemoDataManager.moneyItemList[3].count > 0) //무료사용권 있을 때
        {
            BuyBoxFree(0);
        }
        else if (PlayerPrefs.HasKey("isClothes") == false && DemoDataManager.moneyItemList[5].count > 0)
        {
            BuyBoxFree(1);
        }
        else //없을 때
            BuyItem(0);
    }
    public void PearlboxBtClick()
    {
        if (PlayerPrefs.HasKey("isClothes") && DemoDataManager.moneyItemList[4].count > 0) //무료사용권 있을 때
        {
            BuyBoxFree(2);
        }
        else if (PlayerPrefs.HasKey("isClothes") == false && DemoDataManager.moneyItemList[6].count > 0)
        {
            BuyBoxFree(3);
        }
        else //없을 때
            BuyItem(1);
    }
    public void Anchovy1000BtClick()
    {
        BuyItem(2);
    }
    public void Anchovy2000BtClick()
    {
        BuyItem(3);
    }
    public void Anchovy3000BtClick()
    {
        BuyItem(4);
    }
    public void Pearl50BtClick()
    {
        BuyItem(5);
    }
    public void Pearl100BtClick()
    {
        BuyItem(6);
    }
    public void Pearl200BtClick()
    {
        BuyItem(7);
    }
    public void Key1BtClick()
    {
        BuyItem(8);
    }
    public void Key3BtClick()
    {
        BuyItem(9);
    }
    public void Key5BtClick()
    {
        BuyItem(10);
    }
    //message yes or no
    public void NoBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        isFreeUse = false;
        messageCanvas.gameObject.SetActive(false);
    }
    public void YesBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (DemoTopManager.isLobby) //로비에서 뒤로가기 눌렀을때 메시지 창
            Application.Quit();
        else
        {
            if (isFreeUse) //무료뽑기 진행 시
            {
                if (isFreeBox[0])
                {
                    PlayerPrefs.SetInt("멸치의상뽑기", 1);
                    DemoDataManager.moneyItemList[3].count -= 1;
                }
                else if (isFreeBox[1])
                {
                    PlayerPrefs.SetInt("멸치무기뽑기", 1);
                    DemoDataManager.moneyItemList[5].count -= 1;
                }
                else if (isFreeBox[2])
                {
                    PlayerPrefs.SetInt("진주의상뽑기", 1);
                    DemoDataManager.moneyItemList[4].count -= 1;
                }
                else if (isFreeBox[3])
                {
                    PlayerPrefs.SetInt("진주무기뽑기", 1);
                    DemoDataManager.moneyItemList[6].count -= 1;
                }
                isFreeUse = false;
                StartCoroutine(WatingForBoxOpen());
            }
            else //돈 계산 필요 시
                PayMoney();
        }
    }
    public void OkBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        yesnoBts.SetActive(true);
        okBt.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }
    void BuyItem(int itemnumber)
    {
        soundEffectAS.clip = paperClickClip;
        soundEffectAS.Play();
        messageCanvas.gameObject.SetActive(true);
        messageTx.text = buyitemdataList[itemnumber].productName + buyitemdataList[itemnumber].priceName + buyitemdataList[itemnumber].price +
            buyitemdataList[itemnumber].priceUnit + "로 구매하시겠습니까?";

        //무엇을 사는지
        for (int i = 0; i < 11; i++)
        {
            if (i == itemnumber)
                productNumber[i] = true;
            else
                productNumber[i] = false;
        }
    }
    void PayMoney()
    {
        int tempNumber = 0;
        bool isBuy = false;

        //어떤 물건을 살 건지
        for (int i = 0; i < 11; i++)
        {
            if (productNumber[i])
                tempNumber = i;
        }
        //구매 가능 여부
        for (int i = 0; i < 7; i++)
        {
            if (DemoDataManager.moneyItemList[i].name.Equals(buyitemdataList[tempNumber].priceName))
            {
                if (DemoDataManager.moneyItemList[i].count < buyitemdataList[tempNumber].price)
                {
                    messageTx.text = buyitemdataList[tempNumber].priceName + "가 부족해요!";
                    yesnoBts.SetActive(false);
                    okBt.gameObject.SetActive(true);
                }
                else
                    isBuy = true;
            }
        }
        //구매 가능하다면
        if (isBuy)
        {
            //상자일때
            if (productNumber[0])
            {
                if (PlayerPrefs.HasKey("isClothes"))
                    PlayerPrefs.SetInt("멸치의상뽑기", 1);
                else
                    PlayerPrefs.SetInt("멸치무기뽑기", 1);
            }
            else if (productNumber[1])
            {
                if (PlayerPrefs.HasKey("isClothes"))
                    PlayerPrefs.SetInt("진주의상뽑기", 1);
                else
                    PlayerPrefs.SetInt("진주무기뽑기", 1);
            }
            //계산
            for (int i = 0; i < 7; i++)
            {
                if (DemoDataManager.moneyItemList[i].name.Equals(buyitemdataList[tempNumber].getProductName))
                    DemoDataManager.moneyItemList[i].count += buyitemdataList[tempNumber].getProduct;
                if (DemoDataManager.moneyItemList[i].name.Equals(buyitemdataList[tempNumber].priceName))
                    DemoDataManager.moneyItemList[i].count -= buyitemdataList[tempNumber].price;
            }
            if (buyitemdataList[tempNumber].getProductName == "멸치") //업적 연동
                DemoDataManager.achievementDataList[0].progressvalue += buyitemdataList[tempNumber].getProduct;
            if (productNumber[0] || productNumber[1]) //상자구매시 구매화면
            {
                StartCoroutine(WatingForBoxOpen());
            }
            else //나머지 구매화면
            {
                messageTx.text = "구매되었습니다!";
                yesnoBts.SetActive(false);
                okBt.gameObject.SetActive(true);
            }
        }
    }
    void BuyBoxFree(int boxnumber)
    {
        soundEffectAS.clip = paperClickClip;
        soundEffectAS.Play();
        messageCanvas.gameObject.SetActive(true);
        messageTx.text = "무료이용권을 사용하시겠습니까?";
        //어떤 상자를 구매하는지
        for (int i = 0; i < 4; i++)
        {
            if (i == boxnumber)
                isFreeBox[i] = true;
            else
                isFreeBox[i] = false;
        }
        isFreeUse = true;
    }
    IEnumerator WatingForBoxOpen() //뽑기 화면 나타내기
    {
        messageImg.gameObject.SetActive(false);
        boxWatingImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        boxWatingImg.gameObject.SetActive(false);
        boxonImg.gameObject.SetActive(true);
    }
    //구매 상품 목록 클래스
    public class BuyItemdData
    {
        public string productName, priceName, priceUnit, getProductName;
        public int price, getProduct;
        public BuyItemdData(string _producName, string _priceName, int _price, string _priceUnit, string _getProductName, int _getProduct)
        {
            productName = _producName; priceName = _priceName; price = _price;
            priceUnit = _priceUnit; getProductName = _getProductName; getProduct = _getProduct;
        }
    }
}
