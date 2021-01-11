using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoAlarmManager : MonoBehaviour
{
    public Image achieveAlarmImg;
    public Image inventoryAlarmImg;
    public Image weaponAlarmImg;
    public Image collectionAlarmImg;

    void OnEnable()
    {
        int achieveCount = 0;
        int weaponCount = 0;
        int invenCount = 0;
        int collectionRewardCount = 0;
        int collectionCount = 0;
        for (int i = 0; i < DemoDataManager.achievementDataList.Count; i++) //업적 알람
        {
            if (DemoDataManager.achievementDataList[i].progressvalue >= DemoDataManager.achievementDataList[i].value &&
                DemoDataManager.achievementDataList[i].maxlevel != DemoDataManager.achievementDataList[i].level)
                achieveAlarmImg.gameObject.SetActive(true);
            if (DemoDataManager.achievementDataList[i].progressvalue < DemoDataManager.achievementDataList[i].value)
                achieveCount++;
        }
        if (achieveCount == DemoDataManager.achievementDataList.Count)
            achieveAlarmImg.gameObject.SetActive(false);

        for (int i = 0; i < DemoDataManager.allWeaponItemList.Count; i++) //무기고 알람
        {
            if (DemoDataManager.allWeaponItemList[i].isnew)
                weaponAlarmImg.gameObject.SetActive(true);
            else
                weaponCount++;
        }
        if (weaponCount == DemoDataManager.allWeaponItemList.Count)
            weaponAlarmImg.gameObject.SetActive(false);

        for (int i = 0; i < DemoDataManager.allClothesItemList.Count; i++) //인벤토리 알람
        {
            if (DemoDataManager.allClothesItemList[i].isnew)
                inventoryAlarmImg.gameObject.SetActive(true);
            else
                invenCount++;
        }
        if (invenCount == DemoDataManager.allClothesItemList.Count)
            inventoryAlarmImg.gameObject.SetActive(false);

        for (int i = 0; i < DemoDataManager.monsterCollectionDataList.Count; i++) //도감 알람
        {
            if (DemoDataManager.monsterCollectionDataList[i].stage <= DemoDataManager.characterDatasList[0].stage && DemoDataManager.monsterCollectionDataList[i].isreward == false)
                collectionAlarmImg.gameObject.SetActive(true);
            else if (DemoDataManager.monsterCollectionDataList[i].stage <= DemoDataManager.characterDatasList[0].stage && DemoDataManager.monsterCollectionDataList[i].isreward)
                collectionRewardCount++;
            if (DemoDataManager.monsterCollectionDataList[i].stage <= DemoDataManager.characterDatasList[0].stage)
                collectionCount++;
        }
        if (collectionCount == collectionRewardCount)
            collectionAlarmImg.gameObject.SetActive(false);

    }
}
