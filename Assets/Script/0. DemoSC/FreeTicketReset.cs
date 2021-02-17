using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FreeTicketReset : MonoBehaviour
{
    private void Start()
    {
        if (DemoDataManager.moneyItemList[3].count <= 0)
        {
            if (System.DateTime.Now.ToString("dd") != PlayerPrefs.GetString("이용권용 종료 시간")) //게임 들어왔을 때 날짜가 다르면 충전
                DemoDataManager.moneyItemList[3].count++;
        }
        if (DemoDataManager.moneyItemList[5].count <= 0)
        {
            if (System.DateTime.Now.ToString("dd") != PlayerPrefs.GetString("이용권용 종료 시간"))
                DemoDataManager.moneyItemList[5].count++;
        }
    }
    private void Update()
    {
        if (DemoDataManager.moneyItemList[3].count <= 0) //멸치 의상 쿠폰
        {
            if (System.DateTime.Now.ToString("hhss") == "0000") //게임 도중 오전 12시가 되면 충전
            {
                DemoDataManager.moneyItemList[3].count++;
            }
        }
        if (DemoDataManager.moneyItemList[5].count <= 0)
        {
            if (System.DateTime.Now.ToString("hhss") == "0000")
            { 
                DemoDataManager.moneyItemList[5].count++;
            }
        }
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetString("이용권용 종료 시간", System.DateTime.Now.ToString("dd"));
    }
    void OnApplicationPause(bool pause) //true일 때 게임 벗어난 상태
    {
        if (pause)
        {
            PlayerPrefs.SetString("이용권용 종료 시간", System.DateTime.Now.ToString("dd"));
        }
    }
}
