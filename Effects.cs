using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effects : MonoBehaviour
{

    float startSize;
    Color myGrey;
    public AudioClip sfx;
    public string type;
    public bool destruction;

    void Start()
    {
        if (type == "flash")
        {
            myGrey = new Color(.4f, .4f, .4f);
            GetComponent<Image>().color = myGrey;
        }
        if (type == "score display" || type == "plus num")
        {
            startSize = GetComponent<Text>().fontSize;
        }
        if (type == "plus num")
        {
            GetComponent<Text>().fontSize = Mathf.RoundToInt(GetComponent<Text>().fontSize * .3f);
        }

    }
    void Update()
    {
        if (type == "score display")
        {
            GetComponent<Text>().fontSize = Mathf.RoundToInt(Mathf.Lerp(GetComponent<Text>().fontSize, startSize, 0.12f));
        }
        if (type == "cross")
        {
            transform.localScale = Vector3.Lerp(transform.localScale, (transform.localScale * 1.05f), 0.1f);
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.clear, 0.05f);
        }
        if (type == "plus num")
        {
            Destroy(gameObject, 3f);
            Vector3 pos = GetComponent<RectTransform>().transform.position;
            RectTransform parentRec = transform.parent.GetComponent<RectTransform>();
            GetComponent<RectTransform>().transform.position = Vector3.LerpUnclamped(pos, parentRec.transform.position, .05f);
            GetComponent<Text>().fontSize = Mathf.RoundToInt(Mathf.Lerp(GetComponent<Text>().fontSize, startSize, .2f));
            GetComponent<Text>().color = Color.Lerp(GetComponent<Text>().color, Color.clear, .03f);
        }
        if (type == "flash")
        {
            Color myColor = GetComponent<Image>().color;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            if (!destruction)
            {
                GetComponent<Image>().color = Color.LerpUnclamped(myColor, Color.clear, 0.08f);
            }
            else
            {
                GetComponent<Image>().color = Color.LerpUnclamped(myColor, Color.black, 0.02f);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == "menu")
        {
            ChooseRandColor();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (type == "menu")
        {
            GameObject child = gameObject.transform.GetChild(0).GetChild(1).gameObject;
            child.GetComponent<Image>().fillAmount += 0.0225f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (type == "menu")
        {
            //myGrey = new Color(.4f, .4f, .4f);
        }
    }
    void ChooseRandColor()
    {
        if (type == "menu")
        {
            iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)1);
            AudioSource.PlayClipAtPoint(sfx, transform.position);
            //List<Color32> colors = GameObject.FindGameObjectWithTag("GameController").GetComponent<InfiniteController>().colorPool;
            //myGrey = colors[Random.Range(0, colors.Count)];
        }
    }
}
