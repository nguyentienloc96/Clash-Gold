﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

[System.Serializable]
public enum TypeGoldMine
{
    Enemy,
    Player
}

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
    public TypeGoldMine typeGoleMine;

    public Collider2D colliderLand;

    public List<Hero> lstHeroGoldMine;

    public List<Transform> lsPos;

    [Header("UI")]
    public Text txtLevel;
    public SpriteRenderer sprGoldMine;
    public GameObject Canvas;
    public GameObject buttonUp;
    public GameObject buttonRelease;

    private List<ItemThrowHero> lsIconHero = new List<ItemThrowHero>();
    private List<int> lsIconHeroOn = new List<int>();

    public bool isCanon;
    public float timeCanon;

    void Start()
    {
        this.RegisterListener(EventID.StartGame, (param) => OnStartGame());
    }

    void OnStartGame()
    {
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

    public void SetInfo(int _capGold, int _priceGoldUp, int _level)
    {
        capGold = (int)(_capGold * Mathf.Pow(GameConfig.Instance.CapGoldUp, _level));
        priceGold = (long)(_capGold * Mathf.Pow(GameConfig.Instance.PriceGoldUp, _level));
    }

    public void SetSpriteBox(int _l)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (_l >= this.level || Mathf.Abs(_l - level) <= 3)
            {
                sprGoldMine.sprite = GameManager.Instance.sprBoxMap[numberBoxGoldMine + 8];
            }
            else
            {
                sprGoldMine.sprite = GameManager.Instance.sprBoxMap[numberBoxGoldMine + 4];
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
            //Debug.Log("Add Gold : " + capGold);
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
            GameManager.Instance.goldMineCurrent = this;
            UIManager.Instance.panelThrowHero.SetActive(true);
            lsIconHero.Clear();
            lsIconHeroOn.Clear();
            for (int k = 0; k < UIManager.Instance.contentThrowHero.childCount; k++)
            {
                Destroy(UIManager.Instance.contentThrowHero.GetChild(k).gameObject);
            }
            if (this.lstHeroGoldMine.Count < 3)
            {
                for (int i = 0; i < GameManager.Instance.lstHousePlayer.Count; i++)
                {
                    if (GameManager.Instance.lstHousePlayer[i].typeState == TypeStateHouse.None)
                    {
                        GameObject obj = Instantiate(UIManager.Instance.itemThrowHero, UIManager.Instance.contentThrowHero);
                        ItemThrowHero item = obj.GetComponent<ItemThrowHero>();
                        item.houseHero = GameManager.Instance.lstHousePlayer[i];
                        item.iconHero.sprite = UIManager.Instance.sprAvatarHero[GameManager.Instance.lstHousePlayer[i].idHero - 1];
                        item.txtCountHero.text = "Count Hero : " + GameManager.Instance.lstHousePlayer[i].countHero;
                        lsIconHero.Add(item);
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.lstHeroGoldMine.Count; i++)
                {
                    GameObject obj = Instantiate(UIManager.Instance.itemThrowHero, UIManager.Instance.contentThrowHero);
                    ItemThrowHero item = obj.GetComponent<ItemThrowHero>();
                    for (int j = 0; j < GameManager.Instance.lstHousePlayer.Count; j++)
                    {
                        if (GameManager.Instance.lstHousePlayer[j].idHero == lstHeroGoldMine[i].infoHero.ID)
                        {
                            item.houseHero = GameManager.Instance.lstHousePlayer[j];
                        }
                    }
                    item.iconHero.sprite = UIManager.Instance.sprAvatarHero[lstHeroGoldMine[i].infoHero.ID - 1];
                    lsIconHero.Add(item);
                }
            }
        }
    }

    public void ThrowHero()
    {
        for (int i = 0; i < lsIconHero.Count; i++)
        {
            if (lsIconHero[i].sliderHero.value > 0)
            {
                int numberadd = (int)(lsIconHero[i].houseHero.countHero * lsIconHero[i].sliderHero.value);
                bool isHeroOn = false;
                foreach (Hero hr in lstHeroGoldMine)
                {
                    if (hr.infoHero.ID == lsIconHero[i].houseHero.idHero)
                    {
                        hr.AddHero(+numberadd);
                        isHeroOn = true;
                        break;
                    }
                }
                if (!isHeroOn)
                {
                    InstantiateHero(lsIconHero[i].houseHero.idHero - 1, numberadd);
                }
                lsIconHero[i].houseHero.countHero -= numberadd;
            }
        }
        lsIconHero.Clear();
    }

    public void Update()
    {
        if (lsIconHero.Count > 0)
        {
            int countOn = 0;
            for (int i = 0; i < lsIconHero.Count; i++)
            {
                if (countOn >= 3 - lstHeroGoldMine.Count)
                {
                    break;
                }
                if (lsIconHero[i].sliderHero.value > 0)
                {
                    countOn++;
                }
            }
            if (countOn >= 3 - lstHeroGoldMine.Count)
            {
                foreach (ItemThrowHero item in lsIconHero)
                {
                    if (item.sliderHero.value == 0)
                    {
                        bool isHeroGoldMine = false;
                        foreach (Hero h in lstHeroGoldMine)
                        {
                            if (h.infoHero.ID == item.houseHero.idHero)
                            {
                                isHeroGoldMine = true;
                            }
                        }
                        if (!isHeroGoldMine)
                        {
                            item.sliderHero.interactable = false;
                        }
                    }
                }
            }
            else
            {
                foreach (ItemThrowHero item in lsIconHero)
                {
                    item.sliderHero.interactable = true;
                }
            }
        }
    }

    public void CheckUpgrade(int _x)
    {
        levelWillUpgrade = level + _x;
        capWillUpgrade = (int)(capGold * Mathf.Pow(GameConfig.Instance.CapGoldUp, _x));
        priceWillUpgrade = (long)(capGold * Mathf.Pow(GameConfig.Instance.PriceGoldUp, _x));
    }

    void UpgradeGoldMine()
    {
        if (GameManager.Instance.gold < priceWillUpgrade)
            return;

        priceGold = priceWillUpgrade;
        GameManager.Instance.AddGold(-priceGold);
        capGold = capWillUpgrade;
        level = levelWillUpgrade;
        txtLevel.text = "Lv " + level.ToString();
    }

    public void AttackPlayer()
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            int check = 0;
            float dis = Vector3.Distance(transform.position, GameManager.Instance.lstGoldMinePlayer[0].transform.position);
            for (int i = 1; i < GameManager.Instance.lstGoldMinePlayer.Count; i++)
            {
                if (Vector3.Distance(transform.position, GameManager.Instance.lstGoldMinePlayer[i].transform.position) < dis)
                {
                    check = i;
                }
            }
            GameManager.Instance.GolEnemyIsAttack = this;
            GameManager.Instance.GolHeroBeingAttack = GameManager.Instance.lstGoldMinePlayer[check];
            Box boxStart = this.transform.parent.GetComponent<Box>();
            Box boxEnd = GameManager.Instance.lstGoldMinePlayer[check].transform.parent.GetComponent<Box>();
            List<Box> lsPosMove = GameManager.Instance.PathFinding(boxStart, boxEnd);
            float speed = 10f;
            for (int i = 0; i < lstHeroGoldMine.Count; i++)
            {
                Hero hero;
                hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[lstHeroGoldMine[i].infoHero.ID - 1]
                    , lsPos[i].position
                    , Quaternion.identity);
                hero.gameObject.name = "Enemy";
                hero.SetInfoHero();
                hero.infoHero.capWar = 0;
                int numberAttack = (int)(lstHeroGoldMine[i].infoHero.numberHero * 0.5f);
                hero.AddHero(numberAttack);
                lstHeroGoldMine[i].AddHero(-numberAttack);
                hero.speedMin = speed;
                hero.StartMoveToLsPosition(lsPosMove);
                GameManager.Instance.lsEnemyAttackGoldMine.Add(hero);
            }
        }
    }

    public void InstantiateHeroStart(bool isHero)
    {
        List<int> lsIdHero = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        for (int i = 0; i < 3; i++)
        {
            int typeEnemy;
            int numberEnemy = 1;
            if (i == 0)
            {
                int randomFly = Random.Range(0, GameManager.Instance.lsHeroFly.Length);
                typeEnemy = GameManager.Instance.lsHeroFly[randomFly];
                lsIdHero.Remove(typeEnemy);
            }
            else
            {
                int randomCanMove = Random.Range(0, lsIdHero.Count);
                typeEnemy = lsIdHero[randomCanMove];
                lsIdHero.Remove(typeEnemy);
            }

            if (!isHero)
            {

                Hero hero;
                hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[typeEnemy]
                    , lsPos[i].position
                    , Quaternion.identity
                    , GameManager.Instance.enemyManager);

                hero.IDGold = id;
                hero.gameObject.name = "Enemy";
                lstHeroGoldMine.Add(hero);
                hero.SetInfoHero();
                hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
                hero.AddHero(numberEnemy);
                hero.posStart = lsPos[i].position;
                hero.GetComponent<BoxCollider2D>().enabled = false;
            }

        }
    }

    public void InstantiateHero(int idHero, int number)
    {
        Hero hero;
        hero = Instantiate(GameManager.Instance.lsPrefabsHero[idHero]
            , lsPos[lstHeroGoldMine.Count].position
            , Quaternion.identity
            , GameManager.Instance.enemyManager);
        hero.gameObject.name = "Hero";
        hero.SetInfoHero();
        hero.infoHero.capWar = 0;
        hero.AddHero(number);
        hero.posStart = lsPos[lstHeroGoldMine.Count].position;
        lstHeroGoldMine.Add(hero);
    }

    public void InstantiateEnemy(int idHero, int number, int i)
    {
        Hero hero;
        hero = Instantiate(GameManager.Instance.lsPrefabsHero[idHero]
            , lsPos[i].position
            , Quaternion.identity
            , GameManager.Instance.enemyManager);
        hero.gameObject.name = "Enemy";
        hero.SetInfoHero();
        hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
        hero.AddHero(number);
        hero.posStart = lsPos[i].position;
        lstHeroGoldMine.Add(hero);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (other.CompareTag("Castle"))
            {
                GameManager.Instance.GolEnemyBeingAttack = this;
                GameManager.Instance.posTriggerGoldMine = other.transform.position;
                AttackGoldMineEnemy();
                GameManager.Instance.isAttack = true;
                GameManager.Instance.isAttackGoldMineEnemy = true;
                if (GameManager.Instance.lsEnemyAttackGoldMine.Count > 0)
                {
                    for (int i = 0; i < GameManager.Instance.lsEnemyAttackGoldMine.Count; i++)
                    {
                        GameManager.Instance.lsEnemyAttackGoldMine[i].isPause = true;
                    }
                }
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                GameManager.Instance.GolHeroBeingAttack = this;
                GameManager.Instance.posTriggerGoldMine = other.transform.position;
                AttackGoldMineHero();
                GameManager.Instance.isAttack = true;
                GameManager.Instance.isAttackGoldMineEnemy = false;
                for (int i = 0; i < GameManager.Instance.lsEnemyAttackGoldMine.Count; i++)
                {
                    GameManager.Instance.lsEnemyAttackGoldMine[i].isPause = true;
                    GameManager.Instance.lsEnemyAttackGoldMine[i].GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }

    public void AttackGoldMineEnemy()
    {
        DeadzoneCamera.Instance.cameraAttack.gameObject.SetActive(true);
        UIManager.Instance.cavas.worldCamera = DeadzoneCamera.Instance.cameraAttack;
        UIManager.Instance.mapAttack.SetActive(true);
        UIManager.Instance.mapMove.SetActive(false);
        UIManager.Instance.panelThrowHeroAttack.SetActive(true);
        GameManager.Instance.numberThrowHero = 0;
        for (int i = 0; i < lstHeroGoldMine.Count && i < 3; i++)
        {
            if (lstHeroGoldMine[i].infoHero.ID != 2)
            {
                Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[lstHeroGoldMine[i].infoHero.ID - 1], GameManager.Instance.lsPosEnemy[i]);
                hero.gameObject.name = "Enemy";
                hero.SetInfoHero();
                hero.infoHero.capWar = 0;
                hero.AddHero(lstHeroGoldMine[i].infoHero.numberHero);
                hero.isAttack = true;
                GameManager.Instance.lsEnemy.Add(hero);
            }
            else
            {
                float XADD = -0.5f;
                for (int j = 0; j < 4; j++)
                {
                    Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[1], GameManager.Instance.lsPosEnemy[i]);
                    if (j == 1)
                    {
                        hero.transform.position += new Vector3(0, -0.5f, 0f);
                    }
                    else if (j == 3)
                    {
                        hero.transform.position += new Vector3(0, 0.5f, 0f);
                    }
                    else
                    {
                        hero.transform.position += new Vector3(XADD, 0f, 0f);
                    }
                    XADD += 0.5f;
                    hero.gameObject.name = "Enemy";
                    hero.SetInfoHero();
                    hero.infoHero.capWar = 0;
                    hero.AddHero(lstHeroGoldMine[i].infoHero.numberHero / 3);
                    hero.isAttack = true;
                    GameManager.Instance.lsEnemy.Add(hero);

                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (i < GameManager.Instance.castlePlayer.lsHouseRelease.Count)
            {
                UIManager.Instance.lsItemHeroAttack[i].gameObject.SetActive(true);
                UIManager.Instance.lsItemHeroAttack[i].countHero = GameManager.Instance.castlePlayer.lsHouseRelease[i].countHero;
                UIManager.Instance.lsItemHeroAttack[i].houseHero = GameManager.Instance.castlePlayer.lsHouseRelease[i];
                UIManager.Instance.lsItemHeroAttack[i].iconHero.sprite = UIManager.Instance.sprAvatarHero[GameManager.Instance.castlePlayer.lsHouseRelease[i].idHero - 1];
                UIManager.Instance.lsItemHeroAttack[i].txtCountHero.text = UIManager.Instance.lsItemHeroAttack[i].countHero.ToString();
            }
            else
            {
                UIManager.Instance.lsItemHeroAttack[i].gameObject.SetActive(false);
            }
        }
    }

    public void AttackGoldMineHero()
    {
        DeadzoneCamera.Instance.cameraAttack.gameObject.SetActive(true);
        UIManager.Instance.cavas.worldCamera = DeadzoneCamera.Instance.cameraAttack;
        UIManager.Instance.mapAttack.SetActive(true);
        UIManager.Instance.mapMove.SetActive(false);
        for (int i = 0; i < lstHeroGoldMine.Count && i < 3; i++)
        {
            if (lstHeroGoldMine[i].infoHero.ID != 2)
            {
                Hero hero = Instantiate(GameManager.Instance.lsPrefabsHero[lstHeroGoldMine[i].infoHero.ID - 1], GameManager.Instance.lsPosHero[i]);
                hero.gameObject.name = "Hero";
                hero.SetInfoHero();
                hero.infoHero.capWar = 0;
                hero.AddHero(lstHeroGoldMine[i].infoHero.numberHero);
                hero.isAttack = true;
                GameManager.Instance.lsHero.Add(hero);
            }
            else
            {
                float XADD = -0.5f;
                for (int j = 0; j < 4; j++)
                {
                    Hero hero = Instantiate(GameManager.Instance.lsPrefabsHero[1], GameManager.Instance.lsPosHero[i]);
                    if (j == 1)
                    {
                        hero.transform.position += new Vector3(0, -0.5f, 0f);
                    }
                    else if (j == 3)
                    {
                        hero.transform.position += new Vector3(0, 0.5f, 0f);
                    }
                    else
                    {
                        hero.transform.position += new Vector3(XADD, 0f, 0f);
                    }
                    XADD += 0.5f;
                    hero.gameObject.name = "Hero";
                    hero.SetInfoHero();
                    hero.infoHero.capWar = 0;
                    hero.AddHero(lstHeroGoldMine[i].infoHero.numberHero / 3);
                    hero.isAttack = true;
                    GameManager.Instance.lsHero.Add(hero);

                }
            }
        }

        for (int i = 0; i < GameManager.Instance.lsEnemyAttackGoldMine.Count && i < 3; i++)
        {
            if (GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.ID != 2)
            {
                Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.ID - 1], GameManager.Instance.lsPosEnemy[i]);
                hero.gameObject.name = "Enemy";
                hero.SetInfoHero();
                hero.infoHero.capWar = 0;
                hero.AddHero(GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.numberHero);
                hero.isAttack = true;
                GameManager.Instance.lsEnemy.Add(hero);
            }
            else
            {
                float XADD = -0.5f;
                for (int j = 0; j < 3; j++)
                {
                    Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[1], GameManager.Instance.lsPosEnemy[i]);
                    if (j == 1)
                    {
                        hero.transform.position += new Vector3(XADD, -0.5f, 0f);
                    }
                    else
                    {
                        hero.transform.position += new Vector3(XADD, 0f, 0f);
                    }
                    XADD += 0.5f;
                    hero.gameObject.name = "Enemy";
                    hero.SetInfoHero();
                    hero.infoHero.capWar = 0;
                    hero.AddHero(GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.numberHero / 3);
                    hero.isAttack = true;
                    GameManager.Instance.lsEnemy.Add(hero);
                }
            }
        }
    }

    public void DeleteHero()
    {
        foreach (Hero h in lstHeroGoldMine)
        {
            Destroy(h.gameObject);
        }
        lstHeroGoldMine.Clear();
    }

    public void AddLevel()
    {
        level++;
        SetLevel(level);
        foreach (Hero hero in lstHeroGoldMine)
        {
            hero.infoHero.capWar = GameConfig.Instance.lstInfoHero[hero.infoHero.ID - 1].capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
        }
        SetSpriteBox(GameManager.Instance.maxLevelHouse);
    }
}
