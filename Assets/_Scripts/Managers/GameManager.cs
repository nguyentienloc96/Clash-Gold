using System;
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
    [HideInInspector]
    public bool isPlay = false;
    public long gold;
    public int coin;
    public float ratioBorn; //level game: de, trung binh, kho
    public Castle castlePlayer;
    public List<GoldMine> lstGoldMinePlayer = new List<GoldMine>();
    public List<House> lstHousePlayer = new List<House>();
    public List<BuildHouse> lstBuildHouse = new List<BuildHouse>();
    public DateTime dateEnemyAttack;
    public int maxLevelHouse;

    [Header("INFO ENEMY")]
    public List<GoldMine> lstGoldMineEnemy = new List<GoldMine>();

    public List<Hero> lsPrefabsHero;
    public List<Hero> lsPrefabsEnemy;

    public int[] lsHeroFly;
    public int[] lsHeroCanMove;

    [Header("OTHER")]
    public Camera cameraMain;
    public Transform heroManager;
    public Transform enemyManager;

    [Header("MAP")]
    public Transform[] posMap;
    public GameObject[] prefabsBoxMap;
    public Sprite[] sprBoxMap;

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
        GenerateMap();
        for (int i = 0; i < UIManager.Instance.lstHouse.Count; i++)
        {
            BuildHouse bh = new BuildHouse();
            bh.ID = i;
            if (i >= 5)
                bh.isUnlock = false;
            else
                bh.isUnlock = true;
            if (i == 8)
                bh.isUnlock = true;
            lstBuildHouse.Add(bh);
        }

        if (lstGoldMinePlayer.Count >= 2)
        {
            dateEnemyAttack = dateGame.AddDays(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlay)
        {
            time += Time.deltaTime;
            if (time >= GameConfig.Instance.Timeday)
            {
                int month = dateGame.Month;
                int year = dateGame.Year;
                dateGame = dateGame.AddDays(1f);
                SetDateUI();
                this.PostEvent(EventID.NextDay);
                time = 0;
            }

            if (lstGoldMinePlayer.Count >= 2 && dateGame >= dateEnemyAttack)
            {
                int a = UnityEngine.Random.Range(0, lstGoldMineEnemy.Count);
                this.PostEvent(EventID.EnemyAttackPlayer, a);
                dateEnemyAttack = dateGame.AddDays(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
            }

            if(lstGoldMineEnemy.Count <= 0)
            {
                UIManager.Instance.panelVictory.SetActive(true);
            }
        }
    }

    #region === MAP ===
    public void GenerateMap()
    {
        for (int i = 0; i < 20; i++)
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

            if (i == 10)
            {
                _rotation = new Vector3(0, 0, 0);
                GoldMine g = Instantiate(prefabsBoxMap[3], posMap[i].position, Quaternion.Euler(_rotation), posMap[i]).GetComponent<GoldMine>();
                g.SetLevel(1);
                g.id = i;
                g.SetInfo(GameConfig.Instance.CapGold0, GameConfig.Instance.PriceGoldUp, 1);
                g.numberBoxGoldMine = 3;
                g.typeGoleMine = TypeGoldMine.Player;
                g.InstantiateHero(true);
                lstGoldMinePlayer.Add(g);
            }
            else
            {
                GoldMine g = Instantiate(prefabsBoxMap[a], posMap[i].position, Quaternion.Euler(_rotation), posMap[i]).GetComponent<GoldMine>();
                int _level;
                if (i >= 5 && i <= 15 && i != 8 && i != 12)
                {
                    if (UnityEngine.Random.Range(0f, 1f) >= 0.25f)
                    {
                        _level = UnityEngine.Random.Range(1, 3);
                    }
                    else
                    {
                        _level = UnityEngine.Random.Range(4, 15);
                    }
                }
                else
                {
                    _level = UnityEngine.Random.Range(15, 20);
                }
                g.SetLevel(_level);
                g.id = i;
                g.numberBoxGoldMine = a;
                g.SetInfo(GameConfig.Instance.CapGold0, GameConfig.Instance.PriceGoldUp, _level);
                g.typeGoleMine = TypeGoldMine.Enemy;
                g.InstantiateHero(false);
                g.Canvas.GetComponent<RectTransform>().localRotation = Quaternion.Euler(_rotation);
                lstGoldMineEnemy.Add(g);
            }
        }
    }
    #endregion

    #region === ADD GOLD, COIN ===

    public void AddGold(long _gold)
    {
        gold += _gold;
        if (gold < 0)
        {
            gold = 0;
        }
        UIManager.Instance.txtGold.text = UIManager.Instance.ConvertNumber(gold);
    }

    public void AddCoin(int _coin)
    {
        coin += _coin;
        if (coin < 0)
        {
            coin = 0;
        }
        UIManager.Instance.txtCoin.text = UIManager.Instance.ConvertNumber(coin);
    }
    #endregion

    #region === DATE GAME ===
    public void LoadDate()
    {
        if (PlayerPrefs.GetInt(KeyPrefs.IS_CONTINUE) == 0)
        {
            dateGame = DateTime.Now;
        }
        else
        {
            dateGame = DateTime.Now;
        }
        SetDateUI();
    }

    public void SetDateUI()
    {
        txtDate.text = "Date: " + dateGame.Day.ToString("00") + "/" + dateGame.Month.ToString("00") + "/" + dateGame.Year.ToString("0000");
    }
    #endregion
}
