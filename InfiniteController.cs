using System.Collections.Generic;
using UnityEngine;

public class InfiniteController : MonoBehaviour
{
    public List<Color32> colorPool;
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
}
