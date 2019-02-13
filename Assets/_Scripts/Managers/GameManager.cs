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
    public List<int> lstNumberHouseBuiled = new List<int>();
    public List<BuildHouse> lstBuildHouse = new List<BuildHouse>();
    public DateTime dateEnemyAttack;
    public int maxLevelHouse;

    [Header("INFO ENEMY")]
    public List<GoldMine> lstGoldMineEnemy = new List<GoldMine>();

    public List<Hero> lsPrefabsHero;
    public List<Hero> lsPrefabsEnemy;

    public int[] lsHeroFly = new int[] { 2, 4, 8, 9, 10, 11, 12, 17 };
    public int[] lsHeroCanMove = new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };

    [Header("OTHER")]
    public Camera cameraMain;
    public Transform heroManager;
    public Transform enemyManager;

    [Header("MAP")]
    public Transform posMapParent;
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

            if (i == 10)
            {
                GoldMine g = Instantiate(prefabsBoxMap[3], posMap[i].position, Quaternion.Euler(_rotation), posMap[i]).GetComponent<GoldMine>();
                g.SetLevel(1);
                g.numberBoxGoldMine = a;
                lstGoldMinePlayer.Add(g);
                g.typeGoleMine = TypeGoldMine.Player;
                g.InstantiateHero(true);
            }
            else
            {
                GoldMine g = Instantiate(prefabsBoxMap[a], posMap[i].position, Quaternion.Euler(_rotation), posMap[i]).GetComponent<GoldMine>();
                g.SetLevel(UnityEngine.Random.Range(0, 20));
                g.numberBoxGoldMine = a;
                lstGoldMineEnemy.Add(g);
                g.typeGoleMine = TypeGoldMine.Enemy;
                g.InstantiateHero(false);
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
