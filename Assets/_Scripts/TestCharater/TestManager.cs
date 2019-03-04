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

    private Vector2[] PosGolds = new Vector2[] { new Vector2(3, 3), new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 0), new Vector2(1, 2), new Vector2(2, 2), new Vector2(2, 3), new Vector2(2, 4), new Vector2(2, 5), new Vector2(2, 6), new Vector2(3, 6), new Vector2(4, 1), new Vector2(4, 2), new Vector2(4, 3), new Vector2(4, 4), new Vector2(4, 5), new Vector2(4, 6), new Vector2(5, 3), new Vector2(5, 4) };

    public void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Instantiate(prefabsBoxMap, new Vector3(i * weight, -j * weight), Quaternion.identity, boxManager);
                if(Array.Find(PosGolds, sound => sound.x == col && sound.y == row) != null)
                {

                }
            }
        }
    }

}
