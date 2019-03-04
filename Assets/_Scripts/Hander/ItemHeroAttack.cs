using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHeroAttack : MonoBehaviour
{
    public House houseHero;
    public Image iconHero;
    public Text txtCountHero;
    public int countHero;

    public void SelectHero()
    {
       GameManager.Instance.itemSelectHero = this;
    }
}
