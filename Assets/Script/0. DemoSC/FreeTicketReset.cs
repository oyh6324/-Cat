using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FreeTicketReset : MonoBehaviour
{
    string getTime = "0000"; //무료이용권 얻는 시간

    private void Start()
    {
        if (DemoDataManager.Instance.moneyItemList[3].count <= 0) //무료이용권이 없다면
        {
            if (System.DateTime.Now.ToString("dd") != PlayerPrefs.GetString("이용권용 종료 시간")) //게임 들어왔을 때 날짜가 다르면 충전
                DemoDataManager.Instance.moneyItemList[3].count++; //멸치 의상
        }
        if (DemoDataManager.Instance.moneyItemList[5].count <= 0)
        {
            if (System.DateTime.Now.ToString("dd") != PlayerPrefs.GetString("이용권용 종료 시간"))
                DemoDataManager.Instance.moneyItemList[5].count++; //멸치 무기
        }
    }
    private void Update()
    {
        if (DemoDataManager.Instance.moneyItemList[3].count <= 0)
        {
            if (System.DateTime.Now.ToString("hhmm") == getTime) //게임 도중 충전 시간이 되면 충전
            {
                DemoDataManager.Instance.moneyItemList[3].count++; //멸치 의상
            }
        }
        if (DemoDataManager.Instance.moneyItemList[5].count <= 0)
        {
            if (System.DateTime.Now.ToString("hhmm") == getTime)
            { 
                DemoDataManager.Instance.moneyItemList[5].count++; //멸치 무기
            }
        }
    }
    private void OnDestroy() //게임 종료 시
    {
        //게임 종료 시간 저장
        PlayerPrefs.SetString("이용권용 종료 시간", System.DateTime.Now.ToString("dd"));
    }
    void OnApplicationPause(bool pause) //true일 때 게임 벗어난 상태
    {
        if (pause)
        {
            //게임 종료 시간 저장
            PlayerPrefs.SetString("이용권용 종료 시간", System.DateTime.Now.ToString("dd"));
        }
    }
}
