using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public struct BuildHouse
{
    public int ID;
    public bool isUnlock;
}

[System.Serializable]
public struct HeroInfoST
{
    public int ID;
    public int CountHero;
}

[System.Serializable]
public struct GoldMineInfoST
{
    public int ID;
    public string name;
    public int level;
    public bool isPlayer;
    public int indexLoadGoldMine;
    public float xR;
    public float yR;
    public float zR;
    public List<HeroInfoST> lsHeroGoldMine;
}

[System.Serializable]
public struct BoxInfoST
{
    public int col;
    public int row;

    public GoldMineInfoST goldMineInfo;
}

[System.Serializable]
public struct HouseInfoST
{
    public int ID;
    public int level;
    public long price;
    public int capWar;
    public bool isLock;
    public HeroInfoST heroInfo;
}

[System.Serializable]
public struct CastleInfoST
{
    public List<HouseInfoST> lsHouse;
}

public class DataPlayer : MonoBehaviour
{
    public static DataPlayer Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public int idMapBox;
    public int maxLevelHouse;
    public long gold;
    public int coin;
    public float ratioBorn;
    public long dateGame;
    public long dateEnemyAttack;
    public CastleInfoST castlePlayer;
    public List<BoxInfoST> lsBox = new List<BoxInfoST>();
    public List<BuildHouse> lstBuildHouse = new List<BuildHouse>();
    public List<HouseInfoST> lsHousePlayer = new List<HouseInfoST>();
    public void Start()
    {
        if (!PlayerPrefs.HasKey(KeyPrefs.IS_CONTINUE))
        {
            Fade.Instance.EndFade(0);
            PlayerPrefs.SetInt(KeyPrefs.IS_CONTINUE, 0);
        }
        else
        {
            if(PlayerPrefs.GetInt(KeyPrefs.IS_CONTINUE) != 0)
            {
                LoadDataPlayer();
                UIManager.Instance.btnContinue.interactable = true;
            }
            else
            {
                Fade.Instance.EndFade(0);
                UIManager.Instance.btnContinue.interactable = false;
            }
        }
    }

