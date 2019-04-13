using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject partEfx;
    public GameObject cross;
    public GameObject fadder;
    GameObject heartContainer;
    GameObject renderingCam;

    InfiniteController gameController;
    BGGradientController bbg;

    Vector3 endSize;

    public List<Color32> possibleColors;
    public Color32 targetColor;

    public AudioClip scoreUp;
    public AudioClip wrongColor;

    public int score;

    public static bool playerDied;
    bool supported;

    private void Start()
    {
        Init();
    }
    void FixedUpdate()
    {
        if (playerDied)
        {
            if (fadder.GetComponent<Image>().color.a > .9)
            { // temp restart
                playerDied = false;
                SceneManager.LoadScene("menu");
            }
            return;
        }
        if (heartContainer.GetComponent<HeartContainer>().playerLivesLeft == 0)
        {
            PlayerDeath();
        }

        MoveWithPressure();
        Sparkle();
        AdjustColor();
    }
    bool ColorsMatch(Color32 colorA, Color32 colorB)
    {
        return colorA.r != colorB.r ? false : colorA.g != colorB.g ? false : colorA.b != colorB.b ? false : true;
    }
    void PlayerDeath()
    {
        fadder.GetComponent<Effects>().destruction = true;
        playerDied = true;
        GetComponent<CircleCollider2D>().enabled = false;
    }
    void AdjustColor()
    {
        Color lerpC = Color.Lerp(GetComponent<SpriteRenderer>().color, targetColor, 0.075f);
        GameObject firstChild = gameObject.transform.GetChild(0).gameObject;
        GetComponent<SpriteRenderer>().color = lerpC;
        GetComponent<TrailRenderer>().startColor = lerpC;
        GetComponent<TrailRenderer>().endColor = new Color(lerpC.r, lerpC.g, lerpC.b, .2f);
        transform.localScale = Vector3.Lerp(transform.localScale, endSize, 0.05f);
        firstChild.SetActive(GetComponent<SpriteRenderer>().enabled);
    }
    void Sparkle()
    {
        GameObject particleSys = transform.GetChild(0).gameObject;
        ParticleSystem.MainModule childPS = particleSys.GetComponent<ParticleSystem>().main;
        Color childPScolor = childPS.startColor.color;

        childPScolor = Color.Lerp(childPScolor, possibleColors[Random.Range(0, possibleColors.Count)], 0.15f);
    }
    void MoveWithPressure()
    {
        Vector3 targetPos = new Vector3(TouchInput.PressureToScreenPos(), transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, .2f);
    }
    void SpawnPart(Color32 color, Vector3 position)
    {
        GameObject thisPartEfx = Instantiate(partEfx, position, Quaternion.identity) as GameObject;
        Color32 ps = thisPartEfx.GetComponent<ParticleSystem>().main.startColor.colorMin;
        ps = color;
        Destroy(thisPartEfx, 3f);
    }
    public void Shrink()
    {
        Vector3 currentSize = transform.localScale;
        endSize = transform.localScale * .925f;
        transform.GetChild(0).transform.localScale = transform.GetChild(0).transform.localScale * .95f;
        TrailRenderer tr = gameObject.GetComponent<TrailRenderer>();
        tr.startWidth = tr.startWidth * .95f;
    }
    void Hurt(GameObject blockHitPlayer)
    {
        AudioSource.PlayClipAtPoint(wrongColor, transform.position);
        HeartContainer hearts = heartContainer.GetComponent<HeartContainer>();
        hearts.isHurt[hearts.playerLivesLeft - 1] = true;

        Shake.WrongColor();
        blockHitPlayer.GetComponent<ChildBlock>().targetColor = Color.black;
        blockHitPlayer.transform.parent.GetComponent<ParentBlockController>().BreakAway();
        fadder.GetComponent<Image>().color = Color.white * .8f;
        iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)5);
    }
    void IncreaseScore(GameObject playerHitBlock)
    {
        iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)1);
        AudioSource.PlayClipAtPoint(scoreUp, transform.position);
        playerHitBlock.GetComponent<ChildBlock>().targetColor = Color.white;
        //playerHitBlock.transform.parent.GetComponent<ParentBlockController>().BreakAway();
        //Color color = new Color(playerHitBlock.GetComponent<SpriteRenderer>().color.r,
        //                        playerHitBlock.GetComponent<SpriteRenderer>().color.g,
        //                        playerHitBlock.GetComponent<SpriteRenderer>().color.r, 0.9f);
        //fadder.GetComponent<Image>().color = color;
        bbg.Increase();
        score++;
    }
    void Init()
    {
        supported = iOSHapticFeedback.Instance.IsSupported();
        if (supported)
        {
            Debug.Log("Your device does not support iOS haptic feedback");
        }
        renderingCam = Camera.main.transform.parent.GetChild(1).gameObject;
        bbg = renderingCam.GetComponent<BGGradientController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<InfiniteController>();
        heartContainer = GameObject.FindGameObjectWithTag("Heart Container");
        endSize = transform.localScale;

        for (int i = 0; i < gameController.blockAmmount; i++)
        {
            int randomColor = Random.Range(0, gameController.colorPool.Count - 1);
            possibleColors.Add(gameController.colorPool[randomColor]);
            gameController.colorPool.Remove(gameController.colorPool[randomColor]);
        }
        targetColor = possibleColors[Random.Range(0, possibleColors.Count)];
        renderingCam.SetActive(true);
        GetComponent<SpriteRenderer>().color = Color.clear;
    }
    public void AddColor()
    {
        int randomColor = Random.Range(0, gameController.colorPool.Count - 1);
        possibleColors.Add(gameController.colorPool[randomColor]);
        gameController.colorPool.Remove(gameController.colorPool[randomColor]);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SpriteRenderer>() != null)
        {
            Color32 collColor = collision.gameObject.GetComponent<ChildBlock>().targetColor;

            if (ColorsMatch(targetColor, collColor))
            {
                IncreaseScore(collision.gameObject);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)1);
        if (collision.gameObject.GetComponent<SpriteRenderer>() != null)
        {
            Color32 collColor = collision.gameObject.GetComponent<ChildBlock>().targetColor;

            if (ColorsMatch(targetColor, collColor) == false)
            {
                Hurt(collision.gameObject);
                GameObject thisCross = Instantiate(cross, collision.contacts[0].point, cross.transform.rotation) as GameObject;
                Destroy(thisCross, 4f);
            }
        }
    }
}