using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StageManager : MonoBehaviour
{
    public GameObject startCanvas;
    public GameObject[] map;
    public GameObject[] stage;

    public GameObject messageCanvas;
    public GameObject clears;
    public Text expTx;

    public GameObject rewards;
    public Text rewardTitle;
    public Image anchovyImg;
    public Text anchovyTx;
    public Image pearlImg;
    public Text pearlTx;
    public Image weaponImg;
    public Sprite 수리검;
    public Sprite 음파건;

    public GameObject fail;
    public Image message;
    public Text messageTx;
    public GameObject yesNo;
    public GameObject ok;

    public GameObject player;

    public AudioMixer audiomixer;
    private AudioSource messageAS;
    public AudioClip buttonClip;
    public AudioClip failClip;
    public AudioClip clearClip;

    public Vector2[] spawn;

    private int stageNumber;
    private bool clearCheck;
    private bool failCheck;
    private bool isHomeBt;
    private void Awake()
    {
        //음향
        if (PlayerPrefs.HasKey("효과음제거"))
            audiomixer.SetFloat("Effect", -80f);
        else
            audiomixer.SetFloat("Effect", 0f);

        if (PlayerPrefs.HasKey("배경음제거"))
            audiomixer.SetFloat("BGM", -80f);
        else
            audiomixer.SetFloat("BGM", 0f);

        stageNumber= PlayerPrefs.GetInt("stageNumber");
        map[(stageNumber-1)/5].SetActive(true); //바탕
        stage[stageNumber - 1].SetActive(true); //스테이지

        Debug.Log("현재 스테이지 "+stageNumber);

        StartCoroutine(startCanvasAction());


        Debug.Log(DemoDataManager.Instance.characterDatasList[0].top);
        Debug.Log(DemoDataManager.Instance.characterDatasList[0].bottoms);
        Debug.Log(DemoDataManager.Instance.characterDatasList[0].helmet);
    }
    private void OnEnable()
    {
        spawnPlayer();
    }
    void Update()
    {
        if(GameObject.FindWithTag("Enemy")==null)
        {
            StageClear();
        }
        if (player.GetComponent<PlayerMove>().HeartCnt < 1)
        {
            StageFail();
        }
    }
    void spawnPlayer()
    {
        player.transform.position = spawn[stageNumber - 1];
    }
    IEnumerator startCanvasAction()
    {
        startCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        startCanvas.SetActive(false);
    }
    void StageFail()
    {
        if (failCheck) return;

        audiomixer.SetFloat("BGM", -80f);
        messageCanvas.SetActive(true);
        messageAS = messageCanvas.GetComponent<AudioSource>();
        messageAS.PlayOneShot(failClip);
        Debug.Log("클리어 실패");
        fail.SetActive(true);
        failCheck = true;
    }
    public void HomeBtClick()
    {
        messageCanvas.SetActive(true);
        messageAS = messageCanvas.GetComponent<AudioSource>();
        messageAS.PlayOneShot(buttonClip);

        message.gameObject.SetActive(true);
        messageTx.text = "게임을 종료하겠습니까? 사용한 열쇠는 돌아오지 않아요!";
        isHomeBt = true;
    }
    public void BackBtClick()
    {
        messageAS.PlayOneShot(buttonClip);
        PlayerPrefs.SetInt("returnCat", 1);
        SceneManager.LoadScene("Cat");
    }
    public void AgainBtClick()
    {
        messageAS.PlayOneShot(buttonClip);
        message.gameObject.SetActive(true);
        messageTx.text = "열쇠를 사용하시겠습니까?";
    }
    public void YesBtClick()
    {
        messageAS.PlayOneShot(buttonClip);

        if (isHomeBt)
        {
            message.gameObject.SetActive(false);
            StageFail();
            isHomeBt = false;
            return;
        }

        if (DemoDataManager.Instance.moneyItemList[2].count<1)
        {
            messageTx.text = "열쇠가 부족해요!";
            yesNo.SetActive(false);
            ok.SetActive(true);
        }
        else
        {
            DemoDataManager.Instance.moneyItemList[2].count--;
            SceneManager.LoadScene("Stage");
        }
    }
    public void NoBtClick()
    {
        messageAS.PlayOneShot(buttonClip);
        message.gameObject.SetActive(false);

        if (isHomeBt)
        {
            messageCanvas.SetActive(false);
            isHomeBt = false;
        }
    }
    public void MokBtClick()
    {
        messageAS.PlayOneShot(buttonClip);
        yesNo.SetActive(true);
        ok.SetActive(false);
        message.gameObject.SetActive(false);
    }
    void StageClear()
    {
        if (clearCheck) return;

        Debug.Log("clear"); // clear 확인 변수값 설정

        audiomixer.SetFloat("BGM", -80f);
        messageCanvas.SetActive(true);
        messageAS = messageCanvas.GetComponent<AudioSource>();
        messageAS.PlayOneShot(clearClip);
        clears.SetActive(true);
        EXPGet();
        clearCheck = true;
    }
    public void ClearBtClick()
    {
        messageAS.PlayOneShot(buttonClip);

        if (PlayerPrefs.HasKey("stage clear")&&PlayerPrefs.GetInt("stage clear") >= stageNumber) //이미 클리어 한 게임이라면
        {
            PlayerPrefs.SetInt("returnCat", 1);
            SceneManager.LoadScene("Cat");
            return;
        }
        else if(stageNumber%5==0) //보스전이라면
        {
            PlayerPrefs.SetInt("stage clear", stageNumber); // 게임 클리어 저장
            rewards.SetActive(true);

            RewardGet();
        }
        else
        {
            PlayerPrefs.SetInt("stage clear", stageNumber); // 게임 클리어 저장
            PlayerPrefs.SetInt("returnCat", 1);
            SceneManager.LoadScene("Cat");
        }
        DemoDataManager.Instance.achievementDataList[2].progressvalue++;
    }
    void EXPGet()
    {
        if (PlayerPrefs.HasKey("stage clear") && PlayerPrefs.GetInt("stage clear") >= stageNumber) //이미 클리어 한 게임이라면
            return;

        expTx.gameObject.SetActive(true);

        switch (stageNumber)
        {
            case 1:
                expTx.text = "EXP +480";
                DemoDataManager.Instance.characterDatasList[0].exp += 480;
                break;
            case 2:
                expTx.text = "EXP +494";
                DemoDataManager.Instance.characterDatasList[0].exp += 494;
                break;
            case 3:
                expTx.text = "EXP +1113";
                DemoDataManager.Instance.characterDatasList[0].exp += 1113;
                break;
            case 4:
                expTx.text = "EXP +2040";
                DemoDataManager.Instance.characterDatasList[0].exp += 2040;
                break;
            case 5:
                expTx.text = "EXP +3320";
                DemoDataManager.Instance.characterDatasList[0].exp += 3320;
                break;
            case 6:
                expTx.text = "EXP +4998";
                DemoDataManager.Instance.characterDatasList[0].exp += 4998;
                break;
            case 7:
                expTx.text = "EXP +7119";
                DemoDataManager.Instance.characterDatasList[0].exp += 7119;
                break;
            case 8:
                expTx.text = "EXP +9728";
                DemoDataManager.Instance.characterDatasList[0].exp += 9728;
                break;
            case 9:
                expTx.text = "EXP +12870";
                DemoDataManager.Instance.characterDatasList[0].exp += 12870;
                break;
            case 10:
                expTx.text = "EXP +16590";
                DemoDataManager.Instance.characterDatasList[0].exp += 16590;
                break;
            default:
                Debug.Log("스테이지 선택 오류");
                break;
        }
    }
    void RewardGet()
    {
        rewardTitle.text = stageNumber.ToString();
        if (stageNumber == 5)
        {
            anchovyImg.gameObject.SetActive(true);
            anchovyTx.text = "X2500";
            pearlImg.gameObject.SetActive(true);
            pearlTx.text = "X10";
            weaponImg.gameObject.SetActive(true);
            weaponImg.sprite = 수리검;

            //데이터 받기
            DemoDataManager.Instance.moneyItemList[0].count += 2500;
            DemoDataManager.Instance.moneyItemList[1].count += 10;
            DemoDataManager.Instance.allWeaponItemList[1].count += 1;

            //new 알림
            if (DemoDataManager.Instance.allWeaponItemList[1].count == 1)
                DemoDataManager.Instance.allWeaponItemList[1].isnew = true;

            //업적 연동
            DemoDataManager.Instance.achievementDataList[0].progressvalue += 2500;
            DemoDataManager.Instance.achievementDataList[1].progressvalue += 10;
        }
        else if (stageNumber == 10)
        {
            anchovyImg.gameObject.SetActive(true);
            anchovyTx.text = "X2500";
            pearlImg.gameObject.SetActive(true);
            pearlTx.text = "X10";
            weaponImg.gameObject.SetActive(true);
            weaponImg.sprite = 음파건;

            //데이터 받기
            DemoDataManager.Instance.moneyItemList[0].count += 3000;
            DemoDataManager.Instance.moneyItemList[1].count += 10;
            DemoDataManager.Instance.allWeaponItemList[2].count += 1;

            //new 알림
            if (DemoDataManager.Instance.allWeaponItemList[2].count == 1)
                DemoDataManager.Instance.allWeaponItemList[2].isnew = true;

            //업적 연동
            DemoDataManager.Instance.achievementDataList[0].progressvalue += 3000;
            DemoDataManager.Instance.achievementDataList[1].progressvalue += 10;
        }
    }
    public void OkBtClick()
    {
        messageAS.PlayOneShot(buttonClip);
        PlayerPrefs.SetInt("returnCat", 1);
        SceneManager.LoadScene("Cat");
    }
}
