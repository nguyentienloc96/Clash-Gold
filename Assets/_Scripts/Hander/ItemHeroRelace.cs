using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHeroRelace : MonoBehaviour
{
    public int idLocation;
    public int idHouseRelace;
    public Image iconHero;
    public Text txtCountHero;

    public void Update()
    {
        //txtCountHero.text = GameManager.Instance.lstHousePlayer[idHouseRelace].countHero.ToString();
    }

    public void RelaceItemHero()
    {
        int idHero = GameManager.Instance.lsHousePlayer[idHouseRelace].info.idHero;
        UIManager.Instance.panelRelace.SetActive(false);
        YesRelaceItemHero(idHero);
    }

    public void YesRelaceItemHero(int idHero)
    {
        GameManager.Instance.castlePlayer.lsHouseRelease[idLocation] = GameManager.Instance.lsHousePlayer[idHouseRelace];
        UIManager.Instance.lstAvatarHeroRelease[idLocation].gameObject.SetActive(true);
        UIManager.Instance.lstAvatarHeroRelease[idLocation].sprite = UIManager.Instance.lsSprAvatarHero[idHero - 1];
    }
}
