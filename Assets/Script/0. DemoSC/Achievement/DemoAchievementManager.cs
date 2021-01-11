using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoAchievementManager : MonoBehaviour
{
    public Image[] progressImg; //진행 중인 업적 그림
    public Text[] progressTitleTx; //진행 중인 업적 제목
    public Text[] progressRewardTx; //진행 중인 업적 보상
    public Text[] progressRewardTx2;
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

    //오디오
    public AudioSource bgmAS;
    public AudioSource soundEffectAS;
    public AudioClip buttonClickClip;
    public AudioClip rewardGetClip;
    public AudioClip achievementBgmClip;

    //애니메이터
    public Animator pearlAnim;
    public Animator anchovyAnim;

    private bool isProgressPage;
    private int thisPage;
    private bool[] isReward;

    private List<ProgressData> progressDataList;
    private List<CompleteData> completeDataList;
    void Awake()
    {
        progressDataList = new List<ProgressData>();
        completeDataList = new List<CompleteData>();
        isReward = new bool[6];
    }
    void OnEnable()
    {
        bgmAS.clip = achievementBgmClip;
        bgmAS.Play();
        isProgressPage = true;
        CategoryChange(progressBt, completeBt);
        PageChange();
    }
    void Update()
    {
        for(int i=0; i<6; i++)
        {
            if (isReward[i])
                RewardMove(i, progressDataList[6 * thisPage + i].rewardname, progressDataList[6 * thisPage + i].rewardname2);
        }
    }
    public void ProgressBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        isProgressPage = true;
        CategoryChange(progressBt, completeBt);
        PageChange();
    }
    public void CompleteBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        isProgressPage = false;
        CategoryChange(completeBt, progressBt);
        PageChange();
    }
    public void RightBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (isProgressPage)
        {
            if (thisPage < Mathf.Ceil((float)progressDataList.Count / 6) - 1)
                thisPage++;
        }
        else
        {
            if (thisPage < Mathf.Ceil((float)completeDataList.Count / 6) - 1)
                thisPage++;
        }
        PageChange();
    }
    public void LeftBtClick()
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (thisPage > 0)
            thisPage--;
        PageChange();
    }
    public void RewardBt0Click()
    {
        RewardGetAndDataSetting(0);
    }
    public void RewardBt1Click()
    {
        RewardGetAndDataSetting(1);
    }
    public void RewardBt2Click()
    {
        RewardGetAndDataSetting(2);
    }
    public void RewardBt3Click()
    {
        RewardGetAndDataSetting(3);
    }
    public void RewardBt4Click()
    {
        RewardGetAndDataSetting(4);
    }
    public void RewardBt5Click()
    {
        RewardGetAndDataSetting(5);
    }
    void CategoryChange(Button outBt, Button inBt)
    {
        thisPage = 0;
        InputData();
        outBt.transform.localPosition = new Vector2(-1160, outBt.transform.localPosition.y);
        inBt.transform.localPosition = new Vector2(-1110, inBt.transform.localPosition.y);
    }
    void PageChange()
    {
        if (isProgressPage) //진행 페이지 볼때
        {
            for (int i = 0; i < 6; i++)
            {
                lockBtImg[i].color = new Color(lockBtImg[i].color.r, lockBtImg[i].color.g, lockBtImg[i].color.b, 0.5f);
                if (progressDataList.Count > thisPage * 6 + i)
                {
                    reward1Img[i].gameObject.SetActive(true);
                    progressImg[i].gameObject.SetActive(true);
                    progressTitleTx[i].text = progressDataList[6 * thisPage + i].name; //타이틀 쓰기
                    progressBar[i].value = (float)progressDataList[6 * thisPage + i].progressvalue / progressDataList[6 * thisPage + i].value; //진행바 채우기
                    progressCountTx[i].text = progressDataList[6 * thisPage + i].progressvalue + "/" + progressDataList[6 * thisPage + i].value; //진행바 내용
                    progressRewardTx[i].text = "X" + progressDataList[6 * thisPage + i].rewardcount; //보상 개수
                    //보상 이미지 씌우기
                    if (progressDataList[6 * thisPage + i].rewardname2 == "")
                    {
                        reward1Img[i].transform.localPosition = new Vector2(reward1Img[i].transform.localPosition.x, 7);
                        reward2Img[i].gameObject.SetActive(false);
                        progressRewardTx2[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        progressRewardTx2[i].text = "X" + progressDataList[6 * thisPage + i].rewardcount2; //보상 개수
                        reward1Img[i].transform.localPosition = new Vector2(reward1Img[i].transform.localPosition.x, 47);
                        reward2Img[i].gameObject.SetActive(true);
                        progressRewardTx2[i].gameObject.SetActive(true);
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

                    for (int j=0; j<DemoDataManager.achievementDataList.Count; j++) //업적 이미지 적용
                    {
                        if (progressDataList[6 * thisPage + i].name == DemoDataManager.achievementDataList[j].name)
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
            if (Mathf.Ceil((float)progressDataList.Count / 6) == 0) //페이지 표시
                pageCountTx.text = thisPage + 1 + "/1";
            else
                pageCountTx.text = thisPage + 1 + "/" + Mathf.Ceil((float)progressDataList.Count / 6);

        }
        else //완료 페이지 볼때
        {
            for (int i = 0; i < 6; i++)
            {
                if (completeDataList.Count > thisPage * 6 + i)
                {
                    progressImg[i].gameObject.SetActive(true);
                    progressTitleTx[i].text = completeDataList[6 * thisPage + i].name;
                    reward1Img[i].gameObject.SetActive(false);
                    reward2Img[i].gameObject.SetActive(false);
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
    IEnumerator RewardMoveManager(int rewardBtNumber,string rewardName1,string rewardName2)
    {
        lockBtImg[rewardBtNumber].color = new Color(lockBtImg[rewardBtNumber].color.r, lockBtImg[rewardBtNumber].color.g, lockBtImg[rewardBtNumber].color.b,
            0f);
        lockBtImg[rewardBtNumber].gameObject.SetActive(true);
        int size= 70;
        for (int i = 0; i < 25; i++) //위로 올라가기
        {
            size += 4;
            progressRewardTx[rewardBtNumber].gameObject.SetActive(false);
            reward1Img[rewardBtNumber].transform.localPosition = new Vector2(reward1Img[rewardBtNumber].transform.localPosition.x,
                reward1Img[rewardBtNumber].transform.localPosition.y + 5);
            reward1Img[rewardBtNumber].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            reward1Img[rewardBtNumber].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

            if (rewardName2 != "")
            {
                progressRewardTx2[rewardBtNumber].gameObject.SetActive(false);
                reward2Img[rewardBtNumber].transform.localPosition = new Vector2(reward2Img[rewardBtNumber].transform.localPosition.x,
                    reward2Img[rewardBtNumber].transform.localPosition.y + 5);
                reward2Img[rewardBtNumber].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
                reward2Img[rewardBtNumber].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.2f);
        if (rewardName1 != "멸치" && rewardName1 != "진주")
        {
            reward1Img[rewardBtNumber].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70);
            reward1Img[rewardBtNumber].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
            progressRewardTx[rewardBtNumber].gameObject.SetActive(true);
            InputData();
            PageChange();
        }
        else
            isReward[rewardBtNumber] = true;
    }
    void RewardMove(int rewardNumer, string rewardName1, string rewardName2) //업데이트 용
    {
        int x=0,y = 0;
        int x2 = 0;
        float speed = 40f;
        float speed2 = 40f;
        if (rewardNumer == 0 || rewardNumer == 1 || rewardNumer == 2)
        {
            //위치 조정
            if (rewardName1 == "멸치")
                x = 150; //335
            if (rewardName1 == "진주")
                x = 550;
            y = 350 + rewardNumer * 250;
            x2 = 150;
            if(rewardName2!="") //스피드 조정
                speed = 40f + (rewardNumer + 3f);
        }
        if (rewardNumer == 3 || rewardNumer == 4 || rewardNumer == 5)
        {
            //위치 조정
            if (rewardName1 == "멸치")
                x = -850; //335
            if (rewardName1 == "진주")
                x = -450;
            x2 = -850;
            y = 350 + (rewardNumer-3) * 250;
            if (rewardName2 != "") //스피드 조정
                speed2 = 40f + (rewardNumer + 3f);
        }
        //첫 번째 보상 이동
        reward1Img[rewardNumer].transform.localPosition = Vector2.MoveTowards(new Vector2(reward1Img[rewardNumer].transform.localPosition.x,
            reward1Img[rewardNumer].transform.localPosition.y), new Vector2(x, y), speed);
        if (rewardName2 != "") //두 번째 보상 이동
            reward2Img[rewardNumer].transform.localPosition = Vector2.MoveTowards(new Vector2(reward2Img[rewardNumer].transform.localPosition.x,
                reward2Img[rewardNumer].transform.localPosition.y), new Vector2(x2, y), speed2);
        if (reward1Img[rewardNumer].transform.localPosition.y + 500 > y) //상단바 애니메이션 시작
        {
            if (rewardName1 == "진주")
                pearlAnim.SetBool("isGet", true);
            if (rewardName1 == "멸치")
                anchovyAnim.SetBool("isGet", true);
            if (rewardName2 != "")
                anchovyAnim.SetBool("isGet", true);
        }
        if (reward1Img[rewardNumer].transform.localPosition.y == y) //보상 애니메이션 종료
        {
            reward1Img[rewardNumer].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70);
            reward1Img[rewardNumer].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
            reward2Img[rewardNumer].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70);
            reward2Img[rewardNumer].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
            reward1Img[rewardNumer].transform.localPosition = new Vector2(-57, 44);
            reward2Img[rewardNumer].transform.localPosition = new Vector2(-57, -37);
            progressRewardTx[rewardNumer].gameObject.SetActive(true);
            progressRewardTx2[rewardNumer].gameObject.SetActive(true);
            isReward[rewardNumer] = false;
            InputData();
            PageChange();
        }
    }
    void RewardGetAndDataSetting(int rewardBtNumber)
    {
        pearlAnim.SetBool("isGet", false);
        anchovyAnim.SetBool("isGet", false);
        StartCoroutine(RewardMoveManager(rewardBtNumber, progressDataList[6 * thisPage + rewardBtNumber].rewardname, progressDataList[6 * thisPage + rewardBtNumber].rewardname2));
        soundEffectAS.clip = rewardGetClip;
        soundEffectAS.Play();
        for (int i = 0; i < DemoDataManager.moneyItemList.Count; i++) //보상 주기
        {
            if (DemoDataManager.moneyItemList[i].name.Equals(progressDataList[6 * thisPage + rewardBtNumber].rewardname))
                DemoDataManager.moneyItemList[i].count += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;
        }
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname2 != "") //다른 보상 주기
        {
            DemoDataManager.moneyItemList[0].count += progressDataList[6 * thisPage + rewardBtNumber].rewardcount2;
            DemoDataManager.achievementDataList[0].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount2;
        }
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname == "멸치") //업적
            DemoDataManager.achievementDataList[0].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname == "진주")
            DemoDataManager.achievementDataList[1].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;

        //데이터 바꾸기
        for (int i = 0; i < DemoDataManager.achievementDataList.Count; i++)
        {
            if (DemoDataManager.achievementDataList[i].name.Equals(progressDataList[6 * thisPage + rewardBtNumber].name))
            {
                DemoDataManager.achievementDataList[i].level += 1;
                progressDataList[6 * thisPage + rewardBtNumber].level = DemoDataManager.achievementDataList[i].level;
                if (progressDataList[6 * thisPage + rewardBtNumber].name == "의상 Lv.3 만들기" || progressDataList[6 * thisPage + rewardBtNumber].name == "의상 Lv.4 만들기" ||
                    progressDataList[6 * thisPage + rewardBtNumber].name == "의상 Lv.3 만들기")
                {
                    if (DemoDataManager.achievementDataList[i].level == 2)
                        DemoDataManager.achievementDataList[i].value = 5;
                    if (DemoDataManager.achievementDataList[i].level == 3)
                        DemoDataManager.achievementDataList[i].value = 10;
                    if (DemoDataManager.achievementDataList[i].level == 4)
                        DemoDataManager.achievementDataList[i].value = 20;
                    if (DemoDataManager.achievementDataList[i].level == 5)
                        DemoDataManager.achievementDataList[i].value = 31;
                    DemoDataManager.achievementDataList[i].rewardcount += 5;
                    DemoDataManager.achievementDataList[i].rewardcount2 += 500;
                }
                if (progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.3 만들기" || progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.4 만들기" ||
                    progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.3 만들기")
                {
                    if (DemoDataManager.achievementDataList[i].level == 2)
                        DemoDataManager.achievementDataList[i].value = 5;
                    if (DemoDataManager.achievementDataList[i].level == 3)
                        DemoDataManager.achievementDataList[i].value = 10;
                    if (DemoDataManager.achievementDataList[i].level == 4)
                        DemoDataManager.achievementDataList[i].value = 17;
                    DemoDataManager.achievementDataList[i].rewardcount += 5;
                    DemoDataManager.achievementDataList[i].rewardcount2 += 500;
                }
            }
        }
        if (progressDataList[6 * thisPage + rewardBtNumber].name == "멸치를 모아봐요")
        {
            DemoDataManager.achievementDataList[0].progressvalue = progressDataList[6 * thisPage + rewardBtNumber].progressvalue - progressDataList[6 * thisPage + rewardBtNumber].value;
            DemoDataManager.achievementDataList[0].value += 5000;
            DemoDataManager.achievementDataList[0].rewardcount += progressDataList[6 * thisPage + rewardBtNumber].level * (progressDataList[6 * thisPage + rewardBtNumber].level - 1);
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "진주를 모아봐요")
        {
            DemoDataManager.achievementDataList[1].progressvalue = progressDataList[6 * thisPage + rewardBtNumber].progressvalue - progressDataList[6 * thisPage + rewardBtNumber].value;
            DemoDataManager.achievementDataList[1].value += 20;
            DemoDataManager.achievementDataList[1].rewardcount += 1000;
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "스테이지 클리어")
        {
            DemoDataManager.achievementDataList[2].value = (progressDataList[6 * thisPage + rewardBtNumber].level - 1) * 10;
            DemoDataManager.achievementDataList[2].rewardcount += 10;
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "헬멧을 모아봐요")
        {
            DemoDataManager.achievementDataList[3].value += 5;
            DemoDataManager.achievementDataList[3].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "상의를 모아봐요")
        {
            DemoDataManager.achievementDataList[4].value += 6;
            DemoDataManager.achievementDataList[4].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "하의를 모아봐요")
        {
            DemoDataManager.achievementDataList[5].value += 5;
            DemoDataManager.achievementDataList[5].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "무기를 모아봐요")
        {
            DemoDataManager.achievementDataList[6].value += 5;
            if (DemoDataManager.achievementDataList[6].level == 3)
                DemoDataManager.achievementDataList[6].value = 17;
            DemoDataManager.achievementDataList[6].rewardname = "진주보석함무기";
        }
    }
    void InputData() //진행 업적과 완료 업적으로 나눠서 데이터 넣기
    {
        progressDataList.Clear();
        completeDataList.Clear();
        for (int i = 0; i < DemoDataManager.achievementDataList.Count; i++)
        {
            if (DemoDataManager.achievementDataList[i].maxlevel >= DemoDataManager.achievementDataList[i].level)
                progressDataList.Add(new ProgressData(DemoDataManager.achievementDataList[i].name, DemoDataManager.achievementDataList[i].rewardname, DemoDataManager.achievementDataList[i].rewardname2,
                    DemoDataManager.achievementDataList[i].level, DemoDataManager.achievementDataList[i].maxlevel, DemoDataManager.achievementDataList[i].progressvalue,
                    DemoDataManager.achievementDataList[i].rewardcount, DemoDataManager.achievementDataList[i].rewardcount2, DemoDataManager.achievementDataList[i].value));
            else
                completeDataList.Add(new CompleteData(DemoDataManager.achievementDataList[i].name, DemoDataManager.achievementDataList[i].rewardname, DemoDataManager.achievementDataList[i].rewardname2,
                    DemoDataManager.achievementDataList[i].level, DemoDataManager.achievementDataList[i].maxlevel, DemoDataManager.achievementDataList[i].rewardcount, DemoDataManager.achievementDataList[i].rewardcount2,
                    DemoDataManager.achievementDataList[i].value));
        }
    }
    public class ProgressData
    {
        public ProgressData(string _name, string _rewardname, string _rewardname2, int _level, int _maxlevel, int _progressvalue, int _rewardcount, int _rewradcount2, int _value)
        {
            name = _name; value = _value; rewardname = _rewardname; rewardname2 = _rewardname2; rewardcount = _rewardcount; rewardcount2 = _rewradcount2;
            maxlevel = _maxlevel; level = _level; progressvalue = _progressvalue;
        }
        public string name, rewardname, rewardname2;
        public int level, maxlevel, rewardcount, rewardcount2, value, progressvalue;
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
