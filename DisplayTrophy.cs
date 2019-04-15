using UnityEngine;
using UnityEngine.UI;
public class DisplayTrophy : MonoBehaviour
{

    public string trophy;
    public float outOf;
    float runningTotal;
    // pressure score deaths timesHit
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        runningTotal = PlayerPrefs.GetFloat(trophy);
        gameObject.GetComponent<Image>().fillAmount = runningTotal / outOf;
    }
}
