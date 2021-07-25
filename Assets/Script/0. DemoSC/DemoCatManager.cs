using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoCatManager : MonoBehaviour
{
    //이름과 경험치 바
    public Slider expSliberBar;
    public Text expSliderBarTx;
    public Text levelTx;
    public Text playerNameTx;
    public Animator catAnim;

    //고양이 옷
    public Image helmetImg;
    public Image topImg;
    public Image bottomsImg;
    public Sprite[] clothesSprite;

    //오디오
    public AudioSource soundEffectAS;
    public AudioClip catClip;
    void OnEnable()
    {
        expSliberBar.value = (float)DemoDataManager.Instance.characterDatasList[0].exp / (float)DemoDataManager.Instance.characterDatasList[0].totalexp; //경험치 슬라이드바 value 지정
        expSliderBarTx.text = DemoDataManager.Instance.characterDatasList[0].exp.ToString() + "/" + DemoDataManager.Instance.characterDatasList[0].totalexp.ToString(); //경험치 text 출력
        levelTx.text = "Lv." + DemoDataManager.Instance.characterDatasList[0].level; //유저 레벨
        playerNameTx.text = DemoDataManager.Instance.characterDatasList[0].name; //유저 이름
    }
    void Update()
    {
        //장비 착용 X -> 장비 이미지 비활성화
        if (DemoDataManager.Instance.characterDatasList[0].helmet == "")
            helmetImg.gameObject.SetActive(false);
        if (DemoDataManager.Instance.characterDatasList[0].top == "")
            topImg.gameObject.SetActive(false);
        if (DemoDataManager.Instance.characterDatasList[0].bottoms == "")
            bottomsImg.gameObject.SetActive(false);
        //장비 착용했을 시 장비 이미지 활성화 및 지정
        for(int i=0; i<DemoDataManager.Instance.allClothesItemList.Count; i++)
        {
            if (DemoDataManager.Instance.characterDatasList[0].helmet == DemoDataManager.Instance.allClothesItemList[i].name)
            {
                helmetImg.sprite = clothesSprite[i];
                helmetImg.gameObject.SetActive(true);
            }
            if (DemoDataManager.Instance.characterDatasList[0].top == DemoDataManager.Instance.allClothesItemList[i].name)
            {
                topImg.sprite = clothesSprite[i];
                topImg.gameObject.SetActive(true);
            }
            if (DemoDataManager.Instance.characterDatasList[0].bottoms == DemoDataManager.Instance.allClothesItemList[i].name)
            {
                bottomsImg.sprite = clothesSprite[i];
                bottomsImg.gameObject.SetActive(true);
            }
        }
        //우주 헬멧 장착 시 고양이 귀 애니메이션 중지
        if(DemoDataManager.Instance.characterDatasList[0].helmet=="우주 헬멧")
        {
            catAnim.SetBool("isHelmet", true);
        }
        else
        {
            catAnim.SetBool("isHelmet", false);
        }
    }
    public void CatBtClick() //고양이 클릭
    {
        StartCoroutine(CatClickAniWaiting());
    }
    IEnumerator CatClickAniWaiting() //고양이 클릭 시 애니메이션
    {
        soundEffectAS.clip = catClip;
        soundEffectAS.Play();
        catAnim.SetBool("isCatClick", true);
        yield return new WaitForSeconds(1f);
        catAnim.SetBool("isCatClick", false);
    }
}
