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

    [Header("¿¹°í¼±")]
    public LineRenderer aimLine;

    public float previewLength = 3f;

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
        Vector2 dir =
            (player.position - transform.position).normalized;

        ShowAimLine(dir);

        Vector3 originalScale = transform.localScale;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DOScale(originalScale * 1.15f, 0.15f)
        );

        seq.Append(
            transform.DOScale(originalScale, 0.1f)
        );

        seq.OnComplete(() =>
        {
            HideAimLine();
            StartCharge();
        });
    }


    void ShowAimLine(Vector2 dir)
    {
        if (aimLine == null)
            return;

        aimLine.enabled = true;

        aimLine.positionCount = 2;

        aimLine.SetPosition(
            0,
            transform.position);

        aimLine.SetPosition(
            1,
            (Vector2)transform.position +
            dir * previewLength);
    }

    void HideAimLine()
    {
        if (aimLine == null)
            return;

        aimLine.enabled = false;
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