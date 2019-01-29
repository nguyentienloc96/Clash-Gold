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
    public Collider2D colliderLand;
    public TypeGoldMine typeGoleMine;
    public List<Hero> lstHeroGoldMine; //list hero trong mo vang

    public List<Transform> lsPos;

    [Header("UI")]
    public Text txtLevel;

    void Start()
    {
        this.RegisterListener(EventID.StartGame, (param) => OnStartGame());
    }

    void OnStartGame()
    {
        level = Random.Range(0, 20);//GameConfig.Instance.GoldMinerAmount);
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
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
            StartCoroutine(IEInstantiate(
                GameManager.Instance.lsPrefabsEnemy[typeEnemy],
                lsPos[i],
                numberEnemy,
                "Enemy",
                level));
        }
    }

    public IEnumerator IEInstantiate(Hero prafabs, Transform posIns, int countHero, string name, int level)
    {
        Hero hero = Instantiate<Hero>(prafabs, posIns.position,Quaternion.identity);
        hero.gameObject.name = name;
        lstHeroGoldMine.Add(hero);
        yield return new WaitForEndOfFrame();
        hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, level);
        hero.infoHero.numberHero = countHero;
        hero.txtCountHero.text = UIManager.Instance.ConvertNumber(hero.infoHero.numberHero);
        hero.infoHero.healthAll = hero.infoHero.health * hero.infoHero.numberHero;
    }

    //public float timer = 0;
    //void Update()
    //{
    //    if (timer >= 3f)
    //    {
    //        this.PostEvent(EventID.NextDay);
    //        timer = 0;
    //    }
    //    else
    //        timer += Time.deltaTime;
    //}

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
            lstHeroGoldMine[i].AddHero((int)lstHeroGoldMine[i].infoHero.capWar);
        }
    }

    public void Btn_ShowPanelUpgrade()
    {
        //
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
    }

    public void AttackPlayer()
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            //Cho linh tu dong qua land nhan vat
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (typeGoleMine == TypeGoldMine.Enemy)
        {
            if (other.CompareTag("Hero"))
            {

            }

        }
        else
        {
            if (other.CompareTag("Hero"))
            {

            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }
}

[System.Serializable]
public enum TypeGoldMine
{
    Enemy,
    Player
}
