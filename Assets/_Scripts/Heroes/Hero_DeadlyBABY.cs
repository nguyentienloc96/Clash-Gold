using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_DeadlyBABY : Hero {

    public override void Attack()
    {
        AnimAttack();
        Hero hero = targetCompetitor;
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
        this.infoHero.ID = 31;
        this.infoHero = GameConfig.Instance.lsInfoHero[this.infoHero.ID - 30 + GameConfig.Instance.lsInfoHero.Count - 4];
        if (GameConfig.Instance.lsEquip[2].isHealth)
        {
            this.infoHero.health += infoHero.health * 0.5f;
        }
        if (GameConfig.Instance.lsEquip[2].isAtk)
        {
            this.infoHero.dame += infoHero.dame * 0.5f;
            this.infoHero.dameDead += infoHero.dameDead * 0.5f;
            this.infoHero.counterDame += infoHero.counterDame * 0.5f;
        }
        if (GameConfig.Instance.lsEquip[2].isHitSpeed)
        {
            this.infoHero.hitSpeed += infoHero.hitSpeed * 0.5f;
        }
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
