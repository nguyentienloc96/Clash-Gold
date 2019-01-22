using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_BloodyWing : Hero {
    public override void Attack()
    {

    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void BeingAttacked(float _dame)
    {
        throw new System.NotImplementedException();
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 17;
        this.infoHero.health = 695;
        this.infoHero.dame = 258;
        this.infoHero.hitSpeed = 1.6f;
        this.infoHero.speed = 5;
        this.infoHero.price = 3000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Hi;
        this.infoHero.range = 0;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.ChemBay;

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
