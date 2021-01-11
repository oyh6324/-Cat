using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public Text titleTx; //스테이지 
    public Image bossPictureImg; //보스 사진
    public Text bossNameTx; //보스 이름
    public Text bossExTx; //보스 설명
    public Image bossLockImg; //보스 잠금 이미지
    public Image bossNewAlarmImg; //new 이미지
    public Button bossBt; //보스 버튼
    public Image[] pictureImg; //몹 이미지
    public Text[] pictureNameTx; //몹 이름
    public Text[] pictureExTx; //몹 설명
    public Image[] pictureLockImg; //몹 잠금
    public Image[] pictureNewAlarmImg; //new 이미지
    public Button[] pictureBt; //몹 버튼
    public Sprite[] monsterSprite;

    private string[] stageStr= { "1~5 Stage", "6~10 Stage","11~15 Stage","16~20 Stage","21~25 Stage","26~30 Stage"};
    private List<monsterData> monsterDataList;
    private int thisPage;

    void Awake()
    {
        monsterDataList = new List<monsterData>();
    }
    void OnEnable()
    {
        thisPage = 0;
        PageChage();
    }
    public void RightBtClick()
    {
        if (thisPage == 5)
            thisPage = 0;
        else
            thisPage++;
        PageChage();
    }
    public void LeftBtClick()
    {
        if (thisPage == 0)
            thisPage = 5;
        else
            thisPage--;
        PageChage();
    }
    public void BossBtClick()
    {
        DataManager.moneyItemList[0].count += 50;
        DataManager.achievementDataList[0].progressvalue += 50;
        for(int i=0; i<monsterDataList.Count; i++)
        {
            if (monsterDataList[i].stage == (thisPage + 1) * 5)
                DataManager.monsterCollectionDataList[i].isreward = true;
        }
        PageChage();
    }
    public void Picture0BtClick()
    {
        PictureReward(0);
    }
    public void Picture1BtClick()
    {
        PictureReward(1);
    }
    public void Picture2BtClick()
    {
        PictureReward(2);
    }
    public void Picture3BtClick()
    {
        PictureReward(3);
    }
    void PictureReward(int index)
    {
        DataManager.moneyItemList[0].count += 30;
        DataManager.achievementDataList[0].progressvalue += 30;
        for (int i = 0; i < monsterDataList.Count; i++)
        {
            if (monsterDataList[i].name == pictureNameTx[index].text)
                DataManager.monsterCollectionDataList[i].isreward = true;
        }
        PageChage();
    }
    void PageChage()
    {
        InputData();
        titleTx.text = stageStr[thisPage];
        int count = 0;

        for(int i=0; i < monsterDataList.Count; i++)
        {
            if (monsterDataList[i].stage == (thisPage + 1) * 5) //해당 페이지의 보스 고르기
            {
                if (monsterDataList[i].stage <= DataManager.characterDatasList[0].stage) //보스를 만났다면 보스 출력
                {
                    bossLockImg.gameObject.SetActive(false);
                    bossPictureImg.sprite = monsterSprite[i];
                    bossNameTx.text = monsterDataList[i].name;
                    bossExTx.text = "이름: " + monsterDataList[i].realname + "\n종류: " + monsterDataList[i].kind + "\n서식지: " + monsterDataList[i].habitat;

                    if (monsterDataList[i].isreward) //보상을 받았는지 안받았는지
                    {
                        bossBt.gameObject.SetActive(false);
                        bossNewAlarmImg.gameObject.SetActive(false);
                    }
                    else
                    {
                        bossBt.gameObject.SetActive(true);
                        bossNewAlarmImg.gameObject.SetActive(true);
                    }
                }
                else //스테이지 미방문 시 출력
                {
                    bossNewAlarmImg.gameObject.SetActive(false);
                    bossLockImg.gameObject.SetActive(true);
                    bossNameTx.text = "???";
                    bossExTx.text = "아직 발견되지 않았습니다.";
                }
            }
            if (monsterDataList[i].stage < (thisPage + 1) * 5 && monsterDataList[i].stage >= (thisPage * 5) + 1) //해당 페이지의 스테이지 잡몹들 고르기
            {
                if (monsterDataList[i].stage <= DataManager.characterDatasList[0].stage) //해당 스테이지를 갔다면 출력
                {
                    pictureLockImg[count].gameObject.SetActive(false);
                    pictureImg[count].sprite = monsterSprite[i];
                    pictureNameTx[count].text = monsterDataList[i].name;
                    pictureExTx[count].text = "이름: " + monsterDataList[i].realname + "\n종류: " + monsterDataList[i].kind + "\n서식지: " + monsterDataList[i].habitat;

                    if (monsterDataList[i].isreward) //보상
                    {
                        pictureBt[count].gameObject.SetActive(false);
                        pictureNewAlarmImg[count].gameObject.SetActive(false);
                    }
                    else
                    {
                        pictureBt[count].gameObject.SetActive(true);
                        pictureNewAlarmImg[count].gameObject.SetActive(true);
                    }
                    count++;
                }
                else //스테이지 미방문 시 출력
                {
                    pictureNewAlarmImg[count].gameObject.SetActive(false);
                    pictureLockImg[count].gameObject.SetActive(true);
                    pictureNameTx[count].text = "???";
                    pictureExTx[count].text = "아직 발견되지 않았습니다.";
                    count++;
                }
            }
        }

    }
    void InputData()
    {
        monsterDataList.Clear();
        for(int i=0; i<DataManager.monsterCollectionDataList.Count; i++)
            monsterDataList.Add(new monsterData(DataManager.monsterCollectionDataList[i].name, DataManager.monsterCollectionDataList[i].realname, DataManager.monsterCollectionDataList[i].habitat,
                DataManager.monsterCollectionDataList[i].kind, DataManager.monsterCollectionDataList[i].stage,DataManager.monsterCollectionDataList[i].isreward));
    }
    public class monsterData
    {
        public monsterData(string _name, string _realname, string _habitat, string _kind, int _stage, bool _isreward)
        {
            name = _name; realname = _realname; habitat = _habitat; kind = _kind; stage = _stage; isreward = _isreward;
        }
        public string name, realname, habitat, kind;
        public int stage;
        public bool isreward;
    }
}
