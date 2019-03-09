using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_BlastingDeath : Hero
{

    public override void Attack()
    {
        AnimAttack();
        Hero hero = targetCompetitor;
        if (hero.infoHero.ID == 10 || hero.infoHero.ID == 11)
        {
            hero.BeingAttacked(hero.infoHero.counterDame * hero.infoHero.numberHero);
        }
        hero.BeingAttacked(infoHero.dame * infoHero.numberHero);
    }

    public override void Die()
    {
        AnimDie();
        Collider2D[] arrCol = Physics2D.OverlapCircleAll(transform.position, infoHero.rangeBoom, 1 << 12);
        if (arrCol.Length > 0)
        {
            foreach (Collider2D col in arrCol)
            {
                if ((gameObject.tag == "Hero" && col.tag == "Enemy") || (gameObject.tag == "Enemy" && col.tag == "Hero"))
                {
                    col.GetComponent<Hero>().BeingAttacked(infoHero.dameDead * countHeroStart);
                }
            }
        }
    }


    public override void BeingAttacked(float _dame)
    {
        TakeDamage(_dame);
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 17;
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
