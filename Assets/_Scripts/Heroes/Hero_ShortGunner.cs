using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_ShortGunner : Hero {
    public PolyNavAgent agent
    {
        get
        {
            if (!_agent)
                _agent = GetComponent<PolyNavAgent>();
            return _agent;
        }
    }
    public override void Attack()
    {
        AnimAttack();
        Vector3 diff = (targetCompetitor.transform.position - posShoot.position).normalized;
        GameObject _bullet = ObjectPoolingManager.Instance.GetObjectForType(nameBullet, posShoot.position);
        _bullet.SetActive(true);
        _bullet.transform.up = diff;
        _bullet.GetComponent<Rigidbody2D>().velocity = diff * infoHero.speedBullet;
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
        this.infoHero.ID = 7;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 696;
        //this.infoHero.dame = 620;
        //this.infoHero.hitSpeed = 2.2f;
        //this.infoHero.speed = 5;
        //this.infoHero.price = 5000;
        //this.infoHero.capWar = 10 * GameConfig.Instance.Lo;
        //this.infoHero.range = 4;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = false;
        //this.infoHero.isBaby = false;
        //this.infoHero.idBaby = 0;
        //this.infoHero.idMom = 0;
        //this.infoHero.typeHero = TypeHero.CungThuong;
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
