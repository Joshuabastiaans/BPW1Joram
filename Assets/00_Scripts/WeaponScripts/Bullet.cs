using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;

    public void SetDamage(int bulletDamage)
    {
        damage = bulletDamage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<ZombieHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
