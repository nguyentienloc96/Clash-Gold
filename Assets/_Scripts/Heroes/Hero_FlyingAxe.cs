using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FlyingAxe : Hero
{

    public override void Attack()
    {
        AnimAttack();
        Hero hero = targetCompetitor;
        if (hero.infoHero.ID == 10)
        {
            BeingAttacked(hero.infoHero.counterDame * hero.infoHero.countHero);
        }
        Collider2D[] arrCol = Physics2D.OverlapCircleAll(transform.position, infoHero.rangeBoom / 5 + 0.75f, 1 << 12);
        if (arrCol.Length > 0)
        {
            foreach (Collider2D col in arrCol)
            {
                if ((gameObject.tag == "Hero" && col.tag == "Enemy") || (gameObject.tag == "Enemy" && col.tag == "Hero"))
                {
                    col.GetComponent<Hero>().BeingAttacked(infoHero.dame * infoHero.countHero);
                    //Debug.Log(infoHero.NameHero + " attack" + hero.infoHero.NameHero + " dame " + (infoHero.dame * infoHero.numberHero));

                }
            }
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
        this.infoHero.ID = 19;
        this.infoHero = GameConfig.Instance.lsInfoHero[this.infoHero.ID - 1];


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
