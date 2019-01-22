using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_BloodySwords : Hero {
    public override void Attack()
    {
        AnimAttack();
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
        this.infoHero.ID = 16;
        this.infoHero.health = 1129;
        this.infoHero.dame = 598;
        this.infoHero.hitSpeed = 1.8f;
        this.infoHero.speed = 10;
        this.infoHero.price = 4000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        this.infoHero.range = 0;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.ChemThuong;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);

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
