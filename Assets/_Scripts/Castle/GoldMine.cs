using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;


[System.Serializable]
public class GoldMine : MonoBehaviour
{
    public int id;
    public float health;
    public int capGold;
    public long priceGold;
    public long priceWillUpgrade;
    public int capWillUpgrade;
    public int levelWillupgrade;
    public int level;
    public Collider2D colliderLand;
    public TypeGoldMine typeGoleMine;
    public List<Hero> lstHeroGoldMine; //list hero trong mo vang

    [Header("UI")]
    public Text txtLevel;
    // Use this for initialization
    void Start()
    {
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadDataGoldMine(int _id,float _health, int _capGold, long _priceGold, int _level, TypeGoldMine _type)
    {
        this.id = _id;
        this.health = _health;
        this.capGold = _capGold;
        this.priceGold = _priceGold;
        this.level = _level;
        this.typeGoleMine = _type;

        if (this.typeGoleMine == TypeGoldMine.Enemy)
        {
            this.RegisterListener(EventID.EnemyAttackPlayer, (param) => OnAttackPlayer(param));
        }
    }

    void OnNextDay()
    {
        SpawmGold();
    }

    void OnAttackPlayer(object param)
    {
        Debug.Log(param);
    }

    void SpawmGold()
    {
        if (typeGoleMine == TypeGoldMine.Player)
        {
            GameManager.Instance.AddGold(capGold);
        }
    }

    public void Btn_ShowPanelUpgrade()
    {
        //
    }

    public void CheckUpgrade(int _x)
    {
        capWillUpgrade = capGold;
        priceWillUpgrade = capGold;
        levelWillupgrade = level;
        for (int i = 1; i <= _x; i++)
        {
            levelWillupgrade++;
            priceWillUpgrade = (long)(priceWillUpgrade * GameConfig.Instance.PriceGoldUp);
            capWillUpgrade = (int)(capWillUpgrade * GameConfig.Instance.CapGoldUp);
        }
    }

    void UpgradeGoldMine()
    {
        if (GameManager.Instance.gold < priceGold)
            return;

        priceGold = priceWillUpgrade;
        GameManager.Instance.AddGold(-(long)(priceGold * GameConfig.Instance.Ri));
        capGold = capWillUpgrade;
    }

    public void AttackPlayer()
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            //Cho linh tu dong qua land nhan vat
        }
    }
}

[System.Serializable]
public enum TypeGoldMine
{
    Enemy,
    Player
}
