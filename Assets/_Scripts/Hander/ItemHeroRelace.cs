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
        txtCountHero.text = GameManager.Instance.lstHousePlayer[idHouseRelace].countHero.ToString();
    }

    public void RelaceItemHero()
    {
        int idHero = GameManager.Instance.lstHousePlayer[idHouseRelace].idHero - 1;
        UIManager.Instance.panelRelace.SetActive(false);
        UIManager.Instance.panelInfoHero.SetActive(true);
        DetailInfoHero detail = UIManager.Instance.panelInfoHero.GetComponent<DetailInfoHero>();
        detail.GetInfo(
            UIManager.Instance.sprAvatarHero[idHero],
            GameConfig.Instance.lstInfoHero[idHero].NameHero,
            GameConfig.Instance.lstInfoHero[idHero].NameHero,
            GameConfig.Instance.lstInfoHero[idHero].NameHero,
            () =>
            {
                YesRelaceItemHero(idHero);
                UIManager.Instance.panelInfoHero.SetActive(false);
            },
            true
            );
    }

    public void YesRelaceItemHero(int idHero)
    {
        GameManager.Instance.castlePlayer.lsHouseRelease[idLocation] = GameManager.Instance.lstHousePlayer[idHouseRelace];
        GameManager.Instance.castlePlayer.lstAvatarHeroRelease[idLocation].gameObject.SetActive(true);
        GameManager.Instance.castlePlayer.lstAvatarHeroRelease[idLocation].sprite = UIManager.Instance.sprAvatarHero[idHero - 1];
    }
}
