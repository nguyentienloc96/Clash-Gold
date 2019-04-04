using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public enum StateGame
{
    Loading,
    Home,
    Playing,
    Finished
}

public enum TypeEquip
{
    Heath,
    HitSpeed,
    Damage,
    PriceHouse,
    PriceUpgrade
}

[System.Serializable]
public struct Equipment
{
    public int IDHero;
    public TypeEquip typeEquip;
    public float percent;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = new GameManager();

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public StateGame stateGame;
    public bool isAttacking;

    [Header("DateTime")]
    public long dateGame;
    public Text txtDate;
    private float time;

    [Header("INFO PLAYER")]
    public long gold;
    public int coin;
    public float ratioBorn;

    [Header("MAP")]
    public GameObject prefabsBox;
    public int idMapBox = -1;
    public int col;
    public int row;
    public int addMapX;
    public int addMapY;
    public float weight;
    public Transform boxManager;
    public Box[,] arrBox;
    public GameObject[] prefabsBoxMap;
    public List<Box> lsBoxMove = new List<Box>();
    public List<Box> lsBoxCanMove = new List<Box>();
    public List<Sprite> lsSpriteMap = new List<Sprite>();
    public List<Box> lsBoxManager = new List<Box>();
    public LineRenderer lineEnemyAttack;

    [Header("GOLD MINE")]
    public List<GoldMine> lsGoldMinePlayer = new List<GoldMine>();
    public List<GoldMine> lsGoldMineEnemy = new List<GoldMine>();
    public List<GoldMine> lsGoldMineManager = new List<GoldMine>();

    [Header("HOUSE")]
    public int maxLevelHouse;
    public List<House> lsHousePlayer = new List<House>();
    public List<BuildHouse> lsBuildHouse = new List<BuildHouse>();

    [Header("OTHER")]
    public Castle castlePlayer;
    public Transform enemyManager;
    public long dateEnemyAttack;
    public long dateUpGoldMine;

    [Header("EQUIPMENT")]
    public List<Equipment> lsEquip = new List<Equipment>();
    public List<ItemEquipment> lsItemEquip = new List<ItemEquipment>();
    public ItemEquipmentSelect itemEquipPrefab;

    [Header("INSHERO")]
    public List<Hero> lsPrefabsHero = new List<Hero>();
    public List<Hero> lsPrefabsEnemy = new List<Hero>();
    public int[] lsHeroFly;

    [Header("ATTACK")]
    public List<Hero> lsHero = new List<Hero>();
    public List<Hero> lsEnemy = new List<Hero>();
    public List<Hero> lsChild = new List<Hero>();

    [Header("ATTACK")]
    public List<Transform> lsPosHero;
    public List<Transform> lsPosEnemy;
    [HideInInspector]
    public bool isAttackGoldMineEnemy;
    [HideInInspector]
    public GoldMine GolEnemyBeingAttack;
    [HideInInspector]
    public GoldMine GolHeroBeingAttack;
    [HideInInspector]
    public GoldMine GolEnemyIsAttack;
    [HideInInspector]
    public GoldMine goldMineCurrent;
    [HideInInspector]
    public Vector3 posTriggerGoldMine;
    [HideInInspector]
    public List<Hero> lsEnemyAttackGoldMine = new List<Hero>();
    [HideInInspector]
    public ItemHeroAttack itemSelectHero;
    [HideInInspector]
    public int numberThrowHero;
    [HideInInspector]
    public GoldMine goldMineInSide;
    public bool isBeingAttack;

    public GameObject itemHero;

    public bool isBreak;

    private void Start()
    {
        arrBox = new Box[col, row];
    }

