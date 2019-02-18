﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_BloodySwords : Hero {

    public override void Attack()
    {
        AnimAttack();
        if (targetCompetitor.CompareTag("Castle"))
        {
            targetCompetitor.GetComponent<Castle>().BeingAttacked(infoHero.dame * infoHero.numberHero);
        }
        else
        {
            Hero hero = targetCompetitor.GetComponent<Hero>();
            if (hero.infoHero.ID == 10)
            {
                hero.BeingAttacked(hero.infoHero.counterDame * hero.infoHero.numberHero);
            }
            hero.BeingAttacked(infoHero.dame * infoHero.numberHero);
        }
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
        this.infoHero.ID = 14;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];


    }

    public void Start()
    {
        StartChild();
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);

    }
    public void Update()
    {
        HeroUpdate();
    }
}
