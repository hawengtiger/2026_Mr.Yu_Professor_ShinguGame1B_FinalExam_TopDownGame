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
            if (!collision.collider.CompareTag("Player"))
                return;

            HitInvincible invincible = collision.collider.GetComponent<HitInvincible>();

            if (invincible != null && !invincible.CanHit())
                return;

            ApplyHit();

            if (invincible != null)
                invincible.Play();
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

                HPUI.Instance.TakeDamage(enemyData.Damage);
                break;

            case EnemyDataSo.EnemyType.BlueInkEnemy:    

            HPUI.Instance.TakeDamage(enemyData.Damage);
                break;
        }
    }
}