    private void Update()
    {
        if (stateGame == StateGame.Playing)
        {
            time += Time.deltaTime;
            if (time >= GameConfig.Instance.Timeday)
            {
                dateGame = dateGame + 1;
                SetDateUI();
                this.PostEvent(EventID.NextDay);
                time = 0;
            }

            if (isBeingAttack && dateGame >= dateEnemyAttack && !isAttacking && lsGoldMinePlayer.Count > 0)
            {
                if (lsEnemyAttackGoldMine.Count <= 0)
                {
                    int a = UnityEngine.Random.Range(0, lsGoldMineEnemy.Count);
                    lsGoldMineEnemy[a].AttackPlayer();
                    dateEnemyAttack = dateGame + (long)(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
                }
            }

            if (dateGame >= dateUpGoldMine && lsGoldMineEnemy.Count > 0)
            {
                int landUpMax = (lsGoldMineEnemy.Count / GameConfig.Instance.LandDiv);
                if (landUpMax < 1)
                    landUpMax = 1;
                int landUp;
                if (landUpMax >= GameConfig.Instance.LandUP)
                {
                    landUp = UnityEngine.Random.Range(GameConfig.Instance.LandUP, landUpMax);
                }
                else
                {
                    landUp = UnityEngine.Random.Range(landUpMax, GameConfig.Instance.LandUP);
                }

                int numberGoldMine = 0;
                if (numberGoldMine < landUp)
                {
                    for (int i = 0; i < lsGoldMineEnemy.Count; i++)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
                        {
                            lsGoldMineEnemy[i].AddLevel();
                            numberGoldMine++;
                        }
                    }
                }
                dateUpGoldMine = dateGame + (long)(GameConfig.Instance.TimeUp);
            }

            if (lsGoldMineEnemy.Count <= 0)
            {
                stateGame = StateGame.Finished;
                UIManager.Instance.panelVictory.SetActive(true);
            }

            if (lsGoldMinePlayer.Count <= 0)
            {
                stateGame = StateGame.Finished;
                UIManager.Instance.panelGameOver.SetActive(true);
            }

            if (isAttacking)
            {
                if (isAttackGoldMineEnemy)
                {
                    if (itemSelectHero != null)
                    {
                        if (Input.GetMouseButtonUp(0) && !UIManager.Instance.panelLetGo.activeSelf)
                        {
                            Vector3 posIns = UIManager.Instance.cameraAttack.ScreenToWorldPoint(Input.mousePosition);
                            posIns.z = 0f;
                            if (posIns.y > 0)
                            {
                                posIns.y = 0;
                            }
                            if (posIns.y < lsPosHero[0].position.y)
                            {
                                posIns.y = lsPosHero[0].position.y;
                            }
                            ThrowHero(itemSelectHero.houseHero, itemSelectHero.countHero, posIns);
                            itemSelectHero.gameObject.SetActive(false);
                            itemSelectHero = null;
                            numberThrowHero--;
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonUp(0) && !UIManager.Instance.panelLetGo.activeSelf)
                        {
                            if (UIManager.Instance.lsItemHeroAttack[0].gameObject.activeSelf || UIManager.Instance.lsItemHeroAttack[1].gameObject.activeSelf || UIManager.Instance.lsItemHeroAttack[2].gameObject.activeSelf)
                            {
                                Vector3 posIns = UIManager.Instance.cameraAttack.ScreenToWorldPoint(Input.mousePosition);
                                posIns.z = 0f;
                                if (posIns.y > 0)
                                {
                                    posIns.y = 0;
                                }
                                if (posIns.y < lsPosHero[0].position.y)
                                {
                                    posIns.y = lsPosHero[0].position.y;
                                }
                                ItemHeroAttack item;
                                if (UIManager.Instance.lsItemHeroAttack[0].gameObject.activeSelf)
                                {
                                    item = UIManager.Instance.lsItemHeroAttack[0];
                                }
                                else if (UIManager.Instance.lsItemHeroAttack[1].gameObject.activeSelf)
                                {
                                    item = UIManager.Instance.lsItemHeroAttack[1];
                                }
                                else
                                {
                                    item = UIManager.Instance.lsItemHeroAttack[2];
                                }
                                ThrowHero(item.houseHero, item.countHero, posIns);
                                item.gameObject.SetActive(false);
                                itemSelectHero = null;
                                numberThrowHero--;
                            }
                        }
                    }

                    if (lsEnemy.Count <= 0)
                    {
                        EndAttack();
                        GolEnemyBeingAttack.DeleteHero();
                        dateEnemyAttack = dateGame + (long)(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
                        ScenesManager.Instance.GoToScene(1, () =>
                         {
                             GolEnemyBeingAttack.info.typeGoleMine = TypeGoldMine.Player;
                             GolEnemyBeingAttack.SetSpriteBox(0);
                             lsGoldMineEnemy.Remove(GolEnemyBeingAttack);
                             lsGoldMinePlayer.Add(GolEnemyBeingAttack);
                             if (lsEnemyAttackGoldMine.Count > 0)
                             {
                                 for (int i = 0; i < lsEnemyAttackGoldMine.Count; i++)
                                 {
                                     lsEnemyAttackGoldMine[i].isPause = false;
                                 }
                             }
                         }, () =>
                         {
                             UIManager.Instance.panelWarring.SetActive(true);
                             UIManager.Instance.detailWarring.GetWarring(3, "You have conquered " + GolEnemyBeingAttack.info.name);
                         });
                    }
                    else if (lsHero.Count <= 0 && numberThrowHero <= 0)
                    {
                        EndAttack();
                        if (goldMineInSide != null && GolEnemyBeingAttack != null && goldMineInSide.info.ID == GolEnemyBeingAttack.info.ID)
                        {
                            if (lsGoldMinePlayer.Count > 0)
                            {
                                int idGoldMineCheck = 0;
                                float disGoldPlayer = Vector3.Distance(lsGoldMinePlayer[0].transform.position, GolEnemyBeingAttack.transform.position);
                                for (int i = 1; i < lsGoldMinePlayer.Count; i++)
                                {
                                    if (disGoldPlayer > Vector3.Distance(lsGoldMinePlayer[i].transform.position, GolEnemyBeingAttack.transform.position))
                                    {
                                        idGoldMineCheck = i;
                                    }
                                }

                                castlePlayer.transform.localPosition = lsGoldMinePlayer[idGoldMineCheck].transform.position;
                                castlePlayer.posMove = lsGoldMinePlayer[idGoldMineCheck].transform.position;
                            }

                        }
                        else
                        {
                            Vector3 posGoldMine = Vector3.zero;
                            posGoldMine = GolEnemyBeingAttack.transform.position;
                            Vector3 posMoveEnd = posTriggerGoldMine - (posGoldMine - posTriggerGoldMine).normalized;
                            castlePlayer.transform.localPosition = posMoveEnd;
                            castlePlayer.posMove = posMoveEnd;
                        }
                        ScenesManager.Instance.GoToScene(1, () =>
                         {
                             if (lsEnemyAttackGoldMine.Count > 0)
                             {
                                 for (int i = 0; i < lsEnemyAttackGoldMine.Count; i++)
                                 {
                                     lsEnemyAttackGoldMine[i].isPause = false;
                                 }
                             }
                         }, () =>
                         {
                             UIManager.Instance.panelWarring.SetActive(true);
                             UIManager.Instance.detailWarring.GetWarring(4, "You lose");
                         });
                    }
                }
                else
                {
                    if (lsHero.Count <= 0)
                    {
                        EndAttack();
                        GolHeroBeingAttack.DeleteHero();
                        if (isBreak)
                        {
                            GolHeroBeingAttack.info.typeGoleMine = TypeGoldMine.Enemy;
                            GolHeroBeingAttack.SetSpriteBox(maxLevelHouse);
                            lsGoldMinePlayer.Remove(GolHeroBeingAttack);
                            lsGoldMineEnemy.Add(GolHeroBeingAttack);
                            for (int i = 0; i < lsEnemyAttackGoldMine.Count; i++)
                            {
                                GolHeroBeingAttack.InstantiateEnemy(lsEnemyAttackGoldMine[i].infoHero.ID
                                    , lsEnemyAttackGoldMine[i].infoHero.countHero, i);
                            }
                            foreach (Hero h in lsEnemyAttackGoldMine)
                            {
                                Destroy(h.gameObject);
                            }
                            lsEnemyAttackGoldMine.Clear();
                            isBreak = false;
                        }
                        else
                        {
                            ScenesManager.Instance.GoToScene(1, () =>
                             {
                                 GolHeroBeingAttack.info.typeGoleMine = TypeGoldMine.Enemy;
                                 GolHeroBeingAttack.SetSpriteBox(maxLevelHouse);
                                 lsGoldMinePlayer.Remove(GolHeroBeingAttack);
                                 lsGoldMineEnemy.Add(GolHeroBeingAttack);
                                 for (int i = 0; i < lsEnemyAttackGoldMine.Count; i++)
                                 {
                                     GolHeroBeingAttack.InstantiateEnemy(lsEnemyAttackGoldMine[i].infoHero.ID
                                         , lsEnemyAttackGoldMine[i].infoHero.countHero, i);
                                 }
                                 foreach (Hero h in lsEnemyAttackGoldMine)
                                 {
                                     Destroy(h.gameObject);
                                 }
                                 lsEnemyAttackGoldMine.Clear();
                             }, () =>
                             {
                                 UIManager.Instance.panelWarring.SetActive(true);
                                 UIManager.Instance.detailWarring.GetWarring(1, GolHeroBeingAttack.info.name + " is invaded");
                             });
                        }
                    }
                    else if (lsEnemy.Count <= 0)
                    {
                        EndAttack();
                        ScenesManager.Instance.GoToScene(1, () =>
                         {
                             foreach (Hero h in lsEnemyAttackGoldMine)
                             {
                                 Destroy(h.gameObject);
                             }
                             lsEnemyAttackGoldMine.Clear();
                         }, () =>
                         {
                             UIManager.Instance.panelWarring.SetActive(true);
                             UIManager.Instance.detailWarring.GetWarring(2, GolHeroBeingAttack.info.name + " is protected successfully");
                         });
                    }
                }
            }
        }
        else if (stateGame == StateGame.Finished)
        {
            if (Input.GetMouseButtonDown(0) &&
                (UIManager.Instance.panelGameOver.activeSelf ||
                UIManager.Instance.panelVictory.activeSelf))
            {
                UIManager.Instance.panelGameOver.SetActive(false);
                UIManager.Instance.panelVictory.SetActive(false);
            }
        }
    }

    #region === ATTACK ===
    public void EndAttack()
    {
        isAttacking = false;
        UIManager.Instance.cavas.worldCamera = DeadzoneCamera.Instance._camera;
        UIManager.Instance.canvasLoading.worldCamera = DeadzoneCamera.Instance._camera;
        UIManager.Instance.mapAttack.SetActive(false);
        UIManager.Instance.panelReleaseAttack.SetActive(false);
        OnEndAttack();
        if (isBreak)
        {
            Vector3 posCastle = castlePlayer.transform.position;
            posCastle.z = -10f;
            DeadzoneCamera.Instance._camera.transform.position = posCastle;
        }
    }

    public void OnEndAttack()
    {
        foreach (Hero hero in lsChild)
        {
            Destroy(hero.gameObject);
        }
        lsChild.Clear();

        foreach (Hero hero in lsHero)
        {
            Destroy(hero.gameObject);
        }
        lsHero.Clear();

        foreach (Hero hero in lsEnemy)
        {
            Destroy(hero.gameObject);
        }
        lsEnemy.Clear();
    }

    public void ThrowHero(House houseHero, int countHero, Vector3 posIns)
    {
        if (houseHero.info.idHero != 2 && houseHero.info.idHero != 1)
        {
            Hero hero = Instantiate(lsPrefabsHero[houseHero.info.idHero - 1], posIns, Quaternion.identity);
            hero.gameObject.name = "Hero";
            hero.SetInfoHero();
            hero.infoHero.capWar = 0;
            hero.countHeroStart = countHero;
            hero.AddHero(countHero);
            hero.house = houseHero;
            hero.isAttack = true;
            lsHero.Add(hero);
        }
        else if (houseHero.info.idHero == 2)
        {
            float XADD = -0.5f;
            for (int j = 0; j < 3; j++)
            {
                Hero hero = Instantiate(lsPrefabsHero[1], posIns, Quaternion.identity);
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
                hero.AddHero(countHero / 3);
                hero.isAttack = true;
                hero.house = houseHero;
                lsHero.Add(hero);
            }
        }
        else if (houseHero.info.idHero == 1)
        {
            float XADD = -0.5f;
            for (int j = 0; j < 4; j++)
            {
                Hero hero = Instantiate(lsPrefabsHero[0], posIns, Quaternion.identity);
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
                hero.AddHero(countHero / 4);
                hero.isAttack = true;
                hero.house = houseHero;
                lsHero.Add(hero);
            }
        }


        for (int i = 0; i < lsEnemy.Count; i++)
        {
            lsEnemy[i].targetCompetitor = null;
        }
    }
    #endregion

    #region === MAP ===
    public void GenerateMapBox()
    {
        idMapBox = UnityEngine.Random.Range(0, GameConfig.Instance.listMap.Count);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Box b = Instantiate(prefabsBox, boxManager.position + new Vector3(j * (weight), -i * (weight)), Quaternion.identity, boxManager).GetComponent<Box>();
                b.info.col = j;
                b.info.row = i;
                arrBox[j, i] = b;
                if (!CheckPos(i, j, idMapBox))
                {
                    b.gameObject.layer = 13;
                    b.info.isLock = true;
                    lsBoxCanMove.Add(b);
                    b.info.goldMine = null;
                }
                else
                {
                    b.transform.GetChild(0).gameObject.SetActive(false);
                    b.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    lsBoxMove.Add(b);
                }
                lsBoxManager.Add(b);
            }
        }

