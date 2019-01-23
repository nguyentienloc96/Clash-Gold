using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataPlayer : MonoBehaviour {
    [HideInInspector]
    public long gold;
    [HideInInspector]
    public int coin;
    [HideInInspector]
    public int goldMount;
    [HideInInspector]
    public int ratioBorn;
    [HideInInspector]
    public DateTime dateGame;
    [HideInInspector]
    public Castle castlePlayer;
    [HideInInspector]
    public List<GoldMine> lstGoldMinePlayer;
    [HideInInspector]
    public List<BuildHouse> lstBuildHouse;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public struct BuildHouse
{
    public int ID;
    public bool isUnlock;
}
