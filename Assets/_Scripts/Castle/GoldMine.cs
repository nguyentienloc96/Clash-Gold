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
    public int levelWillUpgrade;
    public int level;
    public int numberBoxGoldMine;
    public Collider2D colliderLand;
    public TypeGoldMine typeGoleMine;
    public List<Hero> lstHeroGoldMine;
    public List<GameObject> lstCompetitorGoldMine;
    public GameObject castle;

    public List<Transform> lsPos;

    [Header("UI")]
    public Text txtLevel;
    public SpriteRenderer sprGoldMine;
    public GameObject Canvas;
    public GameObject buttonUp;
    public GameObject buttonRelease;

    public bool isCanon;
    public float timeCanon;
    void Start()
    {
        this.RegisterListener(EventID.StartGame, (param) => OnStartGame());
    }

    void Update()
    {
        if (typeGoleMine == TypeGoldMine.Enemy && this.lstHeroGoldMine.Count <= 0)
        {
            GameManager.Instance.lstGoldMineEnemy.Remove(this);
            typeGoleMine = TypeGoldMine.Player;
            GameManager.Instance.lstGoldMinePlayer.Add(this);
            SetSpriteBox(0);
        }

        if (lstCompetitorGoldMine.Count > 0 && !isCanon)
        {
            timeCanon += Time.deltaTime;
            if (timeCanon >= 3)
            {
                Vector3 posCanon = (lstCompetitorGoldMine[0].transform.position + lstHeroGoldMine[0].transform.position) / 2;
                InstantiateCanon(typeGoleMine == TypeGoldMine.Player, posCanon);
                isCanon = true;
            }
        }
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

    void SetSpriteBox(int _l)
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

    public void InstantiateHero(bool isHero)
    {
        for (int i = 0; i < 3; i++)
        {
            int typeEnemy;
            if (i == 0)
            {
                int randomFly = Random.Range(0, GameManager.Instance.lsHeroFly.Length);
                typeEnemy = GameManager.Instance.lsHeroFly[randomFly];
            }
            else
            {
                int randomCanMove = Random.Range(0, GameManager.Instance.lsHeroCanMove.Length);
                typeEnemy = GameManager.Instance.lsHeroCanMove[randomCanMove];
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

    public void InstantiateCanon(bool isHero, Vector3 _toPos)
    {
        int typeEnemy = 8;
        int numberEnemy = Random.Range(50, 150);
        if (!isHero)
        {

            Hero hero;
            hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[typeEnemy]
                , _toPos
                , Quaternion.identity
                , GameManager.Instance.enemyManager);
            hero.gameObject.name = "Enemy";
            lstHeroGoldMine.Add(hero);
            hero.SetInfoHero();
            hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
            hero.AddHero(numberEnemy);
            hero.goldMineProtecting = this;
        }
        else
        {
            Hero hero;
            hero = Instantiate(GameManager.Instance.lsPrefabsHero[typeEnemy]
                , _toPos
                , Quaternion.identity
                , GameManager.Instance.enemyManager);
            hero.gameObject.name = "Hero";
            lstHeroGoldMine.Add(hero);
            hero.SetInfoHero();
            hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
            hero.AddHero(numberEnemy);
            hero.goldMineProtecting = this;
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
            //Cho linh tu dong qua land nhan vat
        }
    }

    public bool isBeingAttack;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (other.CompareTag("Hero") || other.CompareTag("Castle"))
            {
                if (other.tag == "Hero")
                {
                    other.GetComponent<Hero>().goldMineAttacking = this;
                    other.GetComponent<Hero>().isInGoldMine = true;
                    lstCompetitorGoldMine.Add(other.gameObject);
                }
                else
                {
                    castle = other.gameObject;
                }

                if (lstCompetitorGoldMine.Count > 0 || castle != null)
                {
                    foreach (Hero heroMine in lstHeroGoldMine)
                    {
                        heroMine.isInGoldMine = true;
                    }
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (other.CompareTag("Hero") || other.CompareTag("Castle"))
            {
                if (other.tag == "Hero")
                {
                    other.GetComponent<Hero>().goldMineAttacking = null;
                    other.GetComponent<Hero>().isInGoldMine = false;
                    lstCompetitorGoldMine.Remove(other.gameObject);
                }
                else
                {
                    castle = null;
                }

                if (lstCompetitorGoldMine.Count <= 0 && castle == null)
                {
                    foreach (Hero heroMine in lstHeroGoldMine)
                    {
                        heroMine.isInGoldMine = false;
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
