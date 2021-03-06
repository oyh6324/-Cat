using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DemoTopManager : MonoBehaviour
{
    //0:lobby 1:shop 2:inventory 3:Weapon 4:Achievement 5:collection 6: stageSelect
    public Canvas[] onCanvas;

    //멸치, 진주, 열쇠의 개수
    public Text anchovyCount;
    public Text pearlCount;
    public Text keyCount;

    //옵션 창
    public Image optionBlind;
    public Text backgroundOnOff;
    public Text effectOnOff;

    //열쇠 타이머
    private float keyTime;
    public Text keyTimeTx;

    //게임종료 창
    public Canvas messageCanvas;
    public Text messageTx;

    //오디오
    public AudioSource soundEffectAS;
    public AudioSource bgmAS;
    public AudioClip buttonClickClip;

    public static bool isLobby;

    void Update()
    {
        //상단의 멸치, 진주, 열쇠 개수 카운트
        anchovyCount.text = DemoDataManager.moneyItemList[0].count.ToString();
        pearlCount.text = DemoDataManager.moneyItemList[1].count.ToString();
        keyCount.text = DemoDataManager.moneyItemList[2].count.ToString();

        if (DemoDataManager.moneyItemList[2].count < 5) //열쇠 타이머
        {
            keyTime -= Time.deltaTime;
            int minute = (int)keyTime / 60;
            int second = (int)keyTime % 60;
            keyTimeTx.text = minute + "분 " + second + "초";
            if ((int)keyTime == 0)
            {
                DemoDataManager.moneyItemList[2].count += 1;
                keyTime = 900;
            }
        }
        else
        {
            keyTimeTx.text = "";
            keyTime = 900;
        }
        //안드로이드 뒤로가기
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (isLobby)
                {
                    messageCanvas.gameObject.SetActive(true);
                    messageTx.text = "게임을 종료하시겠습니까?";
                }
            }
        }
    }
    void OnEnable()
    {
        if (PlayerPrefs.HasKey("효과음제거"))
        {
            soundEffectAS.volume = 0;
            effectOnOff.text = "Off";
        }
        else
        {
            soundEffectAS.volume = 1;
            effectOnOff.text = "On";
        }

        if (PlayerPrefs.HasKey("배경음제거"))
        {
            bgmAS.volume = 0;
            backgroundOnOff.text = "Off";
        }
        else
        {
            bgmAS.volume = 0.3f;
            backgroundOnOff.text = "On";
        }
    }
    void OnDestroy()
    {
        if (DemoDataManager.moneyItemList[2].count < 5)
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
            if (DemoDataManager.moneyItemList[2].count < 5)
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
    public void BackBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (isLobby)
        {
            messageCanvas.gameObject.SetActive(true);
            messageTx.text = "게임을 종료하시겠습니까?";
        }
        else
        {
            OnCanvas(0); //로비를 제외한 캔버스 off
            isLobby = true;
        }
    }
    public void AnchovyShopBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        DirectShop("멸치상점");
    }
    public void PearlShopBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        DirectShop("진주상점");
    }
    public void KeyShopBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        DirectShop("열쇠상점");
    }
    public void OptionBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        optionBlind.gameObject.SetActive(true);
    }
    //option 영역
    public void BackgroundBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (PlayerPrefs.HasKey("배경음제거")) //배경음 킴
        {
            bgmAS.volume = 0.3f;
            backgroundOnOff.text = "On";
            PlayerPrefs.DeleteKey("배경음제거");
        }
        else //배경음 제거함
        {
            bgmAS.volume = 0;
            backgroundOnOff.text = "Off";
            PlayerPrefs.SetInt("배경음제거", 1);
        }
    }
    public void EffectdBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (PlayerPrefs.HasKey("효과음제거")) //효과음 킴
        {
            soundEffectAS.volume = 1;
            effectOnOff.text = "On";
            PlayerPrefs.DeleteKey("효과음제거");
        }
        else //효과음 제거함
        {
            soundEffectAS.volume = 0;
            effectOnOff.text = "Off";
            PlayerPrefs.SetInt("효과음제거", 1);
        }
    }
    public void OptionBackBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        optionBlind.gameObject.SetActive(false);
    }
    void KeyTimeSet()
    {
        if (PlayerPrefs.HasKey("게임 종료 시간") && PlayerPrefs.HasKey("열쇠 타이머"))
        {
            DateTime outGameTime = Convert.ToDateTime(PlayerPrefs.GetString("게임 종료 시간")); //직전의 게임 종료 시간
            DateTime inGameTime = System.DateTime.Now; //지금 시간
            TimeSpan forKeyTime = inGameTime - outGameTime;
            float totalSeconds = (float)forKeyTime.TotalSeconds;
            if (totalSeconds <= PlayerPrefs.GetFloat("열쇠 타이머"))
                keyTime = PlayerPrefs.GetFloat("열쇠 타이머") - totalSeconds;
            else
            {
                float tempTime = totalSeconds - PlayerPrefs.GetFloat("열쇠 타이머");
                DemoDataManager.moneyItemList[2].count += ((int)tempTime / 900) + 1;
                if (DemoDataManager.moneyItemList[2].count > 5)
                    DemoDataManager.moneyItemList[2].count = 5;
                keyTime = 900 - (tempTime) % 900;
            }
            PlayerPrefs.DeleteKey("게임 종료 시간");
            PlayerPrefs.DeleteKey("열쇠 타이머");
        }
        else
            keyTime = 900;
    }
    void DirectShop(string directName)
    {
        PlayerPrefs.SetInt(directName, 1);
        OnCanvas(1); //상점 열기
        isLobby = false;
    }
    void OnCanvas(int oncanvasnum) //하나를 제외한 나머지 장소 캔버스 없애기
    {
        for (int i = 0; i < onCanvas.Length; i++)
        {
            if (i == oncanvasnum)
                onCanvas[i].gameObject.SetActive(true);
            else
                onCanvas[i].gameObject.SetActive(false);
        }
    }
}
