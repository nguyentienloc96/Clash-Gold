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

    public SpriteRenderer spGround;

    public void Start()
    {
        spGround.sprite = UIManager.Instance.lsSpriteGround[Random.Range(0, UIManager.Instance.lsSpriteGround.Count)];
    }
}
