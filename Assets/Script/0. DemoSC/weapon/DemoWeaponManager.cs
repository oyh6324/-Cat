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

    List<WeaponItemData> weaponItemDataList; //무기 정보 리스트
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
        weaponItemDataList.Clear(); //리스트 정리
        InputWeaponData(); //무기 데이터 넣기
        if (DemoDataManager.Instance.characterDatasList[0].weapon != "") //캐릭터가 장착한 무기가 있다면
        {
            for (int i = 0; i < DemoDataManager.Instance.allWeaponItemList.Count; i++)
            {
                if (weaponItemDataList[i].name.Equals(DemoDataManager.Instance.characterDatasList[0].weapon))
                    thisPage = i; //장착한 무기 페이지
            }
        }
        else //장착한 무기가 없다면
            thisPage = 0; //첫 페이지
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
        if (isRight) //오른쪽으로 이동
        {
            gunImgs.transform.localPosition = Vector2.MoveTowards(new Vector2(gunImgs.transform.localPosition.x, gunImgs.transform.localPosition.y),
                 new Vector2(nextPlaceX, gunImgs.transform.localPosition.y), 50f);
        }
        if (isLeft) //왼쪽으로 이동
        {
            gunImgs.transform.localPosition = Vector2.MoveTowards(new Vector2(gunImgs.transform.localPosition.x, gunImgs.transform.localPosition.y),
                new Vector2(nextPlaceX, gunImgs.transform.localPosition.y), 50f);
        }
        if (gunImgs.transform.localPosition.x == nextPlaceX) //이동할 위치에 도착했다면
            isMove = true;
        else //이동할 위치에 도착하지 않았다면
            isMove = false;

        //장착 관리
        if (DemoDataManager.Instance.characterDatasList[0].weapon == weaponItemDataList[thisPage].name) //장착한 무기가 현재 페이지 무기와 같다면
            installBtTx.text = "장착 해제"; //장착 해제 text 출력
        else  //다르다면
            installBtTx.text = "장착"; //장착 text 출력

        pageTx.text = thisPage + 1 +"/"+ weaponItemDataList.Count.ToString(); //현재 페이지 출력
    }
    void OnDisable()
    {
        StatSetDestroy(); //스탯 리셋

        //플레이어가 착용한 장비 스탯에 무기 스탯 추가
        DemoDataManager.Instance.characterDatasList[0].allstr = DemoDataManager.Instance.characterDatasList[0].str + DemoDataManager.Instance.characterDatasList[0].itemstr;
        DemoDataManager.Instance.characterDatasList[0].alldef = DemoDataManager.Instance.characterDatasList[0].def + DemoDataManager.Instance.characterDatasList[0].itemdef;
        DemoDataManager.Instance.characterDatasList[0].allagi = DemoDataManager.Instance.characterDatasList[0].agi + DemoDataManager.Instance.characterDatasList[0].itemagi;
        DemoDataManager.Instance.characterDatasList[0].allcrip = DemoDataManager.Instance.characterDatasList[0].crip + DemoDataManager.Instance.characterDatasList[0].itemcrip;

        //블라인드 옮기기
        rightBlindImg1.transform.localPosition = new Vector2(1500, -1000);
        rightBlindImg2.transform.localPosition = new Vector2(1500, 1000);
        leftBlindImg1.transform.localPosition = new Vector2(-1500, -1000);
        leftBlindImg2.transform.localPosition = new Vector2(-1500, 1000);
    }
    public void RightBtClick() //오른쪽으로 이동 버튼 클릭
    {
        soundEffectAS.clip = rightLeftBtClip;
        soundEffectAS.Play();
        if (isMove) //이동 가능
        {
            if (thisPage == 4) //마지막 페이지일 때
                thisPage = 0; //첫 페이지로 돌아옴
            else //마지막 페이지가 아닐 때
                thisPage++; //현재 페이지 증가
            nextPlaceX = (int)gunImgs.transform.localPosition.x - 910; //다음 이동할 장소 지정
            isRight = true; //오른쪽으로 이동 신호
            isLeft = false;
            PageSetting();
        }
    }
    public void LeftBtClick() //오른쪽으로 이동 버튼 클릭
    {
        soundEffectAS.clip = rightLeftBtClip;
        soundEffectAS.Play();
        if (isMove) //이동 가능
        {
            if (thisPage == 0) //첫 페이지일 때
                thisPage = 4; //마지막 페이지로 돌아옴
            else //첫 페이지가 아닐 때
                thisPage--; //현재 페이지 감소
            nextPlaceX = (int)gunImgs.transform.localPosition.x + 910; //다음 이동할 장소 지정
            isRight = false;
            isLeft = true; //왼쪽으로 이동 신호
            PageSetting();
        }
    }
    public void GunClick() //무기 버튼 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (isGunClick) //클릭한 무기라면
        {
            StatSetDestroy(); //스탯 창 비활성화
        }
        else 
        {
            isGunClick = true; 
            StatSetting(); //스탯 창 활성화
        }
    }
    public void UpgradeBtClick() //무기 업그레이드 함수
    {
        for(int i=0; i<weaponItemDataList.Count; i++)
        {
            //캐릭터가 장착한 무기라면 원래 스탯 제거
            if(DemoDataManager.Instance.characterDatasList[0].weapon== weaponItemDataList[i].name)
            {
                DemoDataManager.Instance.characterDatasList[0].itemstr -= weaponItemDataList[i].str;
                DemoDataManager.Instance.characterDatasList[0].itemspeed -= weaponItemDataList[i].strspeed;
                DemoDataManager.Instance.characterDatasList[0].itemcrip -= weaponItemDataList[i].crip;
            }
        }

        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        //스탯 증가
        DemoDataManager.Instance.allWeaponItemList[thisPage].strspeed -= (decimal)0.02;
        DemoDataManager.Instance.allWeaponItemList[thisPage].level += 1;
        if (weaponItemDataList[thisPage].name == "에어건")
        {
            DemoDataManager.Instance.allWeaponItemList[thisPage].str += 2;
            DemoDataManager.Instance.allWeaponItemList[thisPage].crip += (decimal)0.1;
        }
        else
        {
            DemoDataManager.Instance.allWeaponItemList[thisPage].crip += (decimal)0.2;
            if (weaponItemDataList[thisPage].name == "수리검")
                DemoDataManager.Instance.allWeaponItemList[thisPage].str += 3;
            else if (weaponItemDataList[thisPage].name == "음파건")
                DemoDataManager.Instance.allWeaponItemList[thisPage].str += 4;
            else if (weaponItemDataList[thisPage].name == "쥬얼건"||weaponItemDataList[thisPage].name == "썬더건")
                DemoDataManager.Instance.allWeaponItemList[thisPage].str += 5;
        }
        DemoDataManager.Instance.allWeaponItemList[thisPage].count = weaponItemDataList[thisPage].count - sumCount+1;

        gunNameTx[2].text = DemoDataManager.Instance.allWeaponItemList[thisPage].name+ "\n<size=35>보유 개수: " + DemoDataManager.Instance.allWeaponItemList[thisPage].count + "</size>"; //개수 바꿔주기
        InputWeaponData();
        StatSetting();
        AchievementCheck();

        for (int i = 0; i < weaponItemDataList.Count; i++)
        {
            //업그레이드 한 스탯 유저에게 저장
            if (DemoDataManager.Instance.characterDatasList[0].weapon == weaponItemDataList[i].name)
            {
                DemoDataManager.Instance.characterDatasList[0].itemstr += weaponItemDataList[i].str;
                DemoDataManager.Instance.characterDatasList[0].itemspeed += weaponItemDataList[i].strspeed;
                DemoDataManager.Instance.characterDatasList[0].itemcrip += weaponItemDataList[i].crip;
            }
        }
    }
    void AchievementCheck() //업적 연동
    {
        int count3 = 0, count4 = 0, count5 = 0;
        for (int i = 0; i < DemoDataManager.Instance.allWeaponItemList.Count; i++)
        {
            if (DemoDataManager.Instance.allWeaponItemList[i].level >= 3)
                count3++;
            if (DemoDataManager.Instance.allWeaponItemList[i].level >= 4)
                count4++;
            if (DemoDataManager.Instance.allWeaponItemList[i].level == 5)
                count5++;
        }
        DemoDataManager.Instance.achievementDataList[10].progressvalue = count3;
        DemoDataManager.Instance.achievementDataList[11].progressvalue = count4;
        DemoDataManager.Instance.achievementDataList[12].progressvalue = count5;
    }
    public void InstallitemBtClick() //무기 장착 버튼 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (DemoDataManager.Instance.characterDatasList[0].weapon == "") //아무것도 착용 하지 않았을 시
        {
            //무기 스탯 장착
            DemoDataManager.Instance.characterDatasList[0].weapon = weaponItemDataList[thisPage].name;
            DemoDataManager.Instance.characterDatasList[0].itemstr += weaponItemDataList[thisPage].str;
            DemoDataManager.Instance.characterDatasList[0].itemspeed += weaponItemDataList[thisPage].strspeed;
            DemoDataManager.Instance.characterDatasList[0].itemcrip += weaponItemDataList[thisPage].crip;

            gunImg[2].sprite = gunInstallSprite; //배경 이미지 변경
        }
        else if (DemoDataManager.Instance.characterDatasList[0].weapon != "") //무언가를 착용한 상태
        {
            if (DemoDataManager.Instance.characterDatasList[0].weapon == weaponItemDataList[thisPage].name) //지금 든 무기 장착 해제
            {
                DemoDataManager.Instance.characterDatasList[0].weapon = "";
                DemoDataManager.Instance.characterDatasList[0].itemstr -= weaponItemDataList[thisPage].str;
                DemoDataManager.Instance.characterDatasList[0].itemspeed -= weaponItemDataList[thisPage].strspeed;
                DemoDataManager.Instance.characterDatasList[0].itemcrip -= weaponItemDataList[thisPage].crip;

                gunImg[2].sprite = gunNonInstallSprite; //배경 이미지 변경
            }
            else //앞에 든 무기 장착 해제 후 지금 무기 장착
            {
                int preItem = 0;
                for (int i = 0; i < DemoDataManager.Instance.allWeaponItemList.Count; i++)
                {
                    if (DemoDataManager.Instance.characterDatasList[0].weapon == weaponItemDataList[i].name)
                        preItem = i;
                }
                DemoDataManager.Instance.characterDatasList[0].itemstr -= weaponItemDataList[preItem].str;
                DemoDataManager.Instance.characterDatasList[0].itemspeed -= weaponItemDataList[preItem].strspeed;
                DemoDataManager.Instance.characterDatasList[0].itemcrip -= weaponItemDataList[preItem].crip;

                DemoDataManager.Instance.characterDatasList[0].weapon = weaponItemDataList[thisPage].name;
                DemoDataManager.Instance.characterDatasList[0].itemstr += weaponItemDataList[thisPage].str;
                DemoDataManager.Instance.characterDatasList[0].itemspeed += weaponItemDataList[thisPage].strspeed;
                DemoDataManager.Instance.characterDatasList[0].itemcrip += weaponItemDataList[thisPage].crip;

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
        if (isRight) //오른쪽으로 이동 시
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
        if (isLeft) //왼쪽으로 이동 시
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
            if (DemoDataManager.Instance.characterDatasList[0].weapon == gunNameTx[i].text)
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
            for (int j = 0; j < DemoDataManager.Instance.allWeaponItemList.Count; j++)
            {
                if (weaponItemDataList[j].name == gunNameTx[i].text && weaponItemDataList[j].count > 0) //가지고 있는 무기라면
                {
                    gunNameTx[i].gameObject.SetActive(true);
                    gunNameTx[i].text += "\n<size=35>보유 개수: " + weaponItemDataList[j].count + "</size>";
                    gunLockImg[i].gameObject.SetActive(false); //잠금 이미지 비활성화
                    gunBt[i].image.sprite = gunSprite[j]; //총 이미지에 알맞는 스프라이트 씌우기
                    if (weaponItemDataList[j].isnew) //새로 얻은 무기라면
                        newAlarmImg[i].gameObject.SetActive(true); //new 알람 활성화
                    else 
                        newAlarmImg[i].gameObject.SetActive(false); //new 알람 비활성화
                }
                else if (weaponItemDataList[j].name == gunNameTx[i].text && weaponItemDataList[j].count == 0) //가지고 있는 무기가 아니라면
                {
                    //무기 이미지 잠금
                    gunNameTx[i].gameObject.SetActive(false);
                    gunLockImg[i].gameObject.SetActive(true);
                    newAlarmImg[i].gameObject.SetActive(false);
                }
            }
        }
        //무기 보유 여부에 따라 장착 잠금 이미지 해제
        if (weaponItemDataList[thisPage].count > 0)
            installBlindImg.gameObject.SetActive(false);
        else
            installBlindImg.gameObject.SetActive(true);
    }
    void StatSetting() //무기 스탯 출력
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

        DemoDataManager.Instance.allWeaponItemList[thisPage].isnew = false;
        newAlarmImg[2].gameObject.SetActive(false);
        InputWeaponData();
    }
    void StatSetDestroy() //무기 스탯 비활성화
    {
        classTx.text = "";
        isGunClick = false;
        statImg.gameObject.SetActive(false);
    }
    void UpgradeSetting()
    {
        //업그레이드 세팅
        int k = 0;
        for (int i = 1; i <= weaponItemDataList[thisPage].level + 1; i++) //업그레이드에 필요한 개수
        {
            if (i > 1)
            {
                sumCount = i + k;
                k = sumCount;
            }
        }
        if (weaponItemDataList[thisPage].level != 5) //무기 레벨이 5 이하라면
        {
            if (weaponItemDataList[thisPage].count < sumCount) //업그레이드 불가능
            {
                upgradeTx.text = "다음 업그레이드까지 " + weaponItemDataList[thisPage].count + "/" + sumCount;
                upgradeBlind.gameObject.SetActive(true); //업그레이드 버튼 잠금 활성화
            }
            else //업그레이드 가능
                upgradeBlind.gameObject.SetActive(false); //업그레이드 버튼 잠금 비활성화
        }
        else //무기 레벨이 5라면
        {
            upgradeBlind.gameObject.SetActive(true); //업그레이드 버튼 잠금 활성화
            upgradeTx.text = "업그레이드가 종료되었습니다.";
        }
    }
    void InputWeaponData() //무기 정보 담기
    {
        weaponItemDataList.Clear();
        for (int i = 0; i < DemoDataManager.Instance.allWeaponItemList.Count; i++)
            weaponItemDataList.Add(new WeaponItemData(DemoDataManager.Instance.allWeaponItemList[i].name, DemoDataManager.Instance.allWeaponItemList[i].ex, DemoDataManager.Instance.allWeaponItemList[i].level,
                DemoDataManager.Instance.allWeaponItemList[i].count, DemoDataManager.Instance.allWeaponItemList[i].star, DemoDataManager.Instance.allWeaponItemList[i].str, DemoDataManager.Instance.allWeaponItemList[i].strspeed,
                DemoDataManager.Instance.allWeaponItemList[i].crip, DemoDataManager.Instance.allWeaponItemList[i].isnew));
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
