using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;

public class DemoClothesItemData
{
    public DemoClothesItemData(string _name, string _setname, string _ex, string _type, int _level, int _count, int _star, int _str, int _def, int _agi,
        int _setstr, int _setdef, int _setagi, int _setnumber, decimal _setcrip, bool _isnew)
    {
        name = _name; level = _level; count = _count; star = _star; str = _str; def = _def; agi = _agi; ex = _ex;
        setname = _setname; setstr = _setstr; setdef = _setdef; setagi = _setagi; setcrip = _setcrip; type = _type;
        setnumber = _setnumber; isnew = _isnew;
    }
    public string name, setname, ex, type;
    public int level, count, star, str, def, agi, setstr, setdef, setagi, setnumber;
    public decimal setcrip;
    public bool isnew;
}

public class DemoWeaponItemData
{
    public DemoWeaponItemData(string _name, string _ex, int _level, int _count, int _star, int _str, decimal _strspeed, decimal _crip, bool _isnew)
    {
        name = _name; level = _level; count = _count; star = _star; str = _str; strspeed = _strspeed; crip = _crip; ex = _ex; isnew = _isnew;
    }
    public string name, ex;
    public int level, count, star, str;
    public decimal strspeed, crip;
    public bool isnew;
}

public class DemoMoneyItemData
{
    public DemoMoneyItemData(string _name, int _count)
    {
        name = _name; count = _count;
    }
    public string name;
    public int count;
}
public class DemoCharacterData
{
    public DemoCharacterData(string _name, string _helmet, string _top, string _bottoms, string _weapon, string _setname, int _level, int _str, int _def, int _agi, int _hp,
        int _itemstr, int _itemdef, int _itemagi, int _allstr, int _alldef, int _allagi, int _setstr, int _setdef, int _setagi, int _stage, int _exp, int _totalexp, 
        decimal _crip, decimal _itemcrip, decimal _allcrip, decimal _setcrip, decimal _itemspeed)
    {
        name = _name; level = _level; str = _str; def = _def; agi = _agi; crip = _crip; hp = _hp;
        helmet = _helmet; top = _top; bottoms = _bottoms; weapon = _weapon;
        itemstr = _itemstr; itemdef = _itemdef; itemagi = _itemagi; itemcrip = _itemcrip; itemspeed = _itemspeed;
        allstr = _allstr; alldef = _alldef; allagi = _allagi; allcrip = _allcrip;
        setstr = _setstr; setdef = _setdef; setagi = _setagi; setcrip = _setcrip; setname = _setname;
        stage = _stage; exp = _exp; totalexp = _totalexp;
    }
    public string name, helmet, top, bottoms, weapon, setname;
    public int level, str, def, agi, hp, itemstr, itemdef, itemagi, allstr, alldef, allagi, setstr, setdef, setagi, stage, exp, totalexp;
    public decimal crip, itemcrip, allcrip, setcrip, itemspeed;
}
public class DemoAchievementData
{
    public DemoAchievementData(string _name, string _rewardname, string _rewardname2, int _level, int _maxlevel, int _progressvalue, int _rewardcount, int _rewradcount2, int _value)
    {
        name = _name; value = _value; rewardname = _rewardname; rewardname2 = _rewardname2; rewardcount = _rewardcount; rewardcount2 = _rewradcount2;
        maxlevel = _maxlevel; level = _level; progressvalue = _progressvalue;
    }
    public string name, rewardname, rewardname2;
    public int value, rewardcount, rewardcount2, maxlevel, level, progressvalue;
}
public class DemoMonsterCollectionData
{
    public DemoMonsterCollectionData(string _name, string _realname, string _habitat, string _kind, int _stage, bool _isreward)
    {
        name = _name; realname = _realname; habitat = _habitat; kind = _kind; stage = _stage; isreward = _isreward;
    }
    public string name, realname, habitat, kind;
    public int stage;
    public bool isreward;
}
public class DemoDataManager : MonoBehaviour
{
    public static List<DemoWeaponItemData> allWeaponItemList;
    public static List<DemoClothesItemData> allClothesItemList;
    public static List<DemoCharacterData> characterDatasList;
    public static List<DemoAchievementData> achievementDataList;
    public static List<DemoMonsterCollectionData> monsterCollectionDataList;
    //0: anchovy 1:pearl 2:key 3:anchovyboxClothes 4:pearlboxClothes 5:anchovyboxWeapon 6:pearlboxWeapon 
    public static List<DemoMoneyItemData> moneyItemList;

