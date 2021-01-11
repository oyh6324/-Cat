using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxRandom : MonoBehaviour
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

    int random;
    int randomPercent;

    void OnEnable()
    {
        //뽑기창 생성
        messageImg.gameObject.SetActive(false);
        boxonImg.gameObject.SetActive(true);

        if (PlayerPrefs.HasKey("멸치의상뽑기"))
        {
            random = Random.Range(0, 20);
            ClothesBoxRandom();
            PlayerPrefs.DeleteKey("멸치의상뽑기");
        }
        if (PlayerPrefs.HasKey("진주의상뽑기"))
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 40) //3등급
                random = Random.Range(13, 20);
            else if (randomPercent < 80) //4등급
                random = Random.Range(20, 26);
            else if (randomPercent < 100) //5등급
                random = Random.Range(26, 31);
            ClothesBoxRandom();
            PlayerPrefs.DeleteKey("진주의상뽑기");
        }
        if (PlayerPrefs.HasKey("멸치무기뽑기"))
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 34) //1등급
                random = Random.Range(0, 2);
            else if (randomPercent < 66) //2등급
                random = Random.Range(2, 5);
            else if (randomPercent < 100) //3등급
                random = Random.Range(5, 10);
            WeaponBoxRandom();
            PlayerPrefs.DeleteKey("멸치무기뽑기");
        }
        if (PlayerPrefs.HasKey("진주무기뽑기"))
        {
            randomPercent = Random.Range(0, 100);
            if (randomPercent < 40) //3등급
                random = Random.Range(5, 10);
            else if (randomPercent < 80) //4등급
                random = Random.Range(10, 15);
            else if (randomPercent < 100) //5등급
                random = Random.Range(15, 17);
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
        boxonImg.gameObject.SetActive(false);
        messageImg.gameObject.SetActive(true);
        messageCanvas.gameObject.SetActive(false);
    }
    void AchievementCheck()
    {
        int helmetCount = 0, topCount = 0, bottomsCount = 0, weaponCount = 0;
        for(int i=0; i<DataManager.allClothesItemList.Count; i++)
        {
            if (DataManager.allClothesItemList[i].type == "헬멧" && DataManager.allClothesItemList[i].count > 0)
                helmetCount++;
            if (DataManager.allClothesItemList[i].type == "상의" && DataManager.allClothesItemList[i].count > 0)
                topCount++;
            if (DataManager.allClothesItemList[i].type == "하의" && DataManager.allClothesItemList[i].count > 0)
                bottomsCount++;
        }
        for(int i=0; i<DataManager.allWeaponItemList.Count; i++)
        {
            if (DataManager.allWeaponItemList[i].count > 0)
                weaponCount++;
        }
        DataManager.achievementDataList[3].progressvalue = helmetCount;
        DataManager.achievementDataList[4].progressvalue = topCount;
        DataManager.achievementDataList[5].progressvalue = bottomsCount;
        DataManager.achievementDataList[6].progressvalue = weaponCount;
    }
    void ClothesBoxRandom()
    {
        //설명에 필요한 정보 옮겨담기
        List<Stat> statList = new List<Stat>();
        string clothesName = DataManager.allClothesItemList[random].name;
        int clothesLevel = DataManager.allClothesItemList[random].level;
        string clothesEx = DataManager.allClothesItemList[random].ex;
        int clothesStar = DataManager.allClothesItemList[random].star;
        decimal clothesSetcrip = DataManager.allClothesItemList[random].setcrip;

        statList.Add(new Stat("STR", DataManager.allClothesItemList[random].str));
        statList.Add(new Stat("DEF", DataManager.allClothesItemList[random].def));
        statList.Add(new Stat("AGI", DataManager.allClothesItemList[random].agi));

        //세트 착용 시 추가 스탯
        statList.Add(new Stat("STR", DataManager.allClothesItemList[random].setstr));
        statList.Add(new Stat("DEF", DataManager.allClothesItemList[random].setdef));
        statList.Add(new Stat("AGI", DataManager.allClothesItemList[random].setagi));

        DataManager.allClothesItemList[random].count += 1; //보유 개수 추가

        for (int i = 0; i < clothesStar; i++) //등급 표시
            classTx.text += "★";

        levelnameTx.text = "Lv." + clothesLevel + " " + clothesName; //레벨과 이름표시
        exTx.text = clothesEx; //설명 표시

        for (int i = 0; i < 3; i++) //스탯 효과 표시
        {
            if (statList[i].number != 0)
                statTx.text += statList[i].name + " +" + statList[i].number+"\n";
        }
        if (DataManager.allClothesItemList[random].setname != "") //세트 효과 표시
        {
            setTx.text = "세트 적용 시\n";
            for (int i = 3; i < 6; i++)
            {
                if (statList[i].number != 0)
                    setTx.text += statList[i].name + " +" + statList[i].number + "  ";
            }
            if (clothesSetcrip != 0)
                setTx.text += "크리티컬 확률 +" + clothesSetcrip + "%";
        }
        if (DataManager.allClothesItemList[random].count == 1)
            DataManager.allClothesItemList[random].isnew = true;
    }
    void WeaponBoxRandom()
    {
        //정보 옮겨담기
        string weaponName = DataManager.allWeaponItemList[random].name;
        int weaponLevel = DataManager.allWeaponItemList[random].level;
        int weaponStar = DataManager.allWeaponItemList[random].star;
        string weaponEX = DataManager.allWeaponItemList[random].ex;
        int weaponStr = DataManager.allWeaponItemList[random].str;
        decimal weaponSpeed = DataManager.allWeaponItemList[random].strspeed;
        decimal weaponCrip = DataManager.allWeaponItemList[random].crip;

        DataManager.allWeaponItemList[random].count += 1;

        for (int i = 0; i < weaponStar; i++) //등급 표시
            classTx.text += "★";

        levelnameTx.text= "Lv." + weaponLevel + " " + weaponName; //레벨과 이름표시
        exTx.text = weaponEX; //설명 표시
        statTx.text = "STR +" + weaponStr + "\nSPEED "+ weaponSpeed +"\n크티리컬 확률 +"+ weaponCrip+"%"; //스탯 효과 표시

        if (weaponName == "버블건")
            setTx.text = "세트 적용 시\nSTR +10 AGI +10 CRI +1%";
        if (weaponName=="레이저건")
            setTx.text = "세트 적용 시\nSTR +10 DEF +10 CRI +2%";
        if(weaponName=="천사링")
            setTx.text = "세트 적용 시\nDEF +30 AGI +20 CRI +2%";
        if(weaponName=="악마삼지창")
            setTx.text = "세트 적용 시\nSTR +10 AGI +30 CRI +2.5%";

        if (DataManager.allWeaponItemList[random].count == 1)
            DataManager.allWeaponItemList[random].isnew = true;
    }
    public class Stat
    {
        public string name;
        public int number;
        public Stat(string _name, int _number) { name = _name; number = _number; }
    }
}
