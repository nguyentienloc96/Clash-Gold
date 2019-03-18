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
    public float ratioBorn;
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
    public float weight;
    public Transform boxManager;
    private Box[,] arrBox = new Box[9, 9];
    public List<Box> lsBoxMove = new List<Box>();
    public List<Box> lsBoxCanMove = new List<Box>();
    public List<Sprite> lsSpriteMap = new List<Sprite>();

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
    public GoldMine goldMineInSide;
    public bool isBeingAttack;

    public bool isBreak;

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

            if (isBeingAttack && dateGame >= dateEnemyAttack && !isAttack)
            {
                if (lsEnemyAttackGoldMine.Count <= 0)
                {
                    int a = UnityEngine.Random.Range(0, lstGoldMineEnemy.Count);
                    lstGoldMineEnemy[a].AttackPlayer();
                    dateEnemyAttack = dateGame.AddDays(GameConfig.Instance.TimeDestroy / GameConfig.Instance.Timeday);
                }
            }

            if (dateGame >= dateUpGoldMine && lstGoldMineEnemy.Count > 0)
            {
                int landUpMax = (lstGoldMineEnemy.Count / GameConfig.Instance.LandDiv);
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

            if (lstGoldMinePlayer.Count <= 0)
            {
                UIManager.Instance.panelGameOver.SetActive(true);
            }

            if (isAttack)
            {
                if (isAttackGoldMineEnemy)
                {
                    if (itemSelectHero != null)
                    {
                        if (Input.GetMouseButtonUp(0) && !UIManager.Instance.panelLetGo.activeSelf)
                        {
                            Vector3 posIns = DeadzoneCamera.Instance.cameraAttack.ScreenToWorldPoint(Input.mousePosition);
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
                                Vector3 posIns = DeadzoneCamera.Instance.cameraAttack.ScreenToWorldPoint(Input.mousePosition);
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
                        if (goldMineInSide != null && GolEnemyBeingAttack != null && goldMineInSide.id == GolEnemyBeingAttack.id)
                        {
                            if (lstGoldMinePlayer.Count > 0)
                            {
                                int idGoldMineCheck = 0;
                                float disGoldPlayer = Vector3.Distance(lstGoldMinePlayer[0].transform.position, GolEnemyBeingAttack.transform.position);
                                for (int i = 1; i < lstGoldMinePlayer.Count; i++)
                                {
                                    if (disGoldPlayer > Vector3.Distance(lstGoldMinePlayer[i].transform.position, GolEnemyBeingAttack.transform.position))
                                    {
                                        idGoldMineCheck = i;
                                    }
                                }

                                castlePlayer.transform.localPosition = lstGoldMinePlayer[idGoldMineCheck].transform.position;
                                castlePlayer.posMove = lstGoldMinePlayer[idGoldMineCheck].transform.position;
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
                        if (isBreak)
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
                            isBreak = false;
                        }
                        else
                        {
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
        UIManager.Instance.cavas.worldCamera = DeadzoneCamera.Instance._camera;
        UIManager.Instance.canvasLoading.worldCamera = DeadzoneCamera.Instance._camera;
        UIManager.Instance.mapAttack.SetActive(false);
        UIManager.Instance.panelThrowHeroAttack.SetActive(false);
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

    #region === MAP ===
    public void GenerateMapBox()
    {
        int idPosGold = UnityEngine.Random.Range(0, GameConfig.Instance.listMap.Count);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Box b = Instantiate(prefabsBox, boxManager.position + new Vector3(j * (weight), -i * (weight)), Quaternion.identity, boxManager).GetComponent<Box>();
                b.col = j;
                b.row = i;
                arrBox[j, i] = b;
                if (!CheckPos(i, j, idPosGold))
                {
                    b.gameObject.layer = 13;
                    b.isLock = true;
                    lsBoxCanMove.Add(b);
                }
                else
                {
                    b.transform.GetChild(0).gameObject.SetActive(false);
                    b.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    lsBoxMove.Add(b);
                }
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
        Debug.Log(1);
        yield return new WaitForEndOfFrame();
        Debug.Log(2);
        List<Box> lsBox = new List<Box>();
        foreach (Box box in lsBoxMove)
        {
            if (CheckGoldMinePlayer(box) >= 3)
            {
                lsBox.Add(box);
            }
        }
        Debug.Log(3);
        yield return new WaitUntil(() => lsBox.Count > 0);
        Debug.Log(4);
        int numberId = 0;
        int idGoldMinePlayer = UnityEngine.Random.Range(0, lsBox.Count);
        Box bPlayer = lsBox[idGoldMinePlayer];
        Debug.Log(5);
        yield return new WaitUntil(() => bPlayer != null);
        Debug.Log(6);
        Box bNearPlayer = null;
        if (bPlayer.col != 8 && !arrBox[bPlayer.col + 1, bPlayer.row].isLock)
        {
            bNearPlayer = arrBox[bPlayer.col + 1, bPlayer.row];
        }
        else if (bPlayer.row != 8 && !arrBox[bPlayer.col, bPlayer.row + 1].isLock)
        {
            bNearPlayer = arrBox[bPlayer.col, bPlayer.row + 1];
        }
        else if (bPlayer.col != 0 && !arrBox[bPlayer.col - 1, bPlayer.row].isLock)
        {
            bNearPlayer = arrBox[bPlayer.col - 1, bPlayer.row];
        }
        else if (bPlayer.row != 0 && !arrBox[bPlayer.col, bPlayer.row - 1].isLock)
        {
            bNearPlayer = arrBox[bPlayer.col, bPlayer.row - 1];
        }
        Debug.Log(7);
        yield return new WaitUntil(() => bNearPlayer != null);
        Debug.Log(8);
        GenerateMap(bPlayer.transform, numberId, 1, true);
        numberId++;
        Vector3 posIns = bPlayer.transform.position;
        posIns.z = -2;
        castlePlayer.transform.position = posIns;
        posIns.z = -10;
        DeadzoneCamera.Instance._camera.transform.position = posIns;
        Debug.Log(9);
        yield return new WaitForEndOfFrame();
        Debug.Log(10);
        foreach (Box b in lsBoxMove)
        {
            if (b != bPlayer)
            {
                if (b != bNearPlayer)
                {
                    GenerateMap(b.transform, numberId, UnityEngine.Random.Range(1, 6));
                }
                else
                {
                    GenerateMap(b.transform, numberId, 1);
                }
                numberId++;
            }
        }
        Debug.Log(11);
        yield return new WaitForSeconds(1f);
        yield return new WaitForEndOfFrame();
        Debug.Log(12);
        Fade.Instance.EndFade();
    }

    public bool CheckPos(int row, int col, int id)
    {
        bool isCheck = false;
        foreach (Vector2 v2 in GameConfig.Instance.listMap[id])
        {
            if (new Vector2(v2.x + 1, v2.y + 1) == new Vector2(col, row))
            {
                isCheck = true;
            }
        }
        return isCheck;
    }

    public void GenerateMap(Transform toPos, int id, int level, bool isGoldPlayer = false)
    {

        int a = (int)UnityEngine.Random.Range(0, 2.9f);
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
            GoldMine g = Instantiate(prefabsBoxMap[2], toPos.position, Quaternion.Euler(_rotation), toPos).GetComponent<GoldMine>();
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
                        lineEnemyAttack.material.mainTextureScale = new Vector3(distance, 1, 1);
                    }
                }
            }
        }

        foreach (Box b in lsBoxMove)
        {
            b.isTop = false;
            b.isBottom = false;
            b.isLeft = false;
            b.isRight = false;
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

    public void CheckBoxCanMove(Box box)
    {
        if (box.col != 8 && !arrBox[box.col + 1, box.row].isLock)
        {
            box.isRight = true;
        }
        if (box.row != 8 && !arrBox[box.col, box.row + 1].isLock)
        {
            box.isBottom = true;
        }
        if (box.col != 0 && !arrBox[box.col - 1, box.row].isLock)
        {
            box.isLeft = true;
        }
        if (box.row != 0 && !arrBox[box.col, box.row - 1].isLock)
        {
            box.isTop = true;
        }
        if (box.col != 8 && box.row != 8 && !arrBox[box.col + 1, box.row + 1].isLock)
        {
            box.isBottomRight = true;
        }
        if (box.col != 0 && box.row != 8 && !arrBox[box.col - 1, box.row + 1].isLock)
        {
            box.isBottomLeft = true;
        }
        if (box.col != 0 && box.row != 0 && !arrBox[box.col - 1, box.row - 1].isLock)
        {
            box.isTopLeft = true;
        }
        if (box.col != 8 && box.row != 0 && !arrBox[box.col + 1, box.row - 1].isLock)
        {
            box.isTopRight = true;
        }



        if (box.isTop && box.isBottom && box.isRight && box.isLeft)
        {
            box.spGround.sprite = lsSpriteMap[5];
        }
        else if (box.isBottom && box.isRight && box.isLeft)
        {
            box.spGround.sprite = lsSpriteMap[4];
            box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
        }
        else if (box.isTop && box.isRight && box.isLeft)
        {
            box.spGround.sprite = lsSpriteMap[4];
            box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        }
        else if (box.isTop && box.isBottom && box.isRight)
        {
            box.spGround.sprite = lsSpriteMap[4];
        }
        else if (box.isTop && box.isBottom && box.isLeft)
        {
            box.spGround.sprite = lsSpriteMap[4];
            box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
        }
        else if (box.isTop && box.isBottom)
        {
            box.spGround.sprite = lsSpriteMap[2];
            box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        }
        else if (box.isTop && box.isLeft)
        {
            if (box.isBottomRight)
            {
                box.spGround.sprite = lsSpriteMap[6];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[3];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
        }
        else if (box.isTop && box.isRight)
        {
            if (box.isBottomLeft)
            {
                box.spGround.sprite = lsSpriteMap[6];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[3];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
        }
        else if (box.isBottom && box.isLeft)
        {
            if (box.isTopRight)
            {
                box.spGround.sprite = lsSpriteMap[6];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[3];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
        }
        else if (box.isBottom && box.isRight)
        {
            if (box.isTopLeft)
            {
                box.spGround.sprite = lsSpriteMap[6];
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[3];
            }
        }
        else if (box.isLeft && box.isRight)
        {
            box.spGround.sprite = lsSpriteMap[2];
        }
        else if (box.isTop)
        {
            if (box.isBottomLeft && box.isBottomRight)
            {
                box.spGround.sprite = lsSpriteMap[7];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
            else if (box.isBottomLeft)
            {
                box.spGround.sprite = lsSpriteMap[8];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
            else if (box.isBottomRight)
            {
                box.spGround.sprite = lsSpriteMap[9];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[1];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
        }
        else if (box.isBottom)
        {
            if (box.isTopLeft && box.isTopRight)
            {
                box.spGround.sprite = lsSpriteMap[7];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (box.isTopLeft)
            {
                box.spGround.sprite = lsSpriteMap[9];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (box.isTopRight)
            {
                box.spGround.sprite = lsSpriteMap[8];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[1];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
        }
        else if (box.isRight)
        {
            if (box.isTopLeft && box.isBottomLeft)
            {
                box.spGround.sprite = lsSpriteMap[7];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (box.isBottomLeft)
            {
                box.spGround.sprite = lsSpriteMap[9];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (box.isTopLeft)
            {
                box.spGround.sprite = lsSpriteMap[8];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[1];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
        }
        else if (box.isLeft)
        {
            if (box.isTopRight && box.isBottomRight)
            {
                box.spGround.sprite = lsSpriteMap[7];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (box.isBottomRight)
            {
                box.spGround.sprite = lsSpriteMap[8];
            }
            else if (box.isTopRight)
            {
                box.spGround.sprite = lsSpriteMap[9];
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[1];
            }
        }
        else
        {
            if (box.isTopRight && box.isTopLeft)
            {
                box.spGround.sprite = lsSpriteMap[10];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (box.isTopRight && box.isBottomRight)
            {
                box.spGround.sprite = lsSpriteMap[10];
            }
            else if (box.isBottomLeft && box.isBottomRight)
            {
                box.spGround.sprite = lsSpriteMap[10];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
            else if (box.isBottomLeft && box.isTopLeft)
            {
                box.spGround.sprite = lsSpriteMap[10];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (box.isTopRight)
            {
                box.spGround.sprite = lsSpriteMap[11];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (box.isTopLeft)
            {
                box.spGround.sprite = lsSpriteMap[11];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (box.isBottomRight)
            {
                box.spGround.sprite = lsSpriteMap[11];
            }
            else if (box.isBottomLeft)
            {
                box.spGround.sprite = lsSpriteMap[11];
                box.spGround.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
            else
            {
                box.spGround.sprite = lsSpriteMap[0];
            }
        }
    }

    public int CheckGoldMinePlayer(Box box)
    {
        int numberCheck = 0;
        if (box.col != 8 && !arrBox[box.col + 1, box.row].isLock)
        {
            numberCheck++;
        }
        if (box.row != 8 && !arrBox[box.col, box.row + 1].isLock)
        {
            numberCheck++;
        }
        if (box.col != 0 && !arrBox[box.col - 1, box.row].isLock)
        {
            numberCheck++;
        }
        if (box.row != 0 && !arrBox[box.col, box.row - 1].isLock)
        {
            numberCheck++;
        }
        return numberCheck;
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
            hero.house = houseHero;
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
                hero.house = houseHero;
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
                hero.house = houseHero;
                lsHero.Add(hero);
            }
        }


        for (int i = 0; i < lsEnemy.Count; i++)
        {
            lsEnemy[i].targetCompetitor = null;
        }
    }
}
