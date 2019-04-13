﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    List<Color32> colors;
    public List<GameObject> buttons;
    public GameObject buttonPanel;
    public GameObject fadder;
    public GameObject infoText;
    public GameObject title;
    public GameObject scoreText;
    GameObject fill;
    Vector2 startSize;
    Color32 color;
    Color32 myGrey;
    Color32 slightlyTransparent;
    Color32 fadeTo;
    bool iTextActive;
    bool buttonPressed;

    void Start()
    {
        for (int i = 0; i < buttonPanel.transform.childCount; i++)
        {
            GameObject button = buttonPanel.transform.GetChild(i).GetChild(0).gameObject;
            buttons.Add(button);
            startSize = button.GetComponent<RectTransform>().localScale;
        }
        string highScore = PlayerPrefs.GetInt("highScore", 0).ToString();
        title.GetComponent<Text>().text = ColorRichText("Title in Progress");
        scoreText.gameObject.GetComponent<Text>().text = ColorRichText("Best Score: " + highScore);
        myGrey = new Color32(220, 220, 220, 255);
        slightlyTransparent = new Color32(0, 0, 0, 175);
        fadder.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        fadder.GetComponent<Image>().color = myGrey;
        colors = Init.colorPool;
        color = colors[Random.Range(0, colors.Count)];
    }
    void LateUpdate()
    {
        Fade();
        FadeText();
        ResetFill();
        FillButton();
    }
    void FadeText()
    {
        Color textColor = infoText.GetComponent<Text>().color;
        infoText.GetComponent<Text>().color =
            iTextActive ? Color.Lerp(textColor, Color.white, .2f) : Color.Lerp(textColor, Color.clear, .2f);
    }
    void ResetFill()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject fill = buttons[i].transform.parent.gameObject;
            RectTransform rect = buttons[i].transform.parent.gameObject.GetComponent<RectTransform>();
            Image button = buttons[i].transform.parent.GetComponent<Image>();
            Debug.Log(rect.gameObject.name);
            fill.GetComponent<Image>().color = Color.Lerp(fill.GetComponent<Image>().color, Color.white, 0.2f);
            button.fillAmount = Mathf.Lerp(button.fillAmount, 0, 0.2f);
            rect.localScale = Vector2.Lerp(rect.localScale, startSize, 0.2f);

        }
    }
    void FillButton()
    {
        if (buttonPressed == true)
        {
            Debug.Log("hi");
            fill = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
            Image fillImg = fill.GetComponent<Image>();
            RectTransform rect = fill.GetComponent<RectTransform>();
            fill.GetComponent<Image>().color = Color.Lerp(fill.GetComponent<Image>().color, color, 0.2f);
            fillImg.fillAmount = Mathf.Lerp(fillImg.fillAmount, TouchInput.NormilizedPressure(), 0.2f);
            rect.localScale = Vector2.Lerp(rect.localScale, startSize * 1.12f, 0.2f);
            if (fillImg.fillAmount > .995f)
            {
                SceneManager.LoadScene("SurvivalV1");
            }
        }
    }
    string ColorRichText(string sentence)
    {
        List<Color32> colors = Init.colorPool;
        string modString = "";
        for (int i = 0; i < sentence.Length; i++)
        {
            string htmlColor = ColorUtility.ToHtmlStringRGBA(colors[Random.Range(0, colors.Count)]);
            string str = string.Format("<color=#{0}>{1}</color>", htmlColor, sentence[i]);
            modString += str;
        }
        Debug.Log(modString);
        return modString;
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
    public void Pressed()
    {
        buttonPressed = true;
        buttons.Remove(EventSystem.current.currentSelectedGameObject);
    }
    public void Realeased()
    {
        color = colors[Random.Range(0, colors.Count)];
        buttonPressed = false;
        buttons.Add(EventSystem.current.currentSelectedGameObject);
    }
}
