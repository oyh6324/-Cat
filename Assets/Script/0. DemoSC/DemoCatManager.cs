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
        expSliberBar.value = (float)DemoDataManager.Instance.characterDatasList[0].exp / (float)DemoDataManager.Instance.characterDatasList[0].totalexp;
        expSliderBarTx.text = DemoDataManager.Instance.characterDatasList[0].exp.ToString() + "/" + DemoDataManager.Instance.characterDatasList[0].totalexp.ToString();
        levelTx.text = "Lv." + DemoDataManager.Instance.characterDatasList[0].level;
        playerNameTx.text = DemoDataManager.Instance.characterDatasList[0].name;
    }
    void Update()
    {
        if (DemoDataManager.Instance.characterDatasList[0].helmet == "")
            helmetImg.gameObject.SetActive(false);
        if (DemoDataManager.Instance.characterDatasList[0].top == "")
            topImg.gameObject.SetActive(false);
        if (DemoDataManager.Instance.characterDatasList[0].bottoms == "")
            bottomsImg.gameObject.SetActive(false);
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

        if(DemoDataManager.Instance.characterDatasList[0].helmet=="우주 헬멧")
        {
            catAnim.SetBool("isHelmet", true);
        }
        else
        {
            catAnim.SetBool("isHelmet", false);
        }
    }
    public void CatBtClick()
    {
        StartCoroutine(CatClickAniWaiting());
    }
    IEnumerator CatClickAniWaiting()
    {
        soundEffectAS.clip = catClip;
        soundEffectAS.Play();
        catAnim.SetBool("isCatClick", true);
        yield return new WaitForSeconds(1f);
        catAnim.SetBool("isCatClick", false);
    }
}
