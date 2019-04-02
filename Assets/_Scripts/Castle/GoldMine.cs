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
public struct GoldMineInfo
{
    public int ID;
    public string name;
    public int level;
    public int capGold;
    public long price;
    public TypeGoldMine typeGoleMine;

    public int levelWillUpgrade;
    public int capWillUpgrade;
    public long priceWillUpgrade;

    public int indexLoadGoldMine;
}

public class GoldMine : MonoBehaviour
{
    [Header("INFO")]
    public GoldMineInfo info;
    public List<Hero> lsHeroGoldMine;

    [Header("INSHERO")]
    public Transform[] lsPosIns;

    [Header("UI")]
    public Text txtName;
    public Text txtLevel;
    public RectTransform canvas;
    public GameObject btnUp;
    public GameObject btnRelease;
    public SpriteRenderer sprGoldMine;

    private List<ItemThrowHero> lsIconHero = new List<ItemThrowHero>();
    private List<int> lsIconHeroOn = new List<int>();

    void Start()
    {
        //this.RegisterListener(EventID.StartGame, (param) => OnStartGame());
        OnStartGame();
    }

    void OnStartGame()
    {
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
        this.RegisterListener(EventID.UpLevelHouse, (param) => SetSpriteBox(GameManager.Instance.maxLevelHouse));
    }

