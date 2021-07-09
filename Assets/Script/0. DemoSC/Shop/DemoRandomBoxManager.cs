using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoRandomBoxManager : MonoBehaviour
{
    public Canvas messageCanvas;
    public Image messageImg;
    public Image boxonImg;

    //뽑기창 내용물
    public Text classTx;
    public Text levelnameTx;
    public Text exTx;
    public Text statTx;
    public Text setTx;
    public Image itemImg;
    public Sprite[] clothesSprite;
    public Sprite[] weaponSprite;
    //오디오
    public AudioSource soundEffectAS;
    public AudioClip buttonClickClip;

    int random;
    int randomPercent;
    void OnEnable()
    {
        //뽑기창 생성
        messageImg.gameObject.SetActive(false);
        boxonImg.gameObject.SetActive(true);

        if (PlayerPrefs.HasKey("멸치의상뽑기"))  //멸치 의상 뽑기 신호
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 50) //1등급
                random = Random.Range(0, 3);
            else if (randomPercent < 90) //2등급
                random = Random.Range(3, 6);
            else //3등급
                random = 6;
            ClothesBoxRandom();
            PlayerPrefs.DeleteKey("멸치의상뽑기");
        }
        if (PlayerPrefs.HasKey("진주의상뽑기")) //진주 의상 뽑기 신호
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 80) //3등급
                random = 6;
            else if (randomPercent < 100) //4등급
                random = Random.Range(6, 9);
            ClothesBoxRandom();
            PlayerPrefs.DeleteKey("진주의상뽑기");
        }
        if (PlayerPrefs.HasKey("멸치무기뽑기")) //멸치 무기 뽑기 신호
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 50) //1등급
                random = 0;
            else if (randomPercent < 90) //2등급
                random = 1;
            else //3등급
                random = Random.Range(2, 4);
            WeaponBoxRandom();
            PlayerPrefs.DeleteKey("멸치무기뽑기");
        }
        if (PlayerPrefs.HasKey("진주무기뽑기")) //진주 무기 뽑기 신호
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 80) //3등급
                random = Random.Range(2, 4);
            else if (randomPercent < 100) //4등급
                random = 4;
            WeaponBoxRandom();
            PlayerPrefs.DeleteKey("진주무기뽑기");
        }
        AchievementCheck(); //업적 연동
    }
    void OnDisable() //오브젝트 비활성화
    { //출력 문구 리셋
        classTx.text = "";
        levelnameTx.text = "";
        exTx.text = "";
        statTx.text = "";
        setTx.text = "";
    }
    public void OkBtClick() //확인 버튼 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        //뽑기 창 비활성화
        boxonImg.gameObject.SetActive(false);
        messageImg.gameObject.SetActive(true);
        messageCanvas.gameObject.SetActive(false);
    }
    void ClothesBoxRandom() //의상 정보 출력
    {
        //설명에 필요한 정보 옮겨담기
        List<Stat> statList = new List<Stat>();
        string clothesName = DemoDataManager.Instance.allClothesItemList[random].name;
        int clothesLevel = DemoDataManager.Instance.allClothesItemList[random].level;
        string clothesEx = DemoDataManager.Instance.allClothesItemList[random].ex;
        int clothesStar = DemoDataManager.Instance.allClothesItemList[random].star;
        decimal clothesSetcrip = DemoDataManager.Instance.allClothesItemList[random].setcrip;

        statList.Add(new Stat("STR", DemoDataManager.Instance.allClothesItemList[random].str));
        statList.Add(new Stat("DEF", DemoDataManager.Instance.allClothesItemList[random].def));
        statList.Add(new Stat("AGI", DemoDataManager.Instance.allClothesItemList[random].agi));

        //세트 착용 시 추가 스탯
        statList.Add(new Stat("STR", DemoDataManager.Instance.allClothesItemList[random].setstr));
        statList.Add(new Stat("DEF", DemoDataManager.Instance.allClothesItemList[random].setdef));
        statList.Add(new Stat("AGI", DemoDataManager.Instance.allClothesItemList[random].setagi));

        DemoDataManager.Instance.allClothesItemList[random].count += 1; //보유 개수 추가

        for (int i = 0; i < clothesStar; i++) //등급 표시
            classTx.text += "★";

        levelnameTx.text = "Lv." + clothesLevel + " " + clothesName; //레벨과 이름표시
        exTx.text = clothesEx; //설명 표시

        for (int i = 0; i < 3; i++) //스탯 효과 표시
        {
            if (statList[i].number != 0)
                statTx.text += statList[i].name + " +" + statList[i].number + "\n";
        }
        if (DemoDataManager.Instance.allClothesItemList[random].setname != "") //세트 효과 표시
        {
            setTx.text = "세트 적용 시\n";
            for (int i = 3; i < 6; i++)
            {
                if (statList[i].number != 0)
                    setTx.text += statList[i].name + " +" + statList[i].number + "\n";
            }
            if (clothesSetcrip != 0)
                setTx.text += "크리티컬 확률 +" + clothesSetcrip + "%";
        }
        if (DemoDataManager.Instance.allClothesItemList[random].count == 1) //새로운 아이템이라면
            DemoDataManager.Instance.allClothesItemList[random].isnew = true; //new 신호

        //의상 스프라이트
        itemImg.sprite = clothesSprite[random];
        //이미지 위치 선정
        itemImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500);
        itemImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
        if (clothesName=="어항"||clothesName=="스노쿨링"||clothesName=="우주 헬멧")
            itemImg.transform.localPosition = new Vector2(-5, 60);
        else if(clothesName=="우주복"||clothesName=="멜빵 바지" || clothesName=="나뭇잎 바지")
            itemImg.transform.localPosition = new Vector2(-5, 210);
        else
            itemImg.transform.localPosition = new Vector2(-5, 80);
    }
    void WeaponBoxRandom() //무기 정보 출력
    {
        //정보 옮겨담기
        string weaponName = DemoDataManager.Instance.allWeaponItemList[random].name;
        int weaponLevel = DemoDataManager.Instance.allWeaponItemList[random].level;
        int weaponStar = DemoDataManager.Instance.allWeaponItemList[random].star;
        string weaponEX = DemoDataManager.Instance.allWeaponItemList[random].ex;
        int weaponStr = DemoDataManager.Instance.allWeaponItemList[random].str;
        decimal weaponSpeed = DemoDataManager.Instance.allWeaponItemList[random].strspeed;
        decimal weaponCrip = DemoDataManager.Instance.allWeaponItemList[random].crip;

        DemoDataManager.Instance.allWeaponItemList[random].count += 1; //보유 개수 증가

        for (int i = 0; i < weaponStar; i++) //등급 표시
            classTx.text += "★";

        levelnameTx.text = "Lv." + weaponLevel + " " + weaponName; //레벨과 이름표시
        exTx.text = weaponEX; //설명 표시
        statTx.text = "STR +" + weaponStr + "\nSPEED " + weaponSpeed + "\n크티리컬 확률 +" + weaponCrip + "%"; //스탯 효과 표시

        if (DemoDataManager.Instance.allWeaponItemList[random].count == 1)
            DemoDataManager.Instance.allWeaponItemList[random].isnew = true;

        //무기 스프라이트
        itemImg.sprite = weaponSprite[random];
        //이미지 위치, 크기 선정
        itemImg.transform.localPosition = new Vector2(-5, 125);
        itemImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        itemImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
    }
    void AchievementCheck() //업적 연동
    {
        int helmetCount = 0, topCount = 0, bottomsCount = 0, weaponCount = 0;
        for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++)
        {
            if (DemoDataManager.Instance.allClothesItemList[i].type == "헬멧" && DemoDataManager.Instance.allClothesItemList[i].count > 0) //헬멧 개수 확인
                helmetCount++;
            if (DemoDataManager.Instance.allClothesItemList[i].type == "상의" && DemoDataManager.Instance.allClothesItemList[i].count > 0) //상의 개수 확인
                topCount++;
            if (DemoDataManager.Instance.allClothesItemList[i].type == "하의" && DemoDataManager.Instance.allClothesItemList[i].count > 0) //하의 개수 확인
                bottomsCount++;
        }
        for (int i = 0; i < DemoDataManager.Instance.allWeaponItemList.Count; i++)
        {
            if (DemoDataManager.Instance.allWeaponItemList[i].count > 0) //무기 개수 확인
                weaponCount++;
        }
        //아이템 개수 관련 업적과 연동
        DemoDataManager.Instance.achievementDataList[3].progressvalue = helmetCount;
        DemoDataManager.Instance.achievementDataList[4].progressvalue = topCount;
        DemoDataManager.Instance.achievementDataList[5].progressvalue = bottomsCount;
        DemoDataManager.Instance.achievementDataList[6].progressvalue = weaponCount;
    }
    public class Stat //아이템 스탯
    {
        public string name;
        public int number;
        public Stat(string _name, int _number) { name = _name; number = _number; }
    }
}
