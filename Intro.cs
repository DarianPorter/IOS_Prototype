using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{

    //public static bool beenPrompted;
    //public GameObject textObj;
    //public GameObject canvas;
    //private List<GameObject> textObjs;
    //private List<string> prompts;
    //private int i;
    //// Use this for initialization
    //void Start()
    //{
    //    if(!beenPrompted){
    //        canvas.SetActive(false);
    //        prompts.Add("Welcome");
    //        prompts.Add("My name is Darian Porter");
    //        prompts.Add("You are about to play a game I made");
    //        prompts.Add("I apreciate you downloading it");
    //        prompts.Add("This Game is all about force you apply to your screen");
    //        prompts.Add("Taking that into consideration, please dont press hard enough to break your screen.... that will be a you problem");
    //        prompts.Add("the game is small and a prof of concept so I will be going back and adding more later");
    //        prompts.Add("if you have any recomendations please feel free to contact me :)");
    //        prompts.Add("Instagaram, GitHub, and Email all linked on the next page");

    //        //InstantiateTeåxt(i);
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if(!beenPrompted){

    //    }

    //}
    //void InstantiateText(int index)
    //{
    //    Vector3 screenMiddleWorld = Camera.main.WorldToScreenPoint(Vector3.zero);
    //    Vector3 newMiddle = new Vector3(
    //        screenMiddleWorld.x,
    //        screenMiddleWorld.y,
    //        0);
    //    GameObject thisText = Instantiate(textObj, newMiddle, Quaternion.identity) as GameObject;
    //    thisText.GetComponent<Text>().text = prompts[index];
    //}
    //void HandleTouch()
    //{
    //    if (Input.touchCount > 0)
    //    {

    //    }
    //}
    public static bool beenPrompted;
    public MenuController MenuController;
    public GameObject fadder;
    public GameObject gameCanvas;
    public List<string> prompts;
    public Font Sandfont;
    public GameObject oldText;
    public GameObject newText;

    private void Start()
    {
        if(!beenPrompted){
            gameCanvas.SetActive(false);
            prompts.Add("Welcome!");
            prompts.Add("My name is Darian Porter");
            prompts.Add("You are about to play a game I made");
            prompts.Add("I apreciate you downloading it");
            prompts.Add("This Game is all about force you apply to your screen");
            prompts.Add("Taking that into consideration, please dont press hard enough to break your screen.... that will be a you problem");
            prompts.Add("the game is small and a prof of concept so I will be going back and adding more later");
            prompts.Add("if you have any recomendations please feel free to contact me :)");
            prompts.Add("Instagaram, GitHub, and Email all linked on the next page");
            fadder = transform.GetChild(0).gameObject;
            fadder.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            for (int i = 0; i < prompts.Count; i ++){
                InstantiateText(i);
            }
        }
    }
    private void Update()
    {
        if(Input.touchCount > 0){
            if(Input.GetTouch(0).phase == TouchPhase.Ended){
                RectTransform faderRect = fadder.GetComponent<RectTransform>();
                Vector2 lerpTo = new Vector2(
                    faderRect.anchoredPosition.x - Screen.width, 
                    faderRect.anchoredPosition.y
                );
                faderRect.anchoredPosition = Vector2.Lerp(faderRect.anchoredPosition, lerpTo, 0.05f);
            }
        }
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