    public void Update()
    {
        if (lsIconHero.Count > 0)
        {
            int countOn = 0;
            for (int i = 0; i < lsIconHero.Count; i++)
            {
                if (countOn >= 3 - lsHeroGoldMine.Count)
                {
                    break;
                }
                if (lsIconHero[i].sliderHero.value > 0)
                {
                    countOn++;
                }
            }
            if (countOn >= 3 - lsHeroGoldMine.Count)
            {
                foreach (ItemThrowHero item in lsIconHero)
                {
                    if (item.sliderHero.value == 0)
                    {
                        bool isHeroGoldMine = false;
                        foreach (Hero h in lsHeroGoldMine)
                        {
                            if (h.infoHero.ID == item.houseHero.info.idHero)
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

    public void GetInfo(int id, string name, int level, TypeGoldMine typeGoleMine, int indexSprite)
    {
        info.ID = id;
        info.name = name;
        txtName.text = name;
        info.level = level;
        SetInfo(level);
        SetLevel(level);
        info.typeGoleMine = typeGoleMine;
        info.indexLoadGoldMine = indexSprite;
        SetSpriteBox(GameManager.Instance.maxLevelHouse);
        if(typeGoleMine == TypeGoldMine.Enemy)
        {
            InstantiateEnemyRandom();
        }
    }

    public void GetInfoJson(GoldMineInfoST gInfo)
    {
        info.ID = gInfo.ID;
        info.name = gInfo.name;
        txtName.text = gInfo.name;
        info.level = gInfo.level;
        SetInfo(gInfo.level);
        SetLevel(gInfo.level);
        info.typeGoleMine = gInfo.isPlayer ? TypeGoldMine.Player : TypeGoldMine.Enemy;
        info.indexLoadGoldMine = gInfo.indexLoadGoldMine;
        SetSpriteBox(GameManager.Instance.maxLevelHouse);
        foreach(HeroInfoST hInfo in gInfo.lsHeroGoldMine)
        {
            if (gInfo.isPlayer)
            {
                InstantiateHeroJson(hInfo.ID, hInfo.CountHero);
            }
            else
            {
                InstantiateEnemyJson(hInfo.ID, hInfo.CountHero);
            }
        }
    }

    public void SetInfo(int _level)
    {
        info.capGold = (int)(GameConfig.Instance.CapGold0 * Mathf.Pow(GameConfig.Instance.CapGoldUp, _level));
        info.price = (long)(GameConfig.Instance.CapGold0 * Mathf.Pow(GameConfig.Instance.PriceGoldUp, _level));
    }

    public void SetSpriteBox(int _levelHouseMax)
    {
        if (info.typeGoleMine == TypeGoldMine.Enemy)
        {
            if (_levelHouseMax >= info.level || Mathf.Abs(_levelHouseMax - info.level) <= 3)
            {
                sprGoldMine.color = Color.green;
            }
            else
            {
                sprGoldMine.color = Color.red;
            }
            btnUp.SetActive(false);
            btnRelease.SetActive(false);
        }
        else
        {
            btnUp.SetActive(true);
            btnRelease.SetActive(true);
            sprGoldMine.color = Color.white;
        }
    }

    public void SetLevel(int level)
    {
        info.level = level;
        txtLevel.text = "Lv " + level.ToString();
    }

    public void AddLevel()
    {
        info.level++;
        SetLevel(info.level);
        foreach (Hero hero in lsHeroGoldMine)
        {
            hero.infoHero.capWar = GameConfig.Instance.lsInfoHero[hero.infoHero.ID - 1].capWar * Mathf.Pow(GameConfig.Instance.Wi, info.level);
        }
        SetSpriteBox(GameManager.Instance.maxLevelHouse);
        info.capGold = (int)(info.capGold * Mathf.Pow(GameConfig.Instance.CapGoldUp, 1));
        info.price = (long)(info.capGold * Mathf.Pow(GameConfig.Instance.PriceGoldUp, 1));
    }

    public void Btn_ShowPanelUpgrade()
    {
        if (info.typeGoleMine == TypeGoldMine.Player)
        {
            CheckUpgrade();
            UIManager.Instance.UpgradeGoldMine
                (info.name,
                GetComponent<SpriteRenderer>().sprite,
                info.level,
                info.levelWillUpgrade,
                info.capGold,
                info.capWillUpgrade,
                info.priceWillUpgrade,
                GameManager.Instance.gold < info.priceWillUpgrade,
                UpgradeGoldMine);
        }
    }

    public void CheckUpgrade()
    {
        info.levelWillUpgrade = info.level + 1;
        info.capWillUpgrade = (int)(info.capGold * Mathf.Pow(GameConfig.Instance.CapGoldUp, 1));
        info.priceWillUpgrade = (long)(info.capGold * Mathf.Pow(GameConfig.Instance.PriceGoldUp, 1));
    }

    private void UpgradeGoldMine()
    {
        if (GameManager.Instance.gold < info.priceWillUpgrade)
            return;

        info.price = info.priceWillUpgrade;
        GameManager.Instance.AddGold(-info.price);
        info.capGold = info.capWillUpgrade;
        info.level = info.levelWillUpgrade;
        txtLevel.text = "Lv " + info.level.ToString();
        CheckUpgrade();
        UIManager.Instance.UpgradeGoldMine
            (info.name,
            GetComponent<SpriteRenderer>().sprite,
            info.level,
            info.levelWillUpgrade,
            info.capGold,
            info.capWillUpgrade,
            info.priceWillUpgrade,
            GameManager.Instance.gold < info.priceWillUpgrade,
            UpgradeGoldMine);
    }

    public void ThrowHero()
    {
        for (int i = 0; i < lsIconHero.Count; i++)
        {
            if (lsIconHero[i].sliderHero.value > 0)
            {
                int numberadd = (int)(lsIconHero[i].houseHero.info.countHero * lsIconHero[i].sliderHero.value);
                bool isHeroOn = false;
                for (int j = 0;j< lsHeroGoldMine.Count;j++)
                {
                    if (lsHeroGoldMine[j].infoHero.ID == lsIconHero[i].houseHero.info.idHero)
                    {
                        lsHeroGoldMine[j].AddHero(+numberadd);
                        isHeroOn = true;
                        break;
                    }
                }
                if (!isHeroOn)
                {
                    InstantiateHero(lsIconHero[i].houseHero.info.idHero, numberadd);
                }
                lsIconHero[i].houseHero.info.countHero -= numberadd;
            }
        }
        lsIconHero.Clear();
    }

    public void Btn_Release()
    {
        if (info.typeGoleMine == TypeGoldMine.Player)
        {
            GameManager.Instance.goldMineCurrent = this;
            UIManager.Instance.panelRelease.SetActive(true);
            lsIconHero.Clear();
            lsIconHeroOn.Clear();
            for (int k = 0; k < UIManager.Instance.contentThrowHero.childCount; k++)
            {
                Destroy(UIManager.Instance.contentThrowHero.GetChild(k).gameObject);
            }
            if (lsHeroGoldMine.Count < 3)
            {
                for (int i = 0; i < GameManager.Instance.lsHousePlayer.Count; i++)
                {
                    if (GameManager.Instance.lsHousePlayer[i].info.typeState == TypeStateHouse.None)
                    {
                        GameObject obj = Instantiate(UIManager.Instance.itemThrowHero, UIManager.Instance.contentThrowHero);
                        ItemThrowHero item = obj.GetComponent<ItemThrowHero>();
                        item.houseHero = GameManager.Instance.lsHousePlayer[i];
                        item.iconHero.sprite = UIManager.Instance.lsSprAvatarHero[GameManager.Instance.lsHousePlayer[i].info.idHero - 1];
                        item.txtCountHero.text = UIManager.Instance.ConvertNumber(GameManager.Instance.lsHousePlayer[i].info.countHero);
                        lsIconHero.Add(item);
                    }
                }
            }
            else
            {
                for (int i = 0; i < lsHeroGoldMine.Count; i++)
                {
                    GameObject obj = Instantiate(UIManager.Instance.itemThrowHero, UIManager.Instance.contentThrowHero);
                    ItemThrowHero item = obj.GetComponent<ItemThrowHero>();
                    for (int j = 0; j < GameManager.Instance.lsHousePlayer.Count; j++)
                    {
                        if (GameManager.Instance.lsHousePlayer[j].info.idHero == lsHeroGoldMine[i].infoHero.ID)
                        {
                            item.houseHero = GameManager.Instance.lsHousePlayer[j];
                        }
                    }
                    item.iconHero.sprite = UIManager.Instance.lsSprAvatarHero[lsHeroGoldMine[i].infoHero.ID - 1];
                    lsIconHero.Add(item);
                }
            }
        }
    }

    public void OnNextDay()
    {
        SpawmGold();
        SpawmHero();
    }

    private void SpawmGold()
    {
        if (info.typeGoleMine == TypeGoldMine.Player)
        {
            GameManager.Instance.AddGold(info.capGold);
        }
    }

    private void SpawmHero()
    {
        for (int i = 0; i < lsHeroGoldMine.Count; i++)
        {
            int numberHero = (int)((lsHeroGoldMine[i].infoHero.capWar * GameManager.Instance.ratioBorn));
            lsHeroGoldMine[i].AddHero(numberHero);
        }
    }

    public void AddHero(int idGoldMine, int numberHero)
    {
        lsHeroGoldMine[idGoldMine].infoHero.countHero += numberHero;
        if (lsHeroGoldMine[idGoldMine].infoHero.countHero < 0)
            lsHeroGoldMine[idGoldMine].infoHero.countHero = 0;
    }

    public void InstantiateEnemyRandom()
    {
        List<int> lsIdHero = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        int randomID0 = 0;
        int randomID1 = Random.Range(0, lsIdHero.Count);
        for (int i = 0; i < 3; i++)
        {
            int typeEnemy;
            int numberEnemy = 1;
            if (i == 0)
            {
                int randomFly = Random.Range(0, GameManager.Instance.lsHeroFly.Length);
                typeEnemy = GameManager.Instance.lsHeroFly[randomFly];
                randomID0 = typeEnemy;
                lsIdHero.Remove(typeEnemy);
            }
            else
            {
                if (info.level <= 2)
                {
                    typeEnemy = randomID0;
                }
                else if (info.level <= 4)
                {
                    typeEnemy = randomID1;
                }
                else
                {
                    int randomCanMove = Random.Range(0, lsIdHero.Count);
                    typeEnemy = lsIdHero[randomCanMove];
                    lsIdHero.Remove(typeEnemy);
                }
            }

            Hero hero;
            hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[typeEnemy]
                , lsPosIns[i].position,Quaternion.identity,GameManager.Instance.enemyManager);

            hero.IDGold = info.ID;
            hero.gameObject.name = "Enemy";
            hero.SetInfoHero();
            hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, info.level);
            hero.AddHero(numberEnemy);
            hero.posStart = lsPosIns[i].position;
            hero.GetComponent<BoxCollider2D>().enabled = false;
            hero.transform.localScale = new Vector3(2f, 2f, 2f);
            lsHeroGoldMine.Add(hero);
        }
    }

    public void InstantiateEnemy(int idHero, int number, int i)
    {
        Hero hero;
        hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[idHero - 1]
            , lsPosIns[i].position, Quaternion.identity, GameManager.Instance.enemyManager);
        hero.gameObject.name = "Enemy";
        hero.SetInfoHero();
        hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, info.level);
        hero.AddHero(number);
        hero.posStart = lsPosIns[i].position;
        hero.transform.localScale = new Vector3(2f, 2f, 2f);
        lsHeroGoldMine.Add(hero);
    }

    public void InstantiateEnemyJson(int idHero, int number)
    {
        Hero hero;
        hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[idHero - 1]
            , lsPosIns[lsHeroGoldMine.Count].position
            , Quaternion.identity
            , GameManager.Instance.enemyManager);
        hero.gameObject.name = "Enemy";
        hero.SetInfoHero();
        hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, info.level);
        hero.AddHero(number);
        hero.posStart = lsPosIns[lsHeroGoldMine.Count].position;
        hero.transform.localScale = new Vector3(2f, 2f, 2f);
        lsHeroGoldMine.Add(hero);
    }

