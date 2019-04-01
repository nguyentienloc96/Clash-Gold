using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_ShortGunner : Hero {

    public override void Attack()
    {
        AnimAttack();
        Vector3 diff = (targetCompetitor.transform.position - posShoot.position).normalized;
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.up = diff;
        _bullet.GetComponent<Rigidbody2D>().velocity = diff * infoHero.speedBullet;
        float pX = ((infoHero.range / 5f) + 0.75f) / 5f;
        int xExp = (int)(Vector3.Distance(transform.position, targetCompetitor.transform.position) / pX);
        if (xExp <= 0)
            xExp = 1;
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame * infoHero.countHero * xExp;
        _bullet.GetComponent<Bullet>().isExplosion = false;
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
        this.infoHero.ID = 5;
        this.infoHero = GameConfig.Instance.lsInfoHero[this.infoHero.ID - 1];
    }

    string nameBullet;
    public void Start()
    {
        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = "ShortGunner"+gameObject.name;
    }

    public void Update()
    {
        HeroUpdate();
    }
}
