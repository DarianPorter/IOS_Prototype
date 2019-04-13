using UnityEngine;

public class ChildBlock : MonoBehaviour
{
    public int blockAmmount;
    public int itteration;
    public Vector3 targetPos;
    public Color32 targetColor;

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.clear;
        targetPos = SetPos(blockAmmount, itteration);
        transform.position = targetPos;
        SetBlockVals();
        transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 1.5f, 0);
    }
    void Update()
    {
        SetBlockVals();
    }
    public void SetBlockVals()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, SetSize(blockAmmount), .5f);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, targetColor, .25f);
        Vector3 newPos = new Vector3(targetPos.x, transform.parent.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, 0.175f);
    }
    public void SetColor()
    {
        ParentBlockController parentController = transform.parent.GetComponent<ParentBlockController>();
        int randomColor = Random.Range(0, parentController.colors.Count);
        targetColor = parentController.colors[randomColor];
        parentController.colors.RemoveAt(randomColor);
    }
    Vector3 SetSize(int ammountOfBlocks)
    {
        float widthInWorldS = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
        Vector3 size = new Vector3(widthInWorldS / ammountOfBlocks, .6f, 0);
        return size;
    }
    Vector3 SetPos(int ammountOfBlocks, int i)
    {
        int screenSpaceInterv = Screen.width / ammountOfBlocks;
        Vector3 spawnPosWS = Camera.main.WorldToScreenPoint(transform.parent.position);
        Vector3 targetP = Camera.main.ScreenToWorldPoint
                                 (new Vector3(
                                      (screenSpaceInterv / 2) + (i * screenSpaceInterv), spawnPosWS.y, Camera.main.transform.position.z * -1));
        return targetP;
    }
}
