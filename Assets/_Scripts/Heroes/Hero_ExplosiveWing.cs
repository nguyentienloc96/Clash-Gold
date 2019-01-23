using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_ExplosiveWing : Hero {
    public override void Attack()
    {
        AnimAttack();
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.right = transform.right;
        _bullet.GetComponent<Rigidbody2D>().velocity = transform.up * infoHero.speedBullet;
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame * infoHero.numberHero;
    }

    public override void Die()
    {
        AnimDie();
        ObjectPoolingManager.Instance.ResetPoolForType(nameBullet);
    }

    public override void BeingAttacked(float _dame)
    {
        TakeDamage(_dame);
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 10;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 1064;
        //this.infoHero.dame = 133;
        //this.infoHero.hitSpeed = 1.5f;
        //this.infoHero.speed = 10;
        //this.infoHero.price = 4000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        //this.infoHero.range = 3.5f;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = false;
        //this.infoHero.isBaby = false;
        //this.infoHero.idBaby = 0;
        //this.infoHero.idMom = 0;
        //this.infoHero.typeHero = TypeHero.CungBay;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;
    }
    string nameBullet;
    public void Start()
    {
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = gameObject.name;
    }

    public void Update()
    {
        HeroUpdate();
    }
}
