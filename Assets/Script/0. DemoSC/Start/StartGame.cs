using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject lobbyCanvas;
    public GameObject barCanvas;
    public GameObject tutorialCanvas;

    public Image background;
    public Image inputName;
    public Image fadeImg;

    public Text nameTx;

    public Image messageImg;
    public Text messageTx;

    //오디오
    public AudioSource bgmAS;
    public AudioSource effectAS;
    public AudioClip startBgm;
    public AudioClip startBt;
    public AudioClip clickClip;

    private FadeIO fadeIO;
    private bool tutorial;
    private void Start()
    {
        bgmAS.clip = startBgm;
        bgmAS.Play();
    }
    public void StartBtClick()
    {
        fadeIO = fadeImg.GetComponent<FadeIO>();
        StartCoroutine(waitForFadeIO());
    }
    public void OkBtClick()
    {
        effectAS.clip = clickClip;
        effectAS.Play();
        messageImg.gameObject.SetActive(true);
        messageTx.text = nameTx.text + ", 이 이름이 맞습니까?";
    }
    public void YesBtClick()
    {
        effectAS.clip = clickClip;
        effectAS.Play();

        DemoDataManager.characterDatasList[0].name = nameTx.text;

        tutorial = true;
        StartCoroutine(waitForFadeIO());
    }
    public void NoBtClick()
    {
        effectAS.clip = clickClip;
        effectAS.Play();

        messageImg.gameObject.SetActive(false);
    }
    IEnumerator waitForFadeIO()
    {
        fadeImg.gameObject.SetActive(true);
        fadeIO.fadeout = true;
        yield return new WaitForSeconds(fadeIO.fadeTime);
        if (DemoDataManager.characterDatasList[0].name == "dmlduddlekduddlqkqh")
        {
            background.gameObject.SetActive(false);
            inputName.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
            lobbyCanvas.SetActive(true);
            barCanvas.SetActive(true);

            if (tutorial)
                tutorialCanvas.SetActive(true);
        }
        fadeIO.fadein = true;
    }
}
