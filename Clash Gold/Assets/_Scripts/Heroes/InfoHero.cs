using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoHero {
    public float health;
    public float dame;
    public float range;
    public float hitSpeed;
    public float speed;
    public float price;
    public float capWar;
    public TypeHero typeHero;
}

public enum TypeHero
{
    None = 0,
    ChemThuong = 1,
    ChemBay = 2,
    CungThuong = 3,
    CungBay = 4,
    Canon = 5
}
