using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public static BossHPBar Instance;

    [Header("보스 HP 바")]
    public Image hpFill;

    [Header("HP 바 전체")]
    public GameObject hpBarRoot;

    private EnemyStats boss;

    private void Awake()
    {
        Instance = this;

        if (hpBarRoot != null)
            hpBarRoot.SetActive(false);
    }

    private void Update()
    {
        if (boss == null)
        {
            if (hpBarRoot != null && hpBarRoot.activeSelf)
                ClearBoss();

            return;
        }

        hpFill.fillAmount =
            boss.currentHp / boss.enemyData.hp;

        if (boss.currentHp <= 0)
        {
            ClearBoss();
        }
    }

    public void SetBoss(EnemyStats target)
    {
        boss = target;

        if (hpBarRoot != null)
            hpBarRoot.SetActive(true);

        if (hpFill != null)
            hpFill.fillAmount = 1f;
    }

    public void ClearBoss()
    {
        boss = null;

        if (hpBarRoot != null)
            hpBarRoot.SetActive(false);
    }
}