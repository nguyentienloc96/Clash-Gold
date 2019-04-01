using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SocialPlatforms.GameCenter;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;

[RequireComponent(typeof(LoadDataJson))]
public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance;
    public static string id = "";
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public long GoldStart;
    public int CoinStart;

    public int GoldMinerAmount;
    public float[] RatioBorn = {1f , 0.7f , 0.3f};
    public int OwnLand1;
    public int OwnLand2;
    public float RatioBornAdd;
    public float TimeDestroy;
    public float Timeday;
    public float TimeAd;
    public float Rgold;
    public int CapGold0;
    public float CapGoldUp;
    public int PriceGoldUp;
    public long Bloodlv0;
    public float Bloodratio;
    public long PriceBlood0;
    public float PriceBloodUp;
    public float Ri;
    public float Wi;
    public float BuildTime;
    public float UpgradeTime;
    public float Hi;
    public float Med;
    public float Lo;
    public float health1;
    public float UnitRange;
    public float dam1;
    public float hitspeed1;
    public float speed_medium;
    public float speed_hard;
    public long Price01;
    public int CapWar01;
    public float TimeCanon;
    public float Timecanonsurvive;
    public int TimeUp;
    public int LandUP;
    public int LandDiv;
    public List<string> lsSpeech = new List<string>();
    public List<InfoHero> lsInfoHero = new List<InfoHero>();
    public List<Vector2[]> listMap = new List<Vector2[]>();
    public List<string> lsStory = new List<string>();
    public List<string> lsNameIsLand = new List<string>();
    public string ID_UnityAds_ios;
    public string ID_Inter_android;
    public string ID_Inter_ios;
    public string ID_Banner_ios;
    public string link_ios;
    public string link_android;
    public string string_Share;
    public string kProductID50 = "consumable";
    public string kProductID300 = "consumable";
    public string kProductID5000 = "consumable";
    string app42_apiKey = "41b8289bb02efae4f37f1c9d891b09bb43f6f801bdbbf17a557bc4598ddf836b";
    string app42_secretKey = "35d9a321b8d4cfc3b375b5f212f15ffab98bb2b53e4b9da20d22881fc01a0efa";

    void Start()
    {
        //if (id == "")
        //{
        //    App42API.Initialize(app42_apiKey, app42_secretKey);
        //    //GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
        //    Social.localUser.Authenticate(success =>
        //    {
        //        if (success)
        //        {
        //            id = Social.localUser.id;
        //            StorageService storageService = App42API.BuildStorageService();
        //            storageService.FindDocumentByKeyValue("Db", "Data", "id", id, new UnityCallBack1());
        //        }
        //        else
        //            Debug.Log("Failed to authenticate");
        //    });
        //}
    }

    public string GetNameIsLand()
    {
        int radom = Random.Range(0, lsNameIsLand.Count);
        string strName = lsNameIsLand[radom];
        lsNameIsLand.Remove(strName);
        return strName;
    }
}

public class SaveGold
{
    public string id;
    public int gold;
    public SaveGold(string id, int gold)
    {
        this.id = id;
        this.gold = gold;
    }
}
