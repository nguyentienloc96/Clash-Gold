using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements; // only compile Ads code on supported platforms
#endif

public class LoadDataJson : MonoBehaviour
{
    public static LoadDataJson Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    private string gameConfig = "GameConfig";

    void Start()
    {
        LoadGameConfig();
        Ads.Instance.RequestAd();
        Ads.Instance.RequestBanner();
        if (PlayerPrefs.GetInt("isTutorial") == 1)
        {
            Ads.Instance.ShowBanner();
        }   
#if UNITY_ADS
        Advertisement.Initialize(GameConfig.Instance.ID_UnityAds_ios, true);
#endif
        Purchaser.Instance.Init();
    }

    public void LoadGameConfig()
    {
        var objJson = SimpleJSON_DatDz.JSON.Parse(loadJson(gameConfig));
        //Debug.Log(objJson);
        Debug.Log("<color=yellow>Done: </color>LoadGameConfig !");
        if (objJson != null)
        {
            GameConfig.Instance.GoldStart = objJson["GoldStart"].AsDouble;
            GameConfig.Instance.CoinStart = objJson["CoinStart"].AsLong;
            GameConfig.Instance.GoldMinerAmount = objJson["GoldMinerAmount"].AsInt;
            for (int i = 0; i < GameConfig.Instance.RatioBorn.Length; i++)
            {
                GameConfig.Instance.RatioBorn[i] = objJson["RatioBorn"][i].AsFloat;
            }
            GameConfig.Instance.OwnLand1 = objJson["OwnLand1"].AsInt;
            GameConfig.Instance.OwnLand2 = objJson["OwnLand2"].AsInt;
            GameConfig.Instance.RatioBornAdd = objJson["RatioBornAdd"].AsFloat;
            GameConfig.Instance.TimeDestroy = objJson["TimeDestroy"].AsFloat;
            GameConfig.Instance.Timeday = objJson["Timeday"].AsFloat;
            GameConfig.Instance.TimeAd = objJson["TimeAd"].AsFloat;
            GameConfig.Instance.Rgold = objJson["Rgold"].AsFloat;
            GameConfig.Instance.CapGold0 = objJson["CapGold0"].AsLong;
            GameConfig.Instance.CapGoldUp = objJson["CapGoldUp"].AsFloat;
            GameConfig.Instance.PriceGoldUp = objJson["PriceGoldUp"].AsInt;
            GameConfig.Instance.Bloodlv0 = objJson["Bloodlv0"].AsLong;
            GameConfig.Instance.Bloodratio = objJson["Bloodratio"].AsFloat;
            GameConfig.Instance.PriceBlood0 = objJson["PriceBlood0"].AsLong;
            GameConfig.Instance.PriceBloodUp = objJson["PriceBloodUp"].AsFloat;
            GameConfig.Instance.Ri = objJson["Ri"].AsFloat;
            GameConfig.Instance.Wi = objJson["Wi"].AsFloat;
            GameConfig.Instance.BuildTime = objJson["BuildTime"].AsFloat;
            GameConfig.Instance.UpgradeTime = objJson["UpgradeTime"].AsFloat;
            GameConfig.Instance.Hi = objJson["Hi"].AsFloat;
            GameConfig.Instance.Med = objJson["Med"].AsFloat;
            GameConfig.Instance.Lo = objJson["Lo"].AsFloat;
            GameConfig.Instance.health1 = objJson["health1"].AsFloat;
            GameConfig.Instance.dam1 = objJson["dam1"].AsFloat;
            GameConfig.Instance.UnitRange = objJson["UnitRange"].AsFloat;
            GameConfig.Instance.hitspeed1 = objJson["hitspeed1"].AsFloat;
            GameConfig.Instance.speed_medium = objJson["speed_medium"].AsFloat;
            GameConfig.Instance.speed_hard = objJson["speed_hard"].AsFloat;
            GameConfig.Instance.Price01 = objJson["Price01"].AsLong;
            GameConfig.Instance.CapWar01 = objJson["CapWar01"].AsInt;
            GameConfig.Instance.TimeCanon = objJson["TimeCanon"].AsFloat;
            GameConfig.Instance.Timecanonsurvive = objJson["Timecanonsurvive"].AsFloat;
            GameConfig.Instance.ID_UnityAds_ios = objJson["ID_UnityAds_ios"];
            GameConfig.Instance.ID_Inter_android = objJson["ID_Inter_android"];
            GameConfig.Instance.ID_Inter_ios = objJson["ID_Inter_ios"];
            GameConfig.Instance.ID_Banner_ios = objJson["ID_Banner_ios"];
            GameConfig.Instance.kProductID50 = objJson["kProductID50"];
            GameConfig.Instance.kProductID300 = objJson["kProductID300"];
            GameConfig.Instance.kProductID5000 = objJson["kProductID5000"];
            GameConfig.Instance.link_ios = objJson["link_ios"];
            GameConfig.Instance.string_Share = objJson["string_Share"];
            for (int j = 0; j < objJson["speech"].Count; j++)
            {
                GameConfig.Instance.lstSpeech.Add(objJson["speech"][j]);
            }
        }
    }

