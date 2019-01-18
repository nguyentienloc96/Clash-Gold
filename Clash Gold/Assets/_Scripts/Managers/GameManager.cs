using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = new GameManager();
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }
    [Header("Info Player")]
    public double gold;
    public long coin;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