    public void InstantiateHeroJson(int idHero, int number)
    {
        Hero hero;
        hero = Instantiate(GameManager.Instance.lsPrefabsHero[idHero - 1]
            , lsPosIns[lsHeroGoldMine.Count].position
            , Quaternion.identity
            , GameManager.Instance.enemyManager);
        hero.gameObject.name = "Hero";
        hero.SetInfoHero();
        hero.infoHero.capWar = hero.infoHero.capWar * Mathf.Pow(GameConfig.Instance.Wi, info.level);
        hero.AddHero(number);
        hero.posStart = lsPosIns[lsHeroGoldMine.Count].position;
        hero.transform.localScale = new Vector3(2f, 2f, 2f);
        lsHeroGoldMine.Add(hero);
    }

    public void InstantiateHero(int idHero, int number)
    {
        Hero hero;
        hero = Instantiate(GameManager.Instance.lsPrefabsHero[idHero - 1]
            , lsPosIns[lsHeroGoldMine.Count].position
            , Quaternion.identity
            , GameManager.Instance.enemyManager);
        hero.gameObject.name = "Hero";
        hero.SetInfoHero();
        hero.infoHero.capWar = 0;
        hero.AddHero(number);
        hero.posStart = lsPosIns[lsHeroGoldMine.Count].position;
        hero.transform.localScale = new Vector3(2f, 2f, 2f);
        lsHeroGoldMine.Add(hero);
    }

