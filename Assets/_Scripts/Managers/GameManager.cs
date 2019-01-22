﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = new GameManager();

    #region DateTime
    [Header("DateTime")]
    public DateTime dateGame;
    public DateTime dateStartPlay;
    public Text txtDate;
    private float time;
    #endregion

    [Header("INFO PLAYER")]
    public long gold;
    public int coin;
    public int goldMount;
    public Castle castlePlayer;
    public List<GoldMine> lstGoldMinePlayer;
    public List<House> lstHousePlayer = new List<House>();
    public List<BuildHouse> lstBuildHouse = new List<BuildHouse>();

    [Header("INFO ENEMY")]
    public List<GoldMine> lstGoldMineEnemy;

    [Header("MANAGER HERO AND ENEMY")]
    public List<Hero> lsHero;
    public List<Hero> lsEnemy;

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        LoadDate();
    }

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            BuildHouse bh = new BuildHouse();
            bh.ID = i;
            if (i >= 5)
                bh.isUnlock = false;
            else
                bh.isUnlock = true;
            lstBuildHouse.Add(bh);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= GameConfig.Instance.Timeday)
        {
            int month = dateGame.Month;
            int year = dateGame.Year;
            dateGame = dateGame.AddDays(1f);
            SetDate();
            this.PostEvent(EventID.NextDay);
            time = 0;
        }
    }

    public void AddGold(long _gold)
    {
        gold += _gold;
        if (gold < 0)
        {
            gold = 0;
        }
    }

    #region === DATE GAME ===
    public void LoadDate()
    {
        dateGame = DateTime.Now;
        SetDate();
    }

    public void SetDate()
    {
        txtDate.text = "Date: " + dateGame.Day.ToString("00") + "/" + dateGame.Month.ToString("00") + "/" + dateGame.Year.ToString("0000");
    }
    #endregion
}
