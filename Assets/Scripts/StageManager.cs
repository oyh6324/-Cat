using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private int stageNumber;
    private bool clearCheck;
    private bool failCheck;
    private void Awake()
    {
        stageNumber= PlayerPrefs.GetInt("stageNumber");
        map[(stageNumber-1)/5].SetActive(true); //바탕
        stage[stageNumber - 1].SetActive(true); //스테이지

        Debug.Log("현재 스테이지 "+stageNumber);

        StartCoroutine(startCanvasAction());
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
    IEnumerator startCanvasAction()
    {
        startCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        startCanvas.SetActive(false);
    }
    void StageFail()
    {
        if (failCheck) return;

        Debug.Log("클리어 실패");
        messageCanvas.SetActive(true);
        fail.SetActive(true);
        failCheck = true;
    }
    public void BackBtClick()
    {
        PlayerPrefs.SetInt("returnCat", 1);
        SceneManager.LoadScene("Cat");
    }
    public void AgainBtClick()
    {
        message.gameObject.SetActive(true);
        messageTx.text = "열쇠를 사용하시겠습니까?";
    }
    public void YesBtClick()
    {
        if(DemoDataManager.moneyItemList[2].count<1)
        {
            messageTx.text = "열쇠가 부족해요!";
            yesNo.SetActive(false);
            ok.SetActive(true);
        }
        else
        {
            DemoDataManager.moneyItemList[2].count--;
            SceneManager.LoadScene("Stage");
        }
    }
    public void NoBtClick()
    {
        message.gameObject.SetActive(false);
    }
    public void MokBtClick()
    {
        yesNo.SetActive(true);
        ok.SetActive(false);
        message.gameObject.SetActive(false);
    }
    void StageClear()
    {
        if (clearCheck) return;

        Debug.Log("clear"); // clear 확인 변수값 설정

        messageCanvas.SetActive(true);
        clears.SetActive(true);
        EXPGet();
        clearCheck = true;
    }
    public void ClearBtClick()
    {
        if(PlayerPrefs.HasKey("stage clear")&&PlayerPrefs.GetInt("stage clear") >= stageNumber) //이미 클리어 한 게임이라면
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
        DemoDataManager.achievementDataList[2].progressvalue++;
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
                DemoDataManager.characterDatasList[0].exp += 480;
                break;
            case 2:
                expTx.text = "EXP +494";
                DemoDataManager.characterDatasList[0].exp += 494;
                break;
            case 3:
                expTx.text = "EXP +1113";
                DemoDataManager.characterDatasList[0].exp += 1113;
                break;
            case 4:
                expTx.text = "EXP +2040";
                DemoDataManager.characterDatasList[0].exp += 2040;
                break;
            case 5:
                expTx.text = "EXP +3320";
                DemoDataManager.characterDatasList[0].exp += 3320;
                break;
            case 6:
                expTx.text = "EXP +4998";
                DemoDataManager.characterDatasList[0].exp += 4998;
                break;
            case 7:
                expTx.text = "EXP +7119";
                DemoDataManager.characterDatasList[0].exp += 7119;
                break;
            case 8:
                expTx.text = "EXP +9728";
                DemoDataManager.characterDatasList[0].exp += 9728;
                break;
            case 9:
                expTx.text = "EXP +12870";
                DemoDataManager.characterDatasList[0].exp += 12870;
                break;
            case 10:
                expTx.text = "EXP +16590";
                DemoDataManager.characterDatasList[0].exp += 16590;
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
            DemoDataManager.moneyItemList[0].count += 2500;
            DemoDataManager.moneyItemList[1].count += 10;
            DemoDataManager.allWeaponItemList[1].count += 1;

            //new 알림
            if (DemoDataManager.allWeaponItemList[1].count == 1)
                DemoDataManager.allWeaponItemList[1].isnew = true;

            //업적 연동
            DemoDataManager.achievementDataList[0].progressvalue += 2500;
            DemoDataManager.achievementDataList[1].progressvalue += 10;
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
            DemoDataManager.moneyItemList[0].count += 3000;
            DemoDataManager.moneyItemList[1].count += 10;
            DemoDataManager.allWeaponItemList[2].count += 1;

            //new 알림
            if (DemoDataManager.allWeaponItemList[2].count == 1)
                DemoDataManager.allWeaponItemList[2].isnew = true;

            //업적 연동
            DemoDataManager.achievementDataList[0].progressvalue += 3000;
            DemoDataManager.achievementDataList[1].progressvalue += 10;
        }
    }
    public void OkBtClick()
    {
        PlayerPrefs.SetInt("returnCat", 1);
        SceneManager.LoadScene("Cat");
    }
}
