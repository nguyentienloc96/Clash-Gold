﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeHero
{
    None = 0,
    ChemThuong = 1,
    ChemBay = 2,
    CungThuong = 3,
    CungBay = 4,
    Canon = 5
}

[System.Serializable]
public struct InfoHero {
    public int ID;
    public string nameHero;
    public string info;
    public int countHero;
    public float health;
    public float dame;
    public float range;
    public float hitSpeed;
    public float speed;
    public float price;
    public float capWar;
    public float counterDame;
    public float speedBullet;
    public bool isMom;
    public bool isBaby;
    public int idMom;
    public int idBaby;
    public TypeHero typeHero;
    public float rangeBoom;
    public float dameDead;
}
