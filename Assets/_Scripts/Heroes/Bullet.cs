using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float dameBullet;
    public float rangeBoom;
    public bool isBoom;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Bullet Hero"))
        {
            if (collision.tag == "Enemy")
            {
                if (!isBoom)
                {
                    Hero hero = collision.GetComponent<Hero>();
                    hero.parHit.transform.right = -transform.up;
                    hero.parHit.transform.eulerAngles -= new Vector3(0f, 0f, 45f);
                    hero.BeingAttacked(dameBullet);
                    gameObject.SetActive(false);
                }
                else
                {
                    Collider2D[] arrCol = Physics2D.OverlapCircleAll(collision.transform.position, rangeBoom / 5f, 1 << 12);
                    if (arrCol.Length > 0)
                    {
                        foreach (Collider2D col in arrCol)
                        {
                            if (col.tag == "Enemy")
                            {
                                col.GetComponent<Hero>().BeingAttacked(dameBullet);
                            }
                        }
                    }
                    gameObject.SetActive(false);
                }
            }
        }

        if (gameObject.CompareTag("Bullet Enemy"))
        {
            if (collision.tag == "Hero")
            {
                if (!isBoom)
                {
                    Hero hero = collision.GetComponent<Hero>();
                    hero.parHit.transform.right = -transform.up;
                    hero.parHit.transform.eulerAngles -= new Vector3(0f, 0f, 45f);
                    hero.BeingAttacked(dameBullet);
                    gameObject.SetActive(false);
                }
                else
                {
                    Collider2D[] arrCol = Physics2D.OverlapCircleAll(collision.transform.position, rangeBoom / 5f, 1 << 12);
                    if (arrCol.Length > 0)
                    {
                        foreach (Collider2D col in arrCol)
                        {
                            if (col.tag == "Hero")
                            {
                                col.GetComponent<Hero>().BeingAttacked(dameBullet);
                            }
                        }
                    }
                    gameObject.SetActive(false);
                }
            }
            
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
