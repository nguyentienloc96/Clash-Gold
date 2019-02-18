using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_TheWingTrio : Hero {

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
            if (hero.infoHero.ID == 10 || hero.infoHero.ID == 11)
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
        this.infoHero.ID = 2;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
    }

    // Use this for initialization
    public void Start()
    {
        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
    }


    public void Update()
    {
        HeroUpdate();
    }
}
