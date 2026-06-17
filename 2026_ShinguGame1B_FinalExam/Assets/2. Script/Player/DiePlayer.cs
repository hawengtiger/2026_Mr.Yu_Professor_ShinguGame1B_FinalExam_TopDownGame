using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class DiePlayer : MonoBehaviour
{
    public bool isDead;

    public GameObject deathPanel;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        deathPanel.SetActive(false);
    }

    private void Update()
    {
        if (PlayerStats.Instance.currentHp <= 0)
        {
            PlayerStats.Instance.currentHp = 0;

            PlayerController player = GetComponent<PlayerController>();
            sr.sprite = player.spriteDown[0];

            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        SoundManager.Instance.PlaySFX("Die");

        GetComponent<PlayerController>().enabled = false;
        GetComponentInChildren<AttackController>().enabled = false;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DORotate(
                new Vector3(0, 0, -90),
                0.5f)
                .SetEase(Ease.OutQuad)
        );

        seq.Join(
            transform.DOScale(
                0.7f,
                0.2f)
        );

        seq.Append(
            transform.DOScale(
                Vector3.zero,
                0.4f)
        );

        seq.OnComplete(() =>
        {
            deathPanel.SetActive(true);

            Time.timeScale = 0f;
        });
    }

    public void Retry()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainScene");
    }
}

