using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_ImmortalScyther : Hero {
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
        //this.infoHero.health = 1095;
        //this.infoHero.dame = 379;
        //this.infoHero.hitSpeed = 1.5f;
        //this.infoHero.speed = 5;
        //this.infoHero.price = 5000;
        //this.infoHero.capWar = 10 * GameConfig.Instance.Lo;
        //this.infoHero.range = 3;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = true;
        //this.infoHero.isBaby = false;
        //this.infoHero.idBaby = 19;
        //this.infoHero.idMom = 0;
        //this.infoHero.typeHero = TypeHero.ChemThuong;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;

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