    public void AttackPlayer()
    {
        if (info.typeGoleMine == TypeGoldMine.Enemy && GameManager.Instance.lsGoldMinePlayer.Count > 0)
        {
            int check = 0;
            float dis = Vector3.Distance(transform.position, GameManager.Instance.lsGoldMinePlayer[0].transform.position);
            List<GoldMine> lsGoldMinePlayer = new List<GoldMine>();
            foreach (GoldMine g in GameManager.Instance.lsGoldMinePlayer)
            {
                if (g != GameManager.Instance.GolHeroBeingAttack)
                {
                    lsGoldMinePlayer.Add(g);
                }
            }
            for (int i = 1; i < lsGoldMinePlayer.Count; i++)
            {
                if (Vector3.Distance(transform.position, lsGoldMinePlayer[i].transform.position) < dis)
                {
                    check = i;
                }
            }
            GameManager.Instance.GolEnemyIsAttack = this;
            GameManager.Instance.GolHeroBeingAttack = lsGoldMinePlayer[check];
            GameManager.Instance.lineEnemyAttack.enabled = true;
            Box boxStart = this.transform.parent.GetComponent<Box>();
            Box boxEnd = lsGoldMinePlayer[check].transform.parent.GetComponent<Box>();
            List<Box> lsPosMove = GameManager.Instance.PathFinding(boxStart, boxEnd);
            float speed = 10f;
            StartCoroutine(IEInsAttackPlayer(speed, lsPosMove));
            UIManager.Instance.panelWarring.SetActive(true);
            UIManager.Instance.detailWarring.GetWarring(0, GameManager.Instance.GolHeroBeingAttack.name + " is under attack");
        }
    }

