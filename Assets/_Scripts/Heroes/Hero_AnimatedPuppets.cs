﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_AnimatedPuppets : Hero {

    public override void Attack()
    {
        AnimAttack();
        Hero hero = targetCompetitor.GetComponent<Hero>();
        if (hero.infoHero.ID == 10)
        {
            BeingAttacked(hero.infoHero.counterDame * hero.infoHero.countHero);
        }
        hero.BeingAttacked(infoHero.dame * infoHero.countHero);
        //Debug.Log(infoHero.NameHero + " attack" + hero.infoHero.NameHero + " dame " + (infoHero.dame * infoHero.numberHero));
    }

    public override void Die()
    {
        AnimDie();
    }

    public override void BeingAttacked(float _dame)
    {
        TakeDamage(_dame);
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 33;
        this.infoHero = GameConfig.Instance.lsInfoHero[this.infoHero.ID - 30 + GameConfig.Instance.lsInfoHero.Count - 4];
    }

    // Use this for initialization
    public void Start()
    {
        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
    }

    // Update is called once per frame
    public void Update()
    {
        HeroUpdate();
    }
}
