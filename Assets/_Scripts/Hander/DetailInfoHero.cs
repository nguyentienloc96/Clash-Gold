﻿using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class DetailInfoHero : MonoBehaviour
{
    public Image imgicon;
    public Text txtName;
    public Text txtInfo;
    public Text txtInfoDetailHero;
    public Text txtPrice;
    public GameObject priceObj;
    public Button btnYes;
    private bool isBuild;

    public void GetInfo(Sprite icon, string name, string info, string detail,long price,UnityAction btnYes_Onclick ,bool isBuild = false)
    {
        imgicon.sprite = icon;
        txtName.text = name;
        txtInfo.text = info;
        txtInfoDetailHero.text = detail;
        btnYes.gameObject.SetActive(true);
        if (price > 0)
        {
            priceObj.SetActive(true);
            txtPrice.text = price.ToString();
        }
        else
        {
            priceObj.SetActive(false);
        }
        btnYes.interactable = true;
        btnYes.onClick.RemoveAllListeners();
        btnYes.onClick.AddListener(btnYes_Onclick);
    }

    public void btnNo_Onclick()
    {
        gameObject.SetActive(false);
    }
}
