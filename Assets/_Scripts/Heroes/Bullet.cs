using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float dameBullet;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Bullet Hero"))
        {
            if (collision.tag == "Enemy")
            {
                Hero hero = collision.GetComponent<Hero>();
                hero.parHit.transform.right = -transform.up;
                hero.parHit.transform.eulerAngles -= new Vector3(0f, 0f, 45f);   
                hero.BeingAttacked(dameBullet);                
                gameObject.SetActive(false);
            }
        }

        if (gameObject.CompareTag("Bullet Enemy"))
        {
            if (collision.tag == "Hero")
            {
                Hero hero = collision.GetComponent<Hero>();
                hero.parHit.transform.right = -transform.up;
                hero.parHit.transform.eulerAngles -= new Vector3(0f, 0f, 45f);
                hero.BeingAttacked(dameBullet);
                gameObject.SetActive(false);
            }
        }
    }

}
