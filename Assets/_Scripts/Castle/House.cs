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

    void Update()
    {
        if (typeState == TypeStateHouse.None && txtCountHero.gameObject.activeSelf)
        {
            txtCountHero.text = UIManager.Instance.ConvertNumber(countHero);
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

    //public void LoadDataHouse(int _idHero, int _level, int _capWar, long _price, TypeStateHouse _type, int _countHero)
    //{
    //    this.idHero = _idHero;
    //    this.level = _level;
    //    this.capWar = _capWar;
    //    this.price = _price;
    //    this.typeState = _type;
    //    this.countHero = _countHero;
    //    if (this.typeState == TypeStateHouse.None)
    //    {
    //        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
    //        this.RegisterListener(EventID.ClickHouse, (param) => OnClickHouse(param));
    //    }
    //}

    public void Btn_OnClick()
    {
        if (typeState == TypeStateHouse.Building || typeState == TypeStateHouse.Upgrading)
            return;

        UIManager.Instance.houseClick = idHouse;
        if (typeState == TypeStateHouse.None)
        {
            this.PostEvent(EventID.ClickHouse, idHouse);

            //if (!buttonUpgrade.gameObject.activeSelf)
            //{
            //    buttonUpgrade.gameObject.SetActive(true);
            //}
            //else
            //{
            //    buttonUpgrade.gameObject.SetActive(false);
            //}

            //if (!buttonRelease.gameObject.activeSelf)
            //{
            //    buttonRelease.gameObject.SetActive(true);
            //}
            //else
            //{
            //    buttonRelease.gameObject.SetActive(false);
            //}
            UIManager.Instance.ShowPanelUpgrade();
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

    public void CheckUpgrade(int _x)
    {
        levelWillupgrade = level + _x;
        priceWillUpgrade = (long)(price * Mathf.Pow(GameConfig.Instance.Ri, _x));
        capWillUpgrade = (int)(capWar * Mathf.Pow(GameConfig.Instance.Wi, _x));
    }

    public void YesUpgrade(int _x)
    {
        if (GameManager.Instance.gold < priceWillUpgrade)
            return;

        UIManager.Instance.SetActivePanel(UIManager.Instance.anim_UpLV_House);
        Invoke("HideAnim", 2f);
        price = priceWillUpgrade;
        GameManager.Instance.AddGold(-price);
        timeUpgrade = GameConfig.Instance.UpgradeTime * _x;
        typeState = TypeStateHouse.Upgrading;
        xUpgrade = _x;
        txtCountHero.gameObject.SetActive(false);
        imgLoadingBuild.gameObject.SetActive(true);
        panelHouse.SetActive(false);
    }

    void HideAnim()
    {
        UIManager.Instance.SetDeActivePanel(UIManager.Instance.anim_UpLV_House);
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
        txtLevel.text = level.ToString();
        if (level > GameManager.Instance.maxLevelHouse)
        {
            GameManager.Instance.maxLevelHouse = level;
            this.PostEvent(EventID.UpLevelHouse);
        }
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
        this.idHero = _id + 1;
        this.txtCountHero.gameObject.SetActive(false);
        this.imgHouse.sprite = UIManager.Instance.lstSpriteHouse[_id];

        imgNotBuild.enabled = false;
        imgLoadingBuild.gameObject.SetActive(true);
        panelHouse.SetActive(false);
        UIManager.Instance.lsBtnIconHouse[_id].interactable = false;
    }

    void BuildComplete()
    {
        typeState = TypeStateHouse.None;
        this.RegisterListener(EventID.NextDay, (param) => OnNextDay());
        this.RegisterListener(EventID.ClickHouse, (param) => OnClickHouse(param));
        this.level = 1;
        this.capWar = (int)GameConfig.Instance.lstInfoHero[idHero - 1].capWar;
        this.countHero = 0;
        imgLoadingBuild.gameObject.SetActive(false);
        panelHouse.SetActive(true);
        txtCountHero.gameObject.SetActive(true);
        txtLevel.text = level.ToString();
        Dictionary<string, int> keyHouse = new Dictionary<string, int>();
        keyHouse.Add("IdHouse", idHouse);
        keyHouse.Add("IdHero", idHero);
        this.PostEvent(EventID.BuildHouseComplete, keyHouse);
    }

    public void SpawmHero()
    {
        countHero += capWar;
    }

    public void OnNextDay()
    {
        SpawmHero();
        //this.PostEvent(EventID.BuildHouseComplete, idHero);
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
