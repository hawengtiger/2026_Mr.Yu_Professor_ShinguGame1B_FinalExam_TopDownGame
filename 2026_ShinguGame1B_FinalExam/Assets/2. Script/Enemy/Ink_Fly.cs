using UnityEngine;
using DG.Tweening;

public class Ink_Fly : MonoBehaviour
{
    public float speed = 6f;

    [Header("잉크 지속시간")]
    public float lifeTime = 2f;

    [Header("마르는 시간")]
    public float dryTime = 0.5f;

    private Transform target;
    private SpriteRenderer sr;

    bool isDrying;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        Invoke(nameof(StartDry), lifeTime);
    }

    private void Update()
    {
        if (isDrying)
            return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction =
            ((Vector2)target.position -
            (Vector2)transform.position).normalized;

        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0, 0, angle);

        transform.position +=
            (Vector3)(direction *
            speed *
            Time.deltaTime);
    }

    void StartDry()
    {
        isDrying = true;

        transform.DOScale(
            Vector3.zero,
            dryTime);

        if (sr != null)
        {
            sr.DOFade(0f, dryTime)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        SoundManager.Instance.PlaySFX("Hit");

        HPUI.Instance.TakeDamage(3);

        Destroy(gameObject);
    }
}