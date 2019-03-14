using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    public int col;
    public int row;
    public bool isLock;
    public bool isLeft;
    public bool isRight;
    public bool isTop;
    public bool isBottom;

    public bool isTopRight;
    public bool isTopLeft;
    public bool isBottomRight;
    public bool isBottomLeft;

    public SpriteRenderer spGround;
}
