using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image dialogue;
    public Text dialTx;
    public GameObject tutorialBt;
    public Image[] blinds; //0: all 1: weapon 2: airgun 3: backBt 4: inventory 5: helmet 6: playBt 7: stageBt
    public GameObject weaponCanvas;
    public GameObject inventoryCanvas;
    public GameObject stageCanvas;

    public float typing_speed;

    public Vector2 weaponBt_forBt;
    public Vector2 airgunBt_forBt;
    public Vector2 backBt_forBt;
    public Vector2 inventoryBt_forBt;
    public Vector2 helmetBt_forBt;
    public Vector2 playBt_forBt;
    public Vector2 stage1Bt_forBt;

    public Vector2 lobby_forDialog;
    public Vector2 weapon_forDialog;
    public Vector2 inventory_forDialog;

    private bool waitingClick;
    private bool isTyping;
    private int tutorialNumber;
    private string[] tutorials;
    private int keyNumber;
    private void Start()
    {
        tutorials=new string[]{"안녕! 나는 고양이 "+DemoDataManager.Instance.characterDatasList[0].name+"!",
                                    "나는 지금 무지 무지 배가 \n고파서 물고기를 잡으러 왔어. ",
                                    "내가 물고기를 잡을 수 있게 \n도와 줄래? ",
                                    "무기고를 눌러 봐! ",
                                    "여기서 물고기를 잡을 도구를 챙길 수 있어. ",
                                    "지금은 하나밖에 없지만 상점에서 \n보석함을 사면 더 얻을 수 있어! ",
                                    "자, 에어건을 장착해볼까? ",
                                    "잘했어. 이제 다른 장비를 \n착용하러 가자. ",
                                    "여기는 내 보물 창고야. \n어항을 착용해볼까? ",
                                    "이게 없으면 숨쉴 수 없지! \n자, 이제 물고기를 잡으러 가자! "};

        dialogue.transform.localPosition = lobby_forDialog;
        StartCoroutine(typing(tutorials[0]));

        keyNumber = DemoDataManager.Instance.moneyItemList[2].count;
    }
    private void Update()
    {
        CheckTutorialPhase();
    }
    public void BtClick()
    {
        if (waitingClick)
            return;

        if (isTyping)
        {
            isTyping = false;
            dialTx.text = tutorials[tutorialNumber];
        }
        else
        {
            tutorialNumber++;
            StartCoroutine(typing(tutorials[tutorialNumber]));
        }
    }
    IEnumerator typing(string message)
    {
        isTyping = true;
        for(int i=0; i< message.Length; i++)
        {
            if (message.Length == i + 1 || isTyping == false)
            {
                isTyping = false;
                break;
            }
            dialTx.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(typing_speed);
        }
    }
    void CheckTutorialPhase()
    {
        if (isTyping||tutorialNumber == 0 || tutorialNumber == 1 || tutorialNumber == 2 || tutorialNumber == 4 || tutorialNumber == 5)
            return;

        waitingClick = true;
        tutorialBt.gameObject.SetActive(true);
        if (tutorialNumber == 3)
        {
            tutorialBt.transform.localPosition = weaponBt_forBt;
            OnBlind(1);
        }
        if(weaponCanvas.activeSelf && tutorialNumber == 3)
        {
            waitingClick = false;

            tutorialBt.gameObject.SetActive(false);
            dialogue.transform.localPosition = weapon_forDialog;
            OnBlind(0);
            BtClick();
        }
        if (tutorialNumber == 6)
        {
            tutorialBt.transform.localPosition = airgunBt_forBt;
            OnBlind(2);
        }
        if(weaponCanvas.activeSelf&& tutorialNumber == 6 && DemoDataManager.Instance.characterDatasList[0].weapon == "에어건")
        {
            waitingClick = false;

            tutorialBt.gameObject.SetActive(false);
            OnBlind(0);
            BtClick();
        }
        if (tutorialNumber == 7)
        {
            tutorialBt.transform.localPosition = backBt_forBt;
            OnBlind(3);
        }
        if(tutorialNumber == 7 && !weaponCanvas.activeSelf)
        {
            tutorialBt.transform.localPosition = inventoryBt_forBt;
            dialogue.gameObject.SetActive(false);
            OnBlind(4);
        }
        if (tutorialNumber == 7 && inventoryCanvas.activeSelf)
        {
            waitingClick = false;

            tutorialBt.gameObject.SetActive(false);
            dialogue.gameObject.SetActive(true);
            dialogue.transform.localPosition = inventory_forDialog;
            OnBlind(0);
            BtClick();
        }
        if (tutorialNumber == 8)
        {
            tutorialBt.transform.localPosition = helmetBt_forBt;
            OnBlind(5);
        }
        if (tutorialNumber == 8 && inventoryCanvas.activeSelf && DemoDataManager.Instance.characterDatasList[0].helmet == "어항")
        {
            waitingClick = false;

            tutorialBt.gameObject.SetActive(false);
            OnBlind(0);
            BtClick();
        }
        if (tutorialNumber == 9)
        {
            tutorialBt.transform.localPosition = backBt_forBt;
            OnBlind(3);
        }
        if(tutorialNumber == 9 && !inventoryCanvas.activeSelf)
        {
            dialogue.gameObject.SetActive(false);
            tutorialBt.transform.localPosition = playBt_forBt;
            OnBlind(6);
        }
        if(tutorialNumber == 9 && stageCanvas.activeSelf)
        {
            tutorialBt.transform.localPosition = stage1Bt_forBt;
            OnBlind(7);
        }
        if (keyNumber > DemoDataManager.Instance.moneyItemList[2].count)
        {
            this.gameObject.SetActive(false);
        }
    }
    void OnBlind(int blindNumber)
    {
        for(int i=0; i<blinds.Length; i++)
        {
            if (i == blindNumber)
                blinds[i].gameObject.SetActive(true);
            else
                blinds[i].gameObject.SetActive(false);
        }
    }
}
