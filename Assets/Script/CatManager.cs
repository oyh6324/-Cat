using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatManager : MonoBehaviour
{
    //이름과 경험치 바
    public Slider expSliberBar;
    public Text expSliderBarTx;
    public Text levelTx;
    public Text playerNameTx;
    public Animator catAnim; //고양이 애니메이터

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
        expSliberBar.value = (float)DataManager.characterDatasList[0].exp / (float)DataManager.characterDatasList[0].totalexp;
        expSliderBarTx.text = DataManager.characterDatasList[0].exp.ToString() + "/" + DataManager.characterDatasList[0].totalexp.ToString();
        levelTx.text = "Lv." + DataManager.characterDatasList[0].level;
        playerNameTx.text = DataManager.characterDatasList[0].name;
    }
    void Update()
    {
        if (DataManager.characterDatasList[0].helmet == "")
            helmetImg.gameObject.SetActive(false);
        if (DataManager.characterDatasList[0].top == "")
            topImg.gameObject.SetActive(false);
        if (DataManager.characterDatasList[0].bottoms == "")
            bottomsImg.gameObject.SetActive(false);
        for (int i = 0; i < DataManager.allClothesItemList.Count; i++)
        {
            if (DataManager.characterDatasList[0].helmet == DataManager.allClothesItemList[i].name)
            {
                helmetImg.sprite = clothesSprite[i];
                helmetImg.gameObject.SetActive(true);
            }
            if (DataManager.characterDatasList[0].top == DataManager.allClothesItemList[i].name)
            {
                topImg.sprite = clothesSprite[i];
                topImg.gameObject.SetActive(true);
            }
            if (DataManager.characterDatasList[0].bottoms == DataManager.allClothesItemList[i].name)
            {
                bottomsImg.sprite = clothesSprite[i];
                bottomsImg.gameObject.SetActive(true);
            }
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
        yield return new WaitForSeconds(1.5f);
        catAnim.SetBool("isCatClick", false);
    }

    //test
    //test2
}
