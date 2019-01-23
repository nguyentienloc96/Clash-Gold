using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_BloodySwords : Hero {
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
        this.infoHero.ID = 16;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 1129;
        //this.infoHero.dame = 598;
        //this.infoHero.hitSpeed = 1.8f;
        //this.infoHero.speed = 10;
        //this.infoHero.price = 4000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        //this.infoHero.range = 3.5f;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = false;
        //this.infoHero.isBaby = false;
        //this.infoHero.idBaby = 0;
        //this.infoHero.idMom = 0;
        //this.infoHero.typeHero = TypeHero.ChemThuong;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;

    }

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
