using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoCollectionManager : MonoBehaviour
{
    public Text titleTx; //스테이지 텍스트
    public Image bossPictureImg; //보스 사진
    public Text bossNameTx; //보스 이름
    public Text bossExTx; //보스 설명
    public Image bossLockImg; //보스 잠금 이미지
    public Image bossNewAlarmImg; //보스 new 이미지
    public Button bossBt; //보스 버튼
    public Image[] pictureImg; //몹 이미지
    public Text[] pictureNameTx; //몹 이름
    public Text[] pictureExTx; //몹 설명
    public Image[] pictureLockImg; //몹 잠금 이미지
    public Image[] pictureNewAlarmImg; //몹 new 이미지
    public Button[] pictureBt; //몹 버튼
    public Sprite[] monsterSprite; //몬스터 스프라이트
    public Image[] anchovyImg; //보상받을 때 이미지
    public Animator anchovyAnim;

    //오디오
    public AudioSource bgmAS;
    public AudioSource soundEffectAS;
    public AudioClip buttonClickClip; //버튼 음
    public AudioClip rewardGetClip; //보상 음
    public AudioClip collectionBgmClip; //도감 bgm

    private string[] stageStr = { "1~5 Stage", "6~10 Stage", "11~15 Stage", "16~20 Stage", "21~25 Stage", "26~30 Stage" };
    private List<monsterData> monsterDataList; //몬스터 정보 리스트
    private int thisPage; //현재 페이지

    void Awake()
    {
        monsterDataList = new List<monsterData>();
    }
    void OnEnable()
    {
        bgmAS.clip = collectionBgmClip;
        bgmAS.Play(); //bgm play
        thisPage = 0; //현재 페이지 초기화
        PageChage();
    }
    public void RightBtClick() //페이지 오른쪽 넘기기
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (thisPage == 5) //마지막 페이지일 때
            thisPage = 0; //처음으로 돌아가기
        else //마지막 페이지가 아닐 때
            thisPage++; //넘기기
        PageChage();
    }
    public void LeftBtClick() //페이지 왼쪽 넘기기
    {
        soundEffectAS.clip = buttonClickClip;
        soundEffectAS.Play();
        if (thisPage == 0) //첫 페이지일 때
            thisPage = 5; //마지막으로 돌아가기
        else //첫 페이지 아닐 때
            thisPage--; //왼쪽 넘기기
        PageChage();
    }
    public void BossBtClick() //보스 이미지 클릭
    {
        StartCoroutine(rewardAnimation(0));
        DemoDataManager.Instance.moneyItemList[0].count += 50; //게임 머니에 보상 추가
        DemoDataManager.Instance.achievementDataList[0].progressvalue += 50; //업적 연동
        for (int i = 0; i < monsterDataList.Count; i++) //보스 정보 찾기
        {
            if (monsterDataList[i].stage == (thisPage + 1) * 5) //보스 보상 확인
                DemoDataManager.Instance.monsterCollectionDataList[i].isreward = true;
        }
        PageChage();
    }

    //몬스터 이미지 클릭
    public void Picture0BtClick()
    {
        StartCoroutine(rewardAnimation(1));
        PictureReward(0);
    }
    public void Picture1BtClick()
    {
        StartCoroutine(rewardAnimation(2));
        PictureReward(1);
    }
    public void Picture2BtClick()
    {
        StartCoroutine(rewardAnimation(3));
        PictureReward(2);
    }
    public void Picture3BtClick()
    {
        StartCoroutine(rewardAnimation(4));
        PictureReward(3);
    }
    void PictureReward(int index) //보스 제외 몬스터 보상
    {
        DemoDataManager.Instance.moneyItemList[0].count += 30; //게임 머니 보상 추가
        DemoDataManager.Instance.achievementDataList[0].progressvalue += 30; //업적 연동
        //몬스터 보상 확인
        for (int i = 0; i < monsterDataList.Count; i++)
        {
            if (monsterDataList[i].name == pictureNameTx[index].text)
                DemoDataManager.Instance.monsterCollectionDataList[i].isreward = true;
        }
        PageChage();
    }
    IEnumerator rewardAnimation(int number) //보상 애니메이션
    {
        soundEffectAS.clip = rewardGetClip;
        soundEffectAS.Play();
        anchovyImg[number].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        anchovyAnim.SetBool("isGet", true);
        yield return new WaitForSeconds(0.5f);
        anchovyAnim.SetBool("isGet", false);
        anchovyImg[number].gameObject.SetActive(false);
    }
    void PageChage()
    {
        InputData(); //리스트에 몬스터 데이터 넣기
        titleTx.text = stageStr[thisPage]; //스테이지 텍스트
        int count = 0;

        for (int i = 0; i < monsterDataList.Count; i++)
        {
            if (monsterDataList[i].stage == (thisPage + 1) * 5) //해당 페이지의 보스 고르기
            {
                if (monsterDataList[i].stage <= DemoDataManager.Instance.characterDatasList[0].stage) //보스를 만났다면 보스 출력
                {
                    bossLockImg.gameObject.SetActive(false); //잠금 이미지 비활성화
                    bossPictureImg.sprite = monsterSprite[i]; //이미지 출력
                    bossNameTx.text = monsterDataList[i].name; //보스 이름
                    bossExTx.text = "이름: " + monsterDataList[i].realname + "\n종류: " + monsterDataList[i].kind + "\n서식지: " + monsterDataList[i].habitat; //보스 정보

                    if (monsterDataList[i].isreward) //보상을 받았다면
                    {
                        bossBt.gameObject.SetActive(false); //보상 버튼 비활성화
                        bossNewAlarmImg.gameObject.SetActive(false); //new 이미지 비활성화
                    }
                    else //보상을 받지 않았다면
                    {
                        bossBt.gameObject.SetActive(true); //보상 버튼 활성화
                        bossNewAlarmImg.gameObject.SetActive(true); //new 이미지 활성화
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
            if (monsterDataList[i].stage < (thisPage + 1) * 5 && monsterDataList[i].stage >= (thisPage * 5) + 1) //해당 페이지의 스테이지 몬스터 고르기
            {
                if (monsterDataList[i].stage <= DemoDataManager.Instance.characterDatasList[0].stage) //해당 스테이지를 갔다면 출력
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
    void InputData() //리스트에 몬스터 데이터 넣기
    {
        monsterDataList.Clear();
        for (int i = 0; i < DemoDataManager.Instance.monsterCollectionDataList.Count; i++)
            monsterDataList.Add(new monsterData(DemoDataManager.Instance.monsterCollectionDataList[i].name, DemoDataManager.Instance.monsterCollectionDataList[i].realname, DemoDataManager.Instance.monsterCollectionDataList[i].habitat,
                DemoDataManager.Instance.monsterCollectionDataList[i].kind, DemoDataManager.Instance.monsterCollectionDataList[i].stage, DemoDataManager.Instance.monsterCollectionDataList[i].isreward));
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
