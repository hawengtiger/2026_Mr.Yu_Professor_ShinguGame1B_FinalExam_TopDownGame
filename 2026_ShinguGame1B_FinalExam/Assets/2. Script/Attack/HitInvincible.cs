using UnityEngine;
using DG.Tweening;

public class HitInvincible : MonoBehaviour
{
    public bool isInvincible;

    [Header("무적 시간")]
    public float redTime = 1f;
    public float blinkTime = 2f;

    private SpriteRenderer sr;
    private Color originColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originColor = sr.color;
    }

    public bool CanHit()
    {
        return !isInvincible;
    }

    public void Play()
    {
        if (isInvincible) return;

        isInvincible = true;

        sr.DOKill();

        Sequence seq = DOTween.Sequence();

        sr.color = new Color(140,0,0,255);

        seq.AppendInterval(redTime);

        seq.AppendCallback(() =>
        {
            sr.color = originColor;
        });

        seq.Append(
            sr.DOFade(100f / 255f, 0.15f)
              .SetLoops(Mathf.RoundToInt(blinkTime / 0.15f), LoopType.Yoyo)
        );

        seq.OnComplete(() =>
        {
            sr.color = originColor;

            Color c = sr.color;
            c.a = 1f;
            sr.color = c;

            isInvincible = false;
        });
    }
}