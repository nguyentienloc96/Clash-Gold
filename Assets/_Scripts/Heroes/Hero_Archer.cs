using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Archer : Hero {
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
        this.infoHero.ID = 14;
        this.infoHero.health = 598;
        this.infoHero.dame = 234;
        this.infoHero.hitSpeed = 1.4f;
        this.infoHero.speed = 5;
        this.infoHero.price = 5000;
        this.infoHero.capWar = 10 * GameConfig.Instance.Hi;
        this.infoHero.range = 5.5f;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = false;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 0;
        this.infoHero.typeHero = TypeHero.CungThuong;
        this.infoHero.speedBullet = 10f;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;
    }

    string nameBullet;
    public void Start()
    {
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = gameObject.tag == "Hero" ? "Archer" : "Archer E";
    }

    public void Update()
    {
        HeroUpdate();
    }

}
