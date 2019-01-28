using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Canon : Hero {
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

    public float timeSurvive = 0;
    public override void SetInfoHero()
    {
        this.infoHero.ID = 11;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 742;
        //this.infoHero.dame = 127;
        //this.infoHero.hitSpeed = 0.8f;
        //this.infoHero.speed = 0;
        //this.infoHero.price = 3000;
        //this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        //this.infoHero.range = 5.5f;
        timeSurvive = GameConfig.Instance.Timecanonsurvive;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = false;
        //this.infoHero.isBaby = false;
        //this.infoHero.idBaby = 0;
        //this.infoHero.idMom = 0;
        //this.infoHero.typeHero = TypeHero.Canon;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;
    }

    string nameBullet;
    public void Start()
    {
        StartChild();
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
