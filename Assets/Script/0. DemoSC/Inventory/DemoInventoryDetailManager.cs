using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoInventoryDetailManager : MonoBehaviour
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

    //오디오
    public AudioSource soundEffectAS;
    public AudioClip buttonClickClip;

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
        for(int i=0; i<DemoDataManager.Instance.allClothesItemList.Count; i++) //아이템 이미지 씌우기
        {
            if (DemoDataManager.Instance.allClothesItemList[i].name == itemName)
                itemImg.sprite = itemSprite[i];
        }
    }
    public void BackBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        detailblindImg.gameObject.SetActive(false);
    }
    public void UpgradeBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        itemCount -= sumCount;
        int preItemStr = 0, preItemDef = 0, preItemAgi = 0;
        for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++)
        {
            if (DemoDataManager.Instance.allClothesItemList[i].name.Equals(itemName))
            {
                preItemStr = itemStr; preItemDef = itemDef; preItemAgi = itemAgi;
                itemStr = itemStr / itemLevel * (itemLevel + 1);
                itemDef = itemDef / itemLevel * (itemLevel + 1);
                itemAgi = itemAgi / itemLevel * (itemLevel + 1);

                DemoDataManager.Instance.allClothesItemList[i].str = itemStr;
                DemoDataManager.Instance.allClothesItemList[i].def = itemDef;
                DemoDataManager.Instance.allClothesItemList[i].agi = itemAgi;
                DemoDataManager.Instance.allClothesItemList[i].level += 1;

                if (preItemStr != 0)
                    DemoDataManager.Instance.characterDatasList[0].itemstr += itemStr - preItemStr;
                if (preItemDef != 0)
                    DemoDataManager.Instance.characterDatasList[0].itemdef += itemDef - preItemDef;
                if (preItemAgi != 0)
                    DemoDataManager.Instance.characterDatasList[0].itemagi += itemAgi - preItemAgi;

                if (itemCount == 0)
                    DemoDataManager.Instance.allClothesItemList[i].count = 1;
                else if (itemCount == 1)
                    DemoDataManager.Instance.allClothesItemList[i].count = 2;
                else
                    DemoDataManager.Instance.allClothesItemList[i].count = itemCount;
            }
        }
        PlayerPrefs.SetInt("아이템정보변경", 1);
        TextSetting();
        AchievementCheck(); //업적 연동
    }
    void AchievementCheck()
    {
        int count3 = 0, count4 = 0, count5 = 0;
        for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++)
        {
            if (DemoDataManager.Instance.allClothesItemList[i].level >= 3)
                count3++;
            if (DemoDataManager.Instance.allClothesItemList[i].level >= 4)
                count4++;
            if (DemoDataManager.Instance.allClothesItemList[i].level == 5)
                count5++;
        }
        DemoDataManager.Instance.achievementDataList[7].progressvalue = count3;
        DemoDataManager.Instance.achievementDataList[8].progressvalue = count4;
        DemoDataManager.Instance.achievementDataList[9].progressvalue = count5;
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
        if (DemoInventoryPageManager.isInvenBtClick[0]) //헬멧 아이템 열 때
        {
            itemName = DemoDataManager.Instance.characterDatasList[0].helmet;
            itemDataSave(itemName);
        }
        if (DemoInventoryPageManager.isInvenBtClick[1]) //상의 열 때
        {
            itemName = DemoDataManager.Instance.characterDatasList[0].top;
            itemDataSave(itemName);
        }
        if (DemoInventoryPageManager.isInvenBtClick[2]) //하의 열 때
        {
            itemName = DemoDataManager.Instance.characterDatasList[0].bottoms;
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
            setNameCountTx.text = itemSetName + " 세트 "; //세트 세팅
            int count = 0;
            for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++) //세트를 다 모았는지
            {
                if (DemoDataManager.Instance.allClothesItemList[i].setname.Equals(itemSetName) && DemoDataManager.Instance.allClothesItemList[i].count > 0)
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
        for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++)
        {
            if (DemoDataManager.Instance.allClothesItemList[i].name.Equals(itemname))
            {
                itemClass = DemoDataManager.Instance.allClothesItemList[i].star;
                itemEx = DemoDataManager.Instance.allClothesItemList[i].ex;
                itemLevel = DemoDataManager.Instance.allClothesItemList[i].level;
                itemStr = DemoDataManager.Instance.allClothesItemList[i].str;
                itemDef = DemoDataManager.Instance.allClothesItemList[i].def;
                itemAgi = DemoDataManager.Instance.allClothesItemList[i].agi;
                itemCount = DemoDataManager.Instance.allClothesItemList[i].count;
                itemSetName = DemoDataManager.Instance.allClothesItemList[i].setname;
                itemSetStr = DemoDataManager.Instance.allClothesItemList[i].setstr;
                itemSetDef = DemoDataManager.Instance.allClothesItemList[i].setdef;
                itemSetAgi = DemoDataManager.Instance.allClothesItemList[i].setagi;
                itemSetCrip = DemoDataManager.Instance.allClothesItemList[i].setcrip;
                itemSetCount = DemoDataManager.Instance.allClothesItemList[i].setnumber;
            }
        }
    }
}
