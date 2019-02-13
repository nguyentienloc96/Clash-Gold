using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_TwistingAxe : Hero {

    public override void Attack()
    {
        AnimAttack();
        if (targetCompetitor.infoHero.ID == 12)
        {
            targetCompetitor.BeingAttacked(targetCompetitor.infoHero.counterDame * targetCompetitor.infoHero.numberHero);
        }
        targetCompetitor.BeingAttacked(infoHero.dame * infoHero.numberHero);
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
        this.infoHero.ID = 18;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
    }

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
