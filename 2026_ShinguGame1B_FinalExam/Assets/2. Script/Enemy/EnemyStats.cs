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
}