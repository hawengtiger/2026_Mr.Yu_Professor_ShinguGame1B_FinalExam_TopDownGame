using UnityEngine;
using DG.Tweening;

public class AttackController : MonoBehaviour
{
    public Transform rangeCircle;

    [Header("공격 오브젝트 설정")]
    public GameObject RangeBoxPrefab;
    public GameObject attackBoxPrefab;

    [Header("산나비 점선 효과 설정")]
    public float scrollSpeed = 5f;
    public float blinkSpeed = 25f;
    public float minAlpha = 0.4f;
    public float maxAlpha = 1.0f;

    private SpriteRenderer rangeBoxSr;
    private Color originStartColor;
    private bool canAttack = true;

    private PlayerStats stats;

    private void Start()
    {
        if (PlayerStats.Instance == null)
        {
            Debug.LogError("PlayerStats.Instance가 없습니다. Player에 PlayerStats 넣었는지 확인!");
            enabled = false;
            return;
        }

        UpdateRangeVisual();
        UpdatePrefabRangeVisual();

        if (RangeBoxPrefab != null)
        {
            rangeBoxSr = RangeBoxPrefab.GetComponent<SpriteRenderer>();
            if (rangeBoxSr != null)
            {
                originStartColor = rangeBoxSr.color;
            }
        }
    }

    private void Update()
    {
        UpdateAimTrajectory();

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Attack();
        }
    }

    void UpdateRangeVisual()
    {
        if (rangeCircle != null)
        {
            rangeCircle.localScale =
                Vector3.one * PlayerStats.Instance.range * 2f;
        }
    }

    void UpdatePrefabRangeVisual()
    {
        if (PlayerStats.Instance == null) return;

        if (attackBoxPrefab != null)
        {
            SpriteRenderer sr =
                attackBoxPrefab.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                attackBoxPrefab.transform.localScale = Vector3.one;
                sr.size = new Vector2(PlayerStats.Instance.range, sr.size.y);
            }
        }

        if (RangeBoxPrefab != null)
        {
            SpriteRenderer sr =
                RangeBoxPrefab.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                RangeBoxPrefab.transform.localScale = Vector3.one;
                sr.size = new Vector2(PlayerStats.Instance.range, sr.size.y);
            }
        }
    }

    void UpdateAimTrajectory()
    {
        if (RangeBoxPrefab == null) return;

        Vector2 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 playerPos = transform.position;

        if (Vector2.Distance(mouse, playerPos) < 0.01f)
            return;

        Vector2 direction =
            (mouse - playerPos).normalized;

        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        RangeBoxPrefab.transform.position = playerPos;
        RangeBoxPrefab.transform.rotation =
            Quaternion.Euler(0, 0, angle);

        if (rangeBoxSr != null && rangeBoxSr.material != null)
        {
            float offset = Time.time * -scrollSpeed;

            rangeBoxSr.material.SetTextureOffset(
                "_MainTex",
                new Vector2(offset, 0));
        }

        float wave = Mathf.Sin(Time.time * blinkSpeed);
        float normalizedWave = (wave + 1f) / 2f;

        float currentAlpha =
            Mathf.Lerp(minAlpha, maxAlpha, normalizedWave);

        if (rangeBoxSr != null)
        {
            Color nextColor = originStartColor;
            nextColor.a = currentAlpha;
            rangeBoxSr.color = nextColor;
        }
    }

    void Attack()
    {
        SoundManager.Instance.PlaySFX("Attack");

        Vector2 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 playerPos = transform.position;

        if (Vector2.Distance(mouse, playerPos) < 0.01f)
            return;

        canAttack = false;

        DOVirtual.DelayedCall(PlayerStats.Instance.cooldown, () =>
        {
            canAttack = true;
        });

        Vector2 direction =
            (mouse - playerPos).normalized;

        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        GameObject box =
            Instantiate(
                attackBoxPrefab,
                playerPos,
                Quaternion.Euler(0, 0, angle));

        box.tag = "WhiteAttack";

        // 여기 추가
        HitEnemies(box);

        SpriteRenderer sr =
            box.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            box.transform.localScale = Vector3.one;
            sr.size = new Vector2(PlayerStats.Instance.range, sr.size.y);

            Sequence seq = DOTween.Sequence();

            seq.AppendInterval(PlayerStats.Instance.attackDuration);

            seq.Append(sr.DOFade(0f, 0.1f));

            seq.OnComplete(() =>
            {
                Destroy(box);
            });
        }
    }

    void HitEnemies(GameObject box)
    {
        BoxCollider2D col =
            box.GetComponent<BoxCollider2D>();

        if (col == null)
            return;

        Collider2D[] hits =
            Physics2D.OverlapBoxAll(
                col.bounds.center,
                col.bounds.size,
                box.transform.eulerAngles.z);

        foreach (Collider2D hit in hits)
        {
            EnemyStats enemy =
                hit.GetComponent<EnemyStats>();

            if (enemy == null)
                continue;

            HitInvincible invincible =
                hit.GetComponent<HitInvincible>();

            if (invincible != null && !invincible.CanHit())
                continue;

            SoundManager.Instance.PlaySFX("EnemyHit");

            enemy.TakeDamage(PlayerStats.Instance.damage);

            if (invincible != null)
                invincible.Play();
        }
    }

    public void RefreshStats()
    {
        UpdateRangeVisual();
        UpdatePrefabRangeVisual();
    }
}