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

        if (PlayerPrefs.HasKey("멸치의상뽑기"))
        {
            random = Random.Range(0, 7);
            ClothesBoxRandom();
            PlayerPrefs.DeleteKey("멸치의상뽑기");
        }
        if (PlayerPrefs.HasKey("진주의상뽑기"))
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 60) //3등급
                random = 6;
            else if (randomPercent < 100) //4등급
                random = Random.Range(6, 9);
            ClothesBoxRandom();
            PlayerPrefs.DeleteKey("진주의상뽑기");
        }
        if (PlayerPrefs.HasKey("멸치무기뽑기"))
        {
            random = Random.Range(0, 4);
            WeaponBoxRandom();
            PlayerPrefs.DeleteKey("멸치무기뽑기");
        }
        if (PlayerPrefs.HasKey("진주무기뽑기"))
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 60) //3등급
                random = Random.Range(2, 4);
            else if (randomPercent < 100) //4등급
                random = 4;
            WeaponBoxRandom();
            PlayerPrefs.DeleteKey("진주무기뽑기");
        }
        AchievementCheck(); //업적 연동
    }
    void OnDisable()
    {
        classTx.text = "";
        levelnameTx.text = "";
        exTx.text = "";
        statTx.text = "";
        setTx.text = "";
    }
    public void OkBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        boxonImg.gameObject.SetActive(false);
        messageImg.gameObject.SetActive(true);
        messageCanvas.gameObject.SetActive(false);
    }
    void ClothesBoxRandom()
    {
        //설명에 필요한 정보 옮겨담기
        List<Stat> statList = new List<Stat>();
        string clothesName = DemoDataManager.allClothesItemList[random].name;
        int clothesLevel = DemoDataManager.allClothesItemList[random].level;
        string clothesEx = DemoDataManager.allClothesItemList[random].ex;
        int clothesStar = DemoDataManager.allClothesItemList[random].star;
        decimal clothesSetcrip = DemoDataManager.allClothesItemList[random].setcrip;

        statList.Add(new Stat("STR", DemoDataManager.allClothesItemList[random].str));
        statList.Add(new Stat("DEF", DemoDataManager.allClothesItemList[random].def));
        statList.Add(new Stat("AGI", DemoDataManager.allClothesItemList[random].agi));

        //세트 착용 시 추가 스탯
        statList.Add(new Stat("STR", DemoDataManager.allClothesItemList[random].setstr));
        statList.Add(new Stat("DEF", DemoDataManager.allClothesItemList[random].setdef));
        statList.Add(new Stat("AGI", DemoDataManager.allClothesItemList[random].setagi));

        DemoDataManager.allClothesItemList[random].count += 1; //보유 개수 추가

        for (int i = 0; i < clothesStar; i++) //등급 표시
            classTx.text += "★";

        levelnameTx.text = "Lv." + clothesLevel + " " + clothesName; //레벨과 이름표시
        exTx.text = clothesEx; //설명 표시

        for (int i = 0; i < 3; i++) //스탯 효과 표시
        {
            if (statList[i].number != 0)
                statTx.text += statList[i].name + " +" + statList[i].number + "\n";
        }
        if (DemoDataManager.allClothesItemList[random].setname != "") //세트 효과 표시
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
        if (DemoDataManager.allClothesItemList[random].count == 1)
            DemoDataManager.allClothesItemList[random].isnew = true;

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
    void WeaponBoxRandom()
    {
        //정보 옮겨담기
        string weaponName = DemoDataManager.allWeaponItemList[random].name;
        int weaponLevel = DemoDataManager.allWeaponItemList[random].level;
        int weaponStar = DemoDataManager.allWeaponItemList[random].star;
        string weaponEX = DemoDataManager.allWeaponItemList[random].ex;
        int weaponStr = DemoDataManager.allWeaponItemList[random].str;
        decimal weaponSpeed = DemoDataManager.allWeaponItemList[random].strspeed;
        decimal weaponCrip = DemoDataManager.allWeaponItemList[random].crip;

        DemoDataManager.allWeaponItemList[random].count += 1;

        for (int i = 0; i < weaponStar; i++) //등급 표시
            classTx.text += "★";

        levelnameTx.text = "Lv." + weaponLevel + " " + weaponName; //레벨과 이름표시
        exTx.text = weaponEX; //설명 표시
        statTx.text = "STR +" + weaponStr + "\nSPEED " + weaponSpeed + "\n크티리컬 확률 +" + weaponCrip + "%"; //스탯 효과 표시

        if (DemoDataManager.allWeaponItemList[random].count == 1)
            DemoDataManager.allWeaponItemList[random].isnew = true;

        //무기 스프라이트
        itemImg.sprite = weaponSprite[random];
        //이미지 위치, 크기 선정
        itemImg.transform.localPosition = new Vector2(-5, 125);
        itemImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        itemImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
    }
    void AchievementCheck()
    {
        int helmetCount = 0, topCount = 0, bottomsCount = 0, weaponCount = 0;
        for (int i = 0; i < DemoDataManager.allClothesItemList.Count; i++)
        {
            if (DemoDataManager.allClothesItemList[i].type == "헬멧" && DemoDataManager.allClothesItemList[i].count > 0)
                helmetCount++;
            if (DemoDataManager.allClothesItemList[i].type == "상의" && DemoDataManager.allClothesItemList[i].count > 0)
                topCount++;
            if (DemoDataManager.allClothesItemList[i].type == "하의" && DemoDataManager.allClothesItemList[i].count > 0)
                bottomsCount++;
        }
        for (int i = 0; i < DemoDataManager.allWeaponItemList.Count; i++)
        {
            if (DemoDataManager.allWeaponItemList[i].count > 0)
                weaponCount++;
        }
        DemoDataManager.achievementDataList[3].progressvalue = helmetCount;
        DemoDataManager.achievementDataList[4].progressvalue = topCount;
        DemoDataManager.achievementDataList[5].progressvalue = bottomsCount;
        DemoDataManager.achievementDataList[6].progressvalue = weaponCount;
    }
    public class Stat
    {
        public string name;
        public int number;
        public Stat(string _name, int _number) { name = _name; number = _number; }
    }
}
