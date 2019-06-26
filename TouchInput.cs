using System.Collections;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public static Vector3 TouchInWorld()
    {
        Vector3 touchPosInWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        Vector3 spawnPos = new Vector3(touchPosInWorld.x, touchPosInWorld.y, Camera.main.transform.position.z * -1);
        return spawnPos;
    }
    public static float NormilizedPressure()
    {
        return Input.touchCount > 0 ? Input.GetTouch(0).pressure / 6.666667f : 0;
    }
    public static float PressureToScreenPos()
    {
        PlayerPrefs.SetFloat("totalPressureApplied",
                             PlayerPrefs.GetFloat("totalPressureApplied") + NormilizedPressure());

        float xVal = Screen.width * NormilizedPressure();
        float xPos = Camera.main.ScreenToWorldPoint(new Vector3(xVal, 0, 0)).x;
        return xPos;
    }
    public static float Held(float minBounds, float maxBounds, float currentVal, float addVal){
        if(Input.touchCount > 0){
            if(currentVal < maxBounds){
                return currentVal + addVal;
            }
        }else{
            if (currentVal > minBounds)
            {
                return currentVal - addVal;
            }
        }
        return currentVal;
    }
}
