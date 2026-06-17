using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    private EnemyStats stats;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("WhiteAttack"))
            return;

        SoundManager.Instance.PlaySFX("EnemyHit");

        stats.TakeDamage(PlayerStats.Instance.damage);

        HitInvincible hit =  GetComponent<HitInvincible>();

        if (hit != null && !hit.CanHit())
            return;

        if (hit != null)
            hit.Play();
    }
}