using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataPlayer : MonoBehaviour {
    [HideInInspector]
    public long gold; //vàng
    [HideInInspector]
    public int coin; //coin
    [HideInInspector]
    public int ratioBorn; //độ khó
    [HideInInspector]
    public DateTime dateGame; //ngày trong game
    [HideInInspector]
    public Castle castlePlayer; //thành của người chơi
    [HideInInspector]
    public List<GoldMine> lstGoldMinePlayer; //list mỏ vàng người chơi
    [HideInInspector]
    public List<BuildHouse> lstBuildHouse; //nhà đã mở hay chưa
    [HideInInspector]
    public List<House> lstHouseInWall; //list nhà trong thành
    [HideInInspector]
    public List<Hero> lstHero; //list hero
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
