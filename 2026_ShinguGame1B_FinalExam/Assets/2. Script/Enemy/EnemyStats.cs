using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
        if (BossHPBar.Instance != null)
        {
            BossHPBar.Instance.ClearBoss();
        }

        if (enemyData.enemyType == EnemyDataSo.EnemyType.Boss)
        {
            SoundManager.Instance.PlaySFX("Clear");

            DOVirtual.DelayedCall(2f, () =>
            {
                SceneManager.LoadScene("EndingScene");
            });

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!collision.collider.CompareTag("Player"))
                return;

            // 보스는 접촉 데미지 없음
            if (enemyData.enemyType == EnemyDataSo.EnemyType.Boss)
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
                SoundManager.Instance.PlaySFX("Hit");
                HPUI.Instance.TakeDamage(enemyData.Damage);
                break;

            case EnemyDataSo.EnemyType.RedInkEnemy:
                SoundManager.Instance.PlaySFX("Hit");
                HPUI.Instance.TakeDamage(enemyData.Damage);
                break;

            case EnemyDataSo.EnemyType.BlueInkEnemy:
                SoundManager.Instance.PlaySFX("Hit");
                HPUI.Instance.TakeDamage(enemyData.Damage);
                break;
        }
    }
}