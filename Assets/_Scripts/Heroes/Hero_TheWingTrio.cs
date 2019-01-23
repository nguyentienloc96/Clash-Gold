using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_TheWingTrio : Hero {
    public override void Attack()
    {
        AnimAttack();
        if (targetCompetitor.infoHero.ID == 12)
        {
            targetCompetitor.BeingAttacked(targetCompetitor.infoHero.counterDame * targetCompetitor.infoHero.numberHero);
        }
        if (targetCompetitor.infoHero.ID == 13)
        {
            BeingAttacked(targetCompetitor.infoHero.counterDame * targetCompetitor.infoHero.numberHero);
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
        this.infoHero.ID = 2;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 190;
        //this.infoHero.dame = 84;
        //this.infoHero.hitSpeed = 1f;
        //this.infoHero.speed = 10;
        //this.infoHero.price = 5000;
        this.infoHero.capWar = 30 * GameConfig.Instance.Lo;
        //this.infoHero.range = 0;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = false;
        //this.infoHero.isBaby = false;
        //this.infoHero.idBaby = 0;
        //this.infoHero.idMom = 0;
        //this.infoHero.typeHero = TypeHero.ChemBay;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;
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
