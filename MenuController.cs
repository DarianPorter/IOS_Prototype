using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public List<GameObject> buttons;
    public GameObject buttonPanel;
    public GameObject buttonPanelButton;
    public GameObject fadder;
    public GameObject infoText;
    GameObject fill;
    RectTransform buttonPanelRect;
    Color32 myGrey;
    Color32 slightlyTransparent;
    Color32 fadeTo;
    float scrollTo;
    bool iTextActive;
    bool rightPressed;
    bool leftPressed;
    bool buttonPressed;

    void Start()
    {
        for (int i = 0; i < buttonPanel.transform.GetChild(1).childCount; i++)
        {
            GameObject button = buttonPanel.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
            buttons.Add(button);
        }
        myGrey = new Color32(230, 230, 230, 255);
        slightlyTransparent = new Color32(0, 0, 0, 125);
        fadder.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        fadder.GetComponent<Image>().color = myGrey;
        infoText.GetComponent<Text>().color = Color.clear;
        scrollTo += ScrollVal();
        buttonPanelRect = buttonPanel.GetComponent<RectTransform>();
    }
    void LateUpdate()
    {
        Fade();
        Scroll();
        FadeText();
        ResetFill();
        FillButton();
    }
    float ScrollVal()
    {
        float childWidth = buttonPanelButton.GetComponent<RectTransform>().sizeDelta.x;
        return Screen.width - childWidth / 2;
    }
    void FadeText()
    {
        Color textColor = infoText.GetComponent<Text>().color;
        infoText.GetComponent<Text>().color =
            iTextActive ? Color.Lerp(textColor, Color.white, .2f) : Color.Lerp(textColor, Color.clear, .2f);
    }
    void Scroll()
    {
        buttonPanelRect.anchoredPosition3D =
            Vector3.Lerp
            (buttonPanelRect.anchoredPosition3D,
             new Vector3(scrollTo, buttonPanelRect.anchoredPosition3D.y, 0), .25f);
    }
    void ResetFill()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Image button = buttons[i].transform.parent.GetComponent<Image>();
            button.fillAmount = Mathf.Lerp(button.fillAmount, 0, 0.2f);
        }
    }
    void FillButton()
    {
        if (buttonPressed == true)
        {
            fill = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
            Image fillImg = fill.GetComponent<Image>();
            fillImg.fillAmount = Mathf.Lerp(fillImg.fillAmount, TouchInput.NormilizedPressure(), 0.2f);
            if (fillImg.fillAmount > .995f)
            {
                SceneManager.LoadScene("SurvivalV1");
            }
        }
    }
    public void InformationTextActive()
    {
        if (iTextActive == false)
        {
            fadeTo = slightlyTransparent;
            iTextActive = true;
            fadder.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            fadeTo = Color.clear;
            iTextActive = false;
            fadder.GetComponent<Image>().raycastTarget = false;
        }
    }
    public void Fade()
    {
        Image fade = fadder.GetComponent<Image>();
        fade.color = Color.Lerp(fade.color, fadeTo, .1f);
    }
    public void ScrollLeft()
    {
        if (leftPressed == false)
        {
            scrollTo -= ScrollVal();
            leftPressed = true;
        }
        else
        {
            scrollTo += ScrollVal();
            leftPressed = false;
        }
    }
    public void ScrollRight()
    {
        if (rightPressed == false)
        {
            scrollTo -= ScrollVal();
            rightPressed = true;
        }
        else
        {
            scrollTo += ScrollVal();
            rightPressed = false;
        }
    }
    public void Pressed()
    {
        buttonPressed = true;
        buttons.Remove(EventSystem.current.currentSelectedGameObject);
    }
    public void Realeased()
    {
        buttonPressed = false;
        buttons.Add(EventSystem.current.currentSelectedGameObject);
    }
}
