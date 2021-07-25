using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIO : MonoBehaviour
{
    public Image fadeImg; //페이드 인/아웃 이미지
    public float fadeTime; //페이드 인/아웃 시간
    public bool fadeout; //페이드 아웃 신호
    public bool fadein; //페이드 인 신호

    private bool isPlaying = false;

    private void Start()
    {
        fadein = true; //페이드 인 시작
    }
    private void Update()
    {
        if (fadeout == true && isPlaying == false) //페이드 아웃 실행
        {
            StartCoroutine(FadeOut());
        }
        else if (fadeout == false && fadein == true) //페이드 인 실행
        {
            StartCoroutine(FadeIn());
        }
    }
    IEnumerator FadeOut() //페이드 아웃 실행
    {
        fadeImg.gameObject.SetActive(true);
        isPlaying = true; //페이드 이미지 사용 중
        Color tempColor = fadeImg.color;
        tempColor.a = 0f; //페이드 이미지의 밝기 0
        while (tempColor.a < 1f) //밝기가 1이 되기 전까지
        {
            tempColor.a += Time.deltaTime / fadeTime; //fadeTime에 맞춰 밝기 상승
            fadeImg.color = tempColor;

            if (tempColor.a >= 1f)
            {
                tempColor.a = 1f; 
            }
            yield return null;
        }
        fadeout = false; //페이드 아웃 종료
    }
    IEnumerator FadeIn() //페이드 인 실행
    {
        Color tempColor = fadeImg.color;
        while (tempColor.a > 0f) //밝기가 0이 되기 전까지
        {
            tempColor.a -= Time.deltaTime / fadeTime; //fadeTime에 맞춰 밝기 하락
            fadeImg.color = tempColor;

            if (tempColor.a <= 0f)
            {
                tempColor.a = 0f;
                fadeImg.color = tempColor;
            }
            yield return null;
        }
        fadeImg.gameObject.SetActive(false); //페이드 이미지 비활성화 
        fadein = false; //페이드 인 종료
        isPlaying = false;
    }
}
