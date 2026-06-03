using UnityEngine;
using DG.Tweening;

public class AttackController : MonoBehaviour
{
    public Transform rangeCircle;

    [Range(0.11f,1f)]
    public float attackRange = 0.11f;

    [Range(0.1f, 1f)]
    public float attackTimeRange = 0.1f;

    [Range(0.1f, 1f)]
    public float attackCooldown = 1f;

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

    private void Start()
    {
        UpdateRangeVisual();

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

    private void OnValidate()
    {
        UpdateRangeVisual();
        UpdatePrefabRangeVisual();
    }

    void UpdateRangeVisual()
    {
        if (rangeCircle != null)
        {
            rangeCircle.localScale = Vector3.one * attackRange * 2f;
        }
    }

    void UpdatePrefabRangeVisual()
    {
        // 스케일 대신 SpriteRenderer의 Size.x(Width)를 직접 수정합니다.
        if (attackBoxPrefab != null)
        {
            SpriteRenderer sr = attackBoxPrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                attackBoxPrefab.transform.localScale = Vector3.one; // 스케일은 1,1,1로 고정
                sr.size = new Vector2(attackRange, sr.size.y);     // 가로 Width 크기 업데이트
            }
        }

        if (RangeBoxPrefab != null)
        {
            SpriteRenderer sr = RangeBoxPrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                RangeBoxPrefab.transform.localScale = Vector3.one;
                sr.size = new Vector2(attackRange, sr.size.y);
            }
        }
    }

    void UpdateAimTrajectory()
    {
        if (RangeBoxPrefab == null) return;

        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 direction = (mouse - playerPos).normalized;

        if (Vector2.Distance(mouse, playerPos) < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        RangeBoxPrefab.transform.position = playerPos;
        RangeBoxPrefab.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (rangeBoxSr != null && rangeBoxSr.material != null)
        {
            float offset = Time.time * -scrollSpeed;
            rangeBoxSr.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }

        float wave = Mathf.Sin(Time.time * blinkSpeed);
        float normalizedWave = (wave + 1f) / 2f;
        float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, normalizedWave);

        if (rangeBoxSr != null)
        {
            Color nextColor = originStartColor;
            nextColor.a = currentAlpha;
            rangeBoxSr.color = nextColor;
        }
    }

    void Attack()
    {
        canAttack = false;

        DOVirtual.DelayedCall(attackCooldown, () =>
        {
            canAttack = true;
        });

        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 direction = (mouse - playerPos).normalized;

        if (Vector2.Distance(mouse, playerPos) < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 공격 박스 생성
        GameObject box = Instantiate(attackBoxPrefab, playerPos, Quaternion.Euler(0, 0, angle));
        box.tag = "WhiteAttack";

        // [핵심 보완] 생성된 공격 박스의 스케일을 1로 고정하고, 가로 Width를 사거리에 딱 맞춤
        SpriteRenderer sr = box.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            box.transform.localScale = Vector3.one;
            sr.size = new Vector2(attackRange, sr.size.y);
        }


        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(attackTimeRange);      // 잠깐 유지

        seq.Append(
            sr.DOFade(0f, 0.1f)        // 빠르게 사라짐
        );

        seq.OnComplete(() =>
        {
            Destroy(box);
        });
    }

}
