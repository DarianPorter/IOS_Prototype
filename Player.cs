using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject partEfx;
    public GameObject cross;
    public GameObject fadder;
    public Text scoreText;
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
    bool pressureSuported;

    private void Start()
    {
        Init();
    }
    void FixedUpdate()
    {
        if (playerDied)
        {
            if (fadder.GetComponent<Image>().color.a > .9)
            {
                playerDied = false;
                SceneManager.LoadScene("menu");
            }
            return;
        }
        if (heartContainer.GetComponent<HeartContainer>().playerLivesLeft == 0)
        {
            PlayerDeath();
        }
        scoreText.text = score.ToString();
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
        PlayerPrefs.SetFloat("totalGamesPlayed", PlayerPrefs.GetFloat("totalGamesPlayed") + 1);
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

        transform.Rotate(0, 0, 3f);
        childPScolor = Color.Lerp(childPScolor, possibleColors[Random.Range(0, possibleColors.Count)], 0.15f);
    }
    void MoveWithPressure()
    {
        if (pressureSuported)
        {
            Vector3 targetPos = new Vector3(TouchInput.PressureToScreenPos(), transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, .25f);
        }else{
            Vector2 bounds = GetBounds();
            Vector3 targetPos = new Vector3(TouchInput.Held(bounds.x, bounds.y, transform.position.x, 0.55f), transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, .25f);
        }
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

        PlayerPrefs.SetFloat("totalTimesHit", PlayerPrefs.GetFloat("totalTimesHit") + 1);
        Shake.WrongColor();
        blockHitPlayer.GetComponent<ChildBlock>().targetColor = Color.black;
        blockHitPlayer.transform.parent.GetComponent<ParentBlockController>().BreakAway();
        fadder.GetComponent<Image>().color = Color.white * .8f;
        iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)5);
    }
    void IncreaseScore(GameObject playerHitBlock)
    {
        PlayerPrefs.SetFloat("totalGameScore", PlayerPrefs.GetFloat("totalGameStore") + 1);
        iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)1);
        AudioSource.PlayClipAtPoint(scoreUp, transform.position);
        playerHitBlock.GetComponent<ChildBlock>().targetColor = Color.white;
        playerHitBlock.transform.parent.GetComponent<ParentBlockController>().BreakAway();
        bbg.Increase();
        score++;
    }
    void Init()
    {
        supported = iOSHapticFeedback.Instance.IsSupported();
        pressureSuported = Input.touchPressureSupported;
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
    private Vector2 CalculateScreenSizeInWorldCoords(){
        var cam = Camera.main;
        var p1 = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        var p2 = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        var p3 = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float width = (p2 - p1).magnitude;
        float height = (p3 - p2).magnitude;

        Vector2 dimensions = new Vector2(width, height);

        return dimensions;
    }

    private Vector2 GetBounds(){
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.width,1000));
        Vector2 dimensions = CalculateScreenSizeInWorldCoords();
        return new Vector2(bounds.x , bounds.x + dimensions.x);
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