using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_DrunkenBomber : Hero
{

    public override void Attack()
    {
        AnimAttack();
        Vector3 diff = (targetCompetitor.transform.position - posShoot.position).normalized;
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.up = diff;
        _bullet.GetComponent<Rigidbody2D>().velocity = diff * infoHero.speedBullet;
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame * infoHero.numberHero;
        _bullet.GetComponent<Bullet>().isBoom = true;
        _bullet.GetComponent<Bullet>().rangeBoom = infoHero.rangeBoom;
    }

    public override void Die()
    {
        AnimDie();
        //ObjectPoolingManager.Instance.ResetPoolForType(nameBullet);
    }


    public override void BeingAttacked(float _dame)
    {
        TakeDamage(_dame);
    }


    public float spreadDame;
    public override void SetInfoHero()
    {
        this.infoHero.ID = 7;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        spreadDame = 4 * GameConfig.Instance.UnitRange;
    }
    string nameBullet;
    public void Start()
    {
        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = gameObject.name;
    }

    public void Update()
    {
        HeroUpdate();
    }
}
