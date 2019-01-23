using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FlyingReflector : Hero {
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
        this.infoHero.ID = 13;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 590;
        //this.infoHero.dame = 192;
        //this.infoHero.hitSpeed = 1.8f;
        //this.infoHero.speed = 5;
        //this.infoHero.price = 4000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        //this.infoHero.range = 5;
        //this.infoHero.counterDame = 50;
        //this.infoHero.isMom = false;
        //this.infoHero.isBaby = false;
        //this.infoHero.idBaby = 0;
        //this.infoHero.idMom = 0;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;

    }

    string nameBullet;
    public void Start()
    {
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = gameObject.tag == "Hero" ? "Flying Archer" : "Flying Archer E";
    }

    public void Update()
    {
        HeroUpdate();
    }
}