    public IEnumerator IEInsAttackPlayer(float speed, List<Box> lsPosMove)
    {
        for (int i = 0; i < lsHeroGoldMine.Count; i++)
        {
            Hero hero;
            hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[lsHeroGoldMine[i].infoHero.ID - 1]
                , lsPosIns[0].position
                , Quaternion.identity);
            hero.gameObject.name = "Enemy";
            hero.SetInfoHero();
            hero.infoHero.capWar = 0;
            int numberAttack = (int)(lsHeroGoldMine[i].infoHero.countHero * 0.5f);
            hero.AddHero(numberAttack);
            lsHeroGoldMine[i].AddHero(-numberAttack);
            hero.speedMin = speed;
            hero.transform.localScale = new Vector3(2f, 2f, 2f);
            hero.StartMoveToLsPosition(lsPosMove);
            GameManager.Instance.lsEnemyAttackGoldMine.Add(hero);
            yield return new WaitForSeconds(2f);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.stateGame == StateGame.Playing)
        {
            if (info.typeGoleMine == TypeGoldMine.Enemy)
            {
                if (other.CompareTag("Castle"))
                {
                    UIManager.Instance.HideAllPanelGame();
                    if (!GameManager.Instance.isBeingAttack)
                    {
                        GameManager.Instance.dateEnemyAttack = GameManager.Instance.dateGame + (long)(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
                        GameManager.Instance.isBeingAttack = true;
                    }
                    GameManager.Instance.GolEnemyBeingAttack = this;
                    GameManager.Instance.posTriggerGoldMine = other.transform.position;
                    AttackGoldMineEnemy();

                    ScenesManager.Instance.GoToScene(1,() =>
                    {
                        GameManager.Instance.isAttacking = true;
                        GameManager.Instance.isAttackGoldMineEnemy = true;
                        if (GameManager.Instance.lsEnemyAttackGoldMine.Count > 0)
                        {
                            for (int i = 0; i < GameManager.Instance.lsEnemyAttackGoldMine.Count; i++)
                            {
                                GameManager.Instance.lsEnemyAttackGoldMine[i].isPause = true;
                            }
                        }
                    }, () =>
                    {
                        StartCoroutine(IEAttack());
                    });

                }
            }
            else
            {
                if (other.CompareTag("Castle"))
                {
                    GameManager.Instance.goldMineInSide = this;
                }
                if (other.CompareTag("Enemy"))
                {
                    GameManager.Instance.lineEnemyAttack.enabled = false;
                    UIManager.Instance.HideAllPanelGame();
                    foreach (Hero hero in GameManager.Instance.lsEnemyAttackGoldMine)
                    {
                        hero.isPause = true;
                        hero.GetComponent<BoxCollider2D>().enabled = false;
                        hero.transform.position = new Vector3(3000, 3000, 3000);
                    }
                    GameManager.Instance.GolHeroBeingAttack = this;
                    GameManager.Instance.posTriggerGoldMine = other.transform.position;
                    AttackGoldMineHero();

                    if (lsHeroGoldMine.Count > 0)
                    {
                        GameManager.Instance.isBreak = false;
                        ScenesManager.Instance.GoToScene(1,() =>
                        {
                            GameManager.Instance.isAttacking = true;
                            GameManager.Instance.isAttackGoldMineEnemy = false;
                        }, () =>
                        {
                            StartCoroutine(IEAttack());
                        });
                    }
                    else
                    {
                        GameManager.Instance.isBreak = true;
                        GameManager.Instance.isAttacking = true;
                        GameManager.Instance.isAttackGoldMineEnemy = false;
                    }
                }
            }
        }
    }

