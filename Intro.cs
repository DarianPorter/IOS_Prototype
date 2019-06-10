using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public static bool beenPrompted;
    public GameObject textObj;

    private List<GameObject> textObjs;
    private List<string> prompts;
    private int i;
    // Use this for initialization
    void Start()
    {
        prompts.Add("Welcome");
        prompts.Add("My name is Darian Porter");
        prompts.Add("You are about to play a game I made");
        prompts.Add("I apreciate you downloading it");
        prompts.Add("This Game is all about force you apply to your screen");
        prompts.Add("as a result please dont press hard enough to break your screen.... that will be a you problem");
        prompts.Add("the game is small and a proff of concept so I will be going back and adding more later");
        prompts.Add("if you have any recomendations please feel free to contact me :)");
        prompts.Add("Instagaram, GitHub, and Email all linked on the next page");
        InstantiateText(i);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void InstantiateText(int index)
    {
        Vector3 screenMiddleWorld = Camera.main.WorldToScreenPoint(Vector3.zero);
        Vector3 newMiddle = new Vector3(
            screenMiddleWorld.x,
            screenMiddleWorld.y,
            0);
        GameObject thisText = Instantiate(textObj, newMiddle, Quaternion.identity) as GameObject;
        thisText.GetComponent<Text>().text = prompts[index];
    }
    void HandleTouch()
    {
        if (Input.touchCount > 0)
        {

        }
    }
}
