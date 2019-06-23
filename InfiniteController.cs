using System.Collections.Generic;
using UnityEngine;

public class InfiniteController : MonoBehaviour
{
    public List<Color32> colorPool = new List<Color32>();
    public GameObject ParentBlock;
    GameObject player;

    public bool pause;

    public int blockAmmount;
    int score;

    float decrSpawnTime;
    float timer;
    float spawnTimer;
    float increaseDiffTimer;
    float tillNextDiffInc;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tillNextDiffInc = 15;
        blockAmmount = 2;
        spawnTimer = 2.8f;
        //ResetColors();
    }

    void Update()
    {
        if (pause || Player.playerDied)
        {
            return;
        }

        score = player.GetComponent<Player>().score;
        if (score > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", score);
        }

        timer += Time.deltaTime;
        if (timer > spawnTimer)
        {
            Instantiate(ParentBlock, transform.position, Quaternion.identity);
            if (spawnTimer > 1.35)
            {
                spawnTimer = spawnTimer * (.975f - decrSpawnTime);
            }
            timer = 0;
        }
        increaseDiffTimer += Time.deltaTime;
        if (increaseDiffTimer > tillNextDiffInc)
        {
            IncreaseDifficulty();
        }
    }
    void IncreaseDifficulty()
    {
        player.GetComponent<Player>().AddColor();
        player.GetComponent<Player>().Shrink();
        blockAmmount++;
        tillNextDiffInc += 5;
        decrSpawnTime += .02f;// .8 too fast 
        spawnTimer = 2.8f;
        increaseDiffTimer = 0;
    }
    public void ResetColors()
    {
        colorPool.Add(new Color32(255, 102, 102, 255));
        colorPool.Add(new Color32(255, 172, 172, 255));
        colorPool.Add(new Color32(255, 200, 0, 255));
        colorPool.Add(new Color32(255, 160, 229, 255));
        colorPool.Add(new Color32(255, 228, 129, 255));
        colorPool.Add(new Color32(184, 196, 255, 255));
        colorPool.Add(new Color32(205, 158, 255, 255));
        colorPool.Add(new Color32(195, 255, 248, 255));
        colorPool.Add(new Color32(219, 255, 194, 255));
    }
}