    public void SaveDataPlayer()
    {
        DataPlayer data = new DataPlayer();
        data.idMapBox = GameManager.Instance.idMapBox;
        data.maxLevelHouse = GameManager.Instance.maxLevelHouse;
        data.gold = GameManager.Instance.gold;
        data.coin = GameManager.Instance.coin;
        data.ratioBorn = GameManager.Instance.ratioBorn;
        data.dateGame = GameManager.Instance.dateGame;
        data.dateEnemyAttack = GameManager.Instance.dateEnemyAttack;
        data.lstBuildHouse = GameManager.Instance.lsBuildHouse;

        data.castlePlayer = new CastleInfoST();
        data.castlePlayer.lsHouse = new List<HouseInfoST>();
        foreach (House h in GameManager.Instance.castlePlayer.lsHouseRelease)
        {
            HouseInfoST hInfo = new HouseInfoST();
            hInfo.ID = h.info.ID;
            hInfo.level = h.info.level;
            hInfo.heroInfo.ID = h.info.idHero;
            hInfo.heroInfo.CountHero = h.info.countHero;
            data.castlePlayer.lsHouse.Add(hInfo);
        }

        foreach (Box b in GameManager.Instance.lsBoxManager)
        {
            BoxInfoST bInfo = new BoxInfoST();
            bInfo.col = b.info.col;
            bInfo.row = b.info.row;
            if (b.info.goldMine != null)
            {
                bInfo.goldMineInfo = new GoldMineInfoST();
                bInfo.goldMineInfo.ID = b.info.goldMine.info.ID;
                bInfo.goldMineInfo.level = b.info.goldMine.info.level;
                bInfo.goldMineInfo.name = b.info.goldMine.info.name;
                bInfo.goldMineInfo.isPlayer = b.info.goldMine.info.typeGoleMine == TypeGoldMine.Player ? true : false;
                bInfo.goldMineInfo.indexLoadGoldMine = b.info.goldMine.info.indexLoadGoldMine;
                bInfo.goldMineInfo.xR = b.info.goldMine.transform.eulerAngles.x;
                bInfo.goldMineInfo.yR = b.info.goldMine.transform.eulerAngles.y;
                bInfo.goldMineInfo.zR = b.info.goldMine.transform.eulerAngles.z;
                bInfo.goldMineInfo.lsHeroGoldMine = new List<HeroInfoST>();
                foreach (Hero h in b.info.goldMine.lsHeroGoldMine)
                {
                    HeroInfoST hInfo = new HeroInfoST();
                    hInfo.ID = h.infoHero.ID;
                    hInfo.CountHero = h.infoHero.countHero;
                    bInfo.goldMineInfo.lsHeroGoldMine.Add(hInfo);
                }
            }
            else
            {
                bInfo.goldMineInfo = new GoldMineInfoST();
                bInfo.goldMineInfo.ID = -1;
            }
            data.lsBox.Add(bInfo);
        }

        foreach (House h in GameManager.Instance.lsHousePlayer)
        {
            HouseInfoST hInfo = new HouseInfoST();
            hInfo.ID = h.info.ID;
            hInfo.level = h.info.level;
            hInfo.price = h.info.price;
            hInfo.capWar = h.info.capWar;
            hInfo.isLock = h.info.typeState == TypeStateHouse.Lock ? true : false;
            hInfo.heroInfo.ID = h.info.idHero;
            hInfo.heroInfo.CountHero = h.info.countHero;
            data.lsHousePlayer.Add(hInfo);
        }

        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        File.WriteAllText(_path, JsonUtility.ToJson(data, true));
        File.ReadAllText(_path);
        PlayerPrefs.SetInt(KeyPrefs.IS_CONTINUE, 1);
        Debug.Log(SimpleJSON_DatDz.JSON.Parse(File.ReadAllText(_path)));

        //string path = "Assets/Resources/DebugJson.json";
        //StreamWriter wfile = new StreamWriter(path);
        //wfile.WriteLine(File.ReadAllText(_path));
        //wfile.Close();
    }

    public void LoadDataPlayer()
    {
        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        string dataAsJson = File.ReadAllText(_path);
        //string dataAsJson = File.ReadAllText("Assets/Resources/DebugJson.json");
        var objJson = SimpleJSON_DatDz.JSON.Parse(dataAsJson);
        Debug.Log(objJson);
        if (objJson != null)
        {
            StartCoroutine(IE_LoadDataPlayer(objJson));
        }
    }

