using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour {
    [HideInInspector]
    public double gold;
    [HideInInspector]
    public long coin;
    [HideInInspector]
    public int goldMount;
    [HideInInspector]
    public Castle castlePlayer;
    [HideInInspector]
    public List<GoldMine> lstGoldMinePlayer;
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