        StartCoroutine(SetBox());
    }

    public IEnumerator SetBox()
    {
        foreach (Box box in lsBoxCanMove)
        {
            CheckBoxCanMove(box);
        }
        yield return new WaitForEndOfFrame();
        List<Box> lsBox = new List<Box>();
        foreach (Box box in lsBoxMove)
        {
            if (CheckGoldMinePlayer(box) >= 3)
            {
                lsBox.Add(box);
            }
        }
        yield return new WaitUntil(() => lsBox.Count > 0);
        int numberId = 0;
        int idGoldMinePlayer = UnityEngine.Random.Range(0, lsBox.Count);
        Box bPlayer = lsBox[idGoldMinePlayer];
        yield return new WaitUntil(() => bPlayer != null);
        Box bNearPlayer = null;
        if (bPlayer.info.col != 8 && !arrBox[bPlayer.info.col + 1, bPlayer.info.row].info.isLock)
        {
            bNearPlayer = arrBox[bPlayer.info.col + 1, bPlayer.info.row];
        }
        else if (bPlayer.info.row != 8 && !arrBox[bPlayer.info.col, bPlayer.info.row + 1].info.isLock)
        {
            bNearPlayer = arrBox[bPlayer.info.col, bPlayer.info.row + 1];
        }
        else if (bPlayer.info.col != 0 && !arrBox[bPlayer.info.col - 1, bPlayer.info.row].info.isLock)
        {
            bNearPlayer = arrBox[bPlayer.info.col - 1, bPlayer.info.row];
        }
        else if (bPlayer.info.row != 0 && !arrBox[bPlayer.info.col, bPlayer.info.row - 1].info.isLock)
        {
            bNearPlayer = arrBox[bPlayer.info.col, bPlayer.info.row - 1];
        }
        yield return new WaitUntil(() => bNearPlayer != null);
        GenerateMap(bPlayer.transform, numberId, 1, bPlayer, true);
        numberId++;
        Vector3 posIns = bPlayer.transform.position;
        posIns.z = -2;
        castlePlayer.transform.position = posIns;
        posIns.z = -10;
        DeadzoneCamera.Instance._camera.transform.position = posIns;
        yield return new WaitForEndOfFrame();
        foreach (Box b in lsBoxMove)
        {
            if (b != bPlayer)
            {
                if (b != bNearPlayer)
                {
                    GenerateMap(b.transform, numberId, UnityEngine.Random.Range(1, 6), b);
                }
                else
                {
                    GenerateMap(b.transform, numberId, 1, b);
                }
                numberId++;
            }
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => lsBoxManager.Count == col * row);
        BuildHouse();
        yield return new WaitForEndOfFrame();
        this.PostEvent(EventID.StartGame);
        stateGame = StateGame.Playing;
    }

    public void GenerateMapBoxJson()
    {
        if (idMapBox != -1)
        {
            stateGame = StateGame.Playing;
            return;
        }
        idMapBox = DataPlayer.Instance.idMapBox;
        maxLevelHouse = DataPlayer.Instance.maxLevelHouse;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Box b = Instantiate(prefabsBox, boxManager.position + new Vector3(j * (weight), -i * (weight)), Quaternion.identity, boxManager).GetComponent<Box>();
                b.info.col = j;
                b.info.row = i;
                arrBox[j, i] = b;
                if (!CheckPos(i, j, idMapBox))
                {
                    b.gameObject.layer = 13;
                    b.info.isLock = true;
                    lsBoxCanMove.Add(b);
                    b.info.goldMine = null;
                }
                else
                {
                    b.transform.GetChild(0).gameObject.SetActive(false);
                    b.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    lsBoxMove.Add(b);
                }
                lsBoxManager.Add(b);
            }
        }

        StartCoroutine(SetBoxJson());
    }

    public IEnumerator SetBoxJson()
    {
        foreach (Box box in lsBoxCanMove)
        {
            CheckBoxCanMove(box);
        }
        yield return new WaitForEndOfFrame();
        int isNext = 0;
        for (int i = 0; i < DataPlayer.Instance.lsBox.Count; i++)
        {
            if (DataPlayer.Instance.lsBox[i].goldMineInfo.ID != -1)
            {
                isNext++;
                Debug.Log(isNext);
                Vector3 _rotation = new Vector3(DataPlayer.Instance.lsBox[i].goldMineInfo.xR, DataPlayer.Instance.lsBox[i].goldMineInfo.yR, DataPlayer.Instance.lsBox[i].goldMineInfo.zR);
                GenerateMapJson(lsBoxManager[i].transform,
                    lsBoxManager[i],
                    DataPlayer.Instance.lsBox[i].goldMineInfo.indexLoadGoldMine,
                    _rotation,
                    DataPlayer.Instance.lsBox[i].goldMineInfo,
                    DataPlayer.Instance.lsBox[i].goldMineInfo.isPlayer);
            }
        }
        yield return new WaitUntil(() => isNext == lsBoxMove.Count);
        yield return new WaitForEndOfFrame();
        int rdPosCastle = UnityEngine.Random.Range(0, lsGoldMinePlayer.Count);
        Vector3 posCastle = lsGoldMinePlayer[rdPosCastle].transform.position;
        castlePlayer.transform.position = posCastle;
        posCastle.z = -10;
        DeadzoneCamera.Instance._camera.transform.position = posCastle;
        for (int i = 0; i < DataPlayer.Instance.lsHousePlayer.Count; i++)
        {
            if (DataPlayer.Instance.lsHousePlayer[i].heroInfo.ID != 0)
            {
                House h = lsHousePlayer[i];
                h.info.typeState = DataPlayer.Instance.lsHousePlayer[i].isLock ? TypeStateHouse.Lock : TypeStateHouse.None;
                if (!DataPlayer.Instance.lsHousePlayer[i].isLock)
                {
                    h.RegisterListener(EventID.NextDay, (param) => h.OnNextDay());
                }
                h.info.level = DataPlayer.Instance.lsHousePlayer[i].level;
                h.info.idHero = DataPlayer.Instance.lsHousePlayer[i].heroInfo.ID;
                h.info.price = DataPlayer.Instance.lsHousePlayer[i].price;
                h.info.capWar = DataPlayer.Instance.lsHousePlayer[i].capWar;
                h.info.countHero = DataPlayer.Instance.lsHousePlayer[i].heroInfo.CountHero;
                h.imgNotBuild.enabled = false;
                h.panelHouse.SetActive(true);
                h.txtCountHero.gameObject.SetActive(true);
                h.txtLevel.text = lsHousePlayer[i].info.level.ToString();
                h.imgHouse.sprite = UIManager.Instance.lsSprAvatarHero[h.info.idHero - 1];
                UIManager.Instance.lsBtnIconHouse[h.info.idHero - 1].interactable = false;
            }
        }
        for (int i = 0; i < DataPlayer.Instance.castlePlayer.lsHouse.Count; i++)
        {
            int idHouse = DataPlayer.Instance.castlePlayer.lsHouse[i].ID;
            castlePlayer.lsHouseRelease.Add(lsHousePlayer[idHouse]);
            UIManager.Instance.lstAvatarHeroRelease[i].gameObject.SetActive(true);
            UIManager.Instance.lstAvatarHeroRelease[i].sprite = UIManager.Instance.lsSprAvatarHero[lsHousePlayer[idHouse].info.idHero - 1];
        }
        yield return new WaitForEndOfFrame();
        BuildHouseJson();
        yield return new WaitForEndOfFrame();
        this.PostEvent(EventID.StartGame);
        stateGame = StateGame.Playing;
    }

    public bool CheckPos(int row, int col, int id)
    {
        bool isCheck = false;
        foreach (Vector2 v2 in GameConfig.Instance.listMap[id])
        {
            if (new Vector2(v2.x + addMapX, v2.y + addMapY) == new Vector2(col, row))
            {
                isCheck = true;
            }
        }
        return isCheck;
    }

    public void GenerateMap(Transform toPos, int id, int level, Box box, bool isGoldPlayer = false)
    {
        int typeGoldMine = (int)UnityEngine.Random.Range(0, 2.9f);
        int randomAngle = UnityEngine.Random.Range(0, 4);
        Vector3 _rotation;
        if (randomAngle == 0)
        {
            _rotation = new Vector3(0, 0, 0);
        }
        else if (randomAngle == 1)
        {
            _rotation = new Vector3(180, 0, 0);
        }
        else if (randomAngle == 2)
        {
            _rotation = new Vector3(0, 180, 0);
        }
        else
        {
            _rotation = new Vector3(180, 180, 0);
        }

        if (typeGoldMine == 3)
            _rotation = new Vector3(0, 0, 0);

        if (isGoldPlayer)
        {
            _rotation = new Vector3(0, 0, 0);
            GoldMine goldMine = Instantiate(prefabsBoxMap[2], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
            goldMine.GetInfo(id, GameConfig.Instance.GetNameIsLand(), level, TypeGoldMine.Player, 2);
            goldMine.canvas.localRotation = Quaternion.Euler(_rotation);
            goldMine.info.indexLoadGoldMine = 2;
            lsGoldMinePlayer.Add(goldMine);
            lsGoldMineManager.Add(goldMine);
            box.info.goldMine = goldMine;
        }
        else
        {
            GoldMine goldMine = Instantiate(prefabsBoxMap[typeGoldMine], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
            goldMine.GetInfo(id, GameConfig.Instance.GetNameIsLand(), level, TypeGoldMine.Enemy, typeGoldMine);
            goldMine.canvas.localRotation = Quaternion.Euler(_rotation);
            goldMine.info.indexLoadGoldMine = typeGoldMine;
            lsGoldMineEnemy.Add(goldMine);
            lsGoldMineManager.Add(goldMine);
            box.info.goldMine = goldMine;
        }

    }

    public void GenerateMapJson(Transform toPos, Box box, int typeGoldMine, Vector3 _rotation, GoldMineInfoST gInfo, bool isGoldPlayer = false)
    {
        if (isGoldPlayer)
        {
            _rotation = new Vector3(0, 0, 0);
            GoldMine goldMine = Instantiate(prefabsBoxMap[2], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
            goldMine.GetInfoJson(gInfo);
            goldMine.canvas.localRotation = Quaternion.Euler(_rotation);
            lsGoldMinePlayer.Add(goldMine);
            lsGoldMineManager.Add(goldMine);
            box.info.goldMine = goldMine;
        }
        else
        {
            GoldMine goldMine = Instantiate(prefabsBoxMap[typeGoldMine], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
            goldMine.GetInfoJson(gInfo);
            goldMine.canvas.localRotation = Quaternion.Euler(_rotation);
            lsGoldMineEnemy.Add(goldMine);
            lsGoldMineManager.Add(goldMine);
            box.info.goldMine = goldMine;
        }

    }

    public List<Box> PathFinding(Box boxStart, Box boxEnd)
    {
        List<Box> lsPathFinding = new List<Box>();
        Box boxNext = boxStart;
        lsPathFinding.Add(boxStart);
        while (boxNext != boxEnd)
        {
            Box boxCheck = CheckBoxNext(boxNext, boxEnd);
            if (boxCheck != null)
            {
                boxNext = boxCheck;
                lsPathFinding.Add(boxNext);
            }
            else
            {
                if (lsPathFinding.Count > 0)
                {
                    lsPathFinding.RemoveAt(lsPathFinding.Count - 1);
                    boxNext = lsPathFinding[lsPathFinding.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        if (boxNext == boxEnd)
        {
            if (lsPathFinding.Count > 0)
            {
                lineEnemyAttack.positionCount = lsPathFinding.Count;
                for (int i = lsPathFinding.Count - 1; i >= 0; i--)
                {
                    Vector3 posNext = lsPathFinding[i].transform.position;
                    posNext.z = -0.5f;
                    lineEnemyAttack.SetPosition(lsPathFinding.Count - 1 - i, posNext);
                    if (i < lsPathFinding.Count - 1)
                    {
                        var distance = Vector3.Distance(lsPathFinding[i + 1].transform.position, posNext);
                        lineEnemyAttack.material.mainTextureScale = new Vector2(distance * 2, 1);
                    }
                }
            }
        }

        foreach (Box b in lsBoxMove)
        {
            b.info.isTop = false;
            b.info.isBottom = false;
            b.info.isLeft = false;
            b.info.isRight = false;
        }

        return lsPathFinding;
    }

    public Box CheckBoxNext(Box box, Box boxEnd)
    {
        List<Box> lsBoxSelect = new List<Box>();
        List<int> lsPosBox = new List<int>();
        if (box.info.col != 8 && !box.info.isTop && !arrBox[box.info.col + 1, box.info.row].info.isLock)
        {
            lsBoxSelect.Add(arrBox[box.info.col + 1, box.info.row]);
            lsPosBox.Add(1);
        }
        if (box.info.row != 8 && !box.info.isRight && !arrBox[box.info.col, box.info.row + 1].info.isLock)
        {
            lsBoxSelect.Add(arrBox[box.info.col, box.info.row + 1]);
            lsPosBox.Add(4);
        }
        if (box.info.col != 0 && !box.info.isBottom && !arrBox[box.info.col - 1, box.info.row].info.isLock)
        {
            lsBoxSelect.Add(arrBox[box.info.col - 1, box.info.row]);
            lsPosBox.Add(2);
        }
        if (box.info.row != 0 && !box.info.isLeft && !arrBox[box.info.col, box.info.row - 1].info.isLock)
        {
            lsBoxSelect.Add(arrBox[box.info.col, box.info.row - 1]);
            lsPosBox.Add(3);
        }

        if (lsBoxSelect.Count > 0)
        {
            int check = 0;

            float dis = Vector3.Distance(boxEnd.transform.position, lsBoxSelect[0].transform.position);
            for (int i = 1; i < lsBoxSelect.Count; i++)
            {
                if (dis > Vector3.Distance(boxEnd.transform.position, lsBoxSelect[i].transform.position))
                {
                    check = i;
                    dis = Vector3.Distance(boxEnd.transform.position, lsBoxSelect[i].transform.position);
                }
            }

            if (lsPosBox[check] == 1)
            {
                box.info.isTop = true;
                lsBoxSelect[check].info.isBottom = true;
            }
            else if (lsPosBox[check] == 2)
            {
                box.info.isBottom = true;
                lsBoxSelect[check].info.isTop = true;
            }
            else if (lsPosBox[check] == 3)
            {
                box.info.isLeft = true;
                lsBoxSelect[check].info.isRight = true;
            }
            else if (lsPosBox[check] == 4)
            {
                box.info.isRight = true;
                lsBoxSelect[check].info.isLeft = true;
            }
            return lsBoxSelect[check];
        }
        return null;
    }

    public void CheckBoxCanMove(Box box)
    {
        int idImg = -1;
        Vector3 localScaleImg = Vector3.zero;
        bool isTopRight = false;
        bool isTopLeft = false;
        bool isBottomRight = false;
        bool isBottomLeft = false;
        if (box.info.col != (col - 1) && !arrBox[box.info.col + 1, box.info.row].info.isLock)
        {
            box.info.isRight = true;
        }
        if (box.info.row != (row - 1) && !arrBox[box.info.col, box.info.row + 1].info.isLock)
        {
            box.info.isBottom = true;
        }
        if (box.info.col != 0 && !arrBox[box.info.col - 1, box.info.row].info.isLock)
        {
            box.info.isLeft = true;
        }
        if (box.info.row != 0 && !arrBox[box.info.col, box.info.row - 1].info.isLock)
        {
            box.info.isTop = true;
        }
        if (box.info.col != (col - 1) && box.info.row != (row - 1) && !arrBox[box.info.col + 1, box.info.row + 1].info.isLock)
        {
            isBottomRight = true;
        }
        if (box.info.col != 0 && box.info.row != (row - 1) && !arrBox[box.info.col - 1, box.info.row + 1].info.isLock)
        {
            isBottomLeft = true;
        }
        if (box.info.col != 0 && box.info.row != 0 && !arrBox[box.info.col - 1, box.info.row - 1].info.isLock)
        {
            isTopLeft = true;
        }
        if (box.info.col != (col - 1) && box.info.row != 0 && !arrBox[box.info.col + 1, box.info.row - 1].info.isLock)
        {
            isTopRight = true;
        }



        if (box.info.isTop && box.info.isBottom && box.info.isRight && box.info.isLeft)
        {
            idImg = 5;
            int rdom = UnityEngine.Random.Range(0, 4);
            localScaleImg = new Vector3(0f, 0f, 90f * rdom);
        }
        else if (box.info.isBottom && box.info.isRight && box.info.isLeft)
        {
            idImg = 4;
            localScaleImg = new Vector3(0f, 0f, -90f);
        }
        else if (box.info.isTop && box.info.isRight && box.info.isLeft)
        {
            idImg = 4;
            localScaleImg = new Vector3(0f, 0f, 90f);
        }
        else if (box.info.isTop && box.info.isBottom && box.info.isRight)
        {
            idImg = 4;
        }
        else if (box.info.isTop && box.info.isBottom && box.info.isLeft)
        {
            idImg = 4;
            localScaleImg = new Vector3(0f, 0f, 180f);
        }
        else if (box.info.isTop && box.info.isBottom)
        {
            idImg = 2;
            localScaleImg = new Vector3(0f, 0f, 90f);
        }
        else if (box.info.isTop && box.info.isLeft)
        {
            if (isBottomRight)
            {
                idImg = 6;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
            else
            {
                idImg = 3;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
        }
        else if (box.info.isTop && box.info.isRight)
        {
            if (isBottomLeft)
            {
                idImg = 6;
                localScaleImg = new Vector3(0f, 0f, 90f);
            }
            else
            {
                idImg = 3;
                localScaleImg = new Vector3(0f, 0f, 90f);
            }
        }
        else if (box.info.isBottom && box.info.isLeft)
        {
            if (isTopRight)
            {
                idImg = 6;
                localScaleImg = new Vector3(0f, 0f, -90f);
            }
            else
            {
                idImg = 3;
                localScaleImg = new Vector3(0f, 0f, -90f);
            }
        }
        else if (box.info.isBottom && box.info.isRight)
        {
            if (isTopLeft)
            {
                idImg = 6;
            }
            else
            {
                idImg = 3;
            }
        }
        else if (box.info.isLeft && box.info.isRight)
        {
            idImg = 2;
        }
        else if (box.info.isTop)
        {
            if (isBottomLeft && isBottomRight)
            {
                idImg = 7;
                localScaleImg = new Vector3(0f, 0f, -90f);
            }
            else if (isBottomLeft)
            {
                idImg = 8;
                localScaleImg = new Vector3(0f, 0f, -90f);
            }
            else if (isBottomRight)
            {
                idImg = 9;
                localScaleImg = new Vector3(0f, 0f, -90f);
            }
            else
            {
                idImg = 1;
                localScaleImg = new Vector3(0f, 0f, -90f);
            }
        }
        else if (box.info.isBottom)
        {
            if (isTopLeft && isTopRight)
            {
                idImg = 7;
                localScaleImg = new Vector3(0f, 0f, 90f);
            }
            else if (isTopLeft)
            {
                idImg = 9;
                localScaleImg = new Vector3(0f, 0f, 90f);
            }
            else if (isTopRight)
            {
                idImg = 8;
                localScaleImg = new Vector3(0f, 0f, 90f);
            }
            else
            {
                idImg = 1;
                localScaleImg = new Vector3(0f, 0f, 90f);
            }
        }
        else if (box.info.isRight)
        {
            if (isTopLeft && isBottomLeft)
            {
                idImg = 7;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
            else if (isBottomLeft)
            {
                idImg = 9;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
            else if (isTopLeft)
            {
                idImg = 8;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
            else
            {
                idImg = 1;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
        }
        else if (box.info.isLeft)
        {
            if (isTopRight && isBottomRight)
            {
                idImg = 7;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
            else if (isBottomRight)
            {
                idImg = 8;
            }
            else if (isTopRight)
            {
                idImg = 9;
            }
            else
            {
                idImg = 1;
            }
        }
        else
        {
            if (isTopRight && isTopLeft)
            {
                idImg = 10;
                localScaleImg = new Vector3(0f, 0f, 90f);
            }
            else if (isTopRight && isBottomRight)
            {
                idImg = 10;
            }
            else if (isBottomLeft && isBottomRight)
            {
                idImg = 10;
                localScaleImg = new Vector3(0f, 0f, -90f);
            }
            else if (isBottomLeft && isTopLeft)
            {
                idImg = 10;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
            else if (isTopRight)
            {
                idImg = 11;
                localScaleImg = new Vector3(0f, 0f, 90f);
            }
            else if (isTopLeft)
            {
                idImg = 11;
                localScaleImg = new Vector3(0f, 0f, 180f);
            }
            else if (isBottomRight)
            {
                idImg = 11;
            }
            else if (isBottomLeft)
            {
                idImg = 11;
                localScaleImg = new Vector3(0f, 0f, -90f);
            }
            else
            {
                idImg = 0;
            }
        }

        box.spGround.sprite = lsSpriteMap[idImg];
        box.spGround.transform.localEulerAngles = localScaleImg;
    }

    public int CheckGoldMinePlayer(Box box)
    {
        int numberCheck = 0;
        if (box.info.col != 8 && !arrBox[box.info.col + 1, box.info.row].info.isLock)
        {
            numberCheck++;
        }
        if (box.info.row != 8 && !arrBox[box.info.col, box.info.row + 1].info.isLock)
        {
            numberCheck++;
        }
        if (box.info.col != 0 && !arrBox[box.info.col - 1, box.info.row].info.isLock)
        {
            numberCheck++;
        }
        if (box.info.row != 0 && !arrBox[box.info.col, box.info.row - 1].info.isLock)
        {
            numberCheck++;
        }
        return numberCheck;
    }

    public void ClearMap()
    {
        for (int i = 0; i < UIManager.Instance.lstAvatarHeroRelease.Length; i++)
        {
            UIManager.Instance.lstAvatarHeroRelease[i].gameObject.SetActive(false);
            UIManager.Instance.lstAvatarHeroRelease[i].sprite = null;
        }
        foreach (GoldMine g in lsGoldMineManager)
        {
            g.ClearAllListener();
        }
        foreach (Box b in lsBoxManager)
        {
            Destroy(b.gameObject);
        }
        for (int i = 0; i < enemyManager.childCount; i++)
        {
            Destroy(enemyManager.GetChild(i).gameObject);
        }
        foreach (Hero h in lsEnemyAttackGoldMine)
        {
            Destroy(h.gameObject);
        }
        castlePlayer.lsHouseRelease.Clear();
        lsBoxManager.Clear();
        lsBoxCanMove.Clear();
        lsBoxMove.Clear();
        lsBuildHouse.Clear();
        lsGoldMineManager.Clear();
        lsGoldMinePlayer.Clear();
        lsGoldMineEnemy.Clear();
        lsEnemyAttackGoldMine.Clear();
        foreach (House h in lsHousePlayer)
        {
            h.info.typeState = TypeStateHouse.Lock;
        }
    }
    #endregion

    #region === HOUSE ===
    public void BuildHouse()
    {
        for (int i = 0; i < UIManager.Instance.lsBuildHouseUI.Count; i++)
        {
            BuildHouse bh = new BuildHouse();
            bh.ID = i;
            //if (i >= 5)
            //    bh.isUnlock = false;
            //else
            //    bh.isUnlock = true;
            //if (i == 8)
            bh.isUnlock = true;
            lsBuildHouse.Add(bh);
        }
        if (lsGoldMinePlayer.Count >= 2)
        {
            dateEnemyAttack = dateGame + (long)(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
        }
        dateUpGoldMine = dateGame + (long)(GameConfig.Instance.TimeUp);
    }

    public void BuildHouseJson()
    {
        for (int i = 0; i < UIManager.Instance.lsBuildHouseUI.Count; i++)
        {
            BuildHouse bh = new BuildHouse();
            bh.ID = DataPlayer.Instance.lstBuildHouse[i].ID;
            bh.isUnlock = DataPlayer.Instance.lstBuildHouse[i].isUnlock;
            lsBuildHouse.Add(bh);
        }
        dateGame = DataPlayer.Instance.dateGame;
        SetDateUI();
        if (lsGoldMinePlayer.Count >= 2)
        {
            dateEnemyAttack = DataPlayer.Instance.dateEnemyAttack;
        }
        dateUpGoldMine = dateGame + (long)(GameConfig.Instance.TimeUp);
    }
    #endregion

    #region === ADD GOLD, COIN ===

    public void GetGold(long _gold)
    {
        gold = _gold;
        UIManager.Instance.txtGold.text = UIManager.Instance.ConvertNumber(gold);
    }

    public void AddGold(long _gold)
    {
        gold += _gold;
        if (gold < 0)
        {
            gold = 0;
        }
        UIManager.Instance.txtGold.text = UIManager.Instance.ConvertNumber(gold);
    }

    public void GetCoin(int _coin)
    {
        coin = _coin;
        UIManager.Instance.txtCoin.text = UIManager.Instance.ConvertNumber(coin);
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
            dateGame = 0;
        }
        else
        {
            dateGame = 0;
        }
        SetDateUI();
    }

    public void SetDateUI()
    {
        txtDate.text = "Day: " + dateGame;
    }
    #endregion
}
