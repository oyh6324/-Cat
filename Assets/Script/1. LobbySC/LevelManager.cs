using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //레벨 업 보상 메세지 창
    public Canvas messageCanvas;
    public Image messageImg;
    public Image levelupMessageImg;
    public Text titleTx;
    public Text rewardTx;
    public Text statTx;

    void OnEnable()
    {
        if (DataManager.characterDatasList[0].exp >= DataManager.characterDatasList[0].totalexp)
            LevelUpReward();
    }
    public void OkBtClick()
    {
        messageImg.gameObject.SetActive(true);
        levelupMessageImg.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }
    void LevelUpReward() //레벨 업 보상과 스탯 증가
    {
        messageCanvas.gameObject.SetActive(true);
        messageImg.gameObject.SetActive(false);
        levelupMessageImg.gameObject.SetActive(true);
        int level, str, agi, def, hp;
        int reward1Count = 0, reward2Count = 0;
        int preHp = DataManager.characterDatasList[0].hp;
        decimal crip;
        string rewardName1 = "", rewardName2 = "";

        DataManager.characterDatasList[0].level += 1; //레벨 올리기
        level = DataManager.characterDatasList[0].level;
        DataManager.characterDatasList[0].exp -= DataManager.characterDatasList[0].totalexp; //유저 경험치 초기화
        DataManager.characterDatasList[0].totalexp = (level + 3) * level * level * 150; //해당 레벨 경험치
        DataManager.characterDatasList[0].hp += DataManager.characterDatasList[0].def * 18; //체력 올리기
        DataManager.characterDatasList[0].str += 3; //공격력 올리기
        DataManager.characterDatasList[0].def += 1; //방어력 올리기
        DataManager.characterDatasList[0].agi += 2; //민첩 올리기
        DataManager.characterDatasList[0].crip += (decimal)0.1; //크리티컬 확률 올리기

        str = DataManager.characterDatasList[0].str;
        def = DataManager.characterDatasList[0].def;
        agi = DataManager.characterDatasList[0].agi;
        crip = DataManager.characterDatasList[0].crip;
        hp = DataManager.characterDatasList[0].hp;
        //보상
        if (level < 5) //1~4
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = level * 500; reward2Count = (level - 1) * 3;
            DataManager.moneyItemList[0].count += reward1Count;
            DataManager.moneyItemList[1].count += reward2Count;
        }
        else if (level == 5) //5
        {
            rewardName1 = "멸치 보석함 무료이용권(의상)"; rewardName2 = "";
            reward1Count = 1;
            DataManager.moneyItemList[3].count += reward1Count;
        }
        else if (level > 5 && level < 10) //6~9
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = (level - 1) * 500; reward2Count = (level - 4) * 5;
            DataManager.moneyItemList[0].count += reward1Count;
            DataManager.moneyItemList[1].count += reward2Count;
        }
        else if (level == 10) //10
        {
            rewardName1 = "진주 보석함 무료이용권(의상)"; rewardName2 = "";
            reward1Count = 1;
            DataManager.moneyItemList[4].count += reward1Count;
        }
        else if (level > 10 && level < 15) //11~14
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = (level - 2) * 500; reward2Count = 19 + (level - 10) * 7;
            DataManager.moneyItemList[0].count += reward1Count;
            DataManager.moneyItemList[1].count += reward2Count;
        }
        else if (level == 15) //15
        {
            rewardName1 = "멸치 보석함 무료이용권(의상)"; rewardName2 = "진주 보석함 무료이용권(의상)";
            reward1Count = 1; reward2Count = 1;
            DataManager.moneyItemList[3].count += reward1Count;
            DataManager.moneyItemList[4].count += reward2Count;
        }
        else if (level > 15 && level < 20) //16~19
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = (level - 3) * 500; reward2Count = 39 + (level - 15) * 9;
            DataManager.moneyItemList[0].count += reward1Count;
            DataManager.moneyItemList[1].count += reward2Count;
        }
        else if (level == 20) //20
        {
            rewardName1 = "멸치"; rewardName2 = "진주";
            reward1Count = 10000; reward2Count = 100;
            DataManager.moneyItemList[0].count += 10000;
            DataManager.moneyItemList[1].count += 100;
            DataManager.moneyItemList[4].count += 1;

            //천사악마택1 만들어야함
        }
        if (rewardName1 == "멸치") //업적 연동
            DataManager.achievementDataList[0].progressvalue += reward1Count;
        if (rewardName2 == "진주")
            DataManager.achievementDataList[1].progressvalue += reward2Count;

        //all 스탯 데이터 초기화
        DataManager.characterDatasList[0].allstr = str + DataManager.characterDatasList[0].itemstr + DataManager.characterDatasList[0].setstr;
        DataManager.characterDatasList[0].alldef = def + DataManager.characterDatasList[0].itemdef + DataManager.characterDatasList[0].setdef;
        DataManager.characterDatasList[0].allagi = agi + DataManager.characterDatasList[0].itemagi + DataManager.characterDatasList[0].setagi;
        DataManager.characterDatasList[0].allcrip = crip + DataManager.characterDatasList[0].itemcrip + DataManager.characterDatasList[0].setcrip;

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
