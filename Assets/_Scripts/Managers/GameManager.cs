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
    public List<GoldMine> lstGoldMineEnemy;

    public List<Hero> lsHero;
    public List<Hero> lsEnemy;
    public List<Hero> lsChild;

    public List<Hero> lsPrefabsHero;
    public List<Hero> lsPrefabsEnemy;

    public int[] lsHeroFly;
    public DateTime dateUpGoldMine;


    [Header("OTHER")]
    public Transform enemyManager;

    [Header("MAP")]
    public GameObject[] prefabsBoxMap;
    public Sprite[] sprBoxMap;
    public GameObject prefabsBox;
    public int row;
    public int col;
    public int weight;
    public Transform boxManager;
    private Box[,] arrBox = new Box[9, 9];
    private Vector2[] PosGolds = new Vector2[] { new Vector2(3, 3), new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 0), new Vector2(1, 2), new Vector2(2, 2), new Vector2(2, 3), new Vector2(2, 4), new Vector2(2, 5), new Vector2(2, 6), new Vector2(3, 6), new Vector2(4, 1), new Vector2(4, 2), new Vector2(4, 3), new Vector2(4, 4), new Vector2(4, 5), new Vector2(4, 6), new Vector2(5, 3), new Vector2(5, 4) };


    [HideInInspector]
    public bool isAttack;
    [HideInInspector]
    public bool isAttackGoldMineEnemy;

    public List<Transform> lsPosHero;
    public List<Transform> lsPosEnemy;
    [HideInInspector]
    public GoldMine GolEnemyBeingAttack;
    [HideInInspector]
    public GoldMine GolHeroBeingAttack;
    [HideInInspector]
    public GoldMine GolEnemyIsAttack;
    [HideInInspector]
    public List<Hero> lsEnemyAttackGoldMine = new List<Hero>();
    [HideInInspector]
    public GoldMine goldMineCurrent;
    [HideInInspector]
    public Vector3 posTriggerGoldMine;

    public LineRenderer lineEnemyAttack;

    public ItemHeroAttack itemSelectHero;
    public int numberThrowHero;
    public bool isInSide;
    public bool isBeingAttack;

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
        GenerateMapBox();
        for (int i = 0; i < UIManager.Instance.lstHouse.Count; i++)
        {
            BuildHouse bh = new BuildHouse();
            bh.ID = i;
            //if (i >= 5)
            //    bh.isUnlock = false;
            //else
            //    bh.isUnlock = true;
            //if (i == 8)
            bh.isUnlock = true;
            lstBuildHouse.Add(bh);
        }

        if (lstGoldMinePlayer.Count >= 2)
        {
            dateEnemyAttack = dateGame.AddDays(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
        }
        dateUpGoldMine = dateGame.AddDays(GameConfig.Instance.TimeUp);
    }

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

            if (UIManager.Instance.mapMove.activeSelf && isBeingAttack && dateGame >= dateEnemyAttack && !isAttack)
            {
                if (lsEnemyAttackGoldMine.Count <= 0)
                {
                    int a = UnityEngine.Random.Range(0, lstGoldMineEnemy.Count);
                    lstGoldMineEnemy[a].AttackPlayer();
                    dateEnemyAttack = dateGame.AddDays(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
                }
            }

            if (dateGame >= dateUpGoldMine)
            {
                int landUpMax = (lstGoldMineEnemy.Count / GameConfig.Instance.LandDiv);
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
                    for (int i = 0; i < lstGoldMineEnemy.Count; i++)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
                        {
                            lstGoldMineEnemy[i].AddLevel();
                            numberGoldMine++;
                        }
                    }
                }
                dateUpGoldMine = dateGame.AddDays(GameConfig.Instance.TimeUp);
            }

            if (lstGoldMineEnemy.Count <= 0)
            {
                UIManager.Instance.panelVictory.SetActive(true);
            }

            if (isAttack)
            {
                if (isAttackGoldMineEnemy)
                {
                    if (itemSelectHero != null)
                    {
                        if (Input.GetMouseButtonUp(0))
                        {
                            Vector3 posIns = DeadzoneCamera.Instance.cameraAttack.ScreenToWorldPoint(Input.mousePosition);
                            posIns.z = 0f;
                            if (posIns.y > 0)
                            {
                                posIns.y = 0;
                            }
                            ThrowHero(itemSelectHero.houseHero, itemSelectHero.countHero, posIns);
                            itemSelectHero.gameObject.SetActive(false);
                            itemSelectHero = null;
                            numberThrowHero--;
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonUp(0))
                        {
                            if (UIManager.Instance.lsItemHeroAttack[0].gameObject.activeSelf || UIManager.Instance.lsItemHeroAttack[1].gameObject.activeSelf || UIManager.Instance.lsItemHeroAttack[2].gameObject.activeSelf)
                            {
                                Vector3 posIns = DeadzoneCamera.Instance.cameraAttack.ScreenToWorldPoint(Input.mousePosition);
                                posIns.z = 0f;
                                if (posIns.y > 0)
                                {
                                    posIns.y = 0;
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
                        dateEnemyAttack = dateGame.AddDays(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
                        ScenesManager.Instance.GoToScene(() =>
                         {
                             GolEnemyBeingAttack.typeGoleMine = TypeGoldMine.Player;
                             GolEnemyBeingAttack.SetSpriteBox(0);
                             lstGoldMineEnemy.Remove(GolEnemyBeingAttack);
                             lstGoldMinePlayer.Add(GolEnemyBeingAttack);
                             if (lsEnemyAttackGoldMine.Count > 0)
                             {
                                 for (int i = 0; i < lsEnemyAttackGoldMine.Count; i++)
                                 {
                                     lsEnemyAttackGoldMine[i].isPause = false;
                                 }
                             }
                         });
                    }
                    else if (lsHero.Count <= 0 && numberThrowHero <= 0)
                    {
                        EndAttack();
                        Vector3 posGoldMine = Vector3.zero;
                        posGoldMine = GolEnemyBeingAttack.transform.position;
                        Vector3 posMoveEnd = posTriggerGoldMine - (posGoldMine - posTriggerGoldMine).normalized;
                        isInSide = false;
                        castlePlayer.transform.localPosition = posMoveEnd;
                        castlePlayer.posMove = posMoveEnd;
                        ScenesManager.Instance.GoToScene(() =>
                         {
                             if (lsEnemyAttackGoldMine.Count > 0)
                             {
                                 for (int i = 0; i < lsEnemyAttackGoldMine.Count; i++)
                                 {
                                     lsEnemyAttackGoldMine[i].isPause = false;
                                 }
                             }
                         });
                    }
                }
                else
                {
                    if (lsHero.Count <= 0)
                    {
                        EndAttack();
                        GolHeroBeingAttack.DeleteHero();
                        ScenesManager.Instance.GoToScene(() =>
                         {
                             GolHeroBeingAttack.typeGoleMine = TypeGoldMine.Enemy;
                             GolHeroBeingAttack.SetSpriteBox(maxLevelHouse);
                             lstGoldMinePlayer.Remove(GolHeroBeingAttack);
                             lstGoldMineEnemy.Add(GolHeroBeingAttack);
                             for (int i = 0; i < lsEnemyAttackGoldMine.Count; i++)
                             {
                                 GolHeroBeingAttack.InstantiateEnemy(lsEnemyAttackGoldMine[i].infoHero.ID - 1
                                     , lsEnemyAttackGoldMine[i].infoHero.numberHero, i);
                             }
                             foreach (Hero h in lsEnemyAttackGoldMine)
                             {
                                 Destroy(h.gameObject);
                             }
                             lsEnemyAttackGoldMine.Clear();
                         });
                    }
                    else if (lsEnemy.Count <= 0)
                    {
                        EndAttack();
                        ScenesManager.Instance.GoToScene(() =>
                         {
                             foreach (Hero h in lsEnemyAttackGoldMine)
                             {
                                 Destroy(h.gameObject);
                             }
                             lsEnemyAttackGoldMine.Clear();
                         });
                    }
                }
            }
            else
            {
                if (lsEnemyAttackGoldMine.Count > 0)
                {
                    lineEnemyAttack.enabled = true;
                }
                else
                {
                    lineEnemyAttack.enabled = false;
                }
            }
        }
    }

    public void EndAttack()
    {
        isAttack = false;
        DeadzoneCamera.Instance.cameraAttack.gameObject.SetActive(false);
        UIManager.Instance.cavas.worldCamera = DeadzoneCamera.Instance._camera;
        UIManager.Instance.canvasLoading.worldCamera = DeadzoneCamera.Instance._camera;
        UIManager.Instance.mapAttack.SetActive(false);
        UIManager.Instance.mapMove.SetActive(true);
        UIManager.Instance.panelThrowHeroAttack.SetActive(false);
        OnEndAttack();
        Vector3 posCastle = castlePlayer.transform.position;
        posCastle.z = -10f;
        DeadzoneCamera.Instance._camera.transform.position = posCastle;
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

    #region === MAP ===
    public void GenerateMapBox()
    {
        int numberID = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Box b = Instantiate(prefabsBox, boxManager.position + new Vector3(j * weight, -i * weight), Quaternion.identity, boxManager).GetComponent<Box>();
                b.col = j;
                b.row = i;
                arrBox[j, i] = b;
                if (!CheckPos(i, j))
                {
                    b.gameObject.layer = 13;
                    b.transform.GetChild(0).gameObject.SetActive(false);
                    b.isLock = true;
                }
                else
                {
                    b.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    if (i == 4 && j == 4)
                    {
                        GenerateMap(b.transform, numberID, 1, true);
                    }
                    else if (i == 3 && j == 4)
                    {
                        GenerateMap(b.transform, numberID, 1);
                    }
                    else
                    {
                        GenerateMap(b.transform, numberID, UnityEngine.Random.Range(1, 6));
                    }
                    numberID++;
                }
            }
        }
    }

    public bool CheckPos(int row, int col)
    {
        bool isCheck = false;
        foreach (Vector2 v2 in PosGolds)
        {
            if (v2 == new Vector2(col, row))
            {
                isCheck = true;
            }
        }
        return isCheck;
    }

    public void GenerateMap(Transform toPos, int id, int level, bool isGoldPlayer = false)
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
        if (isGoldPlayer)
        {
            _rotation = new Vector3(0, 0, 0);
            GoldMine g = Instantiate(prefabsBoxMap[3], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
            g.SetLevel(level);
            g.id = id;
            g.SetInfo(GameConfig.Instance.CapGold0, GameConfig.Instance.PriceGoldUp, level);
            g.numberBoxGoldMine = 3;
            g.typeGoleMine = TypeGoldMine.Player;
            g.InstantiateHeroStart(true);
            lstGoldMinePlayer.Add(g);
        }
        else
        {
            GoldMine g = Instantiate(prefabsBoxMap[a], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
            g.SetLevel(level);
            g.id = id;
            g.numberBoxGoldMine = a;
            g.SetInfo(GameConfig.Instance.CapGold0, GameConfig.Instance.PriceGoldUp, level);
            g.typeGoleMine = TypeGoldMine.Enemy;
            g.InstantiateHeroStart(false);
            g.Canvas.GetComponent<RectTransform>().localRotation = Quaternion.Euler(_rotation);
            lstGoldMineEnemy.Add(g);
        }
    }

    public List<Box> PathFinding(Box boxStart, Box boxEnd)
    {
        List<Box> lsPathFinding = new List<Box>();
        Box boxNext = boxStart;
        lsPathFinding.Add(boxStart);
        while (boxNext != boxEnd)
        {
            Debug.Log(boxNext);
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
                    lineEnemyAttack.SetPosition(lsPathFinding.Count - 1 - i, lsPathFinding[i].transform.position);
                }
            }
        }
        return lsPathFinding;
    }

    public Box CheckBoxNext(Box box, Box boxEnd)
    {
        List<Box> lsBoxSelect = new List<Box>();
        List<int> lsPosBox = new List<int>();
        if (box.col != 8 && !box.isTop && !arrBox[box.col + 1, box.row].isLock)
        {
            lsBoxSelect.Add(arrBox[box.col + 1, box.row]);
            lsPosBox.Add(1);
        }
        if (box.row != 8 && !box.isRight && !arrBox[box.col, box.row + 1].isLock)
        {
            lsBoxSelect.Add(arrBox[box.col, box.row + 1]);
            lsPosBox.Add(4);
        }
        if (box.col != 0 && !box.isBottom && !arrBox[box.col - 1, box.row].isLock)
        {
            lsBoxSelect.Add(arrBox[box.col - 1, box.row]);
            lsPosBox.Add(2);
        }
        if (box.row != 0 && !box.isLeft && !arrBox[box.col, box.row - 1].isLock)
        {
            lsBoxSelect.Add(arrBox[box.col, box.row - 1]);
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
                box.isTop = true;
                lsBoxSelect[check].isBottom = true;
            }
            else if (lsPosBox[check] == 2)
            {
                box.isBottom = true;
                lsBoxSelect[check].isTop = true;
            }
            else if (lsPosBox[check] == 3)
            {
                box.isLeft = true;
                lsBoxSelect[check].isRight = true;
            }
            else if (lsPosBox[check] == 4)
            {
                box.isRight = true;
                lsBoxSelect[check].isLeft = true;
            }
            return lsBoxSelect[check];
        }
        return null;
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

    public void ResetGame()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(0);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public void ThrowHero(House houseHero, int countHero, Vector3 posIns)
    {
        if (houseHero.idHero != 2 && houseHero.idHero != 1)
        {
            Hero hero = Instantiate(lsPrefabsHero[houseHero.idHero - 1], posIns, Quaternion.identity);
            hero.gameObject.name = "Hero";
            hero.SetInfoHero();
            hero.infoHero.capWar = 0;
            hero.countHeroStart = countHero;
            hero.AddHero(countHero);
            hero.isAttack = true;
            lsHero.Add(hero);
        }
        else if (houseHero.idHero == 2)
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
                lsHero.Add(hero);
            }
        }
        else if (houseHero.idHero == 1)
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
                lsHero.Add(hero);
            }
        }


        for (int i = 0; i < lsEnemy.Count; i++)
        {
            lsEnemy[i].targetCompetitor = null;
        }
    }
}
