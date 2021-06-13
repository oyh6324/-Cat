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
        for (int i = 0; i < DemoDataManager.Instance.achievementDataList.Count; i++) //업적 알람
        {
            if (DemoDataManager.Instance.achievementDataList[i].progressvalue >= DemoDataManager.Instance.achievementDataList[i].value &&
                DemoDataManager.Instance.achievementDataList[i].maxlevel > DemoDataManager.Instance.achievementDataList[i].level)
                achieveAlarmImg.gameObject.SetActive(true);
            if (DemoDataManager.Instance.achievementDataList[i].progressvalue < DemoDataManager.Instance.achievementDataList[i].value ||
                DemoDataManager.Instance.achievementDataList[i].maxlevel == DemoDataManager.Instance.achievementDataList[i].level)
                achieveCount++;
        }
        if (achieveCount == DemoDataManager.Instance.achievementDataList.Count)
            achieveAlarmImg.gameObject.SetActive(false);

        for (int i = 0; i < DemoDataManager.Instance.allWeaponItemList.Count; i++) //무기고 알람
        {
            if (DemoDataManager.Instance.allWeaponItemList[i].isnew)
                weaponAlarmImg.gameObject.SetActive(true);
            else
                weaponCount++;
        }
        if (weaponCount == DemoDataManager.Instance.allWeaponItemList.Count)
            weaponAlarmImg.gameObject.SetActive(false);

        for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++) //인벤토리 알람
        {
            if (DemoDataManager.Instance.allClothesItemList[i].isnew)
                inventoryAlarmImg.gameObject.SetActive(true);
            else
                invenCount++;
        }
        if (invenCount == DemoDataManager.Instance.allClothesItemList.Count)
            inventoryAlarmImg.gameObject.SetActive(false);

        for (int i = 0; i < DemoDataManager.Instance.monsterCollectionDataList.Count; i++) //도감 알람
        {
            if (DemoDataManager.Instance.monsterCollectionDataList[i].stage <= DemoDataManager.Instance.characterDatasList[0].stage && DemoDataManager.Instance.monsterCollectionDataList[i].isreward == false)
                collectionAlarmImg.gameObject.SetActive(true);
            else if (DemoDataManager.Instance.monsterCollectionDataList[i].stage <= DemoDataManager.Instance.characterDatasList[0].stage && DemoDataManager.Instance.monsterCollectionDataList[i].isreward)
                collectionRewardCount++;
            if (DemoDataManager.Instance.monsterCollectionDataList[i].stage <= DemoDataManager.Instance.characterDatasList[0].stage)
                collectionCount++;
        }
        if (collectionCount == collectionRewardCount)
            collectionAlarmImg.gameObject.SetActive(false);

    }
}
