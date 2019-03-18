using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailIcon : MonoBehaviour {

    public int idHero;
    public GameObject icon;
    public GameObject info;
    public Text txtInfo;

    public void SwapIcon()
    {
        if (icon.activeSelf)
        {
            icon.SetActive(false);
            info.SetActive(true);
            string infoDetail = "";
            infoDetail += ": " + GameConfig.Instance.lstInfoHero[idHero -1].health + "\n";
            infoDetail += ": " + GameConfig.Instance.lstInfoHero[idHero -1].dame + "\n";
            infoDetail += ": " + GameConfig.Instance.lstInfoHero[idHero -1].hitSpeed + "\n";
            infoDetail += ": " + GameConfig.Instance.lstInfoHero[idHero -1].speed + "\n";
            if (GameConfig.Instance.lstInfoHero[idHero -1].range != 0)
            {
                infoDetail += ": " + GameConfig.Instance.lstInfoHero[idHero -1].range;
            }
            else
            {
                infoDetail += ": Mele";
            }
            txtInfo.text = infoDetail;
        }
        else
        {
            icon.SetActive(true);
            info.SetActive(false);
        }
    }
}
