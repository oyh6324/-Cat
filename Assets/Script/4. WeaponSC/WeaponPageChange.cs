using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Cache;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPageChange : MonoBehaviour
{
    //화면 세팅
    public Image rightBlindImg1;
    public Image rightBlindImg2;
    public Image leftBlindImg1;
    public Image leftBlindImg2;
    public GameObject gunImgs;
    public Image[] gunImg;
    public Image[] gunLockImg;
    public Image[] newAlarmImg;
    public Text[] gunNameTx;
    public Button[] gunBt;
    public Sprite[] gunSprite;
    public Sprite gunInstallSprite;
    public Sprite gunNonInstallSprite;

    //스탯 창 세팅
    public Image statImg;
    public Text classTx;
    public Text nameTx;
    public Text setNameCountTx;
    public Text exTx;
    public Text statTx;
    public Text setStatTx;
    public Image upgradeBlind;
    public Text upgradeTx;

    //장착 버튼
    public Text installBtTx;
    public Image installBlindImg;

    //페이지 안내
    public Text pageTx;

    private int thisPage;
    private bool isRight;
    private bool isLeft;
    private int nextPlaceX;
    private bool isMove;
    private bool isGunClick;
    private int sumCount;

    List<WeaponItemData> weaponItemDataList;
    void Awake()
    {
        nextPlaceX = -1820;
        weaponItemDataList = new List<WeaponItemData>();
    }
    void OnEnable()
    {
        sumCount = 0; //업그레이드 부분
        gunImgs.transform.localPosition = new Vector2(nextPlaceX, gunImgs.transform.localPosition.y);
        isRight = false;
        isLeft = false;
        weaponItemDataList.Clear();
        InputWeaponData();
        if (DataManager.characterDatasList[0].weapon != "")
        {
            for (int i = 0; i < DataManager.allWeaponItemList.Count; i++)
            {
                if (weaponItemDataList[i].name.Equals(DataManager.characterDatasList[0].weapon))
                    thisPage = i;
            }
        }
        else
            thisPage = 0;
        PageSetting();
    }
    void Update()
    {
        //블라인드 옮기기
        rightBlindImg1.transform.localPosition = Vector2.MoveTowards(new Vector2(rightBlindImg1.transform.localPosition.x, rightBlindImg1.transform.localPosition.y),
            new Vector2(880, 0), 50f);
        rightBlindImg2.transform.localPosition = Vector2.MoveTowards(new Vector2(rightBlindImg2.transform.localPosition.x, rightBlindImg2.transform.localPosition.y),
            new Vector2(880, 0), 50f);
        leftBlindImg1.transform.localPosition = Vector2.MoveTowards(new Vector2(leftBlindImg1.transform.localPosition.x, leftBlindImg1.transform.localPosition.y),
            new Vector2(-880, 0), 50f);
        leftBlindImg2.transform.localPosition = Vector2.MoveTowards(new Vector2(leftBlindImg2.transform.localPosition.x, leftBlindImg2.transform.localPosition.y),
            new Vector2(-880, 0), 50f);
        if (isRight)
        {
            gunImgs.transform.localPosition = Vector2.MoveTowards(new Vector2(gunImgs.transform.localPosition.x, gunImgs.transform.localPosition.y),
                 new Vector2(nextPlaceX, gunImgs.transform.localPosition.y), 30f);
        }
        if(isLeft)
        {
            gunImgs.transform.localPosition = Vector2.MoveTowards(new Vector2(gunImgs.transform.localPosition.x, gunImgs.transform.localPosition.y),
                new Vector2(nextPlaceX, gunImgs.transform.localPosition.y), 30f);
        }
        if (gunImgs.transform.localPosition.x== nextPlaceX)
            isMove = true;
        else
            isMove = false;

        //장착 관리
        if (DataManager.characterDatasList[0].weapon == weaponItemDataList[thisPage].name)
            installBtTx.text = "장착 해제";
        else
            installBtTx.text = "장착";

        pageTx.text = thisPage+1 + "/17";
    }
    void OnDisable()
    {
        StatSetDestroy();
        DataManager.characterDatasList[0].allstr = DataManager.characterDatasList[0].str + DataManager.characterDatasList[0].itemstr+DataManager.characterDatasList[0].setstr;
        DataManager.characterDatasList[0].alldef = DataManager.characterDatasList[0].def + DataManager.characterDatasList[0].itemdef + DataManager.characterDatasList[0].setdef;
        DataManager.characterDatasList[0].allagi = DataManager.characterDatasList[0].agi + DataManager.characterDatasList[0].itemagi + DataManager.characterDatasList[0].setagi;
        DataManager.characterDatasList[0].allcrip = DataManager.characterDatasList[0].crip + DataManager.characterDatasList[0].itemcrip + DataManager.characterDatasList[0].setcrip;

        //블라인드 옮기기
        rightBlindImg1.transform.localPosition = new Vector2(2000, -1000);
        rightBlindImg2.transform.localPosition = new Vector2(2000, 1000);
        leftBlindImg1.transform.localPosition = new Vector2(-2000, -1000);
        leftBlindImg2.transform.localPosition = new Vector2(-2000, 1000);
    }
    public void RightBtClick()
    {
        if (isMove)
        {
            if (thisPage == 16)
                thisPage = 0;
            else
                thisPage++;
            nextPlaceX = (int)gunImgs.transform.localPosition.x - 910;
            isRight = true;
            isLeft = false;
            PageSetting();
        }
    }
    public void LeftBtClick()
    {
        if (isMove)
        {
            if (thisPage == 0)
                thisPage = 16;
            else
                thisPage--;
            nextPlaceX = (int)gunImgs.transform.localPosition.x + 910;
            isRight = false;
            isLeft = true;
            PageSetting();
        }
    }
    public void GunClick()
    {
        if (isGunClick)
        {
            StatSetDestroy();
        }
        else
        {
            isGunClick = true;
            StatSetting();
        }
    }
    public void UpgradeBtClick()
    {
        DataManager.allWeaponItemList[thisPage].strspeed -= (decimal)0.02;
        DataManager.allWeaponItemList[thisPage].level += 1;
        if (weaponItemDataList[thisPage].name == "에어건")
        {
            DataManager.allWeaponItemList[thisPage].str += 2;
            DataManager.allWeaponItemList[thisPage].crip += (decimal)0.1;
        }
        else if (weaponItemDataList[thisPage].name == "조개총")
        {
            DataManager.allWeaponItemList[thisPage].str += 3;
            DataManager.allWeaponItemList[thisPage].crip += (decimal)0.1;
        }
        else
        {
            DataManager.allWeaponItemList[thisPage].crip += (decimal)0.2;
            if (weaponItemDataList[thisPage].name == "수리검" || weaponItemDataList[thisPage].name == "아이스건" || weaponItemDataList[thisPage].name == "스톤건")
                DataManager.allWeaponItemList[thisPage].str += 3;
            else if (weaponItemDataList[thisPage].name == "버블건" || weaponItemDataList[thisPage].name == "비비탄총" || weaponItemDataList[thisPage].name == "장미건" ||
                weaponItemDataList[thisPage].name == "음파건" || weaponItemDataList[thisPage].name == "슬라임건")
                DataManager.allWeaponItemList[thisPage].str += 4;
            else if (weaponItemDataList[thisPage].name == "쥬얼건" || weaponItemDataList[thisPage].name == "하트건" || weaponItemDataList[thisPage].name == "레이저건" ||
                weaponItemDataList[thisPage].name == "스타건" || weaponItemDataList[thisPage].name == "썬더건")
                DataManager.allWeaponItemList[thisPage].str += 5;
            else if (weaponItemDataList[thisPage].name == "악마삼지창" || weaponItemDataList[thisPage].name == "천사링")
                DataManager.allWeaponItemList[thisPage].str += 7;
        }
            DataManager.allWeaponItemList[thisPage].count = weaponItemDataList[thisPage].count - sumCount+1;
        InputWeaponData();
        StatSetting();
        UpgradeSetting();
        AchievementCheck();
    }
    void AchievementCheck() //업적 연동
    {
        int count3 = 0, count4 = 0, count5 = 0;
        for (int i=0; i<DataManager.allWeaponItemList.Count; i++)
        {
            if (DataManager.allWeaponItemList[i].level >= 3)
                count3++;
            if (DataManager.allWeaponItemList[i].level >= 4)
                count4++;
            if (DataManager.allWeaponItemList[i].level == 5)
                count5++;
        }
        DataManager.achievementDataList[10].progressvalue = count3;
        DataManager.achievementDataList[11].progressvalue = count4;
        DataManager.achievementDataList[12].progressvalue = count5;
    }
    public void InstallitemBtClick()
    {
        if (DataManager.characterDatasList[0].weapon == "") //아무것도 착용 하지 않았을 시
        {
            DataManager.characterDatasList[0].weapon = weaponItemDataList[thisPage].name;
            DataManager.characterDatasList[0].itemstr += weaponItemDataList[thisPage].str;
            DataManager.characterDatasList[0].itemspeed += weaponItemDataList[thisPage].strspeed;
            DataManager.characterDatasList[0].itemcrip += weaponItemDataList[thisPage].crip;

            gunImg[2].sprite = gunInstallSprite; //배경 이미지 변경
        }
        else if(DataManager.characterDatasList[0].weapon != "") //무언가를 착용한 상태
        {
            if (DataManager.characterDatasList[0].weapon == weaponItemDataList[thisPage].name) //지금 든 무기 장착 해제
            {
                DataManager.characterDatasList[0].weapon = "";
                DataManager.characterDatasList[0].itemstr -= weaponItemDataList[thisPage].str;
                DataManager.characterDatasList[0].itemspeed -= weaponItemDataList[thisPage].strspeed;
                DataManager.characterDatasList[0].itemcrip -= weaponItemDataList[thisPage].crip;

                gunImg[2].sprite = gunNonInstallSprite; //배경 이미지 변경
            }
            else //앞에 든 무기 장착 해제 후 지금 무기 장착
            {
                int preItem = 0;
                for(int i=0; i<DataManager.allWeaponItemList.Count; i++)
                {
                    if (DataManager.characterDatasList[0].weapon == weaponItemDataList[i].name)
                        preItem = i;
                }
                DataManager.characterDatasList[0].itemstr -= weaponItemDataList[preItem].str;
                DataManager.characterDatasList[0].itemspeed -= weaponItemDataList[preItem].strspeed;
                DataManager.characterDatasList[0].itemcrip -= weaponItemDataList[preItem].crip;

                DataManager.characterDatasList[0].weapon = weaponItemDataList[thisPage].name;
                DataManager.characterDatasList[0].itemstr += weaponItemDataList[thisPage].str;
                DataManager.characterDatasList[0].itemspeed += weaponItemDataList[thisPage].strspeed;
                DataManager.characterDatasList[0].itemcrip += weaponItemDataList[thisPage].crip;

                for (int i = 0; i < 5; i++) //배경 이미지 변경
                {
                    if (i == 2)
                        gunImg[i].sprite = gunInstallSprite;
                    else
                        gunImg[i].sprite = gunNonInstallSprite;
                }
            }
        }
        //세트 있으면 효과 붙임 //무기 필요한 세트만 확인 //세트 없으면 세트효과 제거
        if(DataManager.characterDatasList[0].helmet=="우주 헬멧"&&DataManager.characterDatasList[0].top=="우주복"&&
            DataManager.characterDatasList[0].weapon == "레이저건")
        {
            DataManager.characterDatasList[0].setstr = 10;
            DataManager.characterDatasList[0].setdef = 10;
            DataManager.characterDatasList[0].setcrip = 2;
            DataManager.characterDatasList[0].setname = "우주";
        }
        else if (DataManager.characterDatasList[0].helmet == "미역" && DataManager.characterDatasList[0].top == "조개" &&DataManager.characterDatasList[0].bottoms=="인어 다리"&&
            DataManager.characterDatasList[0].weapon == "버블건")
        {
            DataManager.characterDatasList[0].setstr = 10;
            DataManager.characterDatasList[0].setagi = 10;
            DataManager.characterDatasList[0].setcrip = 1;
            DataManager.characterDatasList[0].setname = "인어";
        }
        else if (DataManager.characterDatasList[0].helmet == "천사의 링" && DataManager.characterDatasList[0].top == "천사 옷" &&
            DataManager.characterDatasList[0].weapon == "천사링")
        {
            DataManager.characterDatasList[0].setdef = 30;
            DataManager.characterDatasList[0].setagi = 20;
            DataManager.characterDatasList[0].setcrip = 2;
            DataManager.characterDatasList[0].setname = "천사";
        }
        else if (DataManager.characterDatasList[0].helmet == "악마의 뿔" && DataManager.characterDatasList[0].top == "악마 옷" &&
            DataManager.characterDatasList[0].weapon == "악마삼지창")
        {
            DataManager.characterDatasList[0].setstr = 10;
            DataManager.characterDatasList[0].setagi = 30;
            DataManager.characterDatasList[0].setcrip = (decimal)2.5;
            DataManager.characterDatasList[0].setname = "악마";
        }
        else if(DataManager.characterDatasList[0].setname=="우주"|| DataManager.characterDatasList[0].setname == "인어" || DataManager.characterDatasList[0].setname == "천사" ||
            DataManager.characterDatasList[0].setname == "악마")
        {
            DataManager.characterDatasList[0].setstr = 0;
            DataManager.characterDatasList[0].setagi = 0;
            DataManager.characterDatasList[0].setcrip = 0;
            DataManager.characterDatasList[0].setname = "";
        }
    }
    void PageSetting()
    {
        Text temText;
        Image tempImg;
        Button tempBt;
        if (isRight)
        {
            gunImg[0].transform.localPosition = new Vector2(gunImg[4].transform.localPosition.x + 910, 0); //총 이미지 이동
            gunLockImg[0].transform.localPosition = new Vector2(gunLockImg[4].transform.localPosition.x + 910, 0); //잠금 이미지 이동
            tempImg = gunImg[0];
            gunImg[0] = gunImg[1]; gunImg[1] = gunImg[2]; gunImg[2] = gunImg[3]; gunImg[3] = gunImg[4]; gunImg[4] = tempImg;
            tempImg = gunLockImg[0];
            gunLockImg[0] = gunLockImg[1]; gunLockImg[1] = gunLockImg[2]; gunLockImg[2] = gunLockImg[3];
            gunLockImg[3] = gunLockImg[4]; gunLockImg[4] = tempImg;
            tempImg = newAlarmImg[0];
            newAlarmImg[0] = newAlarmImg[1]; newAlarmImg[1] = newAlarmImg[2]; newAlarmImg[2] = newAlarmImg[3];
            newAlarmImg[3] = newAlarmImg[4]; newAlarmImg[4] = tempImg;
            temText = gunNameTx[0];
            gunNameTx[0] = gunNameTx[1]; gunNameTx[1] = gunNameTx[2]; gunNameTx[2] = gunNameTx[3];
            gunNameTx[3] = gunNameTx[4]; gunNameTx[4] = temText;
            tempBt = gunBt[0]; //총 버튼
            gunBt[0] = gunBt[1]; gunBt[1] = gunBt[2]; gunBt[2] = gunBt[3];
            gunBt[3] = gunBt[4]; gunBt[4] = tempBt;
        }
        if (isLeft)
        {
            gunImg[4].transform.localPosition = new Vector2(gunImg[0].transform.localPosition.x - 910, 0);
            gunLockImg[4].transform.localPosition = new Vector2(gunLockImg[0].transform.localPosition.x - 910, 0);
            tempImg = gunImg[4];
            gunImg[4] = gunImg[3]; gunImg[3] = gunImg[2]; gunImg[2] = gunImg[1]; gunImg[1] = gunImg[0]; gunImg[0] = tempImg;
            tempImg = gunLockImg[4];
            gunLockImg[4] = gunLockImg[3]; gunLockImg[3] = gunLockImg[2]; gunLockImg[2] = gunLockImg[1];
            gunLockImg[1] = gunLockImg[0]; gunLockImg[0] = tempImg;
            tempImg = newAlarmImg[4];
            newAlarmImg[4] = newAlarmImg[3]; newAlarmImg[3] = newAlarmImg[2]; newAlarmImg[2] = newAlarmImg[1];
            newAlarmImg[1] = newAlarmImg[0]; newAlarmImg[0] = tempImg;
            temText = gunNameTx[4];
            gunNameTx[4] = gunNameTx[3]; gunNameTx[3] = gunNameTx[2]; gunNameTx[2] = gunNameTx[1];
            gunNameTx[1] = gunNameTx[0]; gunNameTx[0] = temText;
            tempBt = gunBt[4];
            gunBt[4] = gunBt[3]; gunBt[3] = gunBt[2]; gunBt[2] = gunBt[1];
            gunBt[1] = gunBt[0]; gunBt[0] = tempBt;
        }

        if (thisPage == 0)
        {
            gunNameTx[0].text = weaponItemDataList[15].name;
            gunNameTx[1].text = weaponItemDataList[16].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[thisPage + 1].name;
            gunNameTx[4].text = weaponItemDataList[thisPage + 2].name;
        }
        else if (thisPage == 1)
        {
            gunNameTx[0].text = weaponItemDataList[16].name;
            gunNameTx[1].text = weaponItemDataList[0].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[thisPage + 1].name;
            gunNameTx[4].text = weaponItemDataList[thisPage + 2].name;
        }
        else if (thisPage == 16)
        {
            gunNameTx[0].text = weaponItemDataList[thisPage - 2].name;
            gunNameTx[1].text = weaponItemDataList[thisPage - 1].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[0].name;
            gunNameTx[4].text = weaponItemDataList[1].name;
        }
        else if (thisPage == 15)
        {
            gunNameTx[0].text = weaponItemDataList[thisPage - 2].name;
            gunNameTx[1].text = weaponItemDataList[thisPage - 1].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[16].name;
            gunNameTx[4].text = weaponItemDataList[0].name;
        }
        else
        {
            gunNameTx[0].text = weaponItemDataList[thisPage - 2].name;
            gunNameTx[1].text = weaponItemDataList[thisPage - 1].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[thisPage + 1].name;
            gunNameTx[4].text = weaponItemDataList[thisPage + 2].name;
        }
        for (int i = 0; i < 5; i++) //장착 시 배경 이미지 변경
        {
            if (DataManager.characterDatasList[0].weapon == gunNameTx[i].text)
                gunImg[i].sprite = gunInstallSprite;
            else
                gunImg[i].sprite = gunNonInstallSprite;
        }
        StatSetDestroy();
        CheckWeaponLock();
    }
    void CheckWeaponLock()
    {
        for(int i=0; i<5; i++)
        {
            for(int j=0; j<DataManager.allWeaponItemList.Count; j++)
            {
                if (weaponItemDataList[j].name == gunNameTx[i].text && weaponItemDataList[j].count > 0)
                {
                    gunNameTx[i].gameObject.SetActive(true);
                    gunNameTx[i].text += "\n<size=35>보유 개수: " + weaponItemDataList[j].count + "</size>";
                    gunLockImg[i].gameObject.SetActive(false);
                    gunBt[i].image.sprite = gunSprite[j]; //총 이미지에 알맞는 스프라이트 씌우기
                    if (weaponItemDataList[j].isnew)
                        newAlarmImg[i].gameObject.SetActive(true);
                    else
                        newAlarmImg[i].gameObject.SetActive(false);
                }
                else if (weaponItemDataList[j].name == gunNameTx[i].text && weaponItemDataList[j].count == 0)
                {
                    gunNameTx[i].gameObject.SetActive(false);
                    gunLockImg[i].gameObject.SetActive(true);
                    newAlarmImg[i].gameObject.SetActive(false);
                }
            }
        }
        if (weaponItemDataList[thisPage].count > 0)
            installBlindImg.gameObject.SetActive(false);
        else
            installBlindImg.gameObject.SetActive(true);
    }
    void StatSetting()
    {
        classTx.text = "";
        setNameCountTx.text = "";
        setStatTx.text = "";
        statImg.gameObject.SetActive(true);
        int setItemCount = 1;
        int setCount = 0;
        for (int i = 0; i < weaponItemDataList[thisPage].star; i++)
            classTx.text += "★";

        if (weaponItemDataList[thisPage].name == "버블건")
        {
            setNameCountTx.text = "인어 세트 ";
            for(int i=0; i<31; i++)
            {
                if (DataManager.allClothesItemList[i].setname == "인어" && DataManager.allClothesItemList[i].count > 0)
                {
                    setCount = DataManager.allClothesItemList[i].setnumber;
                    setItemCount++;
                }
            }
            setStatTx.text = "세트 적용 시\nSTR +10 AGI +10 크리티컬 확률 +1%";
            setNameCountTx.text += setItemCount + "/" + setCount;
        }
        if (weaponItemDataList[thisPage].name == "레이저건")
        {
            setNameCountTx.text = "우주 세트 ";
            for (int i = 0; i < 31; i++)
            {
                if (DataManager.allClothesItemList[i].setname == "우주" && DataManager.allClothesItemList[i].count > 0)
                {
                    setCount = DataManager.allClothesItemList[i].setnumber;
                    setItemCount++;
                }
            }
            setStatTx.text = "세트 적용 시\nSTR +10 DEF +10 크리티컬 확률 +2%";
            setNameCountTx.text += setItemCount + "/" + setCount;
        }
        if (weaponItemDataList[thisPage].name == "천사링")
        {
            setNameCountTx.text = "천사 세트 ";
            for (int i = 0; i < 31; i++)
            {
                if (DataManager.allClothesItemList[i].setname == "천사" && DataManager.allClothesItemList[i].count > 0)
                {
                    setCount = DataManager.allClothesItemList[i].setnumber;
                    setItemCount++;
                }
            }
            setStatTx.text = "세트 적용 시\nDEF +30 AGI +20 크리티컬 확률 +2%";
            setNameCountTx.text += setItemCount + "/" + setCount;
        }
        if (weaponItemDataList[thisPage].name == "악마삼지창")
        {
            setNameCountTx.text = "악마 세트 ";
            for (int i = 0; i < 31; i++)
            {
                if (DataManager.allClothesItemList[i].setname == "악마" && DataManager.allClothesItemList[i].count > 0)
                {
                    setCount = DataManager.allClothesItemList[i].setnumber;
                    setItemCount++;
                }
            }
            setStatTx.text = "세트 적용 시\nSTR +10 AGI +30 크리티컬 확률 +2.5%";
            setNameCountTx.text += setItemCount + "/" + setCount;
        }
        nameTx.text = "Lv." + weaponItemDataList[thisPage].level + " " + weaponItemDataList[thisPage].name;
        exTx.text = weaponItemDataList[thisPage].ex;
        statTx.text = "STR +" + weaponItemDataList[thisPage].str + "\nSPEED: " + weaponItemDataList[thisPage].strspeed +
            "\n크리티컬 확률 +" + weaponItemDataList[thisPage].crip + "%";
        UpgradeSetting();

        DataManager.allWeaponItemList[thisPage].isnew = false;
        newAlarmImg[2].gameObject.SetActive(false);
        InputWeaponData();
    }
    void UpgradeSetting()
    {
        //업그레이드 세팅
        int k = 0;
        for (int i = 1; i <= weaponItemDataList[thisPage].level + 1; i++)
        {
            if (i > 1)
            {
                sumCount = i + k;
                k = sumCount;
            }
        }
        if (weaponItemDataList[thisPage].level != 5)
        {
            if (weaponItemDataList[thisPage].count < sumCount)
            {
                upgradeTx.text = "다음 업그레이드까지 " + weaponItemDataList[thisPage].count + "/" + sumCount;
                upgradeBlind.gameObject.SetActive(true);
            }
            else
                upgradeBlind.gameObject.SetActive(false);
        }
        else
        {
            upgradeBlind.gameObject.SetActive(true);
            upgradeTx.text = "업그레이드가 종료되었습니다.";
        }
    }
    void StatSetDestroy()
    {
        classTx.text = "";
        setNameCountTx.text = "";
        setStatTx.text = "";
        isGunClick = false;
        statImg.gameObject.SetActive(false);
    }
    void InputWeaponData() //아이템 옮겨 담기
    {
        weaponItemDataList.Clear();
        for (int i = 0; i < DataManager.allWeaponItemList.Count; i++)
            weaponItemDataList.Add(new WeaponItemData(DataManager.allWeaponItemList[i].name, DataManager.allWeaponItemList[i].ex, DataManager.allWeaponItemList[i].level,
                DataManager.allWeaponItemList[i].count, DataManager.allWeaponItemList[i].star, DataManager.allWeaponItemList[i].str, DataManager.allWeaponItemList[i].strspeed,
                DataManager.allWeaponItemList[i].crip,DataManager.allWeaponItemList[i].isnew));
    }
    public class WeaponItemData
    {
        public WeaponItemData(string _name,string _ex, int _level, int _count, int _star, int _str, decimal _strspeed, decimal _crip,bool _isnew)
        {
            name = _name; ex = _ex; level = _level; count = _count; star = _star; str = _str; strspeed = _strspeed; crip = _crip; isnew = _isnew;
        }
        public string name, ex;
        public int level, count, star, str;
        public decimal strspeed, crip;
        public bool isnew;
    }
}
