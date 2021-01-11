using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPageChange : MonoBehaviour
{
    public Image[] progressImg; //진행 중인 업적 그림
    public Text[] progressTitleTx; //진행 중인 업적 제목
    public Text[] progressRewardTx; //진행 중인 업적 보상
    public Slider[] progressBar; //진행 중인 업적 진행바
    public Text[] progressCountTx; //진행 중인 업적 진행바 텍스트
    public Image[] lockBtImg; //완료 후 버튼 잠금 이미지
    public Image[] achieveImg; //업적 이미지
    public Sprite[] achieveSprite; //업적 스프라이트
    public Image[] reward1Img; //보상 이미지1
    public Image[] reward2Img; //보상 이미지2
    public Sprite[] reward1Sprite; //보상 스프라이트 0:멸치 1:진주 2:멸치보석함의상 3:진주보석함의상 4:멸치보석함무기 5:진주보석함무기
    public Text pageCountTx;
    public Button progressBt;
    public Button completeBt;

    private bool isProgressPage;
    private int thisPage;

    private List<ProgressData> progressDataList;
    private List<CompleteData> completeDataList;
    void Awake()
    {
        progressDataList = new List<ProgressData>();
        completeDataList = new List<CompleteData>();
    }
    void OnEnable()
    {
        isProgressPage = true;
        CategoryChange(progressBt, completeBt);
        PageChange();
    }
    public void ProgressBtClick()
    {
        isProgressPage = true;
        CategoryChange(progressBt, completeBt);
        PageChange();
    }
    public void CompleteBtClick()
    {
        isProgressPage = false;
        CategoryChange(completeBt, progressBt);
        PageChange();
    }
    public void RightBtClick()
    {
        if (isProgressPage)
        {
            if (thisPage < Mathf.Ceil((float)progressDataList.Count / 6)-1)
                thisPage++;
        }
        else
        {
            if (thisPage < Mathf.Ceil((float)completeDataList.Count / 6)-1)
                thisPage++;
        }
        PageChange();
    }
    public void LeftBtClick()
    {
        if(thisPage>0)
            thisPage--;
        PageChange();
    }
    public void RewardBt0Click()
    {
        RewardGetAndDataSetting(0);
        InputData();
        PageChange();
    }
    public void RewardBt1Click()
    {
        RewardGetAndDataSetting(1);
        InputData();
        PageChange();
    }
    public void RewardBt2Click()
    {
        RewardGetAndDataSetting(2);
        InputData();
        PageChange();
    }
    public void RewardBt3Click()
    {
        RewardGetAndDataSetting(3);
        InputData();
        PageChange();
    }
    public void RewardBt4Click()
    {
        RewardGetAndDataSetting(4);
        InputData();
        PageChange();
    }
    public void RewardBt5Click()
    {
        RewardGetAndDataSetting(5);
        InputData();
        PageChange();
    }
    void CategoryChange(Button outBt, Button inBt)
    {
        thisPage = 0;
        InputData();
        outBt.transform.localPosition = new Vector2(-1150, outBt.transform.localPosition.y);
        inBt.transform.localPosition = new Vector2(-1060, inBt.transform.localPosition.y);
    }
    void PageChange()
    {
        if (isProgressPage) //진행 페이지 볼때
        {
            for(int i=0; i<6; i++)
            {
                if (progressDataList.Count > thisPage * 6 + i)
                {
                    progressImg[i].gameObject.SetActive(true);
                    progressTitleTx[i].text = progressDataList[6 * thisPage + i].name; //타이틀 쓰기
                    progressBar[i].value = (float)progressDataList[6 * thisPage + i].progressvalue / progressDataList[6 * thisPage + i].value; //진행바 채우기
                    progressCountTx[i].text = progressDataList[6 * thisPage + i].progressvalue + "/" + progressDataList[6 * thisPage + i].value; //진행바 내용
                                                                                                                                                 //보상 이미지 씌우기
                    if (progressDataList[6 * thisPage + i].rewardname2 == "")
                    {
                        reward1Img[i].transform.localPosition = new Vector2(reward1Img[i].transform.localPosition.x, 7);
                        reward2Img[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        progressRewardTx[i].text += "\n\n" + "X" + progressDataList[6 * thisPage + i].rewardcount2; //보상 개수
                        reward1Img[i].transform.localPosition = new Vector2(reward1Img[i].transform.localPosition.x, 47);
                        reward2Img[i].gameObject.SetActive(true);
                    }
                    if (progressDataList[6 * thisPage + i].rewardname == "멸치")
                        reward1Img[i].sprite = reward1Sprite[0];
                    if (progressDataList[6 * thisPage + i].rewardname == "진주")
                        reward1Img[i].sprite = reward1Sprite[1];
                    if (progressDataList[6 * thisPage + i].rewardname == "멸치보석함의상")
                        reward1Img[i].sprite = reward1Sprite[2];
                    if (progressDataList[6 * thisPage + i].rewardname == "진주보석함의상")
                        reward1Img[i].sprite = reward1Sprite[3];
                    if (progressDataList[6 * thisPage + i].rewardname == "멸치보석함무기")
                        reward1Img[i].sprite = reward1Sprite[2];
                    if (progressDataList[6 * thisPage + i].rewardname == "진주보석함무기")
                        reward1Img[i].sprite = reward1Sprite[2];

                    for (int j = 0; j < DataManager.achievementDataList.Count; j++) //업적 이미지 적용
                    {
                        if (progressDataList[6 * thisPage + i].name == DataManager.achievementDataList[j].name)
                            achieveImg[i].sprite = achieveSprite[j];
                    }
                }
                else
                    progressImg[i].gameObject.SetActive(false);
                if (progressBar[i].value == 1) //업적 달성하면 보상 버튼 열림
                    lockBtImg[i].gameObject.SetActive(false);
                else
                    lockBtImg[i].gameObject.SetActive(true);
            }
            if(Mathf.Ceil((float)progressDataList.Count / 6)==0) //페이지 표시
                pageCountTx.text = thisPage + 1 + "/1";
            else
                pageCountTx.text = thisPage + 1 + "/" + Mathf.Ceil((float)progressDataList.Count / 6);

        }
        else //완료 페이지 볼때
        {
            for(int i=0; i<6; i++)
            {
                if (completeDataList.Count > thisPage * 6 + i)
                {
                    progressImg[i].gameObject.SetActive(true);
                    progressTitleTx[i].text = completeDataList[6 * thisPage + i].name;
                    progressRewardTx[i].text = "보상 완료";
                    progressBar[i].value = 1;
                    progressCountTx[i].text = "Max";
                    lockBtImg[i].gameObject.SetActive(true); //버튼 잠금

                    for (int j = 0; j < DemoDataManager.achievementDataList.Count; j++) //이미지 적용
                    {
                        if (completeDataList[6 * thisPage + i].name == DemoDataManager.achievementDataList[j].name)
                            achieveImg[i].sprite = achieveSprite[j];
                    }
                }
                else
                    progressImg[i].gameObject.SetActive(false);
            }
            if (Mathf.Ceil((float)completeDataList.Count / 6) == 0) //페이지 표시
                pageCountTx.text = thisPage + 1 + "/1";
            else
                pageCountTx.text = thisPage + 1 + "/" + Mathf.Ceil((float)completeDataList.Count / 6);
        }
    }
    void RewardGetAndDataSetting(int rewardBtNumber) 
    {
        for(int i=0; i<DataManager.moneyItemList.Count; i++) //보상 주기
        {
            if (DataManager.moneyItemList[i].name.Equals(progressDataList[6 * thisPage + rewardBtNumber].rewardname))
                DataManager.moneyItemList[i].count += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;
        }
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname2 != "") //다른 보상 주기
        {
            DataManager.moneyItemList[0].count += progressDataList[6 * thisPage + rewardBtNumber].rewardcount2;
            DataManager.achievementDataList[0].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount2;
        }
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname == "멸치") //업적
            DataManager.achievementDataList[0].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname == "진주")
            DataManager.achievementDataList[1].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;

        //데이터 바꾸기
        for(int i=0; i<DataManager.achievementDataList.Count; i++)
        {
            if (DataManager.achievementDataList[i].name.Equals(progressDataList[6 * thisPage + rewardBtNumber].name))
            {
                DataManager.achievementDataList[i].level += 1;
                progressDataList[6 * thisPage + rewardBtNumber].level = DataManager.achievementDataList[i].level;
                if (progressDataList[6 * thisPage + rewardBtNumber].name== "의상 Lv.3 만들기"|| progressDataList[6 * thisPage + rewardBtNumber].name == "의상 Lv.4 만들기"||
                    progressDataList[6 * thisPage + rewardBtNumber].name == "의상 Lv.3 만들기")
                {
                    if (DataManager.achievementDataList[i].level == 2)
                        DataManager.achievementDataList[i].value = 5;
                    if (DataManager.achievementDataList[i].level == 3)
                        DataManager.achievementDataList[i].value = 10;
                    if (DataManager.achievementDataList[i].level == 4)
                        DataManager.achievementDataList[i].value = 20;
                    if (DataManager.achievementDataList[i].level == 5)
                        DataManager.achievementDataList[i].value = 31;
                    DataManager.achievementDataList[i].rewardcount += 5;
                    DataManager.achievementDataList[i].rewardcount2 += 500;
                }
                if (progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.3 만들기" || progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.4 만들기" ||
                    progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.3 만들기")
                {
                    if (DataManager.achievementDataList[i].level == 2)
                        DataManager.achievementDataList[i].value = 5;
                    if (DataManager.achievementDataList[i].level == 3)
                        DataManager.achievementDataList[i].value = 10;
                    if (DataManager.achievementDataList[i].level == 4)
                        DataManager.achievementDataList[i].value = 17;
                    DataManager.achievementDataList[i].rewardcount += 5;
                    DataManager.achievementDataList[i].rewardcount2 += 500;
                }
            }
        }
        if(progressDataList[6*thisPage+rewardBtNumber].name=="멸치를 모아봐요")
        {
            DataManager.achievementDataList[0].progressvalue = progressDataList[6 * thisPage + rewardBtNumber].progressvalue - progressDataList[6 * thisPage + rewardBtNumber].value;
            DataManager.achievementDataList[0].value += 5000;
            DataManager.achievementDataList[0].rewardcount += progressDataList[6 * thisPage + rewardBtNumber].level * (progressDataList[6 * thisPage + rewardBtNumber].level - 1);
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name =="진주를 모아봐요")
        {
            DataManager.achievementDataList[1].progressvalue= progressDataList[6 * thisPage + rewardBtNumber].progressvalue - progressDataList[6 * thisPage + rewardBtNumber].value;
            DataManager.achievementDataList[1].value += 20;
            DataManager.achievementDataList[1].rewardcount += 1000;
        }
        else if(progressDataList[6*thisPage+rewardBtNumber].name=="스테이지 클리어")
        {
            DataManager.achievementDataList[2].value = (progressDataList[6 * thisPage + rewardBtNumber].level - 1) * 10;
            DataManager.achievementDataList[2].rewardcount += 10;
        }
        else if(progressDataList[6*thisPage+rewardBtNumber].name=="헬멧을 모아봐요")
        {
            DataManager.achievementDataList[3].value += 5;
            DataManager.achievementDataList[3].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "상의를 모아봐요")
        {
            DataManager.achievementDataList[4].value += 6;
            DataManager.achievementDataList[4].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "하의를 모아봐요")
        {
            DataManager.achievementDataList[5].value += 5;
            DataManager.achievementDataList[5].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "무기를 모아봐요")
        {
            DataManager.achievementDataList[6].value += 5;
            if (DataManager.achievementDataList[6].level == 3)
                DataManager.achievementDataList[6].value = 17;
            DataManager.achievementDataList[6].rewardname = "진주보석함무기";
        }
    }
    void InputData() //진행 업적과 완료 업적으로 나눠서 데이터 넣기
    {
        progressDataList.Clear();
        completeDataList.Clear();
        for (int i = 0; i < DataManager.achievementDataList.Count; i++)
        {
            if (DataManager.achievementDataList[i].maxlevel >= DataManager.achievementDataList[i].level)
                progressDataList.Add(new ProgressData(DataManager.achievementDataList[i].name, DataManager.achievementDataList[i].rewardname, DataManager.achievementDataList[i].rewardname2,
                    DataManager.achievementDataList[i].level, DataManager.achievementDataList[i].maxlevel,DataManager.achievementDataList[i].progressvalue,
                    DataManager.achievementDataList[i].rewardcount, DataManager.achievementDataList[i].rewardcount2, DataManager.achievementDataList[i].value));
            else
                completeDataList.Add(new CompleteData(DataManager.achievementDataList[i].name, DataManager.achievementDataList[i].rewardname, DataManager.achievementDataList[i].rewardname2,
                    DataManager.achievementDataList[i].level, DataManager.achievementDataList[i].maxlevel, DataManager.achievementDataList[i].rewardcount, DataManager.achievementDataList[i].rewardcount2,
                    DataManager.achievementDataList[i].value));
        }
    }
    public class ProgressData
    {
        public ProgressData(string _name, string _rewardname, string _rewardname2, int _level, int _maxlevel, int _progressvalue,int _rewardcount, int _rewradcount2, int _value)
        {
            name = _name; value = _value; rewardname = _rewardname; rewardname2 = _rewardname2; rewardcount = _rewardcount; rewardcount2 = _rewradcount2;
            maxlevel = _maxlevel; level = _level; progressvalue = _progressvalue;
        }
        public string name, rewardname, rewardname2;
        public int level, maxlevel, rewardcount, rewardcount2, value,progressvalue;
    }
    public class CompleteData
    {
        public CompleteData(string _name, string _rewardname, string _rewardname2, int _level, int _maxlevel, int _rewardcount, int _rewradcount2, int _value)
        {
            name = _name; value = _value; rewardname = _rewardname; rewardname2 = _rewardname2; rewardcount = _rewardcount; rewardcount2 = _rewradcount2;
            maxlevel = _maxlevel; level = _level;
        }
        public string name, rewardname, rewardname2;
        public int level, maxlevel, rewardcount, rewardcount2, value;
    }
}
