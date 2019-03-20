using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailWarring : MonoBehaviour {
  
    public Image icon;
    public Text txtInfo;
    public List<Sprite> lsSpIcon = new List<Sprite>();
    private float timeHide;
    private int idSelect;

    public void GetWarring(int idWarring, string info)
    {
        idSelect = idWarring;
        if (idWarring == 0)
        {
            icon.GetComponent<Animator>().enabled = true;
        }
        else
        {
            icon.GetComponent<Animator>().enabled = false;
        }
        timeHide = 3f;
        icon.sprite = lsSpIcon[idWarring];
        txtInfo.text = info;        
    }

    public void Update()
    {
        timeHide -= Time.deltaTime;
        if(timeHide <= 0)
        {
            if(idSelect == 0)
            {
                UIManager.Instance.warringBeingAttack.SetActive(true);
            }
            gameObject.SetActive(false);
            timeHide = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }
}