    public IEnumerator IEAttack()
    {
        UIManager.Instance.panelLetGo.SetActive(true);
        UIManager.Instance.txtLetGo.text = "3";
        yield return new WaitForSeconds(1f);
        UIManager.Instance.txtLetGo.text = "2";
        yield return new WaitForSeconds(1f);
        UIManager.Instance.txtLetGo.text = "1";
        yield return new WaitForSeconds(1f);
        UIManager.Instance.panelLetGo.SetActive(false);
        foreach (Hero hero in GameManager.Instance.lsHero)
        {
            hero.isAttack = true;
        }
        foreach (Hero hero in GameManager.Instance.lsEnemy)
        {
            hero.isAttack = true;
        }
    }

    public void BeginAttack()
    {
        UIManager.Instance.GetUIAttack();
    }

    public void AttackGoldMineEnemy()
    {
        BeginAttack();
        UIManager.Instance.panelReleaseAttack.SetActive(true);
        GameManager.Instance.numberThrowHero = 3;
        for (int i = 0; i < lsHeroGoldMine.Count && i < 3; i++)
        {
            if (lsHeroGoldMine[i].infoHero.countHero > 0)
            {
                if (lsHeroGoldMine[i].infoHero.ID != 2 && lsHeroGoldMine[i].infoHero.ID != 1)
                {
                    Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[lsHeroGoldMine[i].infoHero.ID - 1], GameManager.Instance.lsPosEnemy[i]);
                    hero.gameObject.name = "Enemy";
                    hero.SetInfoHero();
                    hero.infoHero.capWar = 0;
                    hero.countHeroStart = lsHeroGoldMine[i].infoHero.countHero;
                    hero.AddHero(lsHeroGoldMine[i].infoHero.countHero);
                    hero.goldMine = this;
                    hero.idGoldMine = i;
                    GameManager.Instance.lsEnemy.Add(hero);
                }
                else if (lsHeroGoldMine[i].infoHero.ID == 2)
                {
                    float XADD = -0.5f;
                    for (int j = 0; j < 3; j++)
                    {
                        Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[1], GameManager.Instance.lsPosEnemy[i]);
                        if (j == 1)
                        {
                            hero.transform.position += new Vector3(0, -0.5f, 0f);
                        }
                        else
                        {
                            hero.transform.position += new Vector3(XADD, 0f, 0f);
                        }
                        XADD += 0.5f;
                        hero.gameObject.name = "Enemy";
                        hero.SetInfoHero();
                        hero.infoHero.capWar = 0;
                        hero.AddHero(lsHeroGoldMine[i].infoHero.countHero / 3);
                        hero.goldMine = this;
                        hero.idGoldMine = i;
                        GameManager.Instance.lsEnemy.Add(hero);
                    }
                }
                else if (lsHeroGoldMine[i].infoHero.ID == 1)
                {
                    float XADD = -0.5f;
                    for (int j = 0; j < 4; j++)
                    {
                        Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[0], GameManager.Instance.lsPosEnemy[i]);
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
                        hero.AddHero(lsHeroGoldMine[i].infoHero.countHero / 4);
                        hero.goldMine = this;
                        hero.idGoldMine = i;
                        GameManager.Instance.lsEnemy.Add(hero);
                    }
                }
            }
        }
        int countHero = 0;
        Castle castle = GameManager.Instance.castlePlayer;
        for (int i = 0; i < 3; i++)
        {
            ItemHeroAttack item = UIManager.Instance.lsItemHeroAttack[i];
            if (i < castle.lsHouseRelease.Count && castle.lsHouseRelease[i].info.countHero > 0)
            {
                countHero++;
                item.gameObject.SetActive(true);
                item.countHero = castle.lsHouseRelease[i].info.countHero;
                item.houseHero = castle.lsHouseRelease[i];
                item.iconHero.sprite = UIManager.Instance.lsSprAvatarHero[castle.lsHouseRelease[i].info.idHero - 1];
                item.txtCountHero.text = UIManager.Instance.lsItemHeroAttack[i].countHero.ToString();
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
        GameManager.Instance.numberThrowHero = countHero;
    }

    public void AttackGoldMineHero()
    {
        BeginAttack();
        for (int i = 0; i < lsHeroGoldMine.Count && i < 3; i++)
        {
            if (lsHeroGoldMine[i].infoHero.countHero > 0)
            {
                if (lsHeroGoldMine[i].infoHero.ID != 2 && lsHeroGoldMine[i].infoHero.ID != 1)
                {
                    Hero hero = Instantiate(GameManager.Instance.lsPrefabsHero[lsHeroGoldMine[i].infoHero.ID - 1], GameManager.Instance.lsPosHero[i]);
                    hero.gameObject.name = "Hero";
                    hero.SetInfoHero();
                    hero.infoHero.capWar = 0;
                    hero.countHeroStart = lsHeroGoldMine[i].infoHero.countHero;
                    hero.AddHero(lsHeroGoldMine[i].infoHero.countHero);
                    hero.goldMine = this;
                    hero.idGoldMine = i;
                    GameManager.Instance.lsHero.Add(hero);
                }
                else if (lsHeroGoldMine[i].infoHero.ID == 2)
                {
                    float XADD = -0.5f;
                    for (int j = 0; j < 3; j++)
                    {
                        Hero hero = Instantiate(GameManager.Instance.lsPrefabsHero[1], GameManager.Instance.lsPosHero[i]);
                        if (j == 1)
                        {
                            hero.transform.position += new Vector3(0, -0.5f, 0f);
                        }
                        else
                        {
                            hero.transform.position += new Vector3(XADD, 0f, 0f);
                        }
                        XADD += 0.5f;
                        hero.gameObject.name = "Hero";
                        hero.SetInfoHero();
                        hero.infoHero.capWar = 0;
                        hero.AddHero(lsHeroGoldMine[i].infoHero.countHero / 3);
                        hero.goldMine = this;
                        hero.idGoldMine = i;
                        GameManager.Instance.lsHero.Add(hero);

                    }
                }
                else if (lsHeroGoldMine[i].infoHero.ID == 1)
                {
                    float XADD = -0.5f;
                    for (int j = 0; j < 4; j++)
                    {
                        Hero hero = Instantiate(GameManager.Instance.lsPrefabsHero[0], GameManager.Instance.lsPosHero[i]);
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
                        hero.AddHero(lsHeroGoldMine[i].infoHero.countHero / 4);
                        hero.goldMine = this;
                        hero.idGoldMine = i;
                        GameManager.Instance.lsHero.Add(hero);

                    }
                }
            }
        }

        for (int i = 0; i < GameManager.Instance.lsEnemyAttackGoldMine.Count && i < 3; i++)
        {
            if (GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.countHero > 0)
            {
                if (GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.ID != 2 && GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.ID != 1)
                {
                    Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.ID - 1], GameManager.Instance.lsPosEnemy[i]);
                    hero.gameObject.name = "Enemy";
                    hero.SetInfoHero();
                    hero.infoHero.capWar = 0;
                    hero.countHeroStart = GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.countHero;
                    hero.AddHero(GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.countHero);
                    GameManager.Instance.lsEnemy.Add(hero);
                }
                else if (GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.ID == 2)
                {
                    float XADD = -0.5f;
                    for (int j = 0; j < 3; j++)
                    {
                        Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[1], GameManager.Instance.lsPosEnemy[i]);
                        if (j == 1)
                        {
                            hero.transform.position += new Vector3(0, -0.5f, 0f);
                        }
                        else
                        {
                            hero.transform.position += new Vector3(XADD, 0f, 0f);
                        }
                        XADD += 0.5f;
                        hero.gameObject.name = "Enemy";
                        hero.SetInfoHero();
                        hero.infoHero.capWar = 0;
                        hero.AddHero(GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.countHero / 3);
                        GameManager.Instance.lsEnemy.Add(hero);
                    }
                }
                else if (GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.ID == 1)
                {
                    float XADD = -0.5f;
                    for (int j = 0; j < 4; j++)
                    {
                        Hero hero = Instantiate(GameManager.Instance.lsPrefabsEnemy[0], GameManager.Instance.lsPosEnemy[i]);
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
                        hero.AddHero(GameManager.Instance.lsEnemyAttackGoldMine[i].infoHero.countHero / 4);
                        GameManager.Instance.lsEnemy.Add(hero);
                    }
                }
            }
        }
    }

    public void DeleteHero()
    {
        foreach (Hero h in lsHeroGoldMine)
        {
            Destroy(h.gameObject);
        }
        lsHeroGoldMine.Clear();
    }

}
