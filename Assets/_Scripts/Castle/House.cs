using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class House : MonoBehaviour
{
    public int idHouse;
    public int idHero;
    public int level;
    public long price;
    public int capWar;
    public TypeStateHouse typeState;
    public int countHero;
    public float timeBuild;
    public float timeUpgrade;
    int xUpgrade;
    public long priceWillUpgrade;
    public int capWillUpgrade;
    public int levelWillupgrade;

    public Button buttonUpgrade;
    public Button buttonRelease;
    public Text txtCountHero;
    public Text txtCountTime;
    public Text txtLevel;
    public Image imgHouse;
    public GameObject panelHouse;
    public Image imgLoadingBuild;
    public Image imgLoadingBar;
    public Image imgNotBuild;
    // Use this for initialization
    void Start()
    {
        //if (this.typeState == TypeStateHouse.None)
        //{
        //    this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
        //    this.RegisterListener(EventID.ClickHouse, (param) => OnClickHouse(param));
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (typeState == TypeStateHouse.None && txtCountHero.gameObject.activeSelf)
        {
            txtCountHero.text = countHero.ToString();
        }

        if (typeState == TypeStateHouse.Building)
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

        if (typeState == TypeStateHouse.Upgrading)
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
        //if (time == 0) time = timeBuild;
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void LoadDataHouse(int _idHero, int _level, int _capWar, long _price, TypeStateHouse _type, int _countHero)
    {
        this.idHero = _idHero;
        this.level = _level;
        this.capWar = _capWar;
        this.price = _price;
        this.typeState = _type;
        this.countHero = _countHero;
        if (this.typeState == TypeStateHouse.None)
        {
            this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
            this.RegisterListener(EventID.ClickHouse, (param) => OnClickHouse(param));
        }
    }

    public void Btn_OnClick()
    {
        if (typeState == TypeStateHouse.Building || typeState == TypeStateHouse.Upgrading)
            return;

        UIManager.Instance.houseClick = idHouse;
        if (typeState == TypeStateHouse.None)
        {
            this.PostEvent(EventID.ClickHouse, idHouse);

            if (!buttonUpgrade.gameObject.activeSelf)
            {
                buttonUpgrade.gameObject.SetActive(true);
            }
            else
            {
                buttonUpgrade.gameObject.SetActive(false);
            }

            if (!buttonRelease.gameObject.activeSelf)
            {
                buttonRelease.gameObject.SetActive(true);
            }
            else
            {
                buttonRelease.gameObject.SetActive(false);
            }
        }
        else if (typeState == TypeStateHouse.Lock)
        {
            UIManager.Instance.ShowPanelBuild();
        }
    }

    public void Btn_Upgrade()
    {
        UIManager.Instance.ShowPanelUpgrade();
        buttonUpgrade.gameObject.SetActive(false);
        buttonRelease.gameObject.SetActive(false);

    }

    public void Btn_Release()
    {
        UIManager.Instance.ShowPanelRelease();
        buttonUpgrade.gameObject.SetActive(false);
        buttonRelease.gameObject.SetActive(false);
    }

    public void CheckUpgrade(int _x)
    {
        levelWillupgrade = level + _x;
        priceWillUpgrade = (long)(price * Mathf.Pow(GameConfig.Instance.Ri, _x));
        capWillUpgrade = (int)(capWar * Mathf.Pow(GameConfig.Instance.Wi, _x));
        //capWillUpgrade = capWar;
        //priceWillUpgrade = price;
        //levelWillupgrade = level;
        //for (int i = 1; i <= _x; i++)
        //{
        //    levelWillupgrade++;
        //    priceWillUpgrade = (long)(priceWillUpgrade * GameConfig.Instance.Ri);
        //    capWillUpgrade = (int)(capWillUpgrade * GameConfig.Instance.Wi);
        //}
    }

    public void YesUpgrade(int _x)
    {
        if (GameManager.Instance.gold < price)
            return;

        price = priceWillUpgrade;
        GameManager.Instance.AddGold(-(long)(price * GameConfig.Instance.Ri));
        timeUpgrade = GameConfig.Instance.UpgradeTime * _x;
        typeState = TypeStateHouse.Upgrading;
        xUpgrade = _x;
        txtCountHero.gameObject.SetActive(false);
        imgLoadingBuild.gameObject.SetActive(true);
        panelHouse.SetActive(false);
    }

    void UpgradeComplete()
    {
        typeState = TypeStateHouse.None;
        level = levelWillupgrade;
        capWar = capWillUpgrade;
        timeUpgrade = GameConfig.Instance.UpgradeTime;
        imgLoadingBuild.gameObject.SetActive(false);
        panelHouse.SetActive(true);
        txtCountHero.gameObject.SetActive(true);
        txtLevel.text = "Lv " + level.ToString();
    }

    public void Build(int _id)
    {
        this.price = (int)GameConfig.Instance.lstInfoHero[_id].price;
        if (GameManager.Instance.gold < this.price)
            return;
        GameManager.Instance.AddGold(-price);
        timeBuild = GameConfig.Instance.BuildTime * idHouse;
        timeUpgrade = GameConfig.Instance.UpgradeTime;
        typeState = TypeStateHouse.Building;
        this.idHero = _id;
        this.txtCountHero.gameObject.SetActive(false);
        this.imgHouse.sprite = UIManager.Instance.lstSpriteHouse[_id];

        imgNotBuild.enabled = false;
        imgLoadingBuild.gameObject.SetActive(true);
        panelHouse.SetActive(false);
    }

    void BuildComplete()
    {
        typeState = TypeStateHouse.None;
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
        this.RegisterListener(EventID.ClickHouse, (param) => OnClickHouse(param));
        this.level = 1;
        //this.price = (int)GameConfig.Instance.lstInfoHero[idHero].price;
        this.capWar = (int)GameConfig.Instance.lstInfoHero[idHero].capWar;
        this.countHero = 0;
        imgLoadingBuild.gameObject.SetActive(false);
        panelHouse.SetActive(true);
        txtCountHero.gameObject.SetActive(true);
        txtLevel.text = "Lv " + level.ToString();
        if (this.idHero == 11)
            GameManager.Instance.castlePlayer.isCanReleaseCanon = true;
    }

    public void SpawmHero()
    {
        countHero += capWar;
    }

    public void OnNextDay()
    {
        SpawmHero();
    }

    public void OnClickHouse(object _param)
    {
        if (idHouse != (int)_param)
        {
            if (buttonUpgrade.gameObject.activeSelf)
                buttonUpgrade.gameObject.SetActive(false);
            if (buttonRelease.gameObject.activeSelf)
                buttonRelease.gameObject.SetActive(false);
        }
    }
}

public enum TypeStateHouse
{
    None = 1,
    Lock = 2,
    Building = 3,
    Upgrading = 4
}
