using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public GameObject prefabsBoxMap;
    public int row;
    public int col;
    public int weight;
    public Transform boxManager;
    private Box[,] arrBox = new Box[9, 9];
    private Vector2[] PosGolds = new Vector2[] { new Vector2(3, 3), new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 0), new Vector2(1, 2), new Vector2(2, 2), new Vector2(2, 3), new Vector2(2, 4), new Vector2(2, 5), new Vector2(2, 6), new Vector2(3, 6), new Vector2(4, 1), new Vector2(4, 2), new Vector2(4, 3), new Vector2(4, 4), new Vector2(4, 5), new Vector2(4, 6), new Vector2(5, 3), new Vector2(5, 4) };

    public GameObject[] prefabsBoxBuildMap;
    public Sprite[] sprBoxMap;
    public List<Box> lsPathFinding = new List<Box>();
    public LineRenderer lineRendererPath;

    public void Start()
    {
        GenerateMapBox();
    }

    public void GenerateMapBox()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Box b = Instantiate(prefabsBoxMap, boxManager.position + new Vector3(j * weight, -i * weight), Quaternion.identity, boxManager).GetComponent<Box>();
                b.col = j;
                b.row = i;
                arrBox[j, i] = b;
                if (!CheckPos(i, j))
                {
                    b.transform.GetChild(0).gameObject.SetActive(false);
                    b.isLock = true;
                }
            }
        }
    }


    public bool CheckPos(int row, int col)
    {
        bool isCheck = false;
        foreach (Vector2 v2 in PosGolds)
        {
            if (v2 == new Vector2(col, row))
            {
                isCheck = true;
            }
        }
        return isCheck;
    }

    public void GenerateMap(Transform toPos, bool isGoldPlayer = false)
    {
        int a = (int)UnityEngine.Random.Range(0, 3.9f);
        int b = UnityEngine.Random.Range(0, 4);
        Vector3 _rotation;
        if (b == 0)
        {
            _rotation = new Vector3(0, 0, 0);
        }
        else if (b == 1)
        {
            _rotation = new Vector3(180, 0, 0);
        }
        else if (b == 2)
        {
            _rotation = new Vector3(0, 180, 0);
        }
        else
        {
            _rotation = new Vector3(180, 180, 0);
        }

        if (a == 3)
            _rotation = new Vector3(0, 0, 0);
        if (isGoldPlayer)
        {
            _rotation = new Vector3(0, 0, 0);
            GoldMine g = Instantiate(prefabsBoxBuildMap[3], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
        }
        else
        {
            GoldMine g = Instantiate(prefabsBoxBuildMap[a], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
            g.Canvas.GetComponent<RectTransform>().localRotation = Quaternion.Euler(_rotation);
        }
    }

    public void PathFinding()
    {
        Box boxStart = arrBox[1, 0];
        Box boxEnd = arrBox[4, 6];
        Box boxNext = boxStart;
        lsPathFinding.Add(boxStart);
        while (boxNext != boxEnd)
        {
            Box boxCheck = CheckBoxNext(boxNext, boxEnd);
            if (boxCheck != null)
            {
                boxNext = boxCheck;
                lsPathFinding.Add(boxNext);
            }
            else
            {
                lsPathFinding.RemoveAt(lsPathFinding.Count - 1);
                boxNext = lsPathFinding[lsPathFinding.Count - 1];
            }
        }

        if (boxNext == boxEnd)
        {
            lineRendererPath.positionCount = lsPathFinding.Count;
            for (int i = lsPathFinding.Count - 1; i >= 0; i--)
            {
                lineRendererPath.SetPosition(lsPathFinding.Count - 1 - i, lsPathFinding[i].transform.position);
            }
        }
    }

    public Box CheckBoxNext(Box box, Box boxEnd)
    {
        List<Box> lsBoxSelect = new List<Box>();
        List<int> lsPosBox = new List<int>();
        if (box.col != 9 && !box.isTop && !arrBox[box.col + 1, box.row].isLock)
        {
            lsBoxSelect.Add(arrBox[box.col + 1, box.row]);
            lsPosBox.Add(1);
        }
        if (box.row != 9 && !box.isRight && !arrBox[box.col, box.row + 1].isLock)
        {
            lsBoxSelect.Add(arrBox[box.col, box.row + 1]);
            lsPosBox.Add(4);
        }
        if (box.col != 0 && !box.isBottom && !arrBox[box.col - 1, box.row].isLock)
        {
            lsBoxSelect.Add(arrBox[box.col - 1, box.row]);
            lsPosBox.Add(2);
        }
        if (box.row != 0 && !box.isLeft && !arrBox[box.col, box.row - 1].isLock)
        {
            lsBoxSelect.Add(arrBox[box.col, box.row - 1]);
            lsPosBox.Add(3);
        }

        if (lsBoxSelect.Count > 0)
        {
            int check = 0;

            float dis = Vector3.Distance(boxEnd.transform.position, lsBoxSelect[0].transform.position);
            for (int i = 1; i < lsBoxSelect.Count; i++)
            {
                if (dis > Vector3.Distance(boxEnd.transform.position, lsBoxSelect[i].transform.position))
                {
                    check = i;
                    dis = Vector3.Distance(boxEnd.transform.position, lsBoxSelect[i].transform.position);
                }
            }

            if (lsPosBox[check] == 1)
            {
                box.isTop = true;
                lsBoxSelect[check].isBottom = true;
            }
            else if (lsPosBox[check] == 2)
            {
                box.isBottom = true;
                lsBoxSelect[check].isTop = true;
            }
            else if (lsPosBox[check] == 3)
            {
                box.isLeft = true;
                lsBoxSelect[check].isRight = true;
            }
            else if (lsPosBox[check] == 4)
            {
                box.isRight = true;
                lsBoxSelect[check].isLeft = true;
            }
            return lsBoxSelect[check];
        }
        return null;
    }
}
