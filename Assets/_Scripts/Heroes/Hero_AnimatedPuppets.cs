using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_AnimatedPuppets : Hero {

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
        if (targetCompetitor.infoHero.ID == 12)
        {
            targetCompetitor.BeingAttacked(targetCompetitor.infoHero.counterDame * targetCompetitor.infoHero.numberHero);
        }
        targetCompetitor.BeingAttacked(infoHero.dame * infoHero.numberHero);
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
        this.infoHero.ID = 19;
        this.infoHero = GameConfig.Instance.lstInfoHero[this.infoHero.ID - 1];
        //this.infoHero.health = 98;
        //this.infoHero.dame = 98;
        //this.infoHero.hitSpeed = 1.1f;
        //this.infoHero.speed = 10;
        //this.infoHero.price = 0;
        //this.infoHero.capWar = 0;
        //this.infoHero.range = 0;
        //this.infoHero.counterDame = 0;
        //this.infoHero.isMom = false;
        //this.infoHero.isBaby = true;
        //this.infoHero.idBaby = 0;
        //this.infoHero.idMom = 18;
        //this.infoHero.typeHero = TypeHero.ChemThuong;
        this.txtCountHero.text = UIManager.Instance.ConvertNumber(infoHero.numberHero);
        this.infoHero.healthAll = this.infoHero.health * this.infoHero.numberHero;
    }

    // Use this for initialization
    public void Start()
    {
        StartChild();
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
    }

    // Update is called once per frame
    public void Update()
    {
        HeroUpdate();
    }
}
