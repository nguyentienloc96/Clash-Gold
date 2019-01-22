using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour {
    public int idHouse;
    public int idHero;
    public int level;
    public bool isBuild;
    public bool isUnlock;
    public int countHero;

    public Button buttonUpgrade;
    public Button buttonRelease;
    public Text txtCountHero;
    public Image imgHouse;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isBuild && txtCountHero.gameObject.activeSelf)
        {
            txtCountHero.text = countHero.ToString();
        }
	}

    public void Btn_OnClick()
    {
        if (isBuild)
        {
            buttonUpgrade.gameObject.SetActive(true);
            buttonRelease.gameObject.SetActive(true);
        }
        else
        {
            UIManager.Instance.houseClick = idHouse;
            UIManager.Instance.ShowPanelBuild();
        }
    }

    public void CheckBuild()
    {
        if(isUnlock)
        {

        }
        else
        {

        }
    }

    public void Build(int _id)
    {
        this.idHero = _id;
        this.level = 1;
        this.isBuild = true;
        this.countHero = 0;
        //this.imgHouse = UIManager.Instance.lstHouse[_id].sp;
    }
}
