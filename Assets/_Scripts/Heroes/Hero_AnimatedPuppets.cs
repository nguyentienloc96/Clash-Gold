using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_AnimatedPuppets : Hero {
    public override void Attack()
    {

    }

    public override void Die()
    {
        AnimDie();
        if (gameObject.CompareTag("Enemy"))
        {
            ObjectPoolingManager.Instance.lsEnemy.Remove(this);
        }
        else
        {
            ObjectPoolingManager.Instance.lsHero.Remove(this);
        }
    }

    public override void BeingAttacked(float _dame)
    {
        TakeDamage(_dame);
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 19;
        this.infoHero.health = 98;
        this.infoHero.dame = 98;
        this.infoHero.hitSpeed = 1.1f;
        this.infoHero.speed = 10;
        this.infoHero.price = 0;
        this.infoHero.capWar = 0;
        this.infoHero.range = 0;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = true;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 18;
        this.infoHero.typeHero = TypeHero.ChemThuong;

    }

    // Use this for initialization
    public void Start()
    {
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
    }

    // Update is called once per frame
    public void Update()
    {
        HeroUpdate();
    }
}
