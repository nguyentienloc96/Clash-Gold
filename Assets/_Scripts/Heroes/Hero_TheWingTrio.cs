﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_TheWingTrio : Hero {
    public override void Attack()
    {

    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void BeingAttacked(float _dame)
    {
        throw new System.NotImplementedException();
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 2;
        this.infoHero.health = 190;
        this.infoHero.dame = 84;
        this.infoHero.hitSpeed = 1f;
        this.infoHero.speed = 10;
        this.infoHero.price = 5000;
        this.infoHero.capWar = 30 * GameConfig.Instance.Lo;
        this.infoHero.range = 0;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.ChemBay;

    }
    // Use this for initialization
    public void Start()
    {
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
    }

    public void Update()
    {
        HeroUpdate();
    }
}
