using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
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
        for (int i=0; i<DataManager.achievementDataList.Count; i++) //업적 알람
        {
            if (DataManager.achievementDataList[i].progressvalue >= DataManager.achievementDataList[i].value &&
                DataManager.achievementDataList[i].maxlevel != DataManager.achievementDataList[i].level)
                achieveAlarmImg.gameObject.SetActive(true);
            if (DataManager.achievementDataList[i].progressvalue < DataManager.achievementDataList[i].value)
                achieveCount++;
        }
        if (achieveCount == DataManager.achievementDataList.Count)
            achieveAlarmImg.gameObject.SetActive(false);

        for(int i=0; i<DataManager.allWeaponItemList.Count; i++) //무기고 알람
        {
            if (DataManager.allWeaponItemList[i].isnew)
                weaponAlarmImg.gameObject.SetActive(true);
            else
                weaponCount++;
        }
        if (weaponCount == DataManager.allWeaponItemList.Count)
            weaponAlarmImg.gameObject.SetActive(false);

        for(int i=0; i<DataManager.allClothesItemList.Count; i++) //인벤토리 알람
        {
            if (DataManager.allClothesItemList[i].isnew)
                inventoryAlarmImg.gameObject.SetActive(true);
            else
                invenCount++;
        }
        if (invenCount == DataManager.allClothesItemList.Count)
            inventoryAlarmImg.gameObject.SetActive(false);

        for(int i=0; i<DataManager.monsterCollectionDataList.Count; i++) //도감 알람
        {
            if (DataManager.monsterCollectionDataList[i].stage <= DataManager.characterDatasList[0].stage && DataManager.monsterCollectionDataList[i].isreward == false)
                collectionAlarmImg.gameObject.SetActive(true);
            else if (DataManager.monsterCollectionDataList[i].stage <= DataManager.characterDatasList[0].stage && DataManager.monsterCollectionDataList[i].isreward)
                collectionRewardCount++;
            if (DataManager.monsterCollectionDataList[i].stage <= DataManager.characterDatasList[0].stage)
                collectionCount++;
        }
        if (collectionCount == collectionRewardCount)
            collectionAlarmImg.gameObject.SetActive(false);

    }
}
