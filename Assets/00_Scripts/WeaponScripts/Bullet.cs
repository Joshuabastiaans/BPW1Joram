using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<ZombieHealth>().TakeDamage(damage);
        }
        else
        {
            Debug.Log("Bullet hit something else");
            Destroy(gameObject);
        }
    }
}
