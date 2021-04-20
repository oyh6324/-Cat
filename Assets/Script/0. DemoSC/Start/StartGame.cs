using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject lobbyCanvas;
    public GameObject barCanvas;
    public Image background;
    public Image inputName;
    public Image fadeImg;

    public Text nameTx;

    public Image messageImg;
    public Text messageTx;

    private FadeIO fadeIO;
    public void StartBtClick()
    {
        fadeIO = fadeImg.GetComponent<FadeIO>();
        StartCoroutine(waitForFadeIO());
    }
    public void OkBtClick()
    {
        messageImg.gameObject.SetActive(true);
        messageTx.text = nameTx.text + ", 이 이름이 맞습니까?";
    }
    public void YesBtClick()
    {
        DemoDataManager.characterDatasList[0].name = nameTx.text;
        StartCoroutine(waitForFadeIO());
    }
    public void NoBtClick()
    {
        messageImg.gameObject.SetActive(false);
    }
    IEnumerator waitForFadeIO()
    {
        fadeImg.gameObject.SetActive(true);
        fadeIO.fadeout = true;
        yield return new WaitForSeconds(fadeIO.fadeTime);
        if (DemoDataManager.characterDatasList[0].name == "User")
        {
            background.gameObject.SetActive(false);
            inputName.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
            lobbyCanvas.SetActive(true);
            barCanvas.SetActive(true);
        }
        fadeIO.fadein = true;
    }
}
