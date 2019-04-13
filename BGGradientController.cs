using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGGradientController : MonoBehaviour
{
    MF.GradientBackground gradientBackground;
    Gradient currentGradient;
    Color32 myGrey;
    public Color32 targetColor;
    Player player;
    float lerpTo;
    float lerpingTo;
    public AudioClip full;
    public int timesToBeFilled;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        myGrey = new Color32(230, 230, 230, 255);
        targetColor = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().targetColor;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gradientBackground = GetComponentInParent<MF.GradientBackground>();
        currentGradient = gradientBackground.Gradient;
        SetGradientColorKeys(currentGradient, targetColor, myGrey);
        timesToBeFilled = 5;
    }
    void Update()
    {
        if (lerpTo >= 1)
        {
            NewGColor();
        }
        IncreaseGradient(currentGradient);
    }
    void IncreaseGradient(Gradient currGradient)
    {
        Color baseColorLerped = Color.Lerp(currGradient.colorKeys[1].color, myGrey, .2f);
        Color fillColor = Color.Lerp(currGradient.colorKeys[0].color, targetColor, .05f);

        lerpingTo = Mathf.Lerp(currGradient.colorKeys[1].time, lerpTo, .1f);
        ChangeCurrentGradient(currGradient, lerpingTo, fillColor, baseColorLerped);
    }
    void ChangeCurrentGradient(Gradient currGradient, float gTimeVal, Color32 fillColor, Color32 baseColor)
    {
        currGradient.SetKeys(
            new[] { new GradientColorKey(fillColor, 0), new GradientColorKey(baseColor, gTimeVal) },
            new[] { new GradientAlphaKey(currGradient.alphaKeys[0].alpha, 0), new GradientAlphaKey(currGradient.alphaKeys[1].alpha, gTimeVal) }); gradientBackground.SetDirty();
    }
    public void IncreaseGTime(float timesToBeFilled)
    {
        float increaseBy = 1 / timesToBeFilled;
        lerpTo += increaseBy;
    }
    void NewGColor()
    {
        targetColor = player.possibleColors[Random.Range(0, player.possibleColors.Count)];
        player.targetColor = targetColor;
        timesToBeFilled = Random.Range(3, 6);
        AudioSource.PlayClipAtPoint(full, Vector3.zero);
        lerpTo = 0;
    }
    void SetGradientColorKeys(Gradient currGradient, Color32 fillColor, Color32 baseColor)
    {
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        colorKeys[0] = new GradientColorKey(fillColor, 0);
        colorKeys[1] = new GradientColorKey(baseColor, 0);
        alphaKeys[0] = new GradientAlphaKey(1, 0);
        alphaKeys[1] = new GradientAlphaKey(1, 0);
        currGradient.SetKeys(colorKeys, alphaKeys);
    }
    public void Increase()
    {
        IncreaseGTime(timesToBeFilled);
        ChangeCurrentGradient(currentGradient, lerpingTo, currentGradient.colorKeys[0].color, currentGradient.colorKeys[0].color);
    }
}
