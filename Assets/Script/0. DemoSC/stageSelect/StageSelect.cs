using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    //오디오
    public AudioSource soundEffectAS;
    public AudioClip divingClip;
    public AudioClip buttonClickClip;

    //메세지
    public Canvas messageCanvas;
    public Image stageMessage;
    public Text messageTx;
    public Text stageTitleTx;
    public GameObject yesNobts;
    public GameObject okBt;

    public Image[] locks;
    public Image[] flags;

    private int stageNumber;

    private void OnEnable()
    {
        soundEffectAS.clip = divingClip;
        soundEffectAS.Play();

        StageFlagOn();
    }
    private void Update()
    {
        if (!messageCanvas.gameObject.activeSelf)
            stageTitleTx.gameObject.SetActive(false);
    }
    public void Stage1Click()
    {
        stageNumber = 1;
        StageCilck();
    }
    public void Stage2Click()
    {
        stageNumber = 2;
        StageCilck();
    }
    public void Stage3Click()
    {
        stageNumber = 3;
        StageCilck();
    }
    public void Stage4Click()
    {
        stageNumber = 4;
        StageCilck();
    }
    public void Stage5Click()
    {
        stageNumber = 5;
        StageCilck();
    }
    public void Stage6Click()
    {
        stageNumber = 6;
        StageCilck();
    }
    public void Stage7Click()
    {
        stageNumber = 7;
        StageCilck();
    }
    public void Stage8Click()
    {
        stageNumber = 8;
        StageCilck();
    }
    public void Stage9Click()
    {
        stageNumber = 9;
        StageCilck();
    }
    public void Stage10Click()
    {
        stageNumber = 10;
        StageCilck();
    }
    public void Stage11Click()
    {
        stageNumber = 11;
        StageCilck();
    }
    public void Stage12Click()
    {
        stageNumber = 12;
        StageCilck();
    }
    public void Stage13Click()
    {
        stageNumber = 13;
        StageCilck();
    }
    public void Stage14Click()
    {
        stageNumber = 14;
        StageCilck();
    }
    public void Stage15Click()
    {
        stageNumber = 15;
        StageCilck();
    }
    public void YesBtClick()
    {
        UseKey();
    }
    public void NoBtClick()
    {
        stageMessage.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }
    public void OkBtClick()
    {
        okBt.SetActive(false);
        yesNobts.SetActive(true);
        stageMessage.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }
    private void StageCilck()
    {
        messageCanvas.gameObject.SetActive(true);
        stageMessage.gameObject.SetActive(true);
        stageTitleTx.gameObject.SetActive(true);
        messageTx.text = "열쇠를 사용하시겠습니까?";
        stageTitleTx.text = "Stage " + stageNumber;

        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
    }
    private void UseKey()
    {
        if (DemoDataManager.characterDatasList[0].weapon == "") //아무것도 착용 하지 않았을 시
        { //기본 무기 장착
            DemoDataManager.characterDatasList[0].weapon = DemoDataManager.allWeaponItemList[0].name;
            DemoDataManager.characterDatasList[0].itemstr += DemoDataManager.allWeaponItemList[0].str;
            DemoDataManager.characterDatasList[0].itemspeed += DemoDataManager.allWeaponItemList[0].strspeed;
            DemoDataManager.characterDatasList[0].itemcrip += DemoDataManager.allWeaponItemList[0].crip;

            DemoDataManager.characterDatasList[0].allstr = DemoDataManager.characterDatasList[0].str + DemoDataManager.characterDatasList[0].itemstr;
            DemoDataManager.characterDatasList[0].alldef = DemoDataManager.characterDatasList[0].def + DemoDataManager.characterDatasList[0].itemdef;
            DemoDataManager.characterDatasList[0].allagi = DemoDataManager.characterDatasList[0].agi + DemoDataManager.characterDatasList[0].itemagi;
            DemoDataManager.characterDatasList[0].allcrip = DemoDataManager.characterDatasList[0].crip + DemoDataManager.characterDatasList[0].itemcrip;
        }

        if (DemoDataManager.moneyItemList[2].count > 0) //열쇠가 있다면
        {
            DemoDataManager.moneyItemList[2].count--; //열쇠 소모
            if (stageNumber > DemoDataManager.characterDatasList[0].stage) //새로운 스테이지라면
                DemoDataManager.characterDatasList[0].stage += 1; //캐릭터가 입장한 스테이지 증가

            stageMessage.gameObject.SetActive(false);
            messageCanvas.gameObject.SetActive(false);

            //해당 스테이지로 이동 코드
            //stageNumber로 몇 번째 스테이지인지 알 수 있음
        }
        else //열쇠 없을 때
        {
            messageTx.text = "열쇠가 부족해요!";
            stageTitleTx.gameObject.SetActive(false);
            okBt.SetActive(true);
            yesNobts.SetActive(false);
        }

        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
    }
    private void StageFlagOn()
    {
        if (PlayerPrefs.HasKey("stage clear")==false) return;
        for(int i=0; i<PlayerPrefs.GetInt("stage clear"); i++)
        {
            flags[i].gameObject.SetActive(true);
            locks[i + 1].gameObject.SetActive(false);
        }
    }
}