    public IEnumerator IE_LoadDataPlayer(SimpleJSON_DatDz.JSONNode objJson)
    {
        idMapBox = objJson["idMapBox"].AsInt;
        maxLevelHouse = objJson["maxLevelHouse"].AsInt;
        gold = objJson["gold"].AsInt;
        coin = objJson["coin"].AsInt;
        ratioBorn = objJson["ratioBorn"].AsFloat;
        dateGame = objJson["dateGame"].AsLong;
        dateEnemyAttack = objJson["dateEnemyAttack"].AsLong;

        var lstBuildHouseJson = objJson["lstBuildHouse"];
        for (int i = 0; i < lstBuildHouseJson.Count; i++)
        {
            BuildHouse buildHouse = new BuildHouse();
            buildHouse.ID = lstBuildHouseJson[i]["ID"].AsInt;
            buildHouse.isUnlock = lstBuildHouseJson[i]["isUnlock"].AsBool;
            lstBuildHouse.Add(buildHouse);
        }

        var lstHousePlayerJson = objJson["lsHousePlayer"];
        if (lstHousePlayerJson.Count > 0)
        {
            for (int i = 0; i < lstHousePlayerJson.Count; i++)
            {
                HouseInfoST hInfo = new HouseInfoST();
                hInfo.ID = lstHousePlayerJson[i]["ID"].AsInt;
                hInfo.level = lstHousePlayerJson[i]["level"].AsInt;
                hInfo.price = lstHousePlayerJson[i]["price"].AsLong;
                hInfo.capWar = lstHousePlayerJson[i]["capWar"].AsInt;
                hInfo.isLock = lstHousePlayerJson[i]["isLock"].AsBool;
                hInfo.heroInfo = new HeroInfoST();
                hInfo.heroInfo.ID = lstHousePlayerJson[i]["heroInfo"]["ID"].AsInt;
                hInfo.heroInfo.CountHero = lstHousePlayerJson[i]["heroInfo"]["CountHero"].AsInt;
                lsHousePlayer.Add(hInfo);
            }
        }

        var lsBoxJson = objJson["lsBox"].AsArray;
        for (int i = 0; i < lsBoxJson.Count; i++)
        {
            BoxInfoST bInfo = new BoxInfoST();
            bInfo.col = lsBoxJson[i]["col"].AsInt;
            bInfo.row = lsBoxJson[i]["row"].AsInt;

            var goldMineJson = lsBoxJson[i]["goldMineInfo"];
            bInfo.goldMineInfo = new GoldMineInfoST();
            bInfo.goldMineInfo.ID = goldMineJson["ID"].AsInt;
            bInfo.goldMineInfo.level = goldMineJson["level"].AsInt;
            bInfo.goldMineInfo.name = goldMineJson["name"];
            bInfo.goldMineInfo.isPlayer = goldMineJson["isPlayer"].AsBool;
            bInfo.goldMineInfo.indexLoadGoldMine = goldMineJson["indexLoadGoldMine"].AsInt;
            bInfo.goldMineInfo.xR = goldMineJson["xR"].AsFloat;
            bInfo.goldMineInfo.yR = goldMineJson["yR"].AsFloat;
            bInfo.goldMineInfo.zR = goldMineJson["zR"].AsFloat;
            bInfo.goldMineInfo.lsHeroGoldMine = new List<HeroInfoST>();
            var lsHeroGoldMineJson = goldMineJson["lsHeroGoldMine"];

            if (lsHeroGoldMineJson.Count > 0)
            {
                for (int j = 0; j < lsHeroGoldMineJson.Count; j++)
                {
                    HeroInfoST hInfo = new HeroInfoST();
                    hInfo.ID = lsHeroGoldMineJson[j]["ID"].AsInt;
                    hInfo.CountHero = lsHeroGoldMineJson[j]["CountHero"].AsInt;
                    bInfo.goldMineInfo.lsHeroGoldMine.Add(hInfo);
                }
            }

            lsBox.Add(bInfo);
        }

        yield return new WaitUntil(() => lsBox.Count == lsBoxJson.Count);

        castlePlayer = new CastleInfoST();
        castlePlayer.lsHouse = new List<HouseInfoST>();
        var lsHouseJson = objJson["castlePlayer"]["lsHouse"];

        if (lsHouseJson.Count > 0)
        {
            for (int i = 0; i < lsHouseJson.Count; i++)
            {
                HouseInfoST hInfo = new HouseInfoST();
                hInfo.ID = lsHouseJson[i]["ID"].AsInt;
                hInfo.level = lsHouseJson[i]["level"].AsInt;
                hInfo.isLock = lstHousePlayerJson[i]["isLock"].AsBool;
                hInfo.heroInfo = new HeroInfoST();
                hInfo.heroInfo.ID = lsHouseJson[i]["heroInfo"]["ID"].AsInt;
                hInfo.heroInfo.CountHero = lsHouseJson[i]["heroInfo"]["CountHero"].AsInt;
                castlePlayer.lsHouse.Add(hInfo);
            }
        }

        yield return new WaitUntil(() => castlePlayer.lsHouse.Count == lsHouseJson.Count);

        Fade.Instance.EndFade(0);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true && GameManager.Instance.stateGame == StateGame.Playing)
        {
            SaveDataPlayer();
        }
    }

    public void OnDestroy()
    {
        if (GameManager.Instance.stateGame == StateGame.Playing)
        {
            SaveDataPlayer();
        }
    }
}
