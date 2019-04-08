using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquipment : MonoBehaviour
{

    public int ID;
    public bool isUnLock;
    public Image iconHero;
    public Image iconEquip;
    public Text txtPercent;

    public GameObject objLock;
    public GameObject objOpen;
    public GameObject objHighLight;

    public void OnClick()
    {
        if (isUnLock)
        {
            GameManager.Instance.lsItemEquip[GameManager.Instance.xSelectEquip].objHighLight.SetActive(false);
            GameManager.Instance.xSelectEquip = ID;
            objHighLight.SetActive(true);
        }
        else
        {
            long expNeed = (long)(GameConfig.Instance.Pb1 * Mathf.Pow(GameConfig.Instance.Pbrate, ID));
            if (GameManager.Instance.gold > expNeed)
            {
                GameManager.Instance.AddExp(-expNeed);
                objLock.SetActive(false);
                GameManager.Instance.lsItemEquip[GameManager.Instance.xSelectEquip].objHighLight.SetActive(false);
                GameManager.Instance.xSelectEquip = ID;
                objHighLight.SetActive(true);
                GameManager.Instance.xOpenEquip++;
            }
        }
    }
}
