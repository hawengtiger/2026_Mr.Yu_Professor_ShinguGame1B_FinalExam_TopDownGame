using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackUI : MonoBehaviour
{
    [SerializeField] private Image attackFillImage;

    private Coroutine fillCoroutine;

    private bool isCooldown = true;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && isCooldown)
        {
            OnAttackClicked();
        }
    }

    public void OnAttackClicked()
    {
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);

        fillCoroutine = StartCoroutine(FillCooldown());
    }

    private IEnumerator FillCooldown()
    {
        isCooldown = false;

        float elapsed = 0f;
        attackFillImage.fillAmount = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;

            float currentCooldown = PlayerStats.Instance.cooldown;

            attackFillImage.fillAmount =
                Mathf.Clamp01(elapsed / currentCooldown);

            if (attackFillImage.fillAmount >= 1f)
                break;

            yield return null;
        }

        attackFillImage.fillAmount = 1f;
        isCooldown = true;
        fillCoroutine = null;
    }
}