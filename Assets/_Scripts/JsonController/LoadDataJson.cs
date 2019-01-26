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
    public GameObject gameManager;
    public bool isReset;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        if (isReset)
        {
            PlayerPrefs.DeleteAll();
        }
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
            GameConfig.Instance.GoldStart = objJson["GoldStart"].AsLong;
            GameConfig.Instance.CoinStart = objJson["CoinStart"].AsInt;
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
            GameConfig.Instance.link_android = objJson["link_android"];
            GameConfig.Instance.string_Share = objJson["string_Share"];
            for (int j = 0; j < objJson["speech"].Count; j++)
            {
                GameConfig.Instance.lstSpeech.Add(objJson["speech"][j]);
            }
            for (int t = 0; t < objJson["heroes"].Count; t++)
            {
                InfoHero _info = new InfoHero();
                _info.ID = objJson["heroes"][t]["id"].AsInt;
                _info.health = objJson["heroes"][t]["health1"].AsFloat;
                _info.dame = objJson["heroes"][t]["dam1"].AsFloat;
                _info.hitSpeed = objJson["heroes"][t]["hitspeed1"].AsFloat;
                _info.range = objJson["heroes"][t]["range"].AsFloat;
                _info.speed = objJson["heroes"][t]["speed1"].AsFloat;
                _info.price = objJson["heroes"][t]["Price01"].AsInt;
                int _t = objJson["heroes"][t]["xCapWar"].AsInt;
                if (_t == 0)
                {
                    _info.capWar = objJson["heroes"][t]["CapWar01"].AsInt * GameConfig.Instance.Hi;
                }
                else if (_t == 1)
                {
                    _info.capWar = objJson["heroes"][t]["CapWar01"].AsInt * GameConfig.Instance.Med;
                }
                else if (_t == 2)
                {
                    _info.capWar = objJson["heroes"][t]["CapWar01"].AsInt * GameConfig.Instance.Lo;
                }
                _info.isMom = objJson["heroes"][t]["isMom"].AsBool;
                _info.isBaby = objJson["heroes"][t]["isBaby"].AsBool;
                _info.idMom = objJson["heroes"][t]["idMom"].AsInt;
                _info.idBaby = objJson["heroes"][t]["idBaby"].AsInt;
                _info.speedBullet = objJson["heroes"][t]["speedBullet"].AsFloat;
                if (objJson["heroes"][t]["type"].AsInt == 1)
                {
                    _info.typeHero = TypeHero.ChemThuong;
                }
                else if (objJson["heroes"][t]["type"].AsInt == 2)
                {
                    _info.typeHero = TypeHero.ChemBay;
                }
                else if (objJson["heroes"][t]["type"].AsInt == 3)
                {
                    _info.typeHero = TypeHero.CungThuong;
                }
                else if (objJson["heroes"][t]["type"].AsInt == 4)
                {
                    _info.typeHero = TypeHero.CungBay;
                }
                else if (objJson["heroes"][t]["type"].AsInt == 5)
                {
                    _info.typeHero = TypeHero.Canon;
                }
                GameConfig.Instance.lstInfoHero.Add(_info);
            }          
        }
        gameManager.SetActive(true);
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
