using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataPlayer : MonoBehaviour
{
    [HideInInspector]
    public long gold; //vàng
    [HideInInspector]
    public int coin; //coin
    [HideInInspector]
    public float ratioBorn; //độ khó
    [HideInInspector]
    public DateTime dateGame; //ngày trong game
    [HideInInspector]
    public DateTime dateEnemyAttack; //ngày enemy tấn công
    [HideInInspector]
    public Castle castlePlayer; //thành của người chơi
    [HideInInspector]
    public List<GoldMine> lstGoldMinePlayer; //list mỏ vàng người chơi
    [HideInInspector]
    public List<BuildHouse> lstBuildHouse; //nhà đã mở hay chưa
    [HideInInspector]
    public List<House> lstHouseInWall; //list nhà trong thành
    [HideInInspector]
    public List<Hero> lstHero; //list hero
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator SaveDataPlayer()
    {
        DataPlayer data = new DataPlayer();
        data.gold = GameManager.Instance.gold;
        data.coin = GameManager.Instance.coin;
        data.ratioBorn = GameManager.Instance.ratioBorn;
        data.dateGame = GameManager.Instance.dateGame;
        data.dateEnemyAttack = GameManager.Instance.dateEnemyAttack;
        data.lstBuildHouse = GameManager.Instance.lstBuildHouse;
        data.lstHouseInWall = GameManager.Instance.lstHousePlayer;
        yield return new WaitForEndOfFrame();
        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        File.WriteAllText(_path, JsonUtility.ToJson(data, true));
        File.ReadAllText(_path);
        PlayerPrefs.SetInt(KeyPrefs.IS_CONTINUE, 1);

        Debug.Log(SimpleJSON_DatDz.JSON.Parse(File.ReadAllText(_path)));
    }

    public void LoadDataPlayer()
    {
        string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
        string dataAsJson = File.ReadAllText(_path);
        var objJson = SimpleJSON_DatDz.JSON.Parse(dataAsJson);
        Debug.Log(objJson);
        if (objJson != null)
        {
            StartCoroutine(IE_LoadDataPlayer(objJson));
        }
    }

    public IEnumerator IE_LoadDataPlayer(SimpleJSON_DatDz.JSONNode objJson)
    {
        yield return null;
    }
}

[System.Serializable]
public struct BuildHouse
{
    public int ID;
    public bool isUnlock;
}
