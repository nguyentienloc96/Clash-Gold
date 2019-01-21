using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_AnimatedPuppets : Hero {
    public override void Attack()
    {

    }
    public override void MoveToPosition(Vector2 _toPos)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void CheckEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override void BeingAttacked(int _dame)
    {
        throw new System.NotImplementedException();
    }

    public override void SetInfoHero()
    {
        this.infoHero.ID = 19;
        this.infoHero.health = 98;
        this.infoHero.dame = 98;
        this.infoHero.hitSpeed = 1.1f;
        this.infoHero.speed = 10;
        this.infoHero.price = 0;
        this.infoHero.capWar = 0;
        this.infoHero.range = 0;
        this.infoHero.counterDame = 0;
        this.infoHero.isMum = false;
        this.infoHero.isBaby = true;
        this.infoHero.idBaby = 0;
        this.infoHero.idMom = 18;
        this.infoHero.typeHero = TypeHero.ChemThuong;

    }

    // Use this for initialization
    public void Start()
    {
        SetInfoHero();
        animator.SetFloat("IndexRun", numRun);
        animator.SetFloat("IndexAttack", numAttack);
    }

    // Update is called once per frame
    public void Update()
    {
        AnimtionUpdate();

        TestAnim();
    }

    public void TestAnim()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("J");
            AnimAttack();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P");
            AnimDie();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A");
            AnimRun();
        }
        else
        {
            AnimIdle();
        }

    }

    public void AnimtionUpdate()
    {
        animator.SetBool("Run", typeAction == TypeAction.RUN ? true : false);
        animator.SetBool("Attack", typeAction == TypeAction.ATTACK ? true : false);
        animator.SetBool("Die", typeAction == TypeAction.DIE ? true : false);
    }

    public void AnimAttack()
    {
        typeAction = TypeAction.ATTACK;
    }

    public void AnimDie()
    {
        typeAction = TypeAction.DIE;
    }

    public void AnimRun()
    {
        typeAction = TypeAction.RUN;
    }

    public void AnimIdle()
    {
        typeAction = TypeAction.IDLE;
    }
}
