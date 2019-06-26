using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public int beenPrompted;
    public MenuController MenuController;
    public GameObject fadder;
    public GameObject gameCanvas;
    public List<string> prompts;
    public Font Sandfont;
    public GameObject oldText;
    public GameObject newText;
    public Vector2 lerpTo;
    public RectTransform faderRect;
    public int touchCount;
    public float delay;
    private void Start()
    {
        if(PlayerPrefs.HasKey("prompted") == false){
            PlayerPrefs.SetInt("prompted", 0);
        }
        beenPrompted = PlayerPrefs.GetInt("prompted");
        if (beenPrompted == 0){
            gameCanvas.SetActive(false);
            prompts.Add("Welcome!");
            prompts.Add("My name is Darian Porter.");
            prompts.Add("You are about to play a game I made.");
            if (Input.touchPressureSupported){
                prompts.Add("This Game is all about force you apply to your screen.");
                prompts.Add("The force required for playing is not substantial so dont worry you wont break your screen.");
            }else{
                prompts.Add("Apparently your device does not support Apple's Force Touch,");
                prompts.Add("That's okay, ive created an alternative playing scheme where you can hold the screen.");
                prompts.Add("Take into consideration that this is not the intended way I wanted you to play.");
            }
            prompts.Add("The game is small and a proof of concept so I will be going back and adding more later.");
            prompts.Add("If you have any recommendations of feedback please feel free to contact me :).");
            prompts.Add("Instagaram, GitHub, and Linkedin are all linked on the next page.");
            fadder = transform.GetChild(0).gameObject;
            fadder.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            for (int i = 0; i < prompts.Count; i ++){
                InstantiateText(i);
            }
        }else{
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (beenPrompted == 0)
        {
            delay += Time.deltaTime;
            faderRect = fadder.GetComponent<RectTransform>();
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended && delay > .9f)
                {
                    lerpTo = new Vector2(
                        faderRect.anchoredPosition.x - Screen.width,
                        faderRect.anchoredPosition.y
                    );
                    delay = 0;
                    touchCount++;
                }
            }
            if (touchCount >= prompts.Count)
            {
                gameCanvas.SetActive(true);
                PlayerPrefs.SetInt("prompted", 1);
            }
        }
        faderRect.anchoredPosition = Vector2.Lerp(faderRect.anchoredPosition, lerpTo, 0.22f);

    }
    private void InstantiateText(int itteration){
        fadder.GetComponent<RectTransform>().sizeDelta = new Vector2(
            (Screen.width * itteration) + Screen.width, Screen.height
        );
        GameObject text = new GameObject();
        text.transform.parent = fadder.transform;
        Text textCom = text.AddComponent<Text>();
        textCom.font = Sandfont;
        textCom.resizeTextForBestFit = true;
        textCom.alignment = TextAnchor.MiddleCenter;
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        text.GetComponent<RectTransform>().anchoredPosition = new Vector2((Screen.width * itteration) + Screen.width/2, 0);
        text.GetComponent<RectTransform>().anchorMax = new Vector2(0, .5f);
        text.GetComponent<RectTransform>().anchorMin = new Vector2(0, .5f);
        textCom.resizeTextMaxSize = 100;
        textCom.text = MenuController.ColorRichText(prompts[itteration]);
    }

}
