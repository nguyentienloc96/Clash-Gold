using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public enum TypeStateHouse
{
    None = 1,
    Lock = 2,
    Building = 3,
    Upgrading = 4
}

[System.Serializable]
public struct HouseInfo
{
    public int ID;
    public int level;
    public long price;
    public int capWar;
    public TypeStateHouse typeState;

    public int idHero;
    public int countHero;
}

public class House : MonoBehaviour
{

    public HouseInfo info;

    public float timeBuild;
    public float timeUpgrade;

    public long priceWillUpgrade;
    public int capWillUpgrade;
    public int levelWillupgrade;

    public Text txtCountHero;
    public Text txtCountTime;
    public Text txtLevel;
    public Image imgHouse;
    public GameObject panelHouse;
    public Image imgLoadingBuild;
    public Image imgLoadingBar;
    public Image imgNotBuild;

    void Update()
    {
        if (info.typeState == TypeStateHouse.None && txtCountHero.gameObject.activeSelf)
        {
            txtCountHero.text = UIManager.Instance.ConvertNumber(info.countHero);
        }

        if (info.typeState == TypeStateHouse.Building)
        {
            if (timeBuild <= 0)
            {
                BuildComplete();
                timeBuild = 0;
            }
            timeBuild -= Time.deltaTime;
            imgLoadingBar.fillAmount += (1.0f / (2.2f * timeBuild)) * Time.deltaTime;
            txtCountTime.text = transformToTime(timeBuild);
        }

        if (info.typeState == TypeStateHouse.Upgrading)
        {
            if (timeUpgrade <= 0)
            {
                UpgradeComplete();
                timeUpgrade = 0;
            }
            timeUpgrade -= Time.deltaTime;
            imgLoadingBar.fillAmount += (1.0f / (2.2f * timeUpgrade)) * Time.deltaTime;
            txtCountTime.text = transformToTime(timeUpgrade);
        }
    }

    string transformToTime(float time = 0)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Btn_OnClick()
    {
        if (info.typeState == TypeStateHouse.Building || info.typeState == TypeStateHouse.Upgrading)
            return;

        UIManager.Instance.houseClick = info.ID;
        if (info.typeState == TypeStateHouse.None)
        {
            this.PostEvent(EventID.ClickHouse, info.ID);
            UIManager.Instance.ShowPanelUpgrade();
        }
        else if (info.typeState == TypeStateHouse.Lock)
        {
            UIManager.Instance.ShowPanelBuild();
        }
    }

    public void Btn_Upgrade()
    {
        UIManager.Instance.ShowPanelUpgrade();
    }

    public void CheckUpgrade(int _x)
    {
        levelWillupgrade = info.level + _x;
        priceWillUpgrade = (long)(info.price * Mathf.Pow(GameConfig.Instance.Ri, _x));
        capWillUpgrade = (int)(info.capWar * Mathf.Pow(GameConfig.Instance.Wi, _x));
    }

    public void YesUpgrade(int _x)
    {
        if (GameManager.Instance.gold < priceWillUpgrade)
            return;
        info.price = priceWillUpgrade;
        GameManager.Instance.AddGold(-info.price);
        timeUpgrade = GameConfig.Instance.UpgradeTime * _x;
        info.typeState = TypeStateHouse.Upgrading;
        txtCountHero.gameObject.SetActive(false);
        imgLoadingBuild.gameObject.SetActive(true);
        panelHouse.SetActive(false);
    }

    void UpgradeComplete()
    {
        info.typeState = TypeStateHouse.None;
        info.level = levelWillupgrade;
        info.capWar = capWillUpgrade;
        timeUpgrade = GameConfig.Instance.UpgradeTime;
        imgLoadingBuild.gameObject.SetActive(false);
        panelHouse.SetActive(true);
        txtCountHero.gameObject.SetActive(true);
        txtLevel.text = info.level.ToString();
        if (info.level > GameManager.Instance.maxLevelHouse)
        {
            GameManager.Instance.maxLevelHouse = info.level;
            this.PostEvent(EventID.UpLevelHouse);
        }
    }

