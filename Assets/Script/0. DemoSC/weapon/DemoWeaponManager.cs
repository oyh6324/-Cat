using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoWeaponManager : MonoBehaviour
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

    //오디오
    public AudioSource bgmAS;
    public AudioSource soundEffectAS;
    public AudioClip buttonClickClip;
    public AudioClip weaponBgmClip;
    public AudioClip rightLeftBtClip;

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
        bgmAS.clip = weaponBgmClip;
        bgmAS.Play();
        sumCount = 0; //업그레이드 부분
        gunImgs.transform.localPosition = new Vector2(nextPlaceX, gunImgs.transform.localPosition.y);
        isRight = false;
        isLeft = false;
        weaponItemDataList.Clear();
        InputWeaponData();
        if (DemoDataManager.characterDatasList[0].weapon != "")
        {
            for (int i = 0; i < DemoDataManager.allWeaponItemList.Count; i++)
            {
                if (weaponItemDataList[i].name.Equals(DemoDataManager.characterDatasList[0].weapon))
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
        rightBlindImg1.transform.localPosition = Vector2.MoveTowards(new Vector2(rightBlindImg1.transform.localPosition.x,rightBlindImg1.transform.localPosition.y),
            new Vector2(880, 0), 60);
        rightBlindImg2.transform.localPosition = Vector2.MoveTowards(new Vector2(rightBlindImg2.transform.localPosition.x,rightBlindImg2.transform.localPosition.y),
            new Vector2(880, 0), 60);
        leftBlindImg1.transform.localPosition = Vector2.MoveTowards(new Vector2(leftBlindImg1.transform.localPosition.x, leftBlindImg1.transform.localPosition.y),
            new Vector2(-880, 0),60);
        leftBlindImg2.transform.localPosition = Vector2.MoveTowards(new Vector2(leftBlindImg2.transform.localPosition.x, leftBlindImg2.transform.localPosition.y),
            new Vector2(-880, 0), 60);
        if (isRight)
        {
            gunImgs.transform.localPosition = Vector2.MoveTowards(new Vector2(gunImgs.transform.localPosition.x, gunImgs.transform.localPosition.y),
                 new Vector2(nextPlaceX, gunImgs.transform.localPosition.y), 50f);
        }
        if (isLeft)
        {
            gunImgs.transform.localPosition = Vector2.MoveTowards(new Vector2(gunImgs.transform.localPosition.x, gunImgs.transform.localPosition.y),
                new Vector2(nextPlaceX, gunImgs.transform.localPosition.y), 50f);
        }
        if (gunImgs.transform.localPosition.x == nextPlaceX)
            isMove = true;
        else
            isMove = false;

        //장착 관리
        if (DemoDataManager.characterDatasList[0].weapon == weaponItemDataList[thisPage].name)
            installBtTx.text = "장착 해제";
        else
            installBtTx.text = "장착";

        pageTx.text = thisPage + 1 +"/"+ weaponItemDataList.Count.ToString();
    }
    void OnDisable()
    {
        StatSetDestroy();

        DemoDataManager.characterDatasList[0].allstr = DemoDataManager.characterDatasList[0].str + DemoDataManager.characterDatasList[0].itemstr;
        DemoDataManager.characterDatasList[0].alldef = DemoDataManager.characterDatasList[0].def + DemoDataManager.characterDatasList[0].itemdef;
        DemoDataManager.characterDatasList[0].allagi = DemoDataManager.characterDatasList[0].agi + DemoDataManager.characterDatasList[0].itemagi;
        DemoDataManager.characterDatasList[0].allcrip = DemoDataManager.characterDatasList[0].crip + DemoDataManager.characterDatasList[0].itemcrip;

        //블라인드 옮기기
        rightBlindImg1.transform.localPosition = new Vector2(1500, -1000);
        rightBlindImg2.transform.localPosition = new Vector2(1500, 1000);
        leftBlindImg1.transform.localPosition = new Vector2(-1500, -1000);
        leftBlindImg2.transform.localPosition = new Vector2(-1500, 1000);
    }
    public void RightBtClick()
    {
        soundEffectAS.clip = rightLeftBtClip;
        soundEffectAS.Play();
        if (isMove)
        {
            if (thisPage == 4)
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
        soundEffectAS.clip = rightLeftBtClip;
        soundEffectAS.Play();
        if (isMove)
        {
            if (thisPage == 0)
                thisPage = 4;
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
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
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
        for(int i=0; i<weaponItemDataList.Count; i++)
        {
            if(DemoDataManager.characterDatasList[0].weapon== weaponItemDataList[i].name)
            {
                DemoDataManager.characterDatasList[0].itemstr -= weaponItemDataList[i].str;
                DemoDataManager.characterDatasList[0].itemspeed -= weaponItemDataList[i].strspeed;
                DemoDataManager.characterDatasList[0].itemcrip -= weaponItemDataList[i].crip;
            }
        }

        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        DemoDataManager.allWeaponItemList[thisPage].strspeed -= (decimal)0.02;
        DemoDataManager.allWeaponItemList[thisPage].level += 1;
        if (weaponItemDataList[thisPage].name == "에어건")
        {
            DemoDataManager.allWeaponItemList[thisPage].str += 2;
            DemoDataManager.allWeaponItemList[thisPage].crip += (decimal)0.1;
        }
        else
        {
            DemoDataManager.allWeaponItemList[thisPage].crip += (decimal)0.2;
            if (weaponItemDataList[thisPage].name == "수리검")
                DemoDataManager.allWeaponItemList[thisPage].str += 3;
            else if (weaponItemDataList[thisPage].name == "음파건")
                DemoDataManager.allWeaponItemList[thisPage].str += 4;
            else if (weaponItemDataList[thisPage].name == "쥬얼건"||weaponItemDataList[thisPage].name == "썬더건")
                DemoDataManager.allWeaponItemList[thisPage].str += 5;
        }
        DemoDataManager.allWeaponItemList[thisPage].count = weaponItemDataList[thisPage].count - sumCount+1;

        gunNameTx[2].text = DemoDataManager.allWeaponItemList[thisPage].name+ "\n<size=35>보유 개수: " + DemoDataManager.allWeaponItemList[thisPage].count + "</size>"; //개수 바꿔주기
        InputWeaponData();
        StatSetting();
        AchievementCheck();

        for (int i = 0; i < weaponItemDataList.Count; i++)
        {
            if (DemoDataManager.characterDatasList[0].weapon == weaponItemDataList[i].name)
            {
                DemoDataManager.characterDatasList[0].itemstr += weaponItemDataList[i].str;
                DemoDataManager.characterDatasList[0].itemspeed += weaponItemDataList[i].strspeed;
                DemoDataManager.characterDatasList[0].itemcrip += weaponItemDataList[i].crip;
            }
        }
    }
    void AchievementCheck() //업적 연동
    {
        int count3 = 0, count4 = 0, count5 = 0;
        for (int i = 0; i < DemoDataManager.allWeaponItemList.Count; i++)
        {
            if (DemoDataManager.allWeaponItemList[i].level >= 3)
                count3++;
            if (DemoDataManager.allWeaponItemList[i].level >= 4)
                count4++;
            if (DemoDataManager.allWeaponItemList[i].level == 5)
                count5++;
        }
        DemoDataManager.achievementDataList[10].progressvalue = count3;
        DemoDataManager.achievementDataList[11].progressvalue = count4;
        DemoDataManager.achievementDataList[12].progressvalue = count5;
    }
    public void InstallitemBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (DemoDataManager.characterDatasList[0].weapon == "") //아무것도 착용 하지 않았을 시
        {
            DemoDataManager.characterDatasList[0].weapon = weaponItemDataList[thisPage].name;
            DemoDataManager.characterDatasList[0].itemstr += weaponItemDataList[thisPage].str;
            DemoDataManager.characterDatasList[0].itemspeed += weaponItemDataList[thisPage].strspeed;
            DemoDataManager.characterDatasList[0].itemcrip += weaponItemDataList[thisPage].crip;

            gunImg[2].sprite = gunInstallSprite; //배경 이미지 변경
        }
        else if (DemoDataManager.characterDatasList[0].weapon != "") //무언가를 착용한 상태
        {
            if (DemoDataManager.characterDatasList[0].weapon == weaponItemDataList[thisPage].name) //지금 든 무기 장착 해제
            {
                DemoDataManager.characterDatasList[0].weapon = "";
                DemoDataManager.characterDatasList[0].itemstr -= weaponItemDataList[thisPage].str;
                DemoDataManager.characterDatasList[0].itemspeed -= weaponItemDataList[thisPage].strspeed;
                DemoDataManager.characterDatasList[0].itemcrip -= weaponItemDataList[thisPage].crip;

                gunImg[2].sprite = gunNonInstallSprite; //배경 이미지 변경
            }
            else //앞에 든 무기 장착 해제 후 지금 무기 장착
            {
                int preItem = 0;
                for (int i = 0; i < DemoDataManager.allWeaponItemList.Count; i++)
                {
                    if (DemoDataManager.characterDatasList[0].weapon == weaponItemDataList[i].name)
                        preItem = i;
                }
                DemoDataManager.characterDatasList[0].itemstr -= weaponItemDataList[preItem].str;
                DemoDataManager.characterDatasList[0].itemspeed -= weaponItemDataList[preItem].strspeed;
                DemoDataManager.characterDatasList[0].itemcrip -= weaponItemDataList[preItem].crip;

                DemoDataManager.characterDatasList[0].weapon = weaponItemDataList[thisPage].name;
                DemoDataManager.characterDatasList[0].itemstr += weaponItemDataList[thisPage].str;
                DemoDataManager.characterDatasList[0].itemspeed += weaponItemDataList[thisPage].strspeed;
                DemoDataManager.characterDatasList[0].itemcrip += weaponItemDataList[thisPage].crip;

                for(int i=0; i<5; i++) //배경 이미지 변경
                {
                    if (i == 2)
                        gunImg[i].sprite = gunInstallSprite;
                    else
                        gunImg[i].sprite = gunNonInstallSprite;
                }
            }
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
            tempImg = newAlarmImg[0]; //알람 이미지
            newAlarmImg[0] = newAlarmImg[1]; newAlarmImg[1] = newAlarmImg[2]; newAlarmImg[2] = newAlarmImg[3];
            newAlarmImg[3] = newAlarmImg[4]; newAlarmImg[4] = tempImg;
            temText = gunNameTx[0]; //이름 텍스트
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
            gunNameTx[0].text = weaponItemDataList[3].name;
            gunNameTx[1].text = weaponItemDataList[4].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[thisPage + 1].name;
            gunNameTx[4].text = weaponItemDataList[thisPage + 2].name;
        }
        else if (thisPage == 1)
        {
            gunNameTx[0].text = weaponItemDataList[4].name;
            gunNameTx[1].text = weaponItemDataList[0].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[thisPage + 1].name;
            gunNameTx[4].text = weaponItemDataList[thisPage + 2].name;
        }
        else if (thisPage == 4)
        {
            gunNameTx[0].text = weaponItemDataList[thisPage - 2].name;
            gunNameTx[1].text = weaponItemDataList[thisPage - 1].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[0].name;
            gunNameTx[4].text = weaponItemDataList[1].name;
        }
        else if (thisPage == 3)
        {
            gunNameTx[0].text = weaponItemDataList[thisPage - 2].name;
            gunNameTx[1].text = weaponItemDataList[thisPage - 1].name;
            gunNameTx[2].text = weaponItemDataList[thisPage].name;
            gunNameTx[3].text = weaponItemDataList[4].name;
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
        for(int i=0; i<5; i++) //장착 시 배경 이미지 변경
        {
            if (DemoDataManager.characterDatasList[0].weapon == gunNameTx[i].text)
                gunImg[i].sprite = gunInstallSprite;
            else
                gunImg[i].sprite = gunNonInstallSprite;
        }
        StatSetDestroy();
        CheckWeaponLock();
    }
    void CheckWeaponLock()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < DemoDataManager.allWeaponItemList.Count; j++)
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
        statImg.gameObject.SetActive(true);
        for (int i = 0; i < weaponItemDataList[thisPage].star; i++)
            classTx.text += "★";
        nameTx.text = "Lv." + weaponItemDataList[thisPage].level + " " + weaponItemDataList[thisPage].name;
        exTx.text = weaponItemDataList[thisPage].ex;
        statTx.text= "STR +" + weaponItemDataList[thisPage].str + "\nSPEED: " + weaponItemDataList[thisPage].strspeed +
            "\n크리티컬 확률 +" + weaponItemDataList[thisPage].crip + "%";
        UpgradeSetting();

        DemoDataManager.allWeaponItemList[thisPage].isnew = false;
        newAlarmImg[2].gameObject.SetActive(false);
        InputWeaponData();
    }
    void StatSetDestroy()
    {
        classTx.text = "";
        isGunClick = false;
        statImg.gameObject.SetActive(false);
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
    void InputWeaponData() //아이템 옮겨 담기
    {
        weaponItemDataList.Clear();
        for (int i = 0; i < DemoDataManager.allWeaponItemList.Count; i++)
            weaponItemDataList.Add(new WeaponItemData(DemoDataManager.allWeaponItemList[i].name, DemoDataManager.allWeaponItemList[i].ex, DemoDataManager.allWeaponItemList[i].level,
                DemoDataManager.allWeaponItemList[i].count, DemoDataManager.allWeaponItemList[i].star, DemoDataManager.allWeaponItemList[i].str, DemoDataManager.allWeaponItemList[i].strspeed,
                DemoDataManager.allWeaponItemList[i].crip, DemoDataManager.allWeaponItemList[i].isnew));
    }
    public class WeaponItemData
    {
        public WeaponItemData(string _name, string _ex, int _level, int _count, int _star, int _str, decimal _strspeed, decimal _crip, bool _isnew)
        {
            name = _name; ex = _ex; level = _level; count = _count; star = _star; str = _str; strspeed = _strspeed; crip = _crip; isnew = _isnew;
        }
        public string name, ex;
        public int level, count, star, str;
        public decimal strspeed, crip;
        public bool isnew;
    }
}
