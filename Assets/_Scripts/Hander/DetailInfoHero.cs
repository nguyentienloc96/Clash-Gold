using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class DetailInfoHero : MonoBehaviour
{
    public Image imgicon;
    public Text txtName;
    public Text txtInfo;
    public Text txtInfoDetailHero;
    public Text txtPrice;
    public Button btnYes;

    public void GetInfo(Sprite icon, string name, string info, string detail,long price,UnityAction btnYes_Onclick)
    {
        imgicon.sprite = icon;
        txtName.text = name;
        txtInfo.text = info;
        txtInfoDetailHero.text = detail;
        btnYes.interactable = true;
        btnYes.onClick.RemoveAllListeners();
        btnYes.onClick.AddListener(btnYes_Onclick);
    }

    public void btnNo_Onclick()
    {
        gameObject.SetActive(false);
    }
}
