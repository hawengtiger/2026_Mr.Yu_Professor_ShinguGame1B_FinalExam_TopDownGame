using UnityEngine;
using DG.Tweening;

public class EnemyMoving : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rb;

    public Sprite attackSprite1; // 기본
    public Sprite attackSprite2; // 강하게 보이는 순간

    [Header("설정")]
    public float chargeSpeed = 5f;
    public float chargeDuration = 2f;
    public float waitTime = 1f;

    bool isCharging;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        Invoke(nameof(PrepareCharge), waitTime);
    }

    void PrepareCharge()
    {
        Vector3 originalScale = transform.localScale;

        Sequence seq = DOTween.Sequence();

        // 움찔
        seq.Append(
            transform.DOScale(originalScale * 1.15f, 0.15f)
        );

        seq.Append(
            transform.DOScale(originalScale, 0.1f)
        );

        seq.OnComplete(StartCharge);
    }

    void StartCharge()
    {
        if (player.position.x > transform.position.x)
        {
            sr.flipX = true; // 오른쪽 보기
        }
        else
        {
            sr.flipX = false; // 왼쪽 보기
        }

        sr.sprite = attackSprite2;

        Vector2 dir =
            (player.position - transform.position).normalized;

        rb.linearVelocity = dir * chargeSpeed;

        isCharging = true;

        Invoke(nameof(StopCharge), chargeDuration);
    }

    void StopCharge()
    {
        if (!isCharging)
            return;

        isCharging = false;

        sr.sprite = attackSprite1;

        rb.linearVelocity = Vector2.zero;

        Invoke(nameof(PrepareCharge), waitTime);
    }
}