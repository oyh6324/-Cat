using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject lobbyCanvas; //로비 화면
    public GameObject barCanvas; //상단 바
    public GameObject tutorialCanvas; //튜토리얼 화면
    public GameObject stageSelectCanvas; //스테이지 선택 화면

    public Image background; //배경 화면 이미지
    public Image inputName; //이름 메시지 창
    public Image fadeImg; //페이드 인/아웃 이미지

    public Text nameTx; //이름 출력

    //메시지
    public Image messageImg;
    public Text messageTx;

    //오디오
    public AudioSource bgmAS;
    public AudioSource effectAS;
    public AudioClip startBgm;
    public AudioClip startBt;
    public AudioClip clickClip;

    private FadeIO fadeIO; //페이드 인/아웃 코드
    private bool tutorial; //튜토리얼 확인
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("returnCat")) //스테이지에서 뒤로가기 한 상태라면
        {
            PlayerPrefs.DeleteKey("returnCat");
            this.gameObject.SetActive(false); //스타트 화면 비활성화
            stageSelectCanvas.SetActive(true); //스테이지 선택 화면 활성화
            barCanvas.SetActive(true); //상단 바 활성화
        }
    }
    private void Start()
    {
        //음향
        if (PlayerPrefs.HasKey("효과음제거")) //효과음 제거가 된 상태라면
            effectAS.volume = 0f; //볼륨 0
        else //효과음 제거가 아니라면
            effectAS.volume = 1f; //볼륨 1

        if (PlayerPrefs.HasKey("배경음제거")) //배경음 제거가 된 상태라면
            bgmAS.volume = 0f; //볼륨 0
        else //배경음 제거가 아니라면
            bgmAS.volume = 0.3f; //볼륨 0.3

        bgmAS.clip = startBgm; //게임 시작 배경음
        bgmAS.Play();
    }
    public void StartBtClick() //시작 버튼 클릭
    {
        fadeIO = fadeImg.GetComponent<FadeIO>();
        StartCoroutine(waitForFadeIO());
    }
    public void OkBtClick() //확인 버튼 클릭
    {
        effectAS.clip = clickClip;
        effectAS.Play();
        messageImg.gameObject.SetActive(true); //메시지 활성화
        messageTx.text = nameTx.text + ", 이 이름이 맞습니까?";
    }
    //메시지 버튼
    public void YesBtClick()
    {
        effectAS.clip = clickClip;
        effectAS.Play();

        DemoDataManager.Instance.characterDatasList[0].name = nameTx.text; //유저 이름 데이터에 저장

        tutorial = true; //튜토리얼 실행
        StartCoroutine(waitForFadeIO());
    }
    public void NoBtClick()
    {
        effectAS.clip = clickClip;
        effectAS.Play();

        messageImg.gameObject.SetActive(false); //메시지 비활성화
    }
    IEnumerator waitForFadeIO() //fadeImg를 페이드 인/아웃
    {
        fadeImg.gameObject.SetActive(true);
        fadeIO.fadeout = true; //페이드 아웃 시작
        yield return new WaitForSeconds(fadeIO.fadeTime); //일정 시간 기다리기
        if (DemoDataManager.Instance.characterDatasList[0].name == "dmlduddlekduddlqkqh") //유저 이름이 초기 이름이라면
        {
            background.gameObject.SetActive(false);
            inputName.gameObject.SetActive(true); //이름 입력 활성화
        }
        else //유저 이름이 초기 이름이 아니라면
        {
            //로비로 이동
            this.gameObject.SetActive(false);
            lobbyCanvas.SetActive(true);
            barCanvas.SetActive(true);

            if (tutorial) //튜토리얼 신호
                tutorialCanvas.SetActive(true); //튜토리얼 활성화
        }
        fadeIO.fadein = true; //페이드 인 시작
    }
}