    void Awake()
    {
        if (PlayerPrefs.HasKey("first"))
            LoadAllData();
        else
        {
            PlayerPrefs.SetInt("first", 1);
            SaveFirstData();
        }
    }
    void OnApplicationPause(bool pause) //홈버튼 눌렀을 때
    {
        if (pause)
            SaveAllData();
    }
    void OnDestroy() //게임을 나갔을 때
    {
        SaveAllData();
    }
    void SaveFirstData() //게임을 처음 시작할 때 실행해야 함
    {
        allClothesItemList = new List<DemoClothesItemData>(); //의상
        allWeaponItemList = new List<DemoWeaponItemData>(); //무기
        characterDatasList = new List<DemoCharacterData>(); //캐릭터 스탯
        moneyItemList = new List<DemoMoneyItemData>(); //돈
        achievementDataList = new List<DemoAchievementData>(); //업적
        monsterCollectionDataList = new List<DemoMonsterCollectionData>(); //컬렉션

        //리소스 파일의 텍스트 형식의 의상 데이터 부르기
        TextAsset clothesDataText = Resources.Load("DemoClothesItemDataText", typeof(TextAsset)) as TextAsset;
        string[] lineClothes = clothesDataText.text.Substring(0, clothesDataText.text.Length - 1).Split('\n');
        for (int i = 0; i < lineClothes.Length; i++)
        {
            lineClothes[i] = lineClothes[i].Trim();
            string[] row = lineClothes[i].Split('\t');
            int[] toInt = new int[10];
            for (int j = 4; j < 14; j++)
                toInt[j - 4] = Convert.ToInt32(row[j]);
            decimal toDecimal = Convert.ToDecimal(row[14]);
            bool isnew = false;
            allClothesItemList.Add(new DemoClothesItemData(row[0], row[1], row[2], row[3], toInt[0], toInt[1], toInt[2], toInt[3], toInt[4], toInt[5], toInt[6], toInt[7], toInt[8], toInt[9], toDecimal, isnew));
        }
        //리소스 파일의 텍스트 형식의 무기 데이터 부르기
        TextAsset weaponDataText = Resources.Load("DemoWeaponItemDataText", typeof(TextAsset)) as TextAsset;
        string[] lineWeapon = weaponDataText.text.Substring(0, weaponDataText.text.Length - 1).Split('\n');
        for (int i = 0; i < lineWeapon.Length; i++)
        {
            lineWeapon[i] = lineWeapon[i].Trim();
            string[] row = lineWeapon[i].Split('\t');
            int[] toInt = new int[4];
            for (int j = 2; j < 6; j++)
                toInt[j - 2] = Convert.ToInt32(row[j]);
            decimal[] toDecimal = new decimal[2];
            toDecimal[0] = Convert.ToDecimal(row[6]);
            toDecimal[1] = Convert.ToDecimal(row[7]);
            bool isNew = false;
            allWeaponItemList.Add(new DemoWeaponItemData(row[0], row[1], toInt[0], toInt[1], toInt[2], toInt[3], toDecimal[0], toDecimal[1], isNew));

        }
        //리소스 파일의 텍스트 형식의 업적 데이터 부르기
        TextAsset achievementDataText = Resources.Load("AchievementDataText", typeof(TextAsset)) as TextAsset;
        string[] lineAchievement = achievementDataText.text.Substring(0, achievementDataText.text.Length - 1).Split('\n');
        for (int i = 0; i < lineAchievement.Length; i++)
        {
            lineAchievement[i] = lineAchievement[i].Trim();
            string[] row = lineAchievement[i].Split('\t');
            int[] toInt = new int[6];
            for (int j = 3; j < 9; j++)
                toInt[j - 3] = Convert.ToInt32(row[j]);
            achievementDataList.Add(new DemoAchievementData(row[0], row[1], row[2], toInt[0], toInt[1], toInt[2], toInt[3], toInt[4], toInt[5]));

        }
        //리소스 파일의 텍스트 형식의 컬렉션 데이터 부르기
        TextAsset monsterCollectionDataText = Resources.Load("MonsterCollectionDataText", typeof(TextAsset)) as TextAsset;
        string[] linecollection = monsterCollectionDataText.text.Substring(0, monsterCollectionDataText.text.Length - 1).Split('\n');
        for (int i = 0; i < linecollection.Length; i++)
        {
            linecollection[i] = linecollection[i].Trim();
            string[] row = linecollection[i].Split('\t');
            int stage = Convert.ToInt32(row[4]);
            bool isreward = false;
            monsterCollectionDataList.Add(new DemoMonsterCollectionData(row[0], row[1], row[2], row[3], stage, isreward));
        }
        //캐릭터 데이터 추가
        characterDatasList.Add(new DemoCharacterData("dmlduddlekduddlqkqh", "", "", "", "", "", 1, 7, 3, 5, 100, 0, 0, 0, 7, 3, 5, 0, 0, 0, 0, 0, 600, 5, 0, 5, 0, 0));
        //돈, 무료이용권 데이터 추가
        moneyItemList.Add(new DemoMoneyItemData("멸치", 1000000));
        moneyItemList.Add(new DemoMoneyItemData("진주", 10000));
        moneyItemList.Add(new DemoMoneyItemData("열쇠", 5));
        moneyItemList.Add(new DemoMoneyItemData("멸치보석함의상", 1));
        moneyItemList.Add(new DemoMoneyItemData("진주보석함의상", 10));
        moneyItemList.Add(new DemoMoneyItemData("멸치보석함무기", 10));
        moneyItemList.Add(new DemoMoneyItemData("진주보석함무기", 1));

        SaveAllData();
    }
    void SaveAllData() //json 저장
    {
        //clothesItemData
        string jdataC = JsonConvert.SerializeObject(allClothesItemList);
        File.WriteAllText(Application.persistentDataPath + "/ClothesItem.json", jdataC); //모바일 에디터 둘다 구동 가능 //C:\Users\Name\AppData\LocalLow\회사이름

        //weaponItemData
        string jdataW = JsonConvert.SerializeObject(allWeaponItemList);
        File.WriteAllText(Application.persistentDataPath + "/WeaponItem.json", jdataW);

        //moneyItemData
        string jdataM = JsonConvert.SerializeObject(moneyItemList);
        File.WriteAllText(Application.persistentDataPath + "/MoneyItem.json", jdataM);

        //characterData
        string jdataCh = JsonConvert.SerializeObject(characterDatasList);
        File.WriteAllText(Application.persistentDataPath + "/CharacterData.json", jdataCh);

        //achievementData
        string jdataA = JsonConvert.SerializeObject(achievementDataList);
        File.WriteAllText(Application.persistentDataPath + "/AchievementData.json", jdataA);

        //monsterCollectionData
        string jdataMC = JsonConvert.SerializeObject(monsterCollectionDataList);
        File.WriteAllText(Application.persistentDataPath + "/MonsterCollectionData.json", jdataMC);
    }
    void LoadAllData() //json 불러오기
    {
        //clothesItemData
        string jdataC = File.ReadAllText(Application.persistentDataPath + "/ClothesItem.json");
        allClothesItemList = JsonConvert.DeserializeObject<List<DemoClothesItemData>>(jdataC);

        //weaponItemData
        string jdataW = File.ReadAllText(Application.persistentDataPath + "/WeaponItem.json");
        allWeaponItemList = JsonConvert.DeserializeObject<List<DemoWeaponItemData>>(jdataW);

        //moneyItemData
        string jdataM = File.ReadAllText(Application.persistentDataPath + "/MoneyItem.json");
        moneyItemList = JsonConvert.DeserializeObject<List<DemoMoneyItemData>>(jdataM);

        //characterData
        string jdataCh = File.ReadAllText(Application.persistentDataPath + "/CharacterData.json");
        characterDatasList = JsonConvert.DeserializeObject<List<DemoCharacterData>>(jdataCh);

        //achievementData
        string jdataA = File.ReadAllText(Application.persistentDataPath + "/AchievementData.json");
        achievementDataList = JsonConvert.DeserializeObject<List<DemoAchievementData>>(jdataA);

        //monsterCollectionData
        string jdataMC = File.ReadAllText(Application.persistentDataPath + "/MonsterCollectionData.json");
        monsterCollectionDataList = JsonConvert.DeserializeObject<List<DemoMonsterCollectionData>>(jdataMC);
    }
}
