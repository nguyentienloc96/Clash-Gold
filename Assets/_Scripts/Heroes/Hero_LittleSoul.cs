﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_LittleSoul : Hero {
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
        this.infoHero.ID = 6;
        this.infoHero.health = 67;
        this.infoHero.dame = 67;
        this.infoHero.hitSpeed = 1.1f;
        this.infoHero.speed = 10;
        this.infoHero.price = 0;
        this.infoHero.capWar = 0;
        this.infoHero.range = 0;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = true;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 5;
        this.infoHero.typeHero = TypeHero.ChemBay;

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