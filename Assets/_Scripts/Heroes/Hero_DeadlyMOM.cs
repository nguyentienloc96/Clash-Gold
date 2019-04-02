using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_DeadlyMOM : Hero {

    string nameBullet;
    float timeInstanceChild;

    public override void Attack()
    {
        AnimAttack();
        Vector3 diff = (targetCompetitor.transform.position - posShoot.position).normalized;
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.up = diff;
        _bullet.GetComponent<Rigidbody2D>().velocity = diff * infoHero.speedBullet;
        _bullet.GetComponent<Bullet>().dameBullet = infoHero.dame * infoHero.countHero;
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
        this.infoHero.ID = 3;
        this.infoHero = GameConfig.Instance.lsInfoHero[this.infoHero.ID - 1];
    }

    public void Start()
    {
        StartChild();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
        nameBullet = "Reflector"+gameObject.name;
    }

    public void Update()
    {
        HeroUpdate();
        if(targetCompetitor != null && infoHero.countHero > 0)
        {
            timeInstanceChild += Time.deltaTime;
            if (timeInstanceChild >= 2f)
            {
                InstantiateChild(20,infoHero.countHero ,gameObject.tag == "Hero");
                timeInstanceChild = 0;
            }
        }
    }
}
