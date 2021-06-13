﻿using System.Collections;
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
    public GameObject[] instancePostion; //보상 이미지 인스턴스의 부모
    public Image pearImg; //상단바 진주
    public Image anchovyImg; //상단바 멸치
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

    private List<ProgressData> progressDataList;
    private List<CompleteData> completeDataList;

    void Awake()
    {
        progressDataList = new List<ProgressData>();
        completeDataList = new List<CompleteData>();
    }
    void OnEnable()
    {
        bgmAS.clip = achievementBgmClip;
        bgmAS.Play();
        isProgressPage = true;
        CategoryChange(progressBt, completeBt);
        PageChange();
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
    void RewardSetting(int i, int spriteIndex, string rewardName) //보상 이미지 및 텍스트 셋팅
    {
        reward1Img[i].sprite = reward1Sprite[spriteIndex];
        progressRewardTx[i].gameObject.SetActive(true);

        if (rewardName != "멸치" && rewardName != "진주")
        {
            reward1Img[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f);
            reward1Img[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50f);


            progressRewardTx[i].transform.position = new Vector2(progressRewardTx[i].transform.position.x, reward1Img[i].transform.position.y);
            progressRewardTx[i].rectTransform.offsetMax = new Vector2(0, progressRewardTx[i].rectTransform.offsetMax.y);
            progressRewardTx[i].rectTransform.offsetMin = new Vector2(130, progressRewardTx[i].rectTransform.offsetMin.y);
        }
        else
        {
            reward1Img[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70f);
            reward1Img[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70f);

            progressRewardTx[i].transform.position = new Vector2(progressRewardTx[i].transform.position.x, reward1Img[i].transform.position.y);
            progressRewardTx2[i].transform.position = new Vector2(progressRewardTx2[i].transform.position.x, reward2Img[i].transform.position.y);
            progressRewardTx[i].rectTransform.offsetMax = new Vector2(0, progressRewardTx[i].rectTransform.offsetMax.y);
            progressRewardTx[i].rectTransform.offsetMin = new Vector2(100, progressRewardTx[i].rectTransform.offsetMin.y);
            progressRewardTx2[i].rectTransform.offsetMax = new Vector2(0, progressRewardTx2[i].rectTransform.offsetMax.y);
            progressRewardTx2[i].rectTransform.offsetMin = new Vector2(100, progressRewardTx2[i].rectTransform.offsetMin.y);
        }
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
                 
                    if (progressDataList[6 * thisPage + i].rewardname2 == "") //2보상 있는지 없는지 확인
                    {
                        reward1Img[i].transform.localPosition = new Vector2(reward1Img[i].transform.localPosition.x, 7);
                        reward2Img[i].gameObject.SetActive(false);
                        progressRewardTx2[i].gameObject.SetActive(false);
                    }
                    else //있다면 2 보상 오픈
                    {
                        progressRewardTx2[i].text = "X" + progressDataList[6 * thisPage + i].rewardcount2; //보상 개수
                        reward1Img[i].transform.localPosition = new Vector2(reward1Img[i].transform.localPosition.x, 47);
                        reward2Img[i].gameObject.SetActive(true);
                        progressRewardTx2[i].gameObject.SetActive(true);
                    }

                    //보상 이미지 씌우기
                    if (progressDataList[6 * thisPage + i].rewardname == "멸치")
                        RewardSetting(i, 0, "멸치");
                    if (progressDataList[6 * thisPage + i].rewardname == "진주")
                        RewardSetting(i, 1, "진주");
                    if (progressDataList[6 * thisPage + i].rewardname == "멸치보석함의상")
                        RewardSetting(i, 2, "멸치보석함의상");
                    if (progressDataList[6 * thisPage + i].rewardname == "진주보석함의상")
                        RewardSetting(i, 3, "진주보석함의상");
                    if (progressDataList[6 * thisPage + i].rewardname == "멸치보석함무기")
                        RewardSetting(i, 4, "멸치보석함무기");
                    if (progressDataList[6 * thisPage + i].rewardname == "진주보석함무기")
                        RewardSetting(i, 5, "진주보석함무기");

                    for (int j=0; j<DemoDataManager.Instance.achievementDataList.Count; j++) //업적 이미지 적용
                    {
                        if (progressDataList[6 * thisPage + i].name == DemoDataManager.Instance.achievementDataList[j].name)
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
                    progressRewardTx[i].gameObject.SetActive(false);
                    progressBar[i].value = 1;
                    progressCountTx[i].text = "Max";
                    lockBtImg[i].gameObject.SetActive(true); //버튼 잠금

                    for (int j = 0; j < DemoDataManager.Instance.achievementDataList.Count; j++) //이미지 적용
                    {
                        if (completeDataList[6 * thisPage + i].name == DemoDataManager.Instance.achievementDataList[j].name)
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
    void RewardMoveManager(int rewardBtNumber,string rewardName1,string rewardName2)
    {
        Image rewardInstace1 = Instantiate(reward1Img[rewardBtNumber],reward1Img[rewardBtNumber].transform.parent); //복제
        rewardInstace1.transform.SetParent(instancePostion[rewardBtNumber].transform);
        InputInstanceMove(rewardInstace1);
        //튀는 효과
        int radom = Random.Range(-10000, 10000);
        rewardInstace1.GetComponent<Rigidbody2D>().gravityScale = 50;
        rewardInstace1.GetComponent<Rigidbody2D>().AddForce(new Vector2(radom, 15000));

        if (rewardName2 != "")
        {
            Image rewardInstace2 = Instantiate(reward2Img[rewardBtNumber + 1], reward2Img[rewardBtNumber].transform.parent); //복제
            rewardInstace2.transform.parent = instancePostion[rewardBtNumber].transform;
            InputInstanceMove(rewardInstace2);
            //튀는 효과
            radom = Random.Range(-8000, 8000);
            rewardInstace2.GetComponent<Rigidbody2D>().gravityScale = 50;
            rewardInstace2.GetComponent<Rigidbody2D>().AddForce(new Vector2(radom, 15000));
        }
        lockBtImg[rewardBtNumber].color = new Color(lockBtImg[rewardBtNumber].color.r, lockBtImg[rewardBtNumber].color.g, lockBtImg[rewardBtNumber].color.b,
            0f);
        lockBtImg[rewardBtNumber].gameObject.SetActive(true);
        if (rewardName1 != "멸치" && rewardName1 != "진주")
        {
            reward1Img[rewardBtNumber].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70);
            reward1Img[rewardBtNumber].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
            progressRewardTx[rewardBtNumber].gameObject.SetActive(true);
        }
        InputData();
        PageChange();
    }
    void InputInstanceMove(Image instance)
    {
        instance.gameObject.AddComponent<InstanceMove>();
        instance.gameObject.GetComponent<InstanceMove>().pearlAnim = pearlAnim;
        instance.gameObject.GetComponent<InstanceMove>().anchovyAnim = anchovyAnim;
        instance.gameObject.GetComponent<InstanceMove>().pearlPosition = pearImg.transform.position;
        instance.gameObject.GetComponent<InstanceMove>().anchovyPostion = anchovyImg.transform.position;
        instance.gameObject.GetComponent<InstanceMove>().rewardSprite = reward1Sprite;
    }
    void RewardGetAndDataSetting(int rewardBtNumber)
    {
        pearlAnim.SetBool("isGet", false);
        anchovyAnim.SetBool("isGet", false);
        soundEffectAS.clip = rewardGetClip;
        soundEffectAS.Play();

        for (int i = 0; i < DemoDataManager.Instance.moneyItemList.Count; i++) //보상 주기
        {
            if (DemoDataManager.Instance.moneyItemList[i].name.Equals(progressDataList[6 * thisPage + rewardBtNumber].rewardname))
                DemoDataManager.Instance.moneyItemList[i].count += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;
        }
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname2 != "") //다른 보상 주기
        {
            DemoDataManager.Instance.moneyItemList[0].count += progressDataList[6 * thisPage + rewardBtNumber].rewardcount2;
            DemoDataManager.Instance.achievementDataList[0].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount2;
        }
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname == "멸치") //업적 연동
            DemoDataManager.Instance.achievementDataList[0].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;
        if (progressDataList[6 * thisPage + rewardBtNumber].rewardname == "진주")
            DemoDataManager.Instance.achievementDataList[1].progressvalue += progressDataList[6 * thisPage + rewardBtNumber].rewardcount;

        //데이터 바꾸기
        for (int i = 0; i < DemoDataManager.Instance.achievementDataList.Count; i++)
        {
            if (DemoDataManager.Instance.achievementDataList[i].name.Equals(progressDataList[6 * thisPage + rewardBtNumber].name))
            {
                DemoDataManager.Instance.achievementDataList[i].level += 1;
                progressDataList[6 * thisPage + rewardBtNumber].level = DemoDataManager.Instance.achievementDataList[i].level;
                if (progressDataList[6 * thisPage + rewardBtNumber].name == "의상 Lv.3 만들기" || progressDataList[6 * thisPage + rewardBtNumber].name == "의상 Lv.4 만들기" ||
                    progressDataList[6 * thisPage + rewardBtNumber].name == "의상 Lv.5 만들기")
                {
                    if (DemoDataManager.Instance.achievementDataList[i].level == 2)
                        DemoDataManager.Instance.achievementDataList[i].value = 5;
                    if (DemoDataManager.Instance.achievementDataList[i].level == 3)
                        DemoDataManager.Instance.achievementDataList[i].value = 10;
                    if (DemoDataManager.Instance.achievementDataList[i].level == 4)
                        DemoDataManager.Instance.achievementDataList[i].value = 20;
                    if (DemoDataManager.Instance.achievementDataList[i].level == 5)
                        DemoDataManager.Instance.achievementDataList[i].value = 31;
                    DemoDataManager.Instance.achievementDataList[i].rewardcount += 5;
                    DemoDataManager.Instance.achievementDataList[i].rewardcount2 += 500;
                }
                if (progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.3 만들기" || progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.4 만들기" ||
                    progressDataList[6 * thisPage + rewardBtNumber].name == "무기 Lv.5 만들기")
                {
                    if (DemoDataManager.Instance.achievementDataList[i].level == 2)
                        DemoDataManager.Instance.achievementDataList[i].value = 5;
                    if (DemoDataManager.Instance.achievementDataList[i].level == 3)
                        DemoDataManager.Instance.achievementDataList[i].value = 10;
                    if (DemoDataManager.Instance.achievementDataList[i].level == 4)
                        DemoDataManager.Instance.achievementDataList[i].value = 17;
                    DemoDataManager.Instance.achievementDataList[i].rewardcount += 5;
                    DemoDataManager.Instance.achievementDataList[i].rewardcount2 += 500;
                }
            }
        }
        if (progressDataList[6 * thisPage + rewardBtNumber].name == "멸치를 모아봐요")
        {
            DemoDataManager.Instance.achievementDataList[0].progressvalue = progressDataList[6 * thisPage + rewardBtNumber].progressvalue - progressDataList[6 * thisPage + rewardBtNumber].value;
            DemoDataManager.Instance.achievementDataList[0].value += 5000;
            DemoDataManager.Instance.achievementDataList[0].rewardcount += progressDataList[6 * thisPage + rewardBtNumber].level * (progressDataList[6 * thisPage + rewardBtNumber].level - 1);
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "진주를 모아봐요")
        {
            DemoDataManager.Instance.achievementDataList[1].progressvalue = progressDataList[6 * thisPage + rewardBtNumber].progressvalue - progressDataList[6 * thisPage + rewardBtNumber].value;
            DemoDataManager.Instance.achievementDataList[1].value += 20;
            DemoDataManager.Instance.achievementDataList[1].rewardcount += 1000;
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "스테이지 클리어")
        {
            DemoDataManager.Instance.achievementDataList[2].value = (progressDataList[6 * thisPage + rewardBtNumber].level - 1) * 10;
            DemoDataManager.Instance.achievementDataList[2].rewardcount += 10;
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "헬멧을 모아봐요")
        {
            DemoDataManager.Instance.achievementDataList[3].value += 5;
            DemoDataManager.Instance.achievementDataList[3].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "상의를 모아봐요")
        {
            DemoDataManager.Instance.achievementDataList[4].value += 6;
            DemoDataManager.Instance.achievementDataList[4].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "하의를 모아봐요")
        {
            DemoDataManager.Instance.achievementDataList[5].value += 5;
            DemoDataManager.Instance.achievementDataList[5].rewardname = "진주보석함의상";
        }
        else if (progressDataList[6 * thisPage + rewardBtNumber].name == "무기를 모아봐요")
        {
            DemoDataManager.Instance.achievementDataList[6].value += 5;
            if (DemoDataManager.Instance.achievementDataList[6].level == 3)
                DemoDataManager.Instance.achievementDataList[6].value = 17;
            DemoDataManager.Instance.achievementDataList[6].rewardname = "진주보석함무기";
        }

        RewardMoveManager(rewardBtNumber, progressDataList[6 * thisPage + rewardBtNumber].rewardname, progressDataList[6 * thisPage + rewardBtNumber].rewardname2);
    }
    void InputData() //진행 업적과 완료 업적으로 나눠서 데이터 넣기
    {
        progressDataList.Clear();
        completeDataList.Clear();
        for (int i = 0; i < DemoDataManager.Instance.achievementDataList.Count; i++)
        {
            if (DemoDataManager.Instance.achievementDataList[i].maxlevel >= DemoDataManager.Instance.achievementDataList[i].level)
                progressDataList.Add(new ProgressData(DemoDataManager.Instance.achievementDataList[i].name, DemoDataManager.Instance.achievementDataList[i].rewardname, DemoDataManager.Instance.achievementDataList[i].rewardname2,
                    DemoDataManager.Instance.achievementDataList[i].level, DemoDataManager.Instance.achievementDataList[i].maxlevel, DemoDataManager.Instance.achievementDataList[i].progressvalue,
                    DemoDataManager.Instance.achievementDataList[i].rewardcount, DemoDataManager.Instance.achievementDataList[i].rewardcount2, DemoDataManager.Instance.achievementDataList[i].value));
            else
                completeDataList.Add(new CompleteData(DemoDataManager.Instance.achievementDataList[i].name, DemoDataManager.Instance.achievementDataList[i].rewardname, DemoDataManager.Instance.achievementDataList[i].rewardname2,
                    DemoDataManager.Instance.achievementDataList[i].level, DemoDataManager.Instance.achievementDataList[i].maxlevel, DemoDataManager.Instance.achievementDataList[i].rewardcount, DemoDataManager.Instance.achievementDataList[i].rewardcount2,
                    DemoDataManager.Instance.achievementDataList[i].value));
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
