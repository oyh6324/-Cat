using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    public Sprite[] itemSprite;
    public Image detailblindImg;
    public Image itemImg;
    public Text classTx;
    public Text exTx;
    public Text levelnameTx;
    public Text statTx;
    public Text setStatTx;
    public Text itemCountTx;
    public Text setNameCountTx;

    public Image upgradeBtBlind;
    public Text upgradeTx;

    //아이템 정보
    private string itemName;
    private int itemClass;
    private string itemEx;
    private int itemLevel;
    private int itemStr;
    private int itemDef;
    private int itemAgi;
    private int itemCount;
    private string itemSetName;
    private int itemSetStr;
    private int itemSetDef;
    private int itemSetAgi;
    private decimal itemSetCrip;

    private int itemSetCount;
    private int sumCount;

    void OnEnable()
    {
        TextSetting();
        for (int i = 0; i < DemoDataManager.allClothesItemList.Count; i++) //아이템 이미지 씌우기
        {
            if (DemoDataManager.allClothesItemList[i].name == itemName)
                itemImg.sprite = itemSprite[i];
        }
    }
    public void BackBtClick()
    {
        detailblindImg.gameObject.SetActive(false);
    }
    public void UpgradeBtClick()
    {
        itemCount -= sumCount;
        int preItemStr=0, preItemDef=0, preItemAgi=0;
        for(int i=0; i< DataManager.allClothesItemList.Count; i++)
        {
            if (DataManager.allClothesItemList[i].name.Equals(itemName))
            {
                preItemStr = itemStr; preItemDef = itemDef; preItemAgi = itemAgi;
                itemStr = itemStr / itemLevel * (itemLevel + 1);
                itemDef = itemDef / itemLevel * (itemLevel + 1);
                itemAgi = itemAgi / itemLevel * (itemLevel + 1);

                DataManager.allClothesItemList[i].str = itemStr;
                DataManager.allClothesItemList[i].def = itemDef;
                DataManager.allClothesItemList[i].agi = itemAgi;
                DataManager.allClothesItemList[i].level += 1;

                if(preItemStr!=0)
                    DataManager.characterDatasList[0].itemstr += itemStr-preItemStr;
                if(preItemDef!=0)
                    DataManager.characterDatasList[0].itemdef += itemDef-preItemDef;
                if(preItemAgi!=0)
                    DataManager.characterDatasList[0].itemagi += itemAgi- preItemAgi;

                if (itemCount == 0)
                    DataManager.allClothesItemList[i].count = 1;
                else if(itemCount==1)
                    DataManager.allClothesItemList[i].count = 2;
                else
                    DataManager.allClothesItemList[i].count = itemCount;
            }
        }
        PlayerPrefs.SetInt("아이템정보변경", 1);
        TextSetting();
        AchievementCheck(); //업적 연동
    }
    void AchievementCheck()
    {
        int count3 = 0, count4 = 0, count5 = 0;
        for(int i=0; i<DataManager.allClothesItemList.Count; i++)
        {
            if (DataManager.allClothesItemList[i].level >= 3)
                count3++;
            if (DataManager.allClothesItemList[i].level >= 4)
                count4++;
            if (DataManager.allClothesItemList[i].level == 5)
                count5++;
        }
        DataManager.achievementDataList[7].progressvalue = count3;
        DataManager.achievementDataList[8].progressvalue = count4;
        DataManager.achievementDataList[9].progressvalue = count5;
    }
    void TextSetting()
    {
        classTx.text = "";
        exTx.text = "";
        levelnameTx.text = "";
        statTx.text = "";
        setStatTx.text = "";
        itemCountTx.text = "";
        setNameCountTx.text = "";
        upgradeTx.text = "";
        upgradeBtBlind.gameObject.SetActive(true);
        if (InventoryPageChange.isInvenBtClick[0]) //헬멧 아이템 열 때
        {
            itemName = DataManager.characterDatasList[0].helmet;
            itemDataSave(itemName);
        }
        if (InventoryPageChange.isInvenBtClick[1]) //상의 열 때
        {
            itemName = DataManager.characterDatasList[0].top;
            itemDataSave(itemName);
        }
        if (InventoryPageChange.isInvenBtClick[2]) //하의 열 때
        {
            itemName = DataManager.characterDatasList[0].bottoms;
            itemDataSave(itemName);
        }
        //글 세팅
        for (int i = 0; i < itemClass; i++)
            classTx.text += "★";
        levelnameTx.text = "Lv." + itemLevel + " " + itemName;
        exTx.text = itemEx;
        itemCountTx.text = "보유 개수: " + itemCount;
        if (itemStr != 0)
            statTx.text += "STR +" + itemStr + "\n";
        if (itemDef != 0)
            statTx.text += "DEF +" + itemDef + "\n";
        if (itemAgi != 0)
            statTx.text += "AGI +" + itemAgi;
        if (itemSetName != "")
        {
            setStatTx.text = "세트 적용 시\n";
            if (itemSetStr != 0)
                setStatTx.text += "STR +" + itemSetStr + "\n";
            if (itemSetDef != 0)
                setStatTx.text += "DEF +" + itemSetDef + "\n";
            if (itemSetAgi != 0)
                setStatTx.text += "AGI +" + itemSetAgi + "\n";
            if (itemSetCrip != 0)
            {
                setStatTx.text += "크리티컬 확률 +" + itemSetCrip + "%";
            }

            int count = 0;
            int uniNumber = 0; int fishNumber = 0; int angelNumber = 0; int deveilNumber = 0;
            for(int i=0; i<17; i++)
            {
                if (DataManager.allWeaponItemList[i].name == "레이저건")
                    uniNumber = i;
                if (DataManager.allWeaponItemList[i].name == "버블건")
                    fishNumber = i;
                if (DataManager.allWeaponItemList[i].name == "천사링")
                    angelNumber = i;
                if (DataManager.allWeaponItemList[i].name == "악마삼지창")
                    deveilNumber = i;
            }
            if (itemSetName == "우주" && DataManager.allWeaponItemList[uniNumber].count > 0)
                count++;
            if (itemSetName == "인어" && DataManager.allWeaponItemList[fishNumber].count > 0)
                count++;
            if (itemSetName == "천사" && DataManager.allWeaponItemList[angelNumber].count > 0)
                count++;
            if (itemSetName == "악마" && DataManager.allWeaponItemList[deveilNumber].count > 0)
                count++;
            setNameCountTx.text = itemSetName + " 세트 ";
            for (int i = 0; i < DataManager.allClothesItemList.Count; i++)
            {
                if (DataManager.allClothesItemList[i].setname.Equals(itemSetName) && DataManager.allClothesItemList[i].count > 0)
                    count++;
            }
            setNameCountTx.text += count + "/" + itemSetCount;
        }
        //업그레이드 세팅
        int k = 0;
        for (int i = 1; i <= itemLevel + 1; i++)
        {
            if (i > 1)
            {
                sumCount = i + k;
                k = sumCount;
            }
        }
        if (itemLevel != 5)
        {
            if (itemCount < sumCount)
                upgradeTx.text = "다음 업그레이드까지 " + itemCount + "/" + sumCount;
            else
                upgradeBtBlind.gameObject.SetActive(false);
        }
        else
        {
            upgradeBtBlind.gameObject.SetActive(true);
            upgradeTx.text = "업그레이드가 종료되었습니다.";
        }
    }
    void itemDataSave(string itemname)
    {
        for (int i = 0; i < DataManager.allClothesItemList.Count; i++)
        {
            if (DataManager.allClothesItemList[i].name.Equals(itemname))
            {
                itemClass = DataManager.allClothesItemList[i].star;
                itemEx = DataManager.allClothesItemList[i].ex;
                itemLevel = DataManager.allClothesItemList[i].level;
                itemStr = DataManager.allClothesItemList[i].str;
                itemDef = DataManager.allClothesItemList[i].def;
                itemAgi = DataManager.allClothesItemList[i].agi;
                itemCount = DataManager.allClothesItemList[i].count;
                itemSetName = DataManager.allClothesItemList[i].setname;
                itemSetStr = DataManager.allClothesItemList[i].setstr;
                itemSetDef = DataManager.allClothesItemList[i].setdef;
                itemSetAgi = DataManager.allClothesItemList[i].setagi;
                itemSetCrip = DataManager.allClothesItemList[i].setcrip;
                itemSetCount = DataManager.allClothesItemList[i].setnumber;
            }
        }
    }
}
