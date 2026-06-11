using NUnit.Framework.Interfaces;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyDataSo enemyData;

    public float currentHp;

    public int damage;
    public float speed;
    public float duration;
    public float waitTime;

    private void Awake()
    {
        damage = enemyData.Damage;
        speed = enemyData.Speed;
        duration = enemyData.Duration;
        waitTime = enemyData.WaitTime;

        currentHp = enemyData.hp;
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ApplyHit();
        }
    }

    public void ApplyHit()
    {
        switch (enemyData.enemyType)
        {
            case EnemyDataSo.EnemyType.GrayInkEnemy:

                HPUI.Instance.TakeDamage(enemyData.Damage);

                break;

            case EnemyDataSo.EnemyType.RedInkEnemy:

                PlayerInventory.Instance.AddCoin(enemyData.Damage);

                break;

            case EnemyDataSo.EnemyType.BlueInkEnemy:    
                PlayerInventory.Instance.AddKey(enemyData.Damage);

                break;
        }
    }
}