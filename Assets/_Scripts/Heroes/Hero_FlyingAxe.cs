﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FlyingAxe : Hero {
    public override void Attack()
    {

    }
    public override void MoveToPosition(Vector2 _toPos)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void CheckEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override void BeingAttacked(int _dame)
    {
        throw new System.NotImplementedException();
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 22;
        this.infoHero.health = 700;
        this.infoHero.dame = 250;
        this.infoHero.hitSpeed = 1.6f;
        this.infoHero.speed = 5;
        this.infoHero.price = 5000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Lo;
        this.infoHero.range = 0;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.ChemThuong;

    }

    // Use this for initialization
    void Start()
    {
        SetInfoHero();
        //Debug.Log(this.infoHero);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}