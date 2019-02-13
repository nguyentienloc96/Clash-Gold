using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildHouseObject : MonoBehaviour
{
    public bool isLock;
    public int ID_Hero;
    public Button thisButton;
    public Image imgLock;

    public void Btn_Click()
    {
        if (!isLock)
        {
            UIManager.Instance.Btn_BuildHouse(ID_Hero);
            thisButton.interactable = false;
        }
    }

    public void SetLock(bool _isUnLock)
    {
        if (!_isUnLock)
        {
            imgLock.gameObject.SetActive(true);
            isLock = true;
        }
        else
        {
            isLock = false;
            imgLock.gameObject.SetActive(false);
        }
    }
}
