using System.Collections;
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

    public void InstantiateHeroStart(bool isHero)
    {
        for (int i = 0; i < 3; i++)
        {
            int typeEnemy;
            int numberEnemy = 1;
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
            }

        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (other.CompareTag("Castle"))
            {
                GameManager.Instance.idGold = id;
                Attack();
                //for (int i = 0; i < GameManager.Instance.lstHousePlayer.Count; i++)
                //{
                //    if (GameManager.Instance.lstHousePlayer[i].idHero == 9)
                //    {
                //        if (GameManager.Instance.lstHousePlayer[i].typeState == TypeStateHouse.None)
                //        {
                //            UIManager.Instance.btnReleaseCanon.SetActive(true);
                //            UIManager.Instance.btnReleaseCanon.GetComponent<Button>().interactable = true;
                //            break;
                //        }
                //    }
                //}               
                GameManager.Instance.isAttack = true;
            }
        }
    }

    public void Attack()
    {
        DeadzoneCamera.Instance.cameraAttack.gameObject.SetActive(true);
        UIManager.Instance.cavas.worldCamera = DeadzoneCamera.Instance.cameraAttack;
        UIManager.Instance.mapAttack.SetActive(true);
        UIManager.Instance.mapMove.SetActive(false);
        for (int i = 0; i < lstHeroGoldMine.Count && i < 3; i++)
        {
            Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[lstHeroGoldMine[i].infoHero.ID -1], GameManager.Instance.lsPosEnemy[i]);
            hero.gameObject.name = "Enemy";
            hero.SetInfoHero();
            hero.infoHero.capWar = 0;
            hero.AddHero(lstHeroGoldMine[i].infoHero.numberHero);
            hero.isAttack = true;
            GameManager.Instance.lsEnemy.Add(hero);
        }

        for (int i = 0; i < GameManager.Instance.castlePlayer.lsHouseRelease.Count && i < 3; i++)
        {
            Hero hero = Instantiate(GameManager.Instance.lsPrefabsHero[GameManager.Instance.castlePlayer.lsHouseRelease[i].idHero - 1], GameManager.Instance.lsPosHero[i]);
            hero.gameObject.name = "Hero";
            hero.SetInfoHero();
            hero.infoHero.capWar = 0;
            hero.AddHero(GameManager.Instance.castlePlayer.lsHouseRelease[i].countHero);
            hero.isAttack = true;
            GameManager.Instance.lsHero.Add(hero);
        }
    }

    public void DeleteHero()
    {
        foreach(Hero h in lstHeroGoldMine)
        {
            Destroy(h.gameObject);
        }
        lstHeroGoldMine.Clear();
    }
}