    public void Build(int _id)
    {
        info.price = (int)GameConfig.Instance.lsInfoHero[_id].price;
        UIManager.Instance.panelBuildHouse.SetActive(true);
        DetailInfoHero detail = UIManager.Instance.panelBuildHouse.GetComponent<DetailInfoHero>();
        string infoDetail = "";
        infoDetail += ": " + GameConfig.Instance.lsInfoHero[_id].health + "\n";
        infoDetail += ": " + GameConfig.Instance.lsInfoHero[_id].dame + "\n";
        infoDetail += ": " + GameConfig.Instance.lsInfoHero[_id].hitSpeed + "\n";
        infoDetail += ": " + GameConfig.Instance.lsInfoHero[_id].speed + "\n";
        if (GameConfig.Instance.lsInfoHero[_id].range != 0)
        {
            infoDetail += ": " + GameConfig.Instance.lsInfoHero[_id].range;
        }
        else
        {
            infoDetail += ": Mele";
        }
        detail.GetInfo(
            UIManager.Instance.lsSprAvatarHero[_id],
            GameConfig.Instance.lsInfoHero[_id].nameHero,
            GameConfig.Instance.lsInfoHero[_id].info,
            infoDetail,
            info.price,
            () =>
            {
                YesBuild(_id);
                UIManager.Instance.panelBuildHouse.SetActive(false);
            });
        if (GameManager.Instance.gold < info.price)
        {
            detail.btnYes.interactable = false;
        }
    }

    public void YesBuild(int _id)
    {
        info.price = (int)GameConfig.Instance.lsInfoHero[_id].price;
        if (GameManager.Instance.gold < info.price)
            return;
        GameManager.Instance.AddGold(-info.price);
        timeBuild = GameConfig.Instance.BuildTime * info.ID;
        timeUpgrade = GameConfig.Instance.UpgradeTime;
        info.typeState = TypeStateHouse.Building;
        this.info.idHero = _id + 1;
        this.txtCountHero.gameObject.SetActive(false);
        this.imgHouse.sprite = UIManager.Instance.lsSprAvatarHero[_id];

        imgNotBuild.enabled = false;
        imgLoadingBuild.gameObject.SetActive(true);
        panelHouse.SetActive(false);
        UIManager.Instance.lsBtnIconHouse[_id].interactable = false;
        BuildHouse buildHouse = new BuildHouse();
        buildHouse.ID = GameManager.Instance.lsBuildHouse[_id].ID;
        buildHouse.isUnlock = false;
        GameManager.Instance.lsBuildHouse[_id] = buildHouse;
    }

    void BuildComplete()
    {
        info.typeState = TypeStateHouse.None;
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
        info.level = 1;
        info.capWar = (int)GameConfig.Instance.lsInfoHero[info.idHero - 1].capWar;
        info.countHero = 0;
        imgLoadingBuild.gameObject.SetActive(false);
        panelHouse.SetActive(true);
        txtCountHero.gameObject.SetActive(true);
        txtLevel.text = info.level.ToString();
        Dictionary<string, int> keyHouse = new Dictionary<string, int>();
        keyHouse.Add("IdHouse", info.ID);
        keyHouse.Add("IdHero", info.idHero);
        this.PostEvent(EventID.BuildHouseComplete, keyHouse);
    }

    public void BuildCompleteJson(int level,int idHero,int countHero)
    {
        info.typeState = TypeStateHouse.None;
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
        info.level = level;
        info.idHero = idHero;
        info.capWar = (int)GameConfig.Instance.lsInfoHero[info.idHero - 1].capWar;
        info.countHero = countHero;
        imgLoadingBuild.gameObject.SetActive(false);
        panelHouse.SetActive(true);
        txtCountHero.gameObject.SetActive(true);
        txtLevel.text = info.level.ToString();
    }

    public void SpawmHero()
    {
        info.countHero += info.capWar;
    }

    public void OnNextDay()
    {
        SpawmHero();
    }

    public void AddHero(int numberHero)
    {
        info.countHero += numberHero;
        if (info.countHero < 0)
            info.countHero = 0;
    }
}