    string loadJson(string _nameJson)
    {
        TextAsset _text = Resources.Load(_nameJson) as TextAsset;
        return _text.text;
    }

    public void GoldToDollar()
    {
        //Debug.Log(GameManager.Instance.gold);
        //Debug.Log(PlayerPrefs.GetInt("GoldPre"));
        //if (GameManager.Instance.gold > 0)
        //{
        //    int locationEnd = GameManager.Instance.lsLocation.Count - 1;
        //    int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        //    if (jobEnd == -1)
        //    {
        //        locationEnd--;
        //        jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        //    }
        //    double dollarRecive = 0;
        //    if (GameManager.Instance.gold >= 5)
        //    {
        //        SetNumber(GetNumber2(dola) + 50000, dola);
        //        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 10) - 5);
        //        GameManager.Instance.gold -= 5;
        //        dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price / 5;
        //        GameManager.Instance.dollar += dollarRecive;
        //        gold.text = SetNumberString(PlayerPrefs.GetInt("Gold", 10));
        //    }
        //    else
        //    {
        //        SetNumber(GetNumber2(dola) + PlayerPrefs.GetInt("Gold", 10) * 10000, dola);
        //        PlayerPrefs.SetInt("Gold", 0);
        //        dollarRecive = (GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price / 5) * GameManager.Instance.gold;
        //        GameManager.Instance.dollar += dollarRecive;
        //        GameManager.Instance.gold = 0;
        //        gold.text = "0";
        //    }
        //    UIManager.Instance.PushGiveGold("You have received " + UIManager.Instance.ConvertNumber(dollarRecive) + "$");
        //    if (GameManager.Instance.gold > 10)// && Mathf.Abs(PlayerPrefs.GetInt("GoldPre", 0) - PlayerPrefs.GetInt("Gold", 10)) >= 50)
        //    {
        //        PlayerPrefs.SetInt("GoldPre", (int)GameManager.Instance.gold);
        //        Debug.Log(PlayerPrefs.GetInt("GoldPre"));
        //        StorageService storageService = App42API.BuildStorageService();
        //        storageService.UpdateDocumentByKeyValue("Db", "Data", "id", GameConfig.id, JsonUtility.ToJson(new SaveGold(GameConfig.id, (int)GameManager.Instance.gold)), new UnityCallBack2());
        //    }
        //}
    }

    public void RestoreProgess()
    {
        //loading.SetActive(true);
        StorageService storageService = App42API.BuildStorageService();
        storageService.FindDocumentByKeyValue("Db", "Data", "id", GameConfig.id, new UnityCallBack3());
        //UIManager.Instance.panelSetting.SetActive(false);

        //UIManager.Instance.PushGiveGold("Waiting ...");
    }

}
