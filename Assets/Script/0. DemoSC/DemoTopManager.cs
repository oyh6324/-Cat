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
        anchovyCount.text = DemoDataManager.Instance.moneyItemList[0].count.ToString();
        pearlCount.text = DemoDataManager.Instance.moneyItemList[1].count.ToString();
        keyCount.text = DemoDataManager.Instance.moneyItemList[2].count.ToString();
        
        //열쇠 타이머 시간 재기
        if (DemoDataManager.Instance.moneyItemList[2].count < 5) //열쇠 개수가 5 밑이면
        {
            keyTime -= Time.deltaTime; //시간에 따라 감소
            int minute = (int)keyTime / 60; //분
            int second = (int)keyTime % 60; //초
            keyTimeTx.text = minute + "분 " + second + "초"; //열쇠 타이머 출력
            if ((int)keyTime == 0) //시간이 다 지나면
            {
                DemoDataManager.Instance.moneyItemList[2].count += 1; //열쇠 개수 증가
                keyTime = 900; 
            }
        }
        else //열쇠 개수가 5이상이면
        {
            keyTimeTx.text = ""; //열쇠 타이머 비활성화
            keyTime = 900;
        }
        
        //안드로이드 뒤로가기 버튼
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (isLobby) //로비에 있다면
                {
                    messageCanvas.gameObject.SetActive(true);
                    messageTx.text = "게임을 종료하시겠습니까?";
                }
            }
        }
    }
    void OnEnable()
    {
        KeyTimeSet(); //열쇠 타이머 시간 세팅

        //음향 제어
        if (PlayerPrefs.HasKey("효과음제거")) //효과음 제거 신호
        {
            soundEffectAS.volume = 0;
            effectOnOff.text = "Off";
        }
        else
        {
            soundEffectAS.volume = 1;
            effectOnOff.text = "On";
        }
        if (PlayerPrefs.HasKey("배경음제거")) //배경음 제거 신호
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
    void OnDestroy() //게임 종료 시
    {
        if (Convert.ToInt32(keyCount.text) < 5) //열쇠 개수가 5 이하
        {
            //게임 종료 시간 저장
            PlayerPrefs.SetString("게임 종료 시간", System.DateTime.Now.ToString());
            PlayerPrefs.SetFloat("열쇠 타이머", keyTime);
        }
        else //열쇠 개수가 5 이상
        {
            PlayerPrefs.DeleteKey("게임 종료 시간");
            PlayerPrefs.DeleteKey("열쇠 타이머");
        }
    }
    void OnApplicationPause(bool pause) //게임 포커스 벗어났다면
    {
        if (pause)
        {
            //게임 종료 시간 저장
            if (Convert.ToInt32(keyCount.text) < 5)
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
    public void BackBtClick() //뒤로 가기 버튼 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (isLobby) //로비에 있다면
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
    public void AnchovyShopBtClick() //상단 바 멸치 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        DirectShop("멸치상점"); //상점 이동
    }
    public void PearlShopBtClick() //상단 바 진주 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        DirectShop("진주상점");
    }
    public void KeyShopBtClick() //상단 바 열쇠 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        DirectShop("열쇠상점");
    }
    public void OptionBtClick() //옵션 버튼 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        optionBlind.gameObject.SetActive(true); //옵션 창 활성화
    }
    //option 영역
    public void BackgroundBtClick() //배경음 버튼 클릭
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
    public void EffectdBtClick() //효과음 버튼 클릭
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
    public void OptionBackBtClick() //옵션 뒤로가기 버튼 클릭
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        optionBlind.gameObject.SetActive(false); //옵션 창 비활성화
    }
    void KeyTimeSet() //열쇠 타이머 시간 지정
    {
        if (PlayerPrefs.HasKey("게임 종료 시간") && PlayerPrefs.HasKey("열쇠 타이머")) //타이머 세다가 게임 종료 했을 때
        {
            DateTime outGameTime = Convert.ToDateTime(PlayerPrefs.GetString("게임 종료 시간")); //직전의 게임 종료 시간
            DateTime inGameTime = System.DateTime.Now; //지금 시간
            TimeSpan forKeyTime = inGameTime - outGameTime; //재야 하는 시간
            float totalSeconds = (float)forKeyTime.TotalSeconds;
            if (totalSeconds <= PlayerPrefs.GetFloat("열쇠 타이머")) //타이머가 0이라면
                keyTime = PlayerPrefs.GetFloat("열쇠 타이머") - totalSeconds;
            else //타이머가 0이 아니라면
            {
                float tempTime = totalSeconds - PlayerPrefs.GetFloat("열쇠 타이머");
                DemoDataManager.Instance.moneyItemList[2].count += ((int)tempTime / 900) + 1; //열쇠 개수 증가
                if (DemoDataManager.Instance.moneyItemList[2].count > 5) //열쇠가 5개 넘었다면
                    DemoDataManager.Instance.moneyItemList[2].count = 5; //열쇠 개수 5개로 고정
                keyTime = 900 - (tempTime) % 900; //남은 타이머 시간
            }
            PlayerPrefs.DeleteKey("게임 종료 시간");
            PlayerPrefs.DeleteKey("열쇠 타이머");
        }
        else
            keyTime = 900;
    }
    void DirectShop(string directName) //상점 이동
    {
        PlayerPrefs.SetInt(directName, 1); //상점 페이지 신호
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
