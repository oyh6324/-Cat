using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoAlarmManager : MonoBehaviour
{
    public Image achieveAlarmImg; //업적 new 이미지
    public Image inventoryAlarmImg; //인벤토리 new 이미지
    public Image weaponAlarmImg; //무기고 new 이미지
    public Image collectionAlarmImg; //도감 new 이미지

    void OnEnable()
    {
        int achieveCount = 0;
        int weaponCount = 0;
        int invenCount = 0;
        int collectionRewardCount = 0;
        int collectionCount = 0;

        for (int i = 0; i < DemoDataManager.Instance.achievementDataList.Count; i++) //업적 알람
        {
            //달성한 업적이 있을 때 알람 이미지 open
            if (DemoDataManager.Instance.achievementDataList[i].progressvalue >= DemoDataManager.Instance.achievementDataList[i].value &&
                DemoDataManager.Instance.achievementDataList[i].maxlevel > DemoDataManager.Instance.achievementDataList[i].level)
                achieveAlarmImg.gameObject.SetActive(true);
            if (DemoDataManager.Instance.achievementDataList[i].progressvalue < DemoDataManager.Instance.achievementDataList[i].value ||
                DemoDataManager.Instance.achievementDataList[i].maxlevel == DemoDataManager.Instance.achievementDataList[i].level)
                achieveCount++; //달성하지 않거나 최종 달성한 업적 개수
        }
        if (achieveCount == DemoDataManager.Instance.achievementDataList.Count) //달성하지 않은 업적 개수가 총 업적 개수와 같다면
            achieveAlarmImg.gameObject.SetActive(false); //알람 이미지 비활성화

        for (int i = 0; i < DemoDataManager.Instance.allWeaponItemList.Count; i++) //무기고 알람
        {
            if (DemoDataManager.Instance.allWeaponItemList[i].isnew) //새로 얻은 무기가 있을 때
                weaponAlarmImg.gameObject.SetActive(true); //알람 이미지 활성화
            else //새로 얻은 무기가 없을 때
                weaponCount++;
        }
        if (weaponCount == DemoDataManager.Instance.allWeaponItemList.Count) //모든 무기가 새로운 무기가 아닐 때
            weaponAlarmImg.gameObject.SetActive(false); //알람 이미지 비활성화

        for (int i = 0; i < DemoDataManager.Instance.allClothesItemList.Count; i++) //인벤토리 알람
        {
            if (DemoDataManager.Instance.allClothesItemList[i].isnew) //새로 얻은 장비가 있을 때
                inventoryAlarmImg.gameObject.SetActive(true); //알람 이미지 활성화
            else //새로 얻은 장비가 없을 때
                invenCount++;
        }
        if (invenCount == DemoDataManager.Instance.allClothesItemList.Count) //모든 장비가 새로운 장비가 아닐 때
            inventoryAlarmImg.gameObject.SetActive(false); //알람 이미지 비활성화

        for (int i = 0; i < DemoDataManager.Instance.monsterCollectionDataList.Count; i++) //도감 알람
        {
            if (DemoDataManager.Instance.monsterCollectionDataList[i].stage <= DemoDataManager.Instance.characterDatasList[0].stage && DemoDataManager.Instance.monsterCollectionDataList[i].isreward == false) //새로운 스테이지 발견과 보상을 받지 않았을 때
                collectionAlarmImg.gameObject.SetActive(true); //알람 이미지 활성화
            else if (DemoDataManager.Instance.monsterCollectionDataList[i].stage <= DemoDataManager.Instance.characterDatasList[0].stage && DemoDataManager.Instance.monsterCollectionDataList[i].isreward) //받을 보상이 없을 때
                collectionRewardCount++;
            if (DemoDataManager.Instance.monsterCollectionDataList[i].stage <= DemoDataManager.Instance.characterDatasList[0].stage) //새로운 스테이지가 없을 때
                collectionCount++;
        }
        if (collectionCount == collectionRewardCount) //받을 보상과 새로운 스테이지가 없을 때
            collectionAlarmImg.gameObject.SetActive(false); //알람 이미지 비활성화

    }
}
