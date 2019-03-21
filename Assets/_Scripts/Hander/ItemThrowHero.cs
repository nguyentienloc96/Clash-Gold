using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemThrowHero : MonoBehaviour
{
    public Image iconHero;
    public House houseHero;
    public Slider sliderHero;
    public Text txtCountHero;
    public Text txtThrowHero;

    public void Update()
    {
        if (houseHero != null)
        {
            int number = (int)(sliderHero.value * houseHero.countHero);
            txtThrowHero.text = UIManager.Instance.ConvertNumber(number);
            txtCountHero.text = UIManager.Instance.ConvertNumber(houseHero.countHero);
        }
    }
}
