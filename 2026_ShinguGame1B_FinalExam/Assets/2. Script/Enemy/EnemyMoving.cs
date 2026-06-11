using UnityEngine;
using DG.Tweening;

public class EnemyMoving : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rb;

    public Sprite attackSprite1;
    public Sprite attackSprite2;

    private bool isCharging;
    private SpriteRenderer sr;
    private EnemyStats stats;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        sr = GetComponent<SpriteRenderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        Invoke(nameof(PrepareCharge), stats.waitTime);
    }

    void PrepareCharge()
    {
        Vector3 originalScale = transform.localScale;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(originalScale * 1.15f, 0.15f));
        seq.Append(transform.DOScale(originalScale, 0.1f));

        seq.OnComplete(StartCharge);
    }

    void StartCharge()
    {
        if (player.position.x > transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;

        sr.sprite = attackSprite2;

        Vector2 dir =
            (player.position - transform.position).normalized;

        rb.linearVelocity = dir * stats.speed;

        isCharging = true;

        Invoke(nameof(StopCharge), stats.duration);
    }

    void StopCharge()
    {
        if (!isCharging)
            return;

        isCharging = false;

        sr.sprite = attackSprite1;
        rb.linearVelocity = Vector2.zero;

        Invoke(nameof(PrepareCharge), stats.waitTime);
    }
}