using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour {
    public int idHouse;
    public int idHero;
    public int level;
    public TypeStateHouse typeState;
    public int countHero;
    public float timeBuild;
    public float timeUpgrade;
    int xUpgrade;

    public Button buttonUpgrade;
    public Button buttonRelease;
    public Text txtCountHero;
    public Text txtCountTime;
    public Image imgHouse;
    public Image imgLoadingBuild;
    public Image imgNotBuild;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
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
            transformToTime(timeBuild);
        }

        if (typeState == TypeStateHouse.Upgrading)
        {
            if (timeUpgrade <= 0)
            {
                typeState = TypeStateHouse.None;
                imgLoadingBuild.gameObject.SetActive(false);
                imgHouse.gameObject.SetActive(true);
                txtCountHero.gameObject.SetActive(true);
                timeUpgrade = 0;
            }
            timeUpgrade -= Time.deltaTime;
            transformToTime(timeUpgrade);
        }
	}

    string transformToTime(float time = 0)
    {
        //if (time == 0) time = timeBuild;
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Btn_OnClick()
    {
        if (typeState == TypeStateHouse.Building || typeState == TypeStateHouse.Upgrading)
            return;

        if (typeState == TypeStateHouse.None)
        {
            buttonUpgrade.gameObject.SetActive(true);
            buttonRelease.gameObject.SetActive(true);
        }
        else if (typeState == TypeStateHouse.Lock)
        {
            UIManager.Instance.houseClick = idHouse;
            UIManager.Instance.ShowPanelBuild();
        }
    }

    public void Btn_Upgrade()
    {
        UIManager.Instance.ShowPanelUpgrade();        
    }

    public void YesUpgrade(int _x)
    {
        typeState = TypeStateHouse.Upgrading;
        xUpgrade = _x;
        txtCountHero.gameObject.SetActive(false);
        imgLoadingBuild.gameObject.SetActive(true);
        imgHouse.gameObject.SetActive(false);
    }

    void UpgradeComplete()
    {
        typeState = TypeStateHouse.None;
        //this.level = 1;
        //this.countHero = 0;
        imgLoadingBuild.gameObject.SetActive(false);
        imgHouse.gameObject.SetActive(true);
        txtCountHero.gameObject.SetActive(true);
    }

    public void Build(int _id)
    {
        timeBuild = GameConfig.Instance.BuildTime * idHouse;
        timeUpgrade = GameConfig.Instance.UpgradeTime;
        typeState = TypeStateHouse.Building;
        this.idHero = _id;       
        this.txtCountHero.gameObject.SetActive(false);
        this.imgHouse.sprite = UIManager.Instance.lstSpriteHouse[_id];

        imgNotBuild.enabled = false;
        imgLoadingBuild.gameObject.SetActive(true);
        imgHouse.gameObject.SetActive(false);
    }

    void BuildComplete()
    {
        typeState = TypeStateHouse.None;
        this.level = 1;
        this.countHero = 0;
        imgLoadingBuild.gameObject.SetActive(false);
        imgHouse.gameObject.SetActive(true);
        txtCountHero.gameObject.SetActive(true);
    }
}

public enum TypeStateHouse
{
    None = 1,
    Lock = 2,
    Building = 3,
    Upgrading = 4
}
