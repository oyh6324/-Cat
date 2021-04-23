using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIO : MonoBehaviour
{
    public Image fadeImg;
    bool isPlaying = false;
    public float fadeTime;
    public bool fadeout;
    public bool fadein;
    private void Start()
    {
        fadein = true;
    }
    private void Update()
    {
        if (fadeout == true && isPlaying == false)
        {
            StartCoroutine(FadeOut());
        }
        else if (fadeout == false && fadein == true)
        {
            StartCoroutine(FadeIn());
        }
    }
    IEnumerator FadeOut()
    {
        fadeImg.gameObject.SetActive(true);
        isPlaying = true;
        Color tempColor = fadeImg.color;
        tempColor.a = 0f;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeTime;
            fadeImg.color = tempColor;

            if (tempColor.a >= 1f)
            {
                tempColor.a = 1f;
            }
            yield return null;
        }
        fadeout = false;
    }
    IEnumerator FadeIn()
    {
        Color tempColor = fadeImg.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeTime;
            fadeImg.color = tempColor;

            if (tempColor.a <= 0f)
            {
                tempColor.a = 0f;
                fadeImg.color = tempColor;
            }
            yield return null;
        }
        fadeImg.gameObject.SetActive(false);
        fadein = false;
        isPlaying = false;
    }
}
