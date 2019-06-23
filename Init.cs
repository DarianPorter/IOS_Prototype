using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    public static List<Color32> colorPool = new List<Color32>();
    public GameObject sparkle;
    static bool init;

    private void Awake()
    {
        AddColors();
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Instantiate(sparkle, TouchInput.TouchInWorld(), Quaternion.identity);
                iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)2);
            }
        }
    }
    public void AddColors()
    {
        if(!init){
            colorPool.Add(new Color32(255, 102, 102, 255));
            colorPool.Add(new Color32(255, 172, 172, 255));
            colorPool.Add(new Color32(255, 200, 0, 255));
            colorPool.Add(new Color32(255, 160, 229, 255));
            colorPool.Add(new Color32(255, 228, 129, 255));
            colorPool.Add(new Color32(184, 196, 255, 255));
            colorPool.Add(new Color32(205, 158, 255, 255));
            colorPool.Add(new Color32(195, 255, 248, 255));
            colorPool.Add(new Color32(219, 255, 194, 255));
            init = true;
        }

    }
}
