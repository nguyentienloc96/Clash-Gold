using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_LittleSoul : Hero {
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
        this.infoHero.ID = 6;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 67;
        //this.infoHero.dame = 67;
        //this.infoHero.hitSpeed = 1.1f;
        //this.infoHero.speed = 10;
        //this.infoHero.price = 0;
        //this.infoHero.capWar = 0;
        //this.infoHero.range = 0;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = false;
        //this.infoHero.isBaby = true;
        //this.infoHero.idBaby = 0;
        //this.infoHero.idMom = 5;
        //this.infoHero.typeHero = TypeHero.ChemBay;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;
    }

    // Use this for initialization
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
