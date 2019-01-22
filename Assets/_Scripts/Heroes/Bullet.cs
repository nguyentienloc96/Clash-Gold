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
                collision.GetComponent<Hero>().BeingAttacked((int)dameBullet);
                gameObject.SetActive(false);
            }
        }

        if (gameObject.CompareTag("Bullet Enemy"))
        {
            if (collision.tag == "Hero")
            {
                collision.GetComponent<Hero>().BeingAttacked((int)dameBullet);
                gameObject.SetActive(false);
            }
        }
    }

}
