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

        stats.TakeDamage(PlayerStats.Instance.damage);
    }
}