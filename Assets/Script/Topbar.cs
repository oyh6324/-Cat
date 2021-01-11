using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Topbar : MonoBehaviour
{
    //0:lobby 1:shop 2:inventory 3:Weapon 4:Achievement 5:collection
    public Canvas[] onCanvas;

    //멸치, 진주, 열쇠의 개수
    public Text anchovyCount;
    public Text pearlCount;
    public Text keyCount;

    public Image optionBlind;
    public Text backgroundOnOff;
    public Text effectOnOff;

    //열쇠 타이머
    private float keyTime;
    public Text keyTimeTx;

    void Update()
    {
        anchovyCount.text = DataManager.moneyItemList[0].count.ToString();
        pearlCount.text = DataManager.moneyItemList[1].count.ToString();
        keyCount.text = DataManager.moneyItemList[2].count.ToString();

        if (DataManager.moneyItemList[2].count < 5)
        {
            keyTime -= Time.deltaTime;
            int minute = (int)keyTime / 60;
            int second = (int)keyTime % 60;
            keyTimeTx.text = minute + "분 " + second + "초";
            if ((int)keyTime == 0)
            {
                DataManager.moneyItemList[2].count += 1;
                keyTime = 15;
            }
        }
        else
        {
            keyTimeTx.text = "";
            keyTime = 15;
        }
    }
    void OnEnable()
    {
        if (PlayerPrefs.HasKey("효과음제거"))
            effectOnOff.text = "Off";
        else
            effectOnOff.text = "On";

        if (PlayerPrefs.HasKey("배경음제거"))
            backgroundOnOff.text = "Off";
        else
            backgroundOnOff.text = "On";
    }
    void OnDestroy()
    {
        if (DataManager.moneyItemList[2].count < 5)
        {
            PlayerPrefs.SetString("게임 종료 시간", System.DateTime.Now.ToString());
            PlayerPrefs.SetFloat("열쇠 타이머", keyTime);
        }
        else
        {
            PlayerPrefs.DeleteKey("게임 종료 시간");
            PlayerPrefs.DeleteKey("열쇠 타이머");
        }
    }
    void OnApplicationPause(bool pause) //true일 때 게임 벗어난 상태
    {
        if (pause)
        {
            if (DataManager.moneyItemList[2].count < 5)
            {
                PlayerPrefs.SetString("게임 종료 시간", System.DateTime.Now.ToString());
                PlayerPrefs.SetFloat("열쇠 타이머", keyTime);
            }
            else
            {
                PlayerPrefs.DeleteKey("게임 종료 시간");
                PlayerPrefs.DeleteKey("열쇠 타이머");
            }
        }
        else //다시 돌아왔을 때
            KeyTimeSet();
    }
    void KeyTimeSet()
    {
        if (PlayerPrefs.HasKey("게임 종료 시간") && PlayerPrefs.HasKey("열쇠 타이머"))
        {
            DateTime outGameTime = Convert.ToDateTime(PlayerPrefs.GetString("게임 종료 시간"));
            DateTime inGameTime = System.DateTime.Now;
            TimeSpan forKeyTime = inGameTime - outGameTime;
            float totalSeconds = (float)forKeyTime.TotalSeconds;
            if (totalSeconds <= PlayerPrefs.GetFloat("열쇠 타이머"))
                keyTime = PlayerPrefs.GetFloat("열쇠 타이머") - totalSeconds;
            else
            {
                float tempTime = totalSeconds - PlayerPrefs.GetFloat("열쇠 타이머");
                DataManager.moneyItemList[2].count += ((int)tempTime / 15) + 1;
                if (DataManager.moneyItemList[2].count > 5)
                    DataManager.moneyItemList[2].count = 5;
                keyTime = 15 - (tempTime) % 15;
            }
            PlayerPrefs.DeleteKey("게임 종료 시간");
            PlayerPrefs.DeleteKey("열쇠 타이머");
        }
        else
            keyTime = 15;
    }
    void DirectShop(string directName)
    {
        PlayerPrefs.SetInt(directName, 1);
        OnCanvas(1); //상점 열기
    }
    void OnCanvas(int oncanvasnum) //하나를 제외한 나머지 장소 캔버스 없애기
    {
        for(int i=0; i<onCanvas.Length; i++)
        {
            if (i == oncanvasnum)
                onCanvas[i].gameObject.SetActive(true);
            else
                onCanvas[i].gameObject.SetActive(false);
        }
    }
    public void BackBtClick()
    {
        OnCanvas(0); //로비를 제외한 캔버스 off
    }
    public void AnchovyShopBtClick()
    {
        DirectShop("멸치상점");
    }
    public void PearlShopBtClick()
    {
        DirectShop("진주상점");
    }
    public void KeyShopBtClick()
    {
        DirectShop("열쇠상점");
    }
    public void OptionBtClick()
    {
        optionBlind.gameObject.SetActive(true);
    }

    //option 영역
    public void BackgroundBtClick()
    {
        if (PlayerPrefs.HasKey("배경음제거"))
        {
            PlayerPrefs.DeleteKey("배경음제거");
            backgroundOnOff.text = "On";
        }
        else
        {
            PlayerPrefs.SetInt("배경음제거", 1);
            backgroundOnOff.text = "Off";
        }
    }
    public void EffectdBtClick()
    {
        if (PlayerPrefs.HasKey("효과음제거"))
        {
            PlayerPrefs.DeleteKey("효과음제거");
            effectOnOff.text = "On";
        }
        else
        {
            PlayerPrefs.SetInt("효과음제거", 1);
            effectOnOff.text = "Off";
        }
    }
    public void OptionBackBtClick()
    {
        optionBlind.gameObject.SetActive(false);
    }
}
