using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ParentBlockController : MonoBehaviour
{
    public GameObject block;

    public AudioClip spawn;

    public List<Color32> colors;

    public InfiniteController gameController;

    bool colorSwitch;
    bool swap;
    public bool finishedSpawning;

    public float timer;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<InfiniteController>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < player.GetComponent<Player>().possibleColors.Count; i++)
        {
            colors.Add(player.GetComponent<Player>().possibleColors[i]);
        }
        StartCoroutine(SpawnBlocks(gameController.blockAmmount, .3f));
        Destroy(gameObject, 5);
    }
    void Update()
    {
        if (Player.playerDied)
        {
            return;
        }
        if (finishedSpawning)
        {
            if (!ColorCheck() && colorSwitch == false)
            {
                FixBlockColor();
                colorSwitch = true;
            }
            MaybeSwitch();
        }
    }
    IEnumerator SpawnBlocks(int thisManyBlocks, float waitForSec)
    {
        for (int i = 0; i < thisManyBlocks; i++)
        {
            Shake.Spawn();
            GameObject thisBlock = Instantiate(block);
            SetUpBlock(thisBlock, i, thisManyBlocks);
            iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)1);
            yield return new WaitForSeconds(waitForSec);
        }
        DropParent();
        yield return null;
    }
    void SetUpBlock(GameObject thisBlock, int i, int thisManyBlocks)
    {
        AudioSource.PlayClipAtPoint(spawn, transform.position);
        ChildBlock cb = thisBlock.GetComponent<ChildBlock>();
        thisBlock.transform.parent = transform;
        cb.itteration = i;
        cb.blockAmmount = thisManyBlocks;
        cb.SetColor();
    }
    void DropParent()
    {
        finishedSpawning = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        StopCoroutine(SpawnBlocks(gameController.blockAmmount, .3f));
    }
    void MaybeSwitch()
    {
        timer += Time.deltaTime;
        if (timer > .55f)
        {
            int randChance = Random.Range(0, 4);
            if (randChance == 2 && !swap)
            {
                RandColorSwap();
                swap = true;
            }
        }
    }
    void RandColorSwap()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > 4f)
        {
            List<Vector3> childTPs = new List<Vector3>();
            List<int> vals = new List<int>();
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                childTPs.Add(child.GetComponent<ChildBlock>().targetPos);

            }
            while (vals.Count != childTPs.Count)
            {
                int randI = Random.Range(0, childTPs.Count);
                if (vals.IndexOf(randI) == -1)
                {
                    vals.Add(randI);
                }
            }

            int j = 0;
            while (j < childTPs.Count)
            {
                transform.GetChild(j).GetComponent<ChildBlock>().targetPos = childTPs[vals[j]];
                j++;
            }
        }
    }
    public void BreakAway()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject currChild = transform.GetChild(i).gameObject;
            currChild.GetComponent<BoxCollider2D>().enabled = false;
            currChild.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            int leftOrRight = Random.Range(-1, 2);
            while (leftOrRight == 0)
            {
                leftOrRight = Random.Range(-1, 2);
            }
            currChild.transform.DetachChildren();
            currChild.GetComponent<Rigidbody2D>().AddTorque(350 * leftOrRight);
            currChild.GetComponent<ChildBlock>().targetColor = Color.clear;
        }
    }
    void FixBlockColor()
    {
        int childCount = transform.childCount;
        GameObject randomChild = transform.GetChild(Random.Range(0, childCount)).gameObject;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Color32 targetColor = player.GetComponent<Player>().targetColor;

        randomChild.GetComponent<ChildBlock>().targetColor = targetColor;
    }
    bool ColorCheck()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Color32 targetColor = player.GetComponent<Player>().targetColor;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject currChild = transform.GetChild(i).gameObject;
            Color32 childColor = currChild.GetComponent<ChildBlock>().targetColor;
            if (targetColor.r == childColor.r && targetColor.g == childColor.g && targetColor.b == childColor.b)
            {
                return true;
            }
        }
        return false;
    }
}
