using UnityEngine;

[System.Serializable]
public struct BoxInfo
{
    public int col;
    public int row;
    public bool isLock;
    public bool isLeft;
    public bool isRight;
    public bool isTop;
    public bool isBottom;

    public GoldMine goldMine;
}

public class Box : MonoBehaviour {
    public BoxInfo info;
    public SpriteRenderer spGround;
}
