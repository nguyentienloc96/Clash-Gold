﻿using System.Collections;
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
    public int levelWillUpgrade;
    public int level;
    public int numberBoxGoldMine;
    public Collider2D colliderLand;
    public TypeGoldMine typeGoleMine;
    public List<Hero> lstHeroGoldMine;
    public List<Hero> lstCompetitorGoldMine;

    public List<Transform> lsPos;

    [Header("UI")]
    public TextMesh txtLevel;
    public SpriteRenderer sprGoldMine;
    public GameObject buttonUp;
    public GameObject buttonRelease;

    void Start()
    {
        lstCompetitorGoldMine = new List<Hero>();
        this.RegisterListener(EventID.StartGame, (param) => OnStartGame());        
    }

    void OnStartGame()
    {
        //level = Random.Range(0, 20);//GameConfig.Instance.GoldMinerAmount);
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
        this.RegisterListener(EventID.UpLevelHouse, (param) => OnSetSpriteBox());
    }

    void OnSetSpriteBox()
    {
        SetSpriteBox(GameManager.Instance.maxLevelHouse);
    }

    public void SetLevel(int _l)
    {
        level = _l;
        txtLevel.text = "Lv " + level.ToString();
    }

    public void SetSpriteBox(int _l)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (_l > level || _l <= level - 3)
            {
                sprGoldMine.sprite = GameManager.Instance.sprBoxMap[numberBoxGoldMine + 4];
            }
            else
            {
                sprGoldMine.sprite = GameManager.Instance.sprBoxMap[numberBoxGoldMine + 8];
            }
            buttonUp.SetActive(false);
            buttonRelease.SetActive(false);
        }
        else
        {
            sprGoldMine.sprite = GameManager.Instance.sprBoxMap[numberBoxGoldMine];
            buttonUp.SetActive(true);
            buttonRelease.SetActive(true);
        }
    }

    public void InstantiateHero(bool isHero)
    {
        for (int i = 0; i < 3; i++)
        {
            int typeEnemy = 0;
            if (i == 0)
            {
                //int randomFly = Random.Range(0, GameManager.Instance.lsHeroFly.Length);
                //typeEnemy = GameManager.Instance.lsHeroFly[randomFly];
            }
            else
            {
                //int randomCanMove = Random.Range(0, GameManager.Instance.lsHeroCanMove.Length);
                //typeEnemy = GameManager.Instance.lsHeroCanMove[randomCanMove];
            }

            int numberEnemy = 1;
            if (!isHero)
            {

                Hero hero;
                hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[typeEnemy]
                    , lsPos[i].position
                    , Quaternion.identity
                    , GameManager.Instance.enemyManager);
                hero.gameObject.name = "Enemy";
                lstHeroGoldMine.Add(hero);
                hero.SetInfoHero();
                hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
                hero.AddHero(numberEnemy);

                hero.goldMineProtecting = this;
                hero.posStart = lsPos[i].position;
            }

        }
    }

    public void LoadDataGoldMine(int _id, float _health, int _capGold, long _priceGold, int _level, TypeGoldMine _type)
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
        SpawmHero();
    }

    void OnAttackPlayer(object param)
    {
        //Debug.Log(param);
    }

    void SpawmGold()
    {
        if (typeGoleMine == TypeGoldMine.Player)
        {
            GameManager.Instance.AddGold(capGold);
        }
    }

    void SpawmHero()
    {
        for (int i = 0; i < lstHeroGoldMine.Count; i++)
        {
            lstHeroGoldMine[i].AddHero((int)(lstHeroGoldMine[i].infoHero.capWar * GameManager.Instance.ratioBorn));
        }
    }

    public void Btn_ShowPanelUpgrade()
    {
        if (typeGoleMine == TypeGoldMine.Player)
        {
            CheckUpgrade(1);
            UpgradeGoldMine();
        }
    }

    public void Btn_Release()
    {
        if (typeGoleMine == TypeGoldMine.Player)
        {

        }
    }

    public void CheckUpgrade(int _x)
    {
        capWillUpgrade = (int)(capGold * Mathf.Pow(GameConfig.Instance.CapGoldUp, _x));
        priceWillUpgrade = (long)(capGold * Mathf.Pow(GameConfig.Instance.PriceGoldUp, _x));
        levelWillUpgrade = level += _x;
    }

    void UpgradeGoldMine()
    {
        if (GameManager.Instance.gold < priceGold)
            return;

        priceGold = priceWillUpgrade;
        GameManager.Instance.AddGold(-(long)(priceGold * GameConfig.Instance.Ri));
        capGold = capWillUpgrade;
        level++;
        txtLevel.text = "Lv " + level.ToString();
    }

    public void AttackPlayer()
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            //Cho linh tu dong qua land nhan vat
        }
    }

    public bool isBeingAttack;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (other.CompareTag("Hero"))
            {
                Hero heroCurrent = other.GetComponent<Hero>();
                if (!isBeingAttack)
                {
                    foreach (Hero hero in lstHeroGoldMine)
                    {
                        hero.isInGoldMine = true;
                    }
                    isBeingAttack = true;
                }
                heroCurrent.goldMineAttacking = this;
                heroCurrent.isInGoldMine = true;
                lstCompetitorGoldMine.Add(heroCurrent);
            }

        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                Hero heroCurrent = other.GetComponent<Hero>();
                if (!isBeingAttack)
                {
                    foreach (Hero hero in lstHeroGoldMine)
                    {
                        hero.isInGoldMine = true;
                    }
                    isBeingAttack = true;
                }
                heroCurrent.goldMineAttacking = this;
                heroCurrent.isInGoldMine = true;
                lstCompetitorGoldMine.Add(heroCurrent);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (other.CompareTag("Hero"))
            {
                Hero heroCurrent = other.GetComponent<Hero>();
                heroCurrent.goldMineAttacking = null;
                heroCurrent.targetCompetitor = null;
                heroCurrent.isInGoldMine = false;
                lstCompetitorGoldMine.Remove(heroCurrent);
                if (lstCompetitorGoldMine.Count <= 0)
                {
                    if (isBeingAttack)
                    {
                        foreach (Hero hero in lstHeroGoldMine)
                        {
                            hero.targetCompetitor = null;
                            hero.isInGoldMine = false;
                        }
                        isBeingAttack = false;
                    }
                }
            }

        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                Hero heroCurrent = other.GetComponent<Hero>();
                heroCurrent.goldMineAttacking = null;
                heroCurrent.targetCompetitor = null;
                heroCurrent.isInGoldMine = false;
                lstCompetitorGoldMine.Remove(heroCurrent);
                if (lstCompetitorGoldMine.Count <= 0)
                {
                    if (isBeingAttack)
                    {
                        foreach (Hero hero in lstHeroGoldMine)
                        {
                            hero.targetCompetitor = null;
                            hero.isInGoldMine = false;
                        }
                        isBeingAttack = false;
                    }
                }
            }
        }
    }
}

[System.Serializable]
public enum TypeGoldMine
{
    Enemy,
    Player
}
