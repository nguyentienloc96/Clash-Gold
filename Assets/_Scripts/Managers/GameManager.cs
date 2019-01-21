using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = new GameManager();

    #region DateTime
    [Header("DateTime")]
    public DateTime dateGame;
    public DateTime dateStartPlay;
    public List<Text> listDate;
    private float time;
    #endregion

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        LoadDate();
    }

    [Header("Info Player")]
    public double gold;
    public long coin;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //time += Time.deltaTime;
        //if (time >= GameConfig.Instance.Timeday)
        //{
        //    int month = dateGame.Month;
        //    int year = dateGame.Year;
        //    dateGame = dateGame.AddDays(1f);
        //    SetDate();
        //    time = 0;
        //}
    }

    public void LoadDate()
    {
        dateGame = DateTime.Now;
        SetDate();
    }

    public void SetDate()
    {
        string dayString = dateGame.Day.ToString("00");
        listDate[0].text = dayString;

        string monthString = dateGame.Month.ToString("00");
        listDate[1].text = monthString;

        string yearString = dateGame.Year.ToString("0000");
        listDate[2].text = yearString;
    }
}
