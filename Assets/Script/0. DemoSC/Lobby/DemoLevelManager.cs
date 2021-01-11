using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoLevelManager : MonoBehaviour
{
    //레벨 업 보상 메세지 창
    public Canvas messageCanvas;
    public Image messageImg;
    public Image levelupMessageImg;
    public Text titleTx;
    public Text rewardTx;
    public Text statTx;
    //경험치 바
    public Slider expSliberBar;
    public Text expSliderBarTx;
    public Text levelTx;
    public Text playerNameTx;
    //오디오
    public AudioSource soundEffectAS;
    public AudioClip buttonClickClip;
    void OnEnable()
    {
        if (DemoDataManager.characterDatasList[0].exp >= DemoDataManager.characterDatasList[0].totalexp)
            LevelUpReward();
    }
    public void OkBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        messageImg.gameObject.SetActive(true);
        levelupMessageImg.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }
    void LevelUpReward() //레벨 업 보상과 스탯 증가
    {
        messageCanvas.gameObject.SetActive(true);
        messageImg.gameObject.SetActive(false);
        levelupMessageImg.gameObject.SetActive(true);
        int level ,str, agi, def, hp;
        int reward1Count = 0, reward2Count = 0;
        int preHp=DemoDataManager.characterDatasList[0].hp;
        decimal crip;
        string rewardName1="", rewardName2="";

        DemoDataManager.characterDatasList[0].level += 1; //레벨 올리기
        level = DemoDataManager.characterDatasList[0].level;
        DemoDataManager.characterDatasList[0].exp -= DemoDataManager.characterDatasList[0].totalexp; //유저 경험치 초기화
        DemoDataManager.characterDatasList[0].totalexp = (level + 3) * level * level * 150; //해당 레벨 경험치
        DemoDataManager.characterDatasList[0].hp += DemoDataManager.characterDatasList[0].def * 18; //체력 올리기
        DemoDataManager.characterDatasList[0].str += 3; //공격력 올리기
        DemoDataManager.characterDatasList[0].def += 1; //방어력 올리기
        DemoDataManager.characterDatasList[0].agi += 2; //민첩 올리기
        DemoDataManager.characterDatasList[0].crip += (decimal)0.1; //크리티컬 확률 올리기

        str = DemoDataManager.characterDatasList[0].str;
        def = DemoDataManager.characterDatasList[0].def;
        agi = DemoDataManager.characterDatasList[0].agi;
        crip = DemoDataManager.characterDatasList[0].crip;
        hp = DemoDataManager.characterDatasList[0].hp;
        //보상
        if (level < 5) //1~4
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = level * 500; reward2Count = (level - 1) * 3;
            DemoDataManager.moneyItemList[0].count += reward1Count;
            DemoDataManager.moneyItemList[1].count += reward2Count;
        }
        else if (level == 5) //5
        {
            rewardName1 = "멸치 보석함 무료이용권(의상)"; rewardName2 = "";
            reward1Count = 1;
            DemoDataManager.moneyItemList[3].count += reward1Count;
        }
        else if (level > 5 && level < 10) //6~9
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = (level - 1) * 500; reward2Count = (level - 4) * 5;
            DemoDataManager.moneyItemList[0].count += reward1Count;
            DemoDataManager.moneyItemList[1].count += reward2Count;
        }
        else if (level == 10) //10
        {
            rewardName1 = "진주 보석함 무료이용권(의상)"; rewardName2 = "";
            reward1Count = 1;
            DemoDataManager.moneyItemList[4].count += reward1Count;
        }
        else if (level > 10 && level < 15) //11~14
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = (level - 2) * 500; reward2Count = 19 + (level - 10) * 7;
            DemoDataManager.moneyItemList[0].count += reward1Count;
            DemoDataManager.moneyItemList[1].count += reward2Count;
        }
        else if (level == 15) //15
        {
            rewardName1 = "멸치 보석함 무료이용권(의상)"; rewardName2 = "진주 보석함 무료이용권(의상)";
            reward1Count = 1; reward2Count = 1;
            DemoDataManager.moneyItemList[3].count += reward1Count;
            DemoDataManager.moneyItemList[4].count += reward2Count;
        }
        else if (level > 15 && level < 20) //16~19
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = (level - 3) * 500; reward2Count = 39 + (level - 15) * 9;
            DemoDataManager.moneyItemList[0].count += reward1Count;
            DemoDataManager.moneyItemList[1].count += reward2Count;
        }
        else if (level == 20) //20
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = 10000; reward2Count = 100;
            DemoDataManager.moneyItemList[0].count += 10000;
            DemoDataManager.moneyItemList[1].count += 100;
            DemoDataManager.moneyItemList[4].count += 1;
        }
        if (rewardName1 == "멸치") //업적 연동
            DemoDataManager.achievementDataList[0].progressvalue += reward1Count;
        if(rewardName2=="진주")
            DemoDataManager.achievementDataList[1].progressvalue += reward2Count;

        //all 스탯 데이터 초기화
        DemoDataManager.characterDatasList[0].allstr = str + DemoDataManager.characterDatasList[0].itemstr + DemoDataManager.characterDatasList[0].setstr;
        DemoDataManager.characterDatasList[0].alldef = def + DemoDataManager.characterDatasList[0].itemdef + DemoDataManager.characterDatasList[0].setdef;
        DemoDataManager.characterDatasList[0].allagi = agi + DemoDataManager.characterDatasList[0].itemagi + DemoDataManager.characterDatasList[0].setagi;
        DemoDataManager.characterDatasList[0].allcrip = crip + DemoDataManager.characterDatasList[0].itemcrip + DemoDataManager.characterDatasList[0].setcrip;

        titleTx.text = "Lv." + level;
        rewardTx.text = rewardName1 + " +" + reward1Count;
        if (rewardName2 != "")
            rewardTx.text += " " + rewardName2 + " +" + reward2Count;
        if (level == 20)
            rewardTx.text += "\n 진주 보석함 무료이용권(의상) +1";
        statTx.text = preHp.ToString() + "→" + hp.ToString() + "\n" + (str - 3).ToString() + "→" + str.ToString() + "\n" + (def - 1).ToString() + "→" + def.ToString() +
            "\n" + (agi - 2).ToString() + "→" + agi.ToString() + "\n" + (crip - (decimal)0.1).ToString() + "→" + crip.ToString();
    }
}
