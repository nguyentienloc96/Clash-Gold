using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_BalloonKing : Hero {
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
        this.infoHero.ID = 5;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 750;
        //this.infoHero.dame = 260;
        //this.infoHero.hitSpeed = 1.5f;
        //this.infoHero.speed = 5;
        //this.infoHero.price = 4000;
        //this.infoHero.capWar = 10 * GameConfig.Instance.Med;
        //this.infoHero.range = 1.7f;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = true;
        //this.infoHero.isBaby = false;
        //this.infoHero.idBaby = 6;
        //this.infoHero.idMom = 0;
        //this.infoHero.typeHero = TypeHero.CungBay;        
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
