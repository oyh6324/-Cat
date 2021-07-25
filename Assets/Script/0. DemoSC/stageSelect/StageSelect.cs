using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Image[] locks; //스테이지 잠금 이미지
    public Image[] flags; //스테이지 클리어 깃발 이미지

    private int stageNumber; //스테이지 숫자

    private void OnEnable()
    {
        soundEffectAS.PlayOneShot(divingClip);

        StageFlagOn();
    }
    private void Update()
    {
        if (!messageCanvas.gameObject.activeSelf)
            stageTitleTx.gameObject.SetActive(false);
    }
    //스테이지 버튼
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
    private void StageCilck() //스테이지 클릭 시
    {
        //메시지 활성화
        messageCanvas.gameObject.SetActive(true);
        stageMessage.gameObject.SetActive(true);
        stageTitleTx.gameObject.SetActive(true);
        messageTx.text = "열쇠를 사용하시겠습니까?";
        stageTitleTx.text = "Stage " + stageNumber;

        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
    }
    //메시지 버튼
    public void YesBtClick() //예 버튼 클릭
    {
        UseKey(); //열쇠 사용
    }
    public void NoBtClick() //아니오 버튼 클릭
    {
        //메시지 비활성화
        stageMessage.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }
    public void OkBtClick() //확인 버튼 클릭
    {
        //메시지 비활성화
        okBt.SetActive(false);
        yesNobts.SetActive(true);
        stageMessage.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }
    private void UseKey()
    {
        if (DemoDataManager.Instance.characterDatasList[0].weapon == "") //아무것도 착용 하지 않았을 시
        { //기본 무기 장착(에어건)
            DemoDataManager.Instance.characterDatasList[0].weapon = DemoDataManager.Instance.allWeaponItemList[0].name;
            DemoDataManager.Instance.characterDatasList[0].itemstr += DemoDataManager.Instance.allWeaponItemList[0].str;
            DemoDataManager.Instance.characterDatasList[0].itemspeed += DemoDataManager.Instance.allWeaponItemList[0].strspeed;
            DemoDataManager.Instance.characterDatasList[0].itemcrip += DemoDataManager.Instance.allWeaponItemList[0].crip;

            DemoDataManager.Instance.characterDatasList[0].allstr = DemoDataManager.Instance.characterDatasList[0].str + DemoDataManager.Instance.characterDatasList[0].itemstr;
            DemoDataManager.Instance.characterDatasList[0].alldef = DemoDataManager.Instance.characterDatasList[0].def + DemoDataManager.Instance.characterDatasList[0].itemdef;
            DemoDataManager.Instance.characterDatasList[0].allagi = DemoDataManager.Instance.characterDatasList[0].agi + DemoDataManager.Instance.characterDatasList[0].itemagi;
            DemoDataManager.Instance.characterDatasList[0].allcrip = DemoDataManager.Instance.characterDatasList[0].crip + DemoDataManager.Instance.characterDatasList[0].itemcrip;
        }

        if (DemoDataManager.Instance.moneyItemList[2].count > 0) //열쇠가 있다면
        {
            DemoDataManager.Instance.moneyItemList[2].count--; //열쇠 소모
            if (stageNumber > DemoDataManager.Instance.characterDatasList[0].stage) //새로운 스테이지라면
                DemoDataManager.Instance.characterDatasList[0].stage += 1; //캐릭터가 입장한 스테이지 증가

            //메시지 비활성화
            stageMessage.gameObject.SetActive(false);
            messageCanvas.gameObject.SetActive(false);

            //스테이지 숫자 신호 보낸 후 씬 로드
            PlayerPrefs.SetInt("stageNumber", stageNumber);
            SceneManager.LoadScene("Stage");
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
    private void StageFlagOn() //스테이지 클리어 확인 함수
    {
        if (PlayerPrefs.HasKey("stage clear")==false) return;
        //클리어 했다면
        for(int i=0; i<PlayerPrefs.GetInt("stage clear"); i++)
        {
            flags[i].gameObject.SetActive(true); //깃발 활성화
            locks[i + 1].gameObject.SetActive(false); //클리어한 스테이지 +1 잠금 비활성화
        }
    }
}
