using System.Collections;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    //InfiniteController gameController;

    void Start()
    {
        //gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<InfiniteController>();
    }
    string LeftOrRight()
    {
        string leftOrRight = "";
        if (Input.touchCount > 0)
        {
            float touchLocation = Input.GetTouch(0).position.x;
            leftOrRight = touchLocation > Screen.width / 2 ? "right" : "left";
        }
        return leftOrRight;
    }
    public static float NormilizedPressure()
    {
        return Input.touchCount > 0 ? Input.GetTouch(0).pressure / 6.666667f : 0;
    }
    public static float PressureToScreenPos()
    {
        float xVal = Screen.width * NormilizedPressure();
        float xPos = Camera.main.ScreenToWorldPoint(new Vector3(xVal, 0, 0)).x;
        return xPos;
    }
}